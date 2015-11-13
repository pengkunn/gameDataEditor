using ConfigDataEditorClass;
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
    public partial class frmTemplateManager : Form
    {
        Project _prj;

        public frmTemplateManager(Project prj)
        {
            InitializeComponent();

            _prj = prj;
        }

        private void frmTemplateManager_Load(object sender, EventArgs e)
        {
            listBoxTemplate.DisplayMember = "Name";
            listBoxTemplate.ValueMember = "Code";

            BindingList<Template> bTemplates = new BindingList<Template>(_prj.TemplateList);
            listBoxTemplate.DataSource = bTemplates;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();

            if (result != DialogResult.OK)
                return;

            foreach(string filename in openFileDialog1.FileNames)
            {
                Template template=new Template ();
                try
                {
                    template.Load(filename);
                    _prj.AddTemplate(template);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }

            listBoxTemplate.DisplayMember = "Name";
            listBoxTemplate.ValueMember = "Code";

            BindingList<Template> bTemplates = new BindingList<Template>(_prj.TemplateList);
            listBoxTemplate.DataSource = bTemplates;
        }
    }
}
