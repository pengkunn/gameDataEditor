using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace ConfigDataEditorUI
{
    public partial class frmCreateProject : Form
    {

        public frmCreateProject()
        {
            InitializeComponent();
            this.IsValidate = false;
        }

        public string ProjectName
        {
            get;
            set;
        }

        public string ProjectFileName
        {
            get;
            set;
        }

        public string ProjectFolder
        {
            get;
            set;
        }

        public bool IsValidate
        {
            get;
            private set;
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.ShowDialog();

            txtProjectPath.Text = folderBrowserDialog1.SelectedPath;
            
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

            if (string.IsNullOrEmpty(txtProjectName.Text) || string.IsNullOrEmpty(txtProjectPath.Text))
            {
                MessageBox.Show("必须输入项目名称和路径！");
                return;
            }

            if (!System.IO.Directory.Exists(txtProjectPath.Text))
            {
                MessageBox.Show("路径错误！");
                return;
            }

            this.ProjectName = txtProjectName.Text;
            this.ProjectFolder = txtProjectPath.Text + "\\" + txtProjectName.Text;
            this.ProjectFileName = this.ProjectFolder + "\\" + txtProjectName.Text + ".prj";
            this.IsValidate = true;
            this.Close();
        }




        
    }
}
