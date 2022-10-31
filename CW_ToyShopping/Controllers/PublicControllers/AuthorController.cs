using AutoMapper;
using CW_ToyShopping.Common.Helpers;
using CW_ToyShopping.Enity.PublicModels;
using CW_ToyShopping.IRepository;
using CW_ToyShopping.IService;
using CW_ToyShopping.IService.PublicIService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CW_ToyShopping.Controllers.PublicControllers
{

    public class AuthorController : WeatherForecastController
    {
        private IServiceWrapper _serviceWrapper { get; }

        public AuthorController(IServiceWrapper serviceWrapper)
        {
            _serviceWrapper = serviceWrapper;
        }
        /// <summary>
        /// 查询书籍数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult> GetAuthorsAsync()
        {
            return Ok(await _serviceWrapper.IAuthorService.GetAuthirList());
        }
        /// <summary>
        /// 修改书籍数据
        /// </summary>
        /// <param name="authirDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> UpdateAuthor([FromBody]AuthirDto authirDto)
        {
            return Ok(await _serviceWrapper.IAuthorService.UpdateAuthor(authirDto));
        }
        /// <summary>
        ///  导出Excel
        /// </summary>
        /// <returns></returns>
        [HttpPost]   
        public async Task<ActionResult> PrintExcel([FromBody] List<DicModel> dic)
        {

            Dictionary<string, string> Dic = new Dictionary<string, string>(); // 标题列

            for (int i = 0; i < dic.Count; i++) {

                Dic.Add(dic[i].Filden, dic[i].Dbcol);

            }

            var Items = await _serviceWrapper.IAuthorService.PrintAuthir(dic);


           
            ExcelHelper Npoi = new ExcelHelper();

            return File(Npoi.ToExcelByDataTable2007(Items, Dic), "application/vnd.ms-excel", "导出Excel" + DateTime.Now.ToString("yyyyMMdd") + ".xlsx");

        }
    }
}
