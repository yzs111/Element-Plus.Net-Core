using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using System.Drawing;
using System.Net;
using System.IO;
using Newtonsoft.Json.Linq;

namespace MVC.Areas.Basic_Areas.Models
{
    /// <summary>
    /// 企业微信返回的消息内容
    /// </summary>
    public class GetTokenResult
    {
        /// <summary>
        ///  错误编号
        /// </summary>
        public int errcode { get; set; }

        /// <summary>
        /// 错误消息
        /// </summary>
        public string errmsg { get; set; }

        /// <summary>
        /// Token
        /// </summary>
        public string access_token { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public int expires_in { get; set; }
    }

    /// <summary>
    /// 企业微信发送消息的基础消息内容
    /// </summary>
    public class CorpSendBase
    {
        /// <summary>
        /// UserID列表（消息接收者，多个接收者用‘|’分隔）。特殊情况：指定为@all，则向关注该企业应用的全部成员发送
        /// </summary>
        public string touser { get; set; }

        /// <summary>
        /// PartyID列表，多个接受者用‘|’分隔。当touser为@all时忽略本参数
        /// </summary>
        public string toparty { get; set; }

        /// <summary>
        /// TagID列表，多个接受者用‘|’分隔。当touser为@all时忽略本参数
        /// </summary>
        public string totag { get; set; }

        /// <summary>
        /// 消息类型
        /// </summary>
        public string msgtype { get; set; }

        /// <summary>
        /// 企业应用的id，整型。可在应用的设置页面查看
        /// </summary>
        public int agentid { get; set; }

        /// <summary>
        /// 表示是否是保密消息，0表示否，1表示是，默认0
        /// </summary>
        public string safe { get; set; }

        /// <summary>
        ///  构造函数
        /// </summary>
        public CorpSendBase()
        {
            this.agentid = GlobalContext.agent_id;
            this.safe = "0";
        }
    }

    #region 图片消息的类

    /// <summary>
    /// 图片类
    /// </summary>
    public class WXImage
    {
        /// <summary>
        /// 图片媒体文件id
        /// </summary>
        public string media_id { get; set; }
    }

    /// <summary>
    /// 发送图片消息的类
    /// </summary>
    public class CorpSendPicture : CorpSendBase
    {
        private WXImage _image;

        /// <summary>
        /// 要发送的文本，必须小写，企业微信API不识别大写。
        /// </summary>
        public WXImage image
        {
            get { return _image; }
            set { this._image = value; }
        }

        /// <summary>
        /// 赋值媒体Id
        /// </summary>
        /// <param name="media_id"></param>
        public CorpSendPicture(string media_id)
        {
            base.msgtype = "image";
            this.image = new WXImage
            {
                media_id = media_id
            };
        }
    }

    #endregion 图片消息的类

    #region 文本消息类

    public class Text
    {
        //文本内容
        public string content { get; set; }
    }

    /// <summary>
    /// 文本消息类
    /// </summary>
    public class CorpSendText : CorpSendBase
    {
        private Text _text;

        /// <summary>
        /// 要发送的文本，必须小写，企业微信API不识别大写。
        /// </summary>
        public Text text
        {
            get { return _text; }
            set { this._text = value; }
        }

        public CorpSendText(string content)
        {
            base.msgtype = "text";
            this.text = new Text
            {
                content = content
            };
        }
    }

    #endregion 文本消息类

    /// <summary>
    /// 调用企业微信接口发送消息的类
    /// </summary>
    public class GlobalContext
    {
        /// <summary>
        /// 初始化HttpClient实例，并设置请求头
        /// </summary>
        public static HttpClient HttpClientFactory { get; set; } = new HttpClient()
        {
            BaseAddress = new Uri("https://qyapi.weixin.qq.com/cgi-bin/"), // 设置请求头
        };

        /// <summary>
        /// 企业微信的企业ID
        /// </summary>
        public static string corpid = "wwc14f721b334992fb";

        /// <summary>
        /// 企业微信里面的建立的小程序的Secret
        /// </summary>
        public static string appsecret = "JqN_mIortNpYOwvWZ5UZggS0Xd_VucLOaI5CiyObp78";

