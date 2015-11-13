using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.IO;

namespace TemplateEditorClass
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
        /// 模板id，采用guid
        /// </summary>
        public string ID
        {
            get;
            private set;
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
        /// 模板代码
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
        public IList<Item> Items
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
                this.ID = _xmlDoc.SelectSingleNode("template/id").InnerText;
                this.Name = _xmlDoc.SelectSingleNode("template/name").InnerText;
                this.Code = _xmlDoc.SelectSingleNode("template/code").InnerText;
                this.Memo = _xmlDoc.SelectSingleNode("template/memo").InnerText;

                foreach (XmlNode itemNode in _xmlDoc.SelectNodes("template/item"))
                {
                    if (itemNode.NodeType == XmlNodeType.Element)
                    {
                        Item item = new Item();
                        item.Code = itemNode.SelectSingleNode("itemcode").InnerText;
                        item.DefaultValue = itemNode.SelectSingleNode("defaultvalue").InnerText;
                        item.Memo = itemNode.SelectSingleNode("itemmemo").InnerText;
                        item.Name = itemNode.SelectSingleNode("itemname").InnerText;
                        item.Type = itemNode.SelectSingleNode("type").InnerText;
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

            //若此模板没有ID，则表示此模板为新建模板，需创建ID
            if (string.IsNullOrEmpty(this.ID))
                this.ID = Guid.NewGuid().ToString("N");

            _xmlDoc.SelectSingleNode("template/id").InnerText = this.ID;
            _xmlDoc.SelectSingleNode("template/name").InnerText = this.Name;
            _xmlDoc.SelectSingleNode("template/code").InnerText = this.Code;
            _xmlDoc.SelectSingleNode("template/memo").InnerText = this.Memo;


            foreach (XmlNode itemNode in _xmlDoc.SelectNodes("template/item"))
            {
                _xmlDoc.DocumentElement.RemoveChild(itemNode);
            }


            foreach (Item item in _items)
            {
                XmlNode tempNode = _tempXmlDoc.SelectSingleNode("template/item");
                XmlNode node = _xmlDoc.CreateNode(XmlNodeType.Element, "item", null);
                node.InnerXml = tempNode.InnerXml;

                node.SelectSingleNode("itemid").InnerText = item.ID;
                node.SelectSingleNode("itemcode").InnerText = item.Code;
                node.SelectSingleNode("defaultvalue").InnerText = item.DefaultValue;
                node.SelectSingleNode("itemmemo").InnerText = item.Memo;
                node.SelectSingleNode("itemname").InnerText = item.Name;
                node.SelectSingleNode("type").InnerText = item.Type;


                _xmlDoc.SelectSingleNode("template").AppendChild(node);
            }
              
            _xmlDoc.Save(filename);
        }

        /// <summary>
        /// 根据模板生成数据
        /// </summary>
        /// <param name="filename">生成文件名</param>
        public void Generate(string filename)
        { }


    }
}
