using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateEditorClass
{
    /// <summary>
    /// 模板项
    /// </summary>
    public class Item
    {
        string _id;
        /// <summary>
        /// 模板项id，采用guid
        /// </summary>
        public string ID
        {
            get
            {
                if (string.IsNullOrEmpty(_id))
                {
                    _id = Guid.NewGuid().ToString("N");
                }
                return _id;
            }
            private set
            {
                _id = value;
            }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// 代码
        /// </summary>
        public string Code
        {
            get;
            set;
        }

        /// <summary>
        /// 值类型
        /// </summary>
        public string Type
        {
            get;
            set;
        }

        /// <summary>
        /// 默认值
        /// </summary>
        public string DefaultValue
        {
            get;
            set;
        }

        /// <summary>
        /// 备注
        /// </summary>
        public string Memo
        {
            get;
            set;
        }
    }
}
