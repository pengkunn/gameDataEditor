using ConfigDataEditorClass;
using ConfigDataEditorUI.Common;
using ConfigDataEditorUI.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ConfigDataEditorUI
{
    public partial class frmMain : Form
    {
        Project _project = null;
        string _projectFileName;
        Template _selecedTemplate;
        string _selectedGroupName;
        

        public frmMain()
        {
            InitializeComponent();

            _projectFileName = "";
        }


        private void FillTreeview(Project prj)
        {
            treeView1.Nodes.Clear();
            //加载根节点
            treeView1.Nodes.Add(new TreeNode(prj.Name));
            ImageList newImageList=new ImageList ();
            newImageList.Images.Add("project", Resources.project);
            newImageList.Images.Add("template", Resources.template);
            newImageList.Images.Add("folder",Resources.folder);
            newImageList.Images.Add("data",Resources.data);
            
            treeView1.ImageList = newImageList;
            
            treeView1.Nodes[0].ImageKey = "project";
            treeView1.Nodes[0].SelectedImageKey = "project";

            //加载模板节点
            foreach(Template template in prj.TemplateList)
            {
                TreeNode node = treeView1.Nodes[0].Nodes.Add(template.Name, template.Name);
                node.ImageKey = "template";
                node.SelectedImageKey = "template";
                node.Tag = template;


                //加载数据节点
                DirectoryInfo directoryInfo = new DirectoryInfo(prj.ConfigDataPath + "\\" + template.Name);
                foreach (FileSystemInfo fileSystemInfo in directoryInfo.GetFileSystemInfos())
                {
                    if (fileSystemInfo.GetType() == typeof(DirectoryInfo))
                    {
                        TreeNode folderNode = node.Nodes.Add(fileSystemInfo.Name, fileSystemInfo.Name);
                        folderNode.ImageKey = "folder";
                        folderNode.SelectedImageKey = "folder";
                        folderNode.Tag = template;
                        DirectoryInfo directoryData = (DirectoryInfo)fileSystemInfo;
                        foreach (FileInfo file in directoryData.GetFiles())
                        {
                            ConfigData configFolderData = prj.GetConfigDataByName(file.Name.Replace(file.Extension, ""));
                            TreeNode  folderDataNode = folderNode.Nodes.Add(configFolderData.Code, configFolderData.Name);
                            folderDataNode.ImageKey = "data";
                            folderDataNode.SelectedImageKey = "data";
                            folderDataNode.Tag = configFolderData;
                        }
                    }
                    else if (fileSystemInfo.GetType() == typeof(FileInfo))
                    {
                        ConfigData configdata = prj.GetConfigDataByName(fileSystemInfo.Name.Replace(fileSystemInfo.Extension,""));
                        TreeNode dataNode = node.Nodes.Add(configdata.Code, configdata.Name);
                        dataNode.ImageKey = "data";
                        dataNode.SelectedImageKey = "data";
                        dataNode.Tag = configdata;
                    }
                }

            }

            treeView1.ExpandAll();

            

        }


        private void menuTemplateManager_Click(object sender, EventArgs e)
        {
            TemplateManage();
        }

        private void TemplateManage()
        {
            if (CurrentProject.Instance == null)
                return;

            frmTemplateManager frm = new frmTemplateManager(CurrentProject.Instance);
            frm.ShowDialog();

            this.FillTreeview(CurrentProject.Instance);
        }

        private void menuCreateData_Click(object sender, EventArgs e)
        {
            frmCreateData frm = new frmCreateData();
            DialogResult result = frm.ShowDialog();
            
            if (frm.IsValidate)
            {
                if (CurrentProject.Instance.GetConfigDataByCode(frm.ConfigDataCode) != null)
                {
                    MessageBox.Show("项目中已存在同名数据!");
                    return;
                }

                ConfigData configdata = new ConfigData();
                configdata.Name = frm.ConfigDataName;
                configdata.Code = frm.ConfigDataCode;
                configdata.TemplateCode = _selecedTemplate.Code;
                configdata.ProjectId = CurrentProject.Instance.ID;
                configdata.Order = 1;

                CurrentProject.Instance.AddConfigData(configdata, _selectedGroupName);
                this.FillTreeview(CurrentProject.Instance);
            }

            

        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            _selectedGroupName = "";

            if (e.Button != MouseButtons.Right)
                return;

            if (e.Node.Level == 0)
            {
                contextMenuStrip2.Show(treeView1, e.Location);
            }

            if (e.Node.SelectedImageKey == "template" )
            {
                contextMenuStrip1.Items[1].Enabled = true;
                _selecedTemplate =(Template) e.Node.Tag;
                contextMenuStrip1.Show(treeView1, e.Location);
                
            }
            else if (e.Node.SelectedImageKey == "folder")
            {
                contextMenuStrip1.Items[1].Enabled = false;
                contextMenuStrip1.Show(treeView1, e.Location);
                _selecedTemplate = (Template)e.Node.Parent.Tag;
                _selectedGroupName = e.Node.Text;
            }

        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button != MouseButtons.Left)
                return;
            if (e.Node.Level != 2 && e.Node.Level != 3)
                return;

            if (e.Node.SelectedImageKey != "data")
                return;

            ConfigData configData = (ConfigData)e.Node.Tag;
                
            //已存在同名的数据时，不加载新的tab页
            var query = from tab in tabControl1.Tabs.Cast<DevComponents.DotNetBar.TabItem>()
                      where tab.Name==configData.Name
                      select tab;

            if (query.Count()>0)
                return;

            Template template=(Template) e.Node.Parent.Tag;

            //设置表格布局容器
            TableLayoutPanel panelData = new TableLayoutPanel();
            panelData.AutoSize = true;
            panelData.Dock = DockStyle.Fill;
            panelData.AutoScroll = true;
            panelData.BackColor = Color.Transparent;
            panelData.RowCount = template.ItemList.Count();
            panelData.ColumnCount = 2;
            panelData.RowStyles.Clear();
            for (int i = 0; i < panelData.RowCount; i++)
            {
                panelData.RowStyles.Add(new RowStyle() { SizeType = SizeType.AutoSize });
            }

            panelData.ColumnStyles.Clear();
            ColumnStyle cs1 = new ColumnStyle() { Width = 100, SizeType = SizeType.Absolute };
            panelData.ColumnStyles.Add(cs1);
            ColumnStyle cs2 = new ColumnStyle() { Width = 100, SizeType = SizeType.Percent };
            panelData.ColumnStyles.Add(cs2);

            for (int i=0;i<template.ItemList.Count();i++)
            {
                 
                Label lab=new Label ();
                lab.Text = template.ItemList[i].Name;
                lab.AutoSize = true;
                lab.Anchor = (AnchorStyles)(AnchorStyles.Right | AnchorStyles.Top);
                lab.Margin = new Padding(7);
                

                // Set up the delays for the ToolTip.
                toolTip1.AutoPopDelay = 5000;
                toolTip1.InitialDelay = 1000;
                toolTip1.ReshowDelay = 500;
                // Force the ToolTip text to be displayed whether or not the form is active.
                toolTip1.ShowAlways = true;
                toolTip1.SetToolTip(lab, template.ItemList[i].Memo);

                panelData.Controls.Add(lab);


                Control form = this.LoadFormControl(template.ItemList[i], configData);

                panelData.Controls.Add(form);

            }

            //添加新tab页
            DevComponents.DotNetBar.TabItem tabItem = tabControl1.CreateTab(configData.Name);
            tabItem.Name = configData.Code;
            tabItem.Tag = configData;
            tabItem.AttachedControl.Controls.Add(panelData);

            tabControl1.SelectedTab = tabItem;

            
            
        }

        private Data GetDataFromControl(Item item, Control control)
        {
            Data newData=new Data ();
            newData.ItemCode=item.Code;

            switch (item.Type.ToString().ToLower())
            {
                case "list":
                    ListCtrl listControl = (ListCtrl)control;
                    newData.Value = listControl.Text;
                    break;
                case "int":
                    newData.Value = control.Text;
                    break;
                case "decimal":
                    newData.Value = control.Text;
                    break;
                case "class":
                    ComboBox cboObject = (ComboBox)control;
                    newData.Value = cboObject.SelectedValue.ToString();
                    break;
                case "enum":
                    ComboBox cboEnum = (ComboBox)control;
                    newData.Value = cboEnum.SelectedValue.ToString();
                    break;
                case "text":
                    newData.Value = control.Text;
                    break;
                case "date":
                    DateTimePicker dtp = (DateTimePicker)control;
                    newData.Value = dtp.Value.ToString();
                    break;
                case "path":
                    newData.Value = control.Text;
                    break;
                default:
                    newData.Value = control.Text;
                    break;

            }

            return newData;


        }

        private Control LoadFormControl(Item item , ConfigData configData)
        {

            
            Control control = null;
            Data data = configData.GetData(item.Code);
            switch (item.Type.ToString().ToLower())
            {
                case "list":
                    IList<ConfigData> list = new List<ConfigData>();
                    if (data != null)
                    {
                        foreach (string code in data.ClassValueList)
                        {
                            list.Add(CurrentProject.Instance.GetConfigDataByCode(code));
                        }
                    }
                    control = new ListCtrl(CurrentProject.Instance.GetTemplateByCode(item.ClassName), list);
                    break;
                case "int":
                    control = new NumericUpDown();
                    NumericUpDown numInt = (NumericUpDown)control;
                    numInt.Minimum = -65536;
                    if (data != null)
                    {
                        numInt.Value = int.Parse(data.Value);
                    }
                    break;
                case "decimal":
                    control = new NumericUpDown();
                    NumericUpDown numDecimal = (NumericUpDown)control;
                    numDecimal.DecimalPlaces = 2;
                    numDecimal.Minimum = -65536;
                    if (data != null)
                    {
                        numDecimal.Value = decimal.Parse(data.Value);
                    }
                    break;
                case "class":
                    control = new ComboBox();
                    ComboBox cboObject = (ComboBox)control;
                    cboObject.DisplayMember = "Name";
                    cboObject.ValueMember = "Code";
                    IList<ConfigData> dataList = CurrentProject.Instance.GetDataFromTemplate(CurrentProject.Instance.GetTemplateByCode(item.ClassName));
                    cboObject.DataSource = dataList;
                    cboObject.DropDownStyle = ComboBoxStyle.DropDownList;
                    if (data != null)
                    {
                        cboObject.SelectedValue = data.Value;
                    }
                    break;
                case "enum":
                    control = new ComboBox();
                    ComboBox cboEnum = (ComboBox) control;
                    cboEnum.DisplayMember = "CustomEnumText";
                    cboEnum.ValueMember = "CustomEnumValue";
                    IList<EnumType> enumTypes = item.EnumTypeList;
                    cboEnum.DataSource = enumTypes;
                    cboEnum.DropDownStyle = ComboBoxStyle.DropDownList;
                    if (data != null)
                    {
                        cboEnum.SelectedValue = data.Value;
                    }
                    break;
                case "text":
                    control = new TextBox();
                    if (data != null)
                    {
                        control.Text = data.Value;
                    }
                    break;
                case "date":
                    control = new DateTimePicker();
                    DateTimePicker dtp = (DateTimePicker)control;
                    if (data != null)
                    {
                        dtp.Value = DateTime.Parse(data.Value);
                    }
                    break;
                case "path":
                    control = new TextBox();
                    if (data != null)
                    {
                        control.Text = data.Value;
                    }
                    break;
                default:
                    control = new TextBox();
                    if (data != null)
                    {
                        control.Text = data.Value;
                    }
                    break;

            }

            control.Name = item.Code;
            control.Width = 330;
            

            return control;
        }

        private void 新建NToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CreateProject();
        }

        private void CreateProject()
        {
            

            frmCreateProject frm = new frmCreateProject();
            DialogResult result = frm.ShowDialog();

            if (frm.IsValidate)
            {
                ClearAll();

                System.IO.Directory.CreateDirectory(frm.ProjectFolder);

                _project = new Project();
                _project.Name = frm.ProjectName;
                _projectFileName = frm.ProjectFileName;
                _project.Save(_projectFileName);
                CurrentProject.Instance = _project;

                treeView1.Nodes.Clear();
                treeView1.Nodes.Add(new TreeNode(CurrentProject.Instance.Name));
            }
        }

        private void ClearAll()
        {
            _project = null;
            CurrentProject.Instance = null;
            _projectFileName="";
            _selecedTemplate=null;
            tabControl1.Tabs.Clear();
            treeView1.Nodes.Clear();
            CurrentProject.Instance = null;
        }

        private void 打开OToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenProject();
        }

        private void OpenProject()
        {
            ClearAll();
            openFileDialog1.Filter = "项目文件|*.prj";
            DialogResult result = openFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                _projectFileName = openFileDialog1.FileName;
                _project = new Project();
                _project.Load(_projectFileName);
                CurrentProject.Instance = _project;

                this.FillTreeview(CurrentProject.Instance);
            }
        }

        private void 保存SToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveProject();
        }

        private void SaveProject()
        {
            foreach (DevComponents.DotNetBar.TabItem tab in tabControl1.Tabs)
            {
                ConfigData configData = (ConfigData)tab.Tag;
                Template template = configData.GetTemplate(CurrentProject.Instance);
                configData.DataList.Clear();
                foreach (Item item in template.ItemList)
                {
                    configData.DataList.Add(this.GetDataFromControl(item, tab.AttachedControl.Controls.Find(item.Code, true)[0]));
                }
                configData.Save();
            }
            CurrentProject.Instance.Save(_projectFileName);
        }

        private void 新建NToolStripButton_Click(object sender, EventArgs e)
        {
            CreateProject();
        }

        private void 打开OToolStripButton_Click(object sender, EventArgs e)
        {
            OpenProject();
        }

        private void 保存SToolStripButton_Click(object sender, EventArgs e)
        {
            SaveProject();
        }

        private void 模板管理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            TemplateManage();
        }

        private void 新建分类ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCreateDataGroup frm=new frmCreateDataGroup ();
            frm.ShowDialog();
            CurrentProject.Instance.CreateDataGroup(_selecedTemplate, frm.GroupName);
            FillTreeview(CurrentProject.Instance);
        }



    }
}
