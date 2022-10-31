using CW_ToyShopping.Common.Helpers;
using CW_ToyShopping.Common.Helpers.Output;
using CW_ToyShopping.Common.Helpers.Page;
using CW_ToyShopping.Enity.AdminModels.UserModels;
using CW_ToyShopping.Enity.UserModels;
using CW_ToyShopping.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MVC.Areas.Basic_Areas.Models;
using System;
using System.Collections;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CW_ToyShopping.Controllers.UserControllers
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public class AdminController : WeatherForecastController
    {
        private IServiceWrapper UserService { get; }
        public AdminController(IServiceWrapper userService)
        {
            UserService = userService;
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <returns></returns>
        /// <param name="input">查询参数</param>
        [HttpPost]
        public IActionResult ListUsers(PageInput<User> input)
        {   
            return Ok(UserService.IUserService.GetUserList(input));
        }

      /// <summary>
      /// 新增用户
      /// </summary>
      /// <param name="registerUser">请输入新增用户参数</param>
      /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> AddUserAsync([FromBody] UserDto registerUser)
        {
            return Ok(await UserService.IUserService.CreateUsers(registerUser));
        }

        /// <summary>
        ///  修改用户信息
        /// </summary>
        /// <param name="registerUser">请输入修改的参数</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> EditUser([FromBody] UserDto registerUser)
        {
            return Ok(await UserService.IUserService.EditUser(registerUser));
        }
        /// <summary>
        ///  删除用户信息
        /// </summary>
        /// <param name="id">用户id</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DeteleUser([FromQuery] string id)
        {
            return Ok(await UserService.IUserService.DeteleUser(id));
        }
        /// <summary>
        /// 获取用户详情
        /// </summary>
        /// <param name="UserId">用户Id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetUserDetial([FromQuery] string UserId)
        {
            return Ok(await UserService.IUserService.GetUserDetial(UserId));
        }
        /// <summary>
        ///  修改用户密码
        /// </summary>
        /// <param name="resetPasswordDto">参数</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> UpdatePassWord([FromBody] ResetPasswordDto resetPasswordDto)
        {
            return Ok(await UserService.IUserService.UpdatePassWord(resetPasswordDto));
        }
        /// <summary>
        /// 用户头像上传
        /// </summary>
        /// <param name="photo"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Upload([FromForm] PhotoDto photo)
        {
            //string dd = photo.Files["File"];
            //var form = photo.Files;//定义接收类型的参数
            var new_path = string.Empty;
            Hashtable hash = new Hashtable();

            string BaseUrl = Directory.GetCurrentDirectory();

            IFormFileCollection cols = Request.Form.Files;
            if (cols == null || cols.Count == 0)
            {
                return Ok(ResponseOutput.NotOk("图片上传失败,传入的图片格式有误"));
            }
            foreach (IFormFile file in cols)
            {
                //定义图片数组后缀格式
                string[] LimitPictureType = { ".JPG", ".JPEG", ".GIF", ".PNG", ".BMP" };
                //获取图片后缀是否存在数组中
                string currentPictureExtension = Path.GetExtension(file.FileName).ToUpper();
                if (LimitPictureType.Contains(currentPictureExtension))
                {
                    //new_path = Path.Combine(Directory.GetCurrentDirectory(), "Images", file.FileName);
                    new_path = Path.Combine(BaseUrl, "Images", file.FileName);
                    using (var stream = new FileStream(new_path, FileMode.Create))
                    {
                        //再把文件保存的文件夹中
                        file.CopyTo(stream);
                        hash.Add("file", new_path);
                    }

                    string val = "https:"+"//localhost:5001"  + "/Upload/" + file.FileName;
                    // 对外资源访问路径
                    return Ok(await UserService.IUserService.Updateload(photo.EntId, val));                   
                }
            }
            return Ok(ResponseOutput.NotOk("图片上传失败,请上传指定格式的图片"));
        }
        /// <summary>
        ///  向企业微信发送消息
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
         [HttpPost]
        public IActionResult SentText(string content)
        {
            string userid = "19177220673";

            var content1 = GlobalContext.GetContent(userid, content);

            return Ok(GlobalContext.WxPush(content1));
        }

        [HttpPost]
        public ActionResult UploadFile([FromForm] PhotoDto photo)
        {
            AjaxMsgModel result;
            try
            {
                string BaseUrl = Directory.GetCurrentDirectory();
                IFormFileCollection cols = Request.Form.Files;
                string imgName = string.Empty;
                string SaveDir = string.Empty;
                string FileName = string.Empty;
                string v = string.Empty;
                if (cols == null || cols.Count == 0)
                {
                    return Ok(ResponseOutput.NotOk("文件上传失败"));
                }
                foreach (IFormFile file in cols)
                {
                    //获取图片后缀是否存在数组中
                    string FileExt = Path.GetExtension(file.FileName).ToUpper();

                    v = Guid.NewGuid().ToString("N");//生成文件名
                    FileName = v + FileExt;//表格重命名
                    imgName = v + ".jpg";//去版权图片名称
                    SaveDir = BaseUrl + "/Excel/"; // 存放Excel的文件夹
                    if (!Directory.Exists(SaveDir))//判断指定路径是否存在文件夹
                    {
                        Directory.CreateDirectory(SaveDir);//不存在文件夹，尝试创建。
                    }
                    string new_path = Path.Combine(SaveDir, FileName);
                    using (var stream = new FileStream(new_path, FileMode.Create))
                    {
                        //再把文件保存的文件夹中
                        file.CopyTo(stream);
                    }
                }
                int ATTIDout = 0;

                CountdownEvent latch = new CountdownEvent(1);

                Thread ExcelToImages = new Thread(new ThreadStart(delegate
                {
                    Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();
                    Microsoft.Office.Interop.Excel.Workbook wkb = excel.Workbooks.Open(SaveDir + FileName);//打开指定路径的表
                    Microsoft.Office.Interop.Excel.Sheets ss = wkb.Sheets;//获取工作簿集合
                    foreach (Microsoft.Office.Interop.Excel.Worksheet sheet in wkb.Sheets)//遍历所有工作簿
                    {
                        sheet.Activate();//激活工作簿
                        sheet.Application.ActiveWindow.View = Microsoft.Office.Interop.Excel.XlWindowView.xlNormalView;//设置为普通视图
                        Microsoft.Office.Interop.Excel.Range range = sheet.Range["A1", "G14"];//截取指定区域
                        range.CopyPicture(Microsoft.Office.Interop.Excel.XlPictureAppearance.xlScreen, Microsoft.Office.Interop.Excel.XlCopyPictureFormat.xlBitmap);//按其屏幕显示进行复制，位图格式

                        //bool isjpg = Clipboard.ContainsData(DataFormats.Bitmap);//判断剪贴板是否有指定格式的数据存在
                        //if (isjpg)
                        //{
                        //    Image testjpg = Clipboard.GetImage();//获取剪贴板的图像
                        //    string imgnamev = v + FileName + ".jpg";//重命名+序号
                        //    testjpg.Save(SaveDir + imgnamev, ImageFormat.Jpeg);//保存文件到指定路径并指明图片格式
                        //    testjpg.Dispose();
                        //}
                        //wkb.Close();
                        //excel.Quit();
                        //KillProgram();

                        //latch.Signal();
                    }
                }));
                //ExcelToImages.SetApartmentState(ApartmentState.STA);//需要使用STA单线程执行，否则获取不到剪贴板数据
                ExcelToImages.Start();//启动线程
                latch.Wait();

                result = new AjaxMsgModel
                {
                    Data = new
                    {
                        id = ATTIDout,//附件ID
                        path = "/Upload/Excel/" + imgName,//图片路径
                        downloadPath = "/Upload/Excel/" + FileName//表格路径
                    },
                    Msg = "上传成功！",
                    Statu = AjaxStatu.ok
                };

            }
            catch (Exception ex)
            {
                return Ok(ex.Message);
            }
            return Ok(result);
        }
        /// <summary>
        /// 结束打开的Excel进程
        /// </summary>
        public static void KillProgram()
        {
            foreach (Process process in Process.GetProcesses())
            {
                if (process.ProcessName == "EXCEL") //要结束程序的名称
                {
                    process.Kill();
                }
            }
        }
    }
}
