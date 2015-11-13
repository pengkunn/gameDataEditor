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
    public partial class frmCreateDataGroup : Form
    {
        public frmCreateDataGroup()
        {
            InitializeComponent();
        }

        public string GroupName
        {
            get;
            private set;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtGroupName.Text))
            {
                MessageBox.Show("分类名称不能为空！");
                return;
            }

            this.GroupName = txtGroupName.Text;
            this.Close();
        }
    }
}
