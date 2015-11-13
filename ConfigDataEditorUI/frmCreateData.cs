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
    public partial class frmCreateData : Form
    {
        public frmCreateData()
        {
            InitializeComponent();
            this.IsValidate = false;
        }

        public string ConfigDataName
        {
            get;
            private set;
        }

        public string ConfigDataCode
        {
            get;
            private set;
        }

        public bool IsValidate
        {
            get;
            private set;
        }


        private void btnOK_Click(object sender, EventArgs e)
        {
            this.ConfigDataName = txtObjectName.Text;
            this.ConfigDataCode = txtCode.Text;
            if (string.IsNullOrEmpty(txtObjectName.Text) || string.IsNullOrEmpty(txtCode.Text))
            {
                MessageBox.Show("对象名称和编码不能为空!");
                return;
            }

            IsValidate = true;
            this.Close();
        }

        private void btnCancle_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
