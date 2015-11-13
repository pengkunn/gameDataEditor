using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ConfigDataEditorClass
{
    /// <summary>
    /// 模板项
    /// </summary>
    public class Item
    {
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
        public ItemType Type
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

        /// <summary>
        /// 引用对象的名称
        /// </summary>
        public string ClassName
        {
            get;
            set;
        }

        /// <summary>
        /// 枚举的值
        /// </summary>
        public string EnumValue
        {
            get;
            set;
        }

        public IList<EnumType> EnumTypeList
        {
            get
            {
                string[] enums = this.EnumValue.Split('|');
                List<EnumType> enumTypes = new List<EnumType>();
                foreach (string e in enums)
                {
                    EnumType enumType = new EnumType();
                    enumType.CustomEnumText = e.Split(',')[0];
                    enumType.CustomEnumValue = e.Split(',')[1];
                    enumTypes.Add(enumType);
                }

                return enumTypes;

            }
        }
    }
}
