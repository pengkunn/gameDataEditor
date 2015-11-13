namespace ConfigDataEditorUI.Common
{
    partial class ListCtrl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.cboObject = new System.Windows.Forms.ComboBox();
            this.listObject = new System.Windows.Forms.ListBox();
            this.btnAdd = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cboObject
            // 
            this.cboObject.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboObject.FormattingEnabled = true;
            this.cboObject.Location = new System.Drawing.Point(208, 0);
            this.cboObject.Name = "cboObject";
            this.cboObject.Size = new System.Drawing.Size(121, 20);
            this.cboObject.TabIndex = 0;
            // 
            // listObject
            // 
            this.listObject.FormattingEnabled = true;
            this.listObject.ItemHeight = 12;
            this.listObject.Location = new System.Drawing.Point(0, 0);
            this.listObject.Name = "listObject";
            this.listObject.Size = new System.Drawing.Size(202, 184);
            this.listObject.TabIndex = 1;
            this.listObject.KeyUp += new System.Windows.Forms.KeyEventHandler(this.listObject_KeyUp);
            // 
            // btnAdd
            // 
            this.btnAdd.Location = new System.Drawing.Point(208, 26);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(121, 23);
            this.btnAdd.TabIndex = 2;
            this.btnAdd.Text = "添加";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(2, 187);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(197, 12);
            this.label1.TabIndex = 3;
            this.label1.Text = "选中列表中的项后，按delete键删除";
            // 
            // ObjectListControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.listObject);
            this.Controls.Add(this.cboObject);
            this.Name = "ObjectListControl";
            this.Size = new System.Drawing.Size(330, 202);
            this.Load += new System.EventHandler(this.ObjectListControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cboObject;
        private System.Windows.Forms.ListBox listObject;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Label label1;
    }
}