        /// <summary>
        /// 企业微信应用ID
        /// </summary>
        public static int agent_id = 1000015;//企业微信里面的建立的小程序的APPID

        /// <summary>
        ///  发送消息的Url
        /// </summary>
        public static string messageSendURI = "message/send?access_token={0}";

        /// <summary>
        ///  请求ToKenUrl
        /// </summary>
        public static string getAccessTokenUrl = $"https://qyapi.weixin.qq.com/cgi-bin/gettoken?corpid={corpid}&corpsecret={appsecret}";

        /// <summary>
        /// 上传图片Url
        /// </summary>
        public static string UpdateLoad = "https://qyapi.weixin.qq.com/cgi-bin/media/upload?access_token={0}&type={1}";

        /// <summary>
        /// 过期时间
        /// </summary>
        public static DateTime TimeOutDate { get; set; }

        ///<summary>
        ///Token
        ///</summary>
        public static string Token { get; set; }




        ///<summary>
        ///获取Token
        ///</summary>
        public static Tuple<string, bool> GetToken
        {
            get
            {
                //判断Token是否存在 以及Token是否在有效期内
                if (string.IsNullOrEmpty(Token) || TimeOutDate > DateTime.Now)
                {
                    //构造请求链接
                    var requestBuild = getAccessTokenUrl;

                    var httpResponse = HttpClientFactory.GetAsync(requestBuild).Result;

                    //请求数据转换成实体
                    var dynamic = JsonConvert.DeserializeObject<GetTokenResult>(
                        httpResponse.Content.ReadAsStringAsync().Result
                    );

                    if (dynamic.errcode == 0)
                    {
                        Token = dynamic.access_token;

                        //过期5分钟前刷新Token
                        var expires_in = Convert.ToDouble(dynamic.expires_in - 5 * 60);

                        TimeOutDate = DateTime.Now.AddSeconds(expires_in);

                        return Tuple.Create(Token, true);
                    }
                    else
                    {
                        return Tuple.Create($"获取Token失败，错误： { dynamic.errmsg} ", false);
                    }
                }
                else
                {
                    return Tuple.Create(Token, true);
                }
            }
        }

        /// <summary>
        /// 推送消息
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public static GetTokenResult WxPush(string content)
        {
            //构造请求链接
            var requestBuild = messageSendURI;

            var token = GetToken.Item1; //获取token

            var issuccess = GetToken.Item2; //判断token是否获取成功

            if (!issuccess)
            {
                throw new Exception(token);
            }

            //企业微信发送完整的URL
            requestBuild = string.Format(requestBuild, token);

            //建立HttpClient
            byte[] data = Encoding.UTF8.GetBytes(content);

            //初始化一个新的实例系统的网络
            var bytearray = new ByteArrayContent(data);

            //将POST请求作为异步操作发送到指定的Uri
            var httpResponse = HttpClientFactory.PostAsync(requestBuild, bytearray).Result;

            var dynamic = JsonConvert.DeserializeObject<dynamic>(
                 httpResponse.Content.ReadAsStringAsync().Result
                );

            if (dynamic.errcode == 0)
            {
                return new GetTokenResult() { errmsg = "消息推送成功,系统已经向企业微信发送了一条消息提醒", errcode = 0 };
            }
            if (dynamic.errcode == 81013)
            {
                return new GetTokenResult() { errmsg = "消息推送失败,原因: 当前审核节点人的微信账号不在应用的可见范围中", errcode = 81013 };
            }
            if (dynamic.ercode == 60020)
            {
                return new GetTokenResult() { errmsg = "消息推送失败,原因: 当前网络的IP并不是当前应用的可信IP", errcode = 60020 };
            }
            else
            {
                return new GetTokenResult() { errmsg = $"消息推送失败,原因: {JsonConvert.SerializeObject(dynamic)}", errcode = dynamic.ercode };
            }
        }

