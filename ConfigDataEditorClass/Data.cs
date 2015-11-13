using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ConfigDataEditorClass
{
    public class Data
    {
        public string ItemCode
        {
            get;
            set;
        }

        public string Value
        {
            get;
            set;
        }

        public IList<string> ClassValueList
        {
            get 
            { 
                return this.Value.Split(new char[]{'|'},StringSplitOptions.RemoveEmptyEntries).ToList<string>();
            }
            set
            {
                IList<string> list = value;
                string classValues = "";
                foreach(string classvalue in list)
                {
                    classValues += classvalue + "|";
                }

                if (!string.IsNullOrEmpty(classValues))
                {
                    this.Value = classValues.Substring(0, classValues.Length - 1);
                }
                else
                {
                    this.Value = classValues;
                }
            }
        }

        
    }
}
