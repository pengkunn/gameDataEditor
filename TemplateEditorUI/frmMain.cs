using ConfigDataEditorClass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace TemplateEditorUI
{
    public partial class frmMain : Form
    {
        Template _template;

        public frmMain()
        {
            InitializeComponent();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            panel1.Enabled = true;
            _template = new Template();

            BindingList<Item> bItems = new BindingList<Item>(_template.ItemList);

            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = bItems;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();

            try
            {
                _template = new Template();
                _template.Load(openFileDialog1.FileName);

                txtMemo.Text = _template.Memo;
                txtName.Text = _template.Name;

                BindingList<Item> bItems = new BindingList<Item>(_template.ItemList);

                dataGridView1.AutoGenerateColumns = false;
                dataGridView1.DataSource = bItems;
                dataGridView1.Refresh();
            }
            catch
            {
                MessageBox.Show("打开模板文件出错！");
            }

            panel1.Enabled = true;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_template == null)
            {
                MessageBox.Show("没有需要保存的模板！");
                return;
            }

            saveFileDialog1.ShowDialog();

            _template.Name = txtName.Text;
            _template.Memo = txtMemo.Text;

            _template.Save(saveFileDialog1.FileName);

            MessageBox.Show("模板文件保存成功！");
        }

        private void openFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }

        
    }
}