        /// <summary>
        /// 获取文本消息格式
        /// </summary>
        /// <param name="userId">企业微信用户ID</param>
        /// <param name="msg">消息内容</param>
        /// <returns></returns>
        public static string GetContent(string userId, string msg)
        {
            CorpSendText corpSendText = new CorpSendText(msg);

            corpSendText.touser = userId;

            string strJson = JsonConvert.SerializeObject(corpSendText);//将对象转换成json字符串

            return strJson;
        }

        /// <summary>
        /// 获取图片消息格式
        /// </summary>
        /// <param name="empName"></param>
        /// <param name="media_id"></param>
        /// <returns></returns>
        public static string SendPicture(string empName, string media_id)
        {
            string res = "";

            CorpSendPicture paramData = new CorpSendPicture(media_id);

            paramData.touser = empName;

            res = JsonConvert.SerializeObject(paramData);

            return res;
        }

        /// <summary>
        /// 上传图片后获取media_id
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string Upload_Pic(string path)
        {
            // 媒体id
            string media_id = "";
            // 上传图片的URL
            string urlFormat = UpdateLoad;

            var token = GetToken.Item1; // 获取token

            var issuccess = GetToken.Item2; // token是否获取成功

            if (!issuccess)
            {
                throw new Exception(token);
            }
            var url = string.Format(urlFormat, token, "image");

            string sendResult = DoPostFile(url, path);

            if (sendResult.Length > 0)
            {
                //从包含JSON的字符串中提取
                JObject jo = JObject.Parse(sendResult);
                media_id = jo.SelectToken("media_id").ToString();
            }
            return media_id; ;
        }

        /// <summary>
        /// 上传图片,企业微信默认一个月只能上传3000张图片,一天最多只能传1000张图片
        /// </summary>
        /// <returns></returns>
        public static string DoPostFile(string url, string path)
        {
            try
            {
                string boundary = DateTime.Now.Ticks.ToString("X"); // 随机分隔线
                string startboundary = "--" + boundary;
                string endboundary = "--" + boundary + "--";

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(new Uri(url));
                req.Method = "POST";
                req.ContentType = "multipart/form-data;boundary=" + boundary;
                Stream reqStream = req.GetRequestStream();
                //开始结束的换行符不能少，否则是44001,"errmsg":"empty media data,
                byte[] endBoundaryBytes = Encoding.UTF8.GetBytes("\r\n" + endboundary + "\r\n");
                string name = "11"; // 文件
                string filename = "11"; // 文件名称
                //结束的两个换行符不能少，否则是44001,"errmsg":"empty media data,
                string fileTemplate = "Content-Disposition: form-data; name=\"{0}\";filename=\"{1}\"; filelength={2}\r\nContent-Type: {3}\r\n\r\n";
                byte[] fileBytes = getImageByte(path);
                StringBuilder sb = new StringBuilder();
                sb.Append(startboundary);
                sb.Append("\r\n");
                sb.Append(string.Format(fileTemplate,
                    name,
                    filename,
                    //字节数
                    fileBytes.Length,
                    // 返回指定的文件名的 MIME 映射。
                    MimeMapping.MimeUtility.GetMimeMapping(path)));
                byte[] Content = Encoding.UTF8.GetBytes(sb.ToString());
                //开始标志
                reqStream.Write(Content, 0, Content.Length);
                //文件内容
                reqStream.Write(fileBytes, 0, fileBytes.Length);
                //结束标志
                reqStream.Write(endBoundaryBytes, 0, endBoundaryBytes.Length);
                reqStream.Close();
                HttpWebResponse rsp = (HttpWebResponse)req.GetResponse();
                StreamReader sr = new StreamReader(rsp.GetResponseStream(), Encoding.Default);
                string ret = sr.ReadToEnd();
                sr.Close();
                return ret;
            }
            catch (Exception ex)
            {              
                return ex.Message;
            }
        }

        /// <summary>
        /// //获取图片的字节
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        private static byte[] getImageByte(string imagePath)
        {
            FileStream files = new FileStream(imagePath, FileMode.Open);
            byte[] imgByte = new byte[files.Length];
            files.Read(imgByte, 0, imgByte.Length);
            files.Close();
            return imgByte;
        }
    }
}