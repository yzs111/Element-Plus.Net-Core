using CW_ToyShopping.Common.Helpers.Page;
using CW_ToyShopping.Enity.UserModels;
using CW_ToyShopping.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CW_ToyShopping.Controllers.UserControllers
{
    /// <summary>
    ///  角色管理
    /// </summary>
    public class RolesController : WeatherForecastController
    {
        private IServiceWrapper RoleService { get; }
        public RolesController(IServiceWrapper roleService)
        {
            RoleService = roleService;
        }
        /// <summary>
        /// 获取角色信息
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public IActionResult ListRoles([FromBody]PageInput<Role> input)
        {
            return Ok(RoleService.IRoleService.GetRoleList(input));
        }
        /// <summary>
        /// 新增角色
        /// </summary>
        /// <param name="registerUser">请输入新增角色参数</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddRoleAsync([FromBody] RoleDto registerUser)
        {
            return Ok(await RoleService.IRoleService.CreateRole(registerUser));
        }
        /// <summary>
        ///  修改角色信息
        /// </summary>
        /// <param name="id">用户id</param>
        /// <param name="registerUser">请输入修改角色的参数</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> EditRole([FromBody] RoleDto registerUser)
        {
            return Ok(await RoleService.IRoleService.EditRole(registerUser));
        }
        /// <summary>
        ///  删除角色信息
        /// </summary>
        /// <param name="id">角色id</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> DeteleRole([FromQuery] string id)
        {
            return Ok(await RoleService.IRoleService.DeleteRole(id));
        }

        /// <summary>
        ///  获取角色详情
        /// </summary>
        /// <param name="id">角色Id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> GetRoleDetial(string id)
        {
            return Ok(await RoleService.IRoleService.GetDetial(id));
        }
    }
}
