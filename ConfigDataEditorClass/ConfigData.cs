using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using System.Xml;

namespace ConfigDataEditorClass
{
    public class ConfigData
    {
        string _filename;
        XmlDocument _xmlDoc;
        XmlDocument _tempXmlDoc;

        public ConfigData()
        {
            _xmlDoc = new XmlDocument();
            _tempXmlDoc = new XmlDocument();
            _xmlDoc.Load("ConfigData.xml");
            _tempXmlDoc.Load("ConfigData.xml");

            this.DataList = new List<Data>();
        }


        public string Name
        {
            get;
            set;
        }

        public string Code
        {
            get;
            set;
        }

        public string ProjectId
        {
            get;
            set;
        }

        public string TemplateCode
        {
            get;
            set;
        }

        public int Order
        {
            get;
            set;
        }

        public IList<Data> DataList
        {
            get;
            private set;
        }

        public string FileName
        {
            get
            {
                return _filename;
            }
        }

        public string Directory
        {
            get 
            {
                return Path.GetDirectoryName(this._filename);
            }
        }

        public Template GetTemplate(Project prj)
        {
            
            if (prj.ID != this.ProjectId)
                throw new Exception("数据文件不在指定的项目中！");

            var query = from template in prj.TemplateList
                        where template.Code == this.TemplateCode
                        select template;
            if (query.Count() == 0)
                throw new Exception("在指定项目中未找到模板！");

            return query.First<Template>();
        }

        public Data GetData(string ItemCode)
        {
            var query = from data in this.DataList
                        where data.ItemCode == ItemCode
                        select data;
            if (query.Count() == 0)
                return null;

            return query.First<Data>();

        }

        public void Load(string fileName)
        {
            _filename = fileName;
            if (!File.Exists(_filename))
                throw new Exception("未找到数据文件！");

            try
            {
                _xmlDoc.Load(_filename);
                this.Name = _xmlDoc.SelectSingleNode("ConfigData/Name").InnerText;
                this.Code = _xmlDoc.SelectSingleNode("ConfigData/Code").InnerText;
                this.Order = int.Parse(_xmlDoc.SelectSingleNode("ConfigData/Order").InnerText);
                this.TemplateCode = _xmlDoc.SelectSingleNode("ConfigData/TemplateCode").InnerText;
                this.ProjectId = _xmlDoc.SelectSingleNode("ConfigData/ProjectId").InnerText;

                foreach (XmlNode itemNode in _xmlDoc.SelectNodes("ConfigData/Data"))
                {
                    if (itemNode.NodeType == XmlNodeType.Element)
                    {
                        Data data = new Data();
                        data.ItemCode = itemNode.SelectSingleNode("ItemCode").InnerText;
                        data.Value = itemNode.SelectSingleNode("Value").InnerText;
                        
                        this.DataList.Add(data);
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception("数据文件加载失败！");
            }
        }

        public void Save()
        {
            this.Save(_filename);
        }

        public void Save(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                throw new Exception("未设置数据文件名!");

            _filename = fileName;

            _xmlDoc.SelectSingleNode("ConfigData/Name").InnerText = this.Name;
            _xmlDoc.SelectSingleNode("ConfigData/Code").InnerText = this.Code;
            _xmlDoc.SelectSingleNode("ConfigData/Order").InnerText = this.Order.ToString();
            _xmlDoc.SelectSingleNode("ConfigData/TemplateCode").InnerText = this.TemplateCode;
            _xmlDoc.SelectSingleNode("ConfigData/ProjectId").InnerText = this.ProjectId;


            foreach (XmlNode itemNode in _xmlDoc.SelectNodes("ConfigData/Data"))
            {
                _xmlDoc.DocumentElement.RemoveChild(itemNode);
            }


            foreach (Data data in this.DataList)
            {
                XmlNode tempNode = _tempXmlDoc.SelectSingleNode("ConfigData/Data");
                XmlNode node = _xmlDoc.CreateNode(XmlNodeType.Element, "Data", null);
                node.InnerXml = tempNode.InnerXml;

                node.SelectSingleNode("ItemCode").InnerText = data.ItemCode;
                node.SelectSingleNode("Value").InnerText = data.Value;


                _xmlDoc.SelectSingleNode("ConfigData").AppendChild(node);
            }

            _xmlDoc.Save(fileName);
        }
    }
}
