using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;

using System.Windows.Forms;
using ConfigDataEditorClass;

namespace ConfigDataEditorUI.Common
{
    public partial class ListCtrl : UserControl
    {
        Template _template;
        IList<ConfigData> _configDataList;

        public ListCtrl(Template template,IList<ConfigData> configDataList)
        {
            InitializeComponent();
            _template = template;
            _configDataList=configDataList;
        }

        public Template Template
        {
            get { return _template; }
        }

        public IList<ConfigData> ConfigDataList
        {
            get { return _configDataList; }
        }


        private void ObjectListControl_Load(object sender, EventArgs e)
        {
            cboObject.DisplayMember = "Name";
            cboObject.DataSource = CurrentProject.Instance.GetDataFromTemplate(_template);

            BindListObject();
        }

        private void BindListObject()
        {
            listObject.DisplayMember = "Name";
            BindingList<ConfigData> bConfigData = new BindingList<ConfigData>(_configDataList);
            listObject.DataSource = bConfigData;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var query = from data in _configDataList
                        where data.Code == ((ConfigData)cboObject.SelectedValue).Code
                        select data;
            if (query.Count() > 0)
                return;

            _configDataList.Add((ConfigData)cboObject.SelectedValue);
            BindListObject();
        }

        private void listObject_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                _configDataList.Remove((ConfigData)listObject.SelectedValue);
                BindListObject();
            }
        }

        public override string Text
        {
            get
            {
                string text = "";
                foreach (ConfigData data in _configDataList)
                {
                    text += data.Code+"|";
                }
                if (!string.IsNullOrEmpty(text))
                    text = text.Substring(0, text.Length - 1);

                return text;
            }
            set
            {
                base.Text = value;
            }
        }
    }
}
