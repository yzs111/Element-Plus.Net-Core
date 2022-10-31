using System;
using System.Collections.Generic;
using System.Text;

namespace CW_ToyShopping.Common.Configs
{
    public class AppConfig
    {
        /// <summary>
        /// 跨域地址，默认 http://*:9000
        /// </summary>
        public string[] CorUrls { get; set; }// = new[]{ "http://*:9000" };

        public SenparcSettingEntity SenparcSetting { get; set; }

        public SenparcWeixinSettingEntity SenparcWeixinSetting { get; set; }

    }
    public class SenparcSettingEntity
    {
        public bool IsDebug { get; set; }

        public string DefaultCacheNamespace { get; set; }
    }
    /// <summary>
    ///  企业微信详情信息
    /// </summary>
    public class SenparcWeixinSettingEntity
    {
        public string WeixinCorpId { get; set; }
        public string WeixinCorpAgentId { get; set; }
        public string WeixinCorpSecret { get; set; }
    }
}
