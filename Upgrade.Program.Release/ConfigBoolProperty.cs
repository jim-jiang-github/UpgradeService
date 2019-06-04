using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Upgrade.Program.Release
{
    public class ConfigBoolProperty : CheckBox
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
                this.SetChecked();
            }
        }
        public string PropertyName
        {
            get => this.propertyName;
            set
            {
                this.propertyName = value;
                this.SetChecked();
            }
        }
        #endregion
        protected override void OnCheckedChanged(EventArgs e)
        {
            base.OnCheckedChanged(e);
            PropertyInfo propertyInfo = typeof(Config).GetProperty(this.PropertyName);
            propertyInfo.SetValue(this.Config, this.Checked);
            config.Save();
        }
        private void SetChecked()
        {
            if (this.Config == null || this.PropertyName == null) { return; }
            PropertyInfo propertyInfo = typeof(Config).GetProperty(this.PropertyName);
            this.Checked = (bool)propertyInfo.GetValue(this.Config);
        }
    }
}
