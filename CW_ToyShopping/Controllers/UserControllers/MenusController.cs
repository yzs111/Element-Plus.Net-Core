using CW_ToyShopping.Enity.AdminModels.MenuModels;
using CW_ToyShopping.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CW_ToyShopping.Controllers.UserControllers
{
    public class MenusController : WeatherForecastController
    {
        private IServiceWrapper _repositoryWrapper { get; }
        public MenusController(IServiceWrapper repositoryWrapper)
        {
            _repositoryWrapper = repositoryWrapper;
        }
        /// <summary>
        ///  获取菜单左侧菜单数据
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetMenusList()
        {
            return Ok( await _repositoryWrapper.IMenuService.MenuDtos());
        }
        /// <summary>
        /// 创建权限菜单
        /// </summary>
        /// <param name="menuDto"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> CreateMenu([FromBody]MenuDto menuDto)
        {
            return Ok(await _repositoryWrapper.IMenuService.CreateMenu(menuDto));
        }
    }
}
