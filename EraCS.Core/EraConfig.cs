using Riey.Common.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace EraCS
{
    public abstract class EraConfig : LoadableConfig
    {
        protected readonly ConfigDic _configDic;

        protected EraConfig(ConfigDic configDic) { _configDic = configDic; }
        protected EraConfig(Stream configStream) { _configDic = new ConfigDic(true, true); _configDic.Load(configStream); }

        [LoadableProperty("LightGray", Tag = "ColorSetting")]
        public Color TextColor { get; private set; }

        [LoadableProperty("Black", Tag = "ColorSetting")]
        public Color BackColor { get; private set; }

        [LoadableProperty("20", Tag = "TextSetting")]
        public double TextSize { get; private set; }

        public void Save(Stream output)
        {
            _configDic.Save(output);
        }
    }
}
