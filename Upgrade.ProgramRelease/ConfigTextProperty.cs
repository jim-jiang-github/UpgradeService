using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Upgrade.ProgramRelease
{
    public class ConfigTextProperty : TextBox
    {
        #region 变量
        private Config config;
        private string propertyName;
        #endregion
        #region 属性
        public Config Config
        {
            get => this.config;
            set
            {
                this.config = value;
                this.SetText();
            }
        }
        public string PropertyName
        {
            get => this.propertyName;
            set
            {
                this.propertyName = value;
                this.SetText();
            }
        }
        #endregion
        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            PropertyInfo propertyInfo = typeof(Config).GetProperty(this.PropertyName);
            object property = propertyInfo.GetValue(this.Config);
            if (propertyInfo.PropertyType.IsArray)
            {
                propertyInfo.SetValue(this.Config, this.Text.Split(',').Select(s => s.Trim()).Where(s => s != string.Empty).ToArray());
            }
            else
            {
                propertyInfo.SetValue(this.Config, this.Text);
            }
            config.Save();
        }
        private void SetText()
        {
            if (this.Config == null || this.PropertyName == null) { return; }
            PropertyInfo propertyInfo = typeof(Config).GetProperty(this.PropertyName);
            object property = propertyInfo.GetValue(this.Config);
            if (propertyInfo.PropertyType.IsArray)
            {
                this.Text = property == null ? string.Empty : string.Join(",", ((string[])property));
            }
            else
            {
                this.Text = property == null ? string.Empty : property.ToString();
            }
        }
    }
}
