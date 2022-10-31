using AutoMapper;
using CW_ToyShopping.Common.Cache;
using CW_ToyShopping.Common.Helpers.Output;
using CW_ToyShopping.Common.Helpers.Page;
using CW_ToyShopping.Enity.UserModels;
using CW_ToyShopping.IService.UserIService;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CW_ToyShopping.Service.UserServices
{
   public class RoleService: IRoleService
    {
        private IMapper _mapper { get; }
        private ICache _cache { get; }
        private RoleManager<Role> RoleManager { get; }

        private string[] CaCheKey = new string[] { "GetRoleData" };
        public RoleService(RoleManager<Role> roleManager, IMapper mapper, ICache cache)
        {
            RoleManager = roleManager;
            _mapper = mapper;
            _cache = cache;
        }

        public IResponseOutput GetRoleList(PageInput<Role> input)
        {
            var RoleList = RoleManager.Roles
                .Skip(input.PageSize * (input.PageIndex - 1))
                .Take(input.PageSize)
                .OrderByDescending(x => x.CREATEDATE)
                .ToList();

            if (!string.IsNullOrWhiteSpace(input.Filter?.Name))
            {
                RoleList = RoleList.Where(x => x.Name.ToLower().Contains(input.Filter?.Name.ToLower())).ToList();
            };

            var RoleDot = _mapper.Map<List<RoleDto>>(RoleList);

            var data = new PageOutput<RoleDto>()
            {
                Total = RoleManager.Roles.Count(),
                List = RoleDot,
                PageIndex = input.PageIndex,
                PageSize = input.PageSize
            };
            return ResponseOutput.Ok(data, "查询成功");
        }

        public async Task<IResponseOutput> CreateRole(RoleDto roleDto)
        {
            IdentityResult identityResult = new IdentityResult();

            var role = new Role()
            {
                Name = roleDto.Name,
                ROLEDESC = roleDto.ROLEDESC,
                IsEnble = roleDto.IsEnble,
                CREATEDATE = DateTime.Now,
                CREATEPERSON = roleDto.CREATEPERSON,
                UPDATEDATE = DateTime.Now,
                UPDATEPERSON = roleDto.UPDATEPERSON
            };


            var FindRole = await RoleManager.FindByNameAsync(roleDto.Name);

            if(FindRole != null)
            {
                return ResponseOutput.NotOk("创建失败,该角色已经存在");
            }
            identityResult = await RoleManager.CreateAsync(role);

            if (identityResult.Succeeded)
            {
                _cache.Del(CaCheKey);
                return ResponseOutput.Ok("创建成功");
            }
            else
            {
                return ResponseOutput.NotOk($"Error:{identityResult.Errors.FirstOrDefault()?.Description}");
            }
        }

        public async Task<IResponseOutput> EditRole(RoleDto roleDto)
        {
            var role = await RoleManager.FindByIdAsync(roleDto.RoleId);

            if (role == null)
            {
                return ResponseOutput.NotOk($"无法找到ID为{roleDto.RoleId}的角色");
            }
            role.Name = roleDto.Name;
            role.ROLEDESC = roleDto.ROLEDESC;
            role.IsEnble = roleDto.IsEnble;
            role.UPDATEDATE = DateTime.Now;
            role.UPDATEPERSON = roleDto.UPDATEPERSON;

            IdentityResult result = await RoleManager.UpdateAsync(role);

            if (result.Succeeded)
            {
                _cache.Del(CaCheKey);
                return ResponseOutput.Ok("修改成功");
            }
            else
            {
                return ResponseOutput.NotOk($"Error:{result.Errors.FirstOrDefault()?.Description}");
            }
        }

        public async Task<IResponseOutput> DeleteRole(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);

            if (role == null)
            {
                return ResponseOutput.NotOk($"无法找到ID为{id}的角色");
            }
            if (role.Name == "Administrator")
            {
                return ResponseOutput.NotOk($"无法删除超级管理员角色");
            }

            IdentityResult result = await RoleManager.DeleteAsync(role);
            if (result.Succeeded)
            {
                _cache.Del(CaCheKey);
                return ResponseOutput.Ok("删除成功");
            }
            else
            {
                return ResponseOutput.NotOk($"Error:{result.Errors.FirstOrDefault()?.Description}");
            }
        }
        

        public async Task<IResponseOutput> GetDetial(string id)
        {
            var role = await RoleManager.FindByIdAsync(id);

            if (role == null)
            {
                return ResponseOutput.NotOk($"无法找到ID为{id}的角色");
            }
            var RoleDto = _mapper.Map<RoleDto>(role);

            return ResponseOutput.Ok(RoleDto,"查询成功");
        }
    }
}
