using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using System.Xml;

namespace ConfigDataEditorClass
{
    public class Project
    {
        XmlDocument _xmlDoc;
        List<Template> _templateList;
        List<ConfigData> _configDataList;

        public Project()
        {
            _templateList = new List<Template>();
            _configDataList = new List<ConfigData>();

            _xmlDoc = new XmlDocument();
            _xmlDoc.Load("Project.xml");
        }

        public string ID
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 项目文件存储路径
        /// </summary>
        public string ProjectPath
        {
            get;
            set;
        }

        public string TemplatePath
        {
            get;
            private set;
        }

        public string ConfigDataPath
        {
            get;
            private set;
        }

        /// <summary>
        /// 返回模板的只读集合
        /// </summary>
        public IList<Template> TemplateList
        {
            get
            {
                if (_templateList.Count == 0)
                {
                    foreach (string fileName in Directory.GetFiles(TemplatePath))
                    {
                        Template template = new Template();
                        template.Load(fileName);
                        _templateList.Add(template);
                    }
                }

                return _templateList.AsReadOnly();
            }

        }
        /// <summary>
        /// 返回数据的只读集合
        /// </summary>
        public IList<ConfigData> ConfigDataList
        {
            get
            {
                if (_configDataList.Count == 0)
                {
                    foreach (string fileName in Directory.GetFiles(ConfigDataPath,"*.*",SearchOption.AllDirectories))
                    {
                        ConfigData configData = new ConfigData();
                        configData.Load(fileName);
                        _configDataList.Add(configData);
                    }
                }

                return _configDataList.AsReadOnly();
            }
        }

        public void AddTemplate(Template importTemplate)
        {
            //查找项目中同名模板
            var query = from template in _templateList
                        where template.Code == importTemplate.Code
                        select template.Code;

            if (query.Count() != 0)
                throw new Exception("项目中已存在同名模板，无法导入！");

            _templateList.Add(importTemplate);
            Directory.CreateDirectory(this.ConfigDataPath + "\\" + importTemplate.Name);
            importTemplate.Save(TemplatePath + "\\" + importTemplate.Name + ".xml");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configdata"></param>
        /// <param name="groupName"></param>
        public void AddConfigData(ConfigData configdata,string groupName)
        {
            

            var query = from template in this.TemplateList
                        where template.Code == configdata.TemplateCode
                        select template;
            if (query.Count() == 0)
                throw new Exception("在项目中未找到模板！");

            var queryConfigData = from data in this.ConfigDataList
                                  where data.Code == configdata.Code
                                  select data;
            if (queryConfigData.Count() > 0)
                throw new Exception("项目中已存在同名数据！");

            _configDataList.Add(configdata);

            string path = "";
            if (string.IsNullOrEmpty(groupName))
            { 
                path=this.ConfigDataPath + "\\" + query.First<Template>().Name;
            }
            else
            {
                path=this.ConfigDataPath + "\\" + query.First<Template>().Name+"\\"+groupName;
            }

            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            configdata.Save(path + "\\" + configdata.Name + ".dat");
        }

        public void DeleteConfigData(ConfigData configdata)
        {
            _configDataList.Remove(configdata);

            if(File.Exists(configdata.FileName))
                File.Delete(configdata.FileName);
        }

        public void Load(string filename)
        {
            if (!File.Exists(filename))
                throw new Exception("未找到项目文件！");

            try
            {
                _xmlDoc.Load(filename);
                this.ID = _xmlDoc.SelectSingleNode("Project/ID").InnerText;
                this.Name = _xmlDoc.SelectSingleNode("Project/Name").InnerText;
                this.ProjectPath = _xmlDoc.SelectSingleNode("Project/Path").InnerText;

                string projectDirectory = Path.GetDirectoryName(filename);
                TemplatePath = projectDirectory + "\\Template";
                ConfigDataPath = projectDirectory + "\\ConfigData";
            }
            catch
            {

                throw new Exception("项目文件打开失败！");
            }

        }

        public void Save(string filename)
        {
            if (string.IsNullOrEmpty(filename))
                throw new Exception("未设置文件名!");


            if (string.IsNullOrEmpty(this.ID))
                this.ID = Guid.NewGuid().ToString("N");

            _xmlDoc.SelectSingleNode("Project/ID").InnerText = this.ID;
            _xmlDoc.SelectSingleNode("Project/Name").InnerText = this.Name;
            _xmlDoc.SelectSingleNode("Project/Path").InnerText = this.ProjectPath;

            _xmlDoc.Save(filename);

            string projectDirectory = Path.GetDirectoryName(filename);
            Directory.CreateDirectory(projectDirectory);

            TemplatePath = projectDirectory + "\\Template";
            Directory.CreateDirectory(TemplatePath);

            ConfigDataPath = projectDirectory + "\\ConfigData";
            Directory.CreateDirectory(ConfigDataPath);
        }

        /// <summary>
        /// 创建完整的数据文件路径
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string CreateConfigDataFileName(ConfigData data)
        {
            if (!Directory.Exists(this.ConfigDataPath + "\\" + data.GetTemplate(this).Name))
                Directory.CreateDirectory(this.ConfigDataPath + "\\" + data.GetTemplate(this).Name);

            return this.ConfigDataPath + "\\" + data.GetTemplate(this).Name + "\\" + data.Name + ".dat";
        }

        public IList<ConfigData> GetDataFromTemplate(Template template)
        {
            var query = from data in this.ConfigDataList
                        where data.TemplateCode == template.Code
                        select data;

            return query.ToList<ConfigData>();
        }

        public Template GetTemplateByCode(string code)
        {
            var query = from template in this.TemplateList
                        where template.Code == code
                        select template;

            return query.SingleOrDefault<Template>();
        }

        public ConfigData GetConfigDataByCode(string code)
        {
            var query = from data in this.ConfigDataList
                        where data.Code == code
                        select data;

            return query.SingleOrDefault<ConfigData>();
        }

        public ConfigData GetConfigDataByName(string name)
        {
            var query = from data in this.ConfigDataList
                        where data.Name == name
                        select data;

            return query.SingleOrDefault<ConfigData>();
        }

        public void CreateDataGroup(Template tmp,string groupName)
        {
            if (!Directory.Exists(this.ConfigDataPath + "\\" + tmp.Name + "\\" + groupName))
                Directory.CreateDirectory(this.ConfigDataPath + "\\" + tmp.Name + "\\" + groupName);
            
        }
    }
}
