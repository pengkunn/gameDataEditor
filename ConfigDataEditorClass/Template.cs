using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;

namespace ConfigDataEditorClass
{
    /// <summary>
    /// 模板
    /// </summary>
    public class Template
    {
        IList<Item> _items;
        XmlDocument _xmlDoc;
        XmlDocument _tempXmlDoc;
        

        public Template()
        {
            _items = new List<Item>();
            _xmlDoc = new XmlDocument();
            _tempXmlDoc = new XmlDocument();
            _xmlDoc.Load("Template.xml");
            _tempXmlDoc.Load("Template.xml");
        }


        /// <summary>
        /// 模板名称
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 模板代码，英文，在项目中唯一
        /// </summary>
        public string Code
        {
            get;
            set;
        }


        /// <summary>
        /// 模板备注
        /// </summary>
        public string Memo
        {
            get;
            set;
        }

        /// <summary>
        /// 模版项
        /// </summary>
        public IList<Item> ItemList
        {
            get { return _items; }
            set { _items = value; }
        }

        /// <summary>
        /// 加载模板
        /// </summary>
        /// <param name="filename">模板文件名</param>
        public void Load(string filename)
        {
            if (!File.Exists(filename))
                throw new Exception("未找到模板文件！");

            try
            {
                _xmlDoc.Load(filename);
                this.Code = _xmlDoc.SelectSingleNode("Template/Code").InnerText;
                this.Name = _xmlDoc.SelectSingleNode("Template/Name").InnerText;
                this.Memo = _xmlDoc.SelectSingleNode("Template/Memo").InnerText;

                foreach (XmlNode itemNode in _xmlDoc.SelectNodes("Template/Item"))
                {
                    if (itemNode.NodeType == XmlNodeType.Element)
                    {
                        Item item = new Item();
                        item.Code = itemNode.SelectSingleNode("Code").InnerText;
                        item.DefaultValue = itemNode.SelectSingleNode("DefaultValue").InnerText;
                        item.Memo = itemNode.SelectSingleNode("Memo").InnerText;
                        item.Name = itemNode.SelectSingleNode("Name").InnerText;
                        item.Type =(ItemType) Enum.Parse(typeof(ItemType),itemNode.SelectSingleNode("Type").InnerText);
                        item.ClassName = itemNode.SelectSingleNode("ClassName").InnerText;
                        item.EnumValue = itemNode.SelectSingleNode("EnumValue").InnerText;
                        _items.Add(item);
                    }
                }
            }
            catch
            {
                throw new Exception("模板文件加载失败！");
            }

            

        }

        /// <summary>
        /// 保存模板
        /// </summary>
        /// <param name="filename">模板文件名</param>
        public void Save(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                throw new Exception("未设置模板文件名!");


            _xmlDoc.SelectSingleNode("Template/Code").InnerText = this.Code;
            _xmlDoc.SelectSingleNode("Template/Name").InnerText = this.Name;
            _xmlDoc.SelectSingleNode("Template/Memo").InnerText = this.Memo;


            foreach (XmlNode itemNode in _xmlDoc.SelectNodes("Template/Item"))
            {
                _xmlDoc.DocumentElement.RemoveChild(itemNode);
            }


            foreach (Item item in _items)
            {
                XmlNode tempNode = _tempXmlDoc.SelectSingleNode("Template/Item");
                XmlNode node = _xmlDoc.CreateNode(XmlNodeType.Element, "Item", null);
                node.InnerXml = tempNode.InnerXml;

                node.SelectSingleNode("Code").InnerText = item.Code;
                node.SelectSingleNode("DefaultValue").InnerText = item.DefaultValue;
                node.SelectSingleNode("Memo").InnerText = item.Memo;
                node.SelectSingleNode("Name").InnerText = item.Name;
                node.SelectSingleNode("Type").InnerText = item.Type.ToString();
                node.SelectSingleNode("ClassName").InnerText = item.ClassName;
                node.SelectSingleNode("EnumValue").InnerText = item.EnumValue;


                _xmlDoc.SelectSingleNode("Template").AppendChild(node);
            }
              
            _xmlDoc.Save(filename);
        }

    }
}
