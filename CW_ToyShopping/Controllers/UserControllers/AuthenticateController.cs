using CW_ToyShopping.Common.Configs;
using CW_ToyShopping.Common.Helpers;
using CW_ToyShopping.Common.Helpers.Output;
using CW_ToyShopping.Enity.UserModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CW_ToyShopping.Controllers.UserControllers
{
    public class AuthenticateController : WeatherForecastController
    {
        private ConfigHelper _configHelper { get; }
        private  IHostEnvironment _env { get; }
        private Jwtconfig _jwtconfig { get; }

        public IConfiguration Configuration { get; }

        public RoleManager<Role> RoleManager { get; }

        public UserManager<User> UserManager { get; }

        public AuthenticateController(Jwtconfig jwtconfig,
            UserManager<User> userManager,
            RoleManager<Role> roleManager,
            IConfiguration configuration)
        {
            _configHelper = new ConfigHelper();
            _jwtconfig = jwtconfig;
            Configuration = configuration;
            RoleManager = roleManager;
            UserManager = userManager;
        }

        /// <summary>
        /// 输入用户名和密码,获取Token
        /// </summary>
        /// <param name="loginUser">请输入用户名密码</param>
        /// <returns></returns>
       [AllowAnonymous]
       [HttpPost]
        public async Task<IActionResult> GenerateToken([FromBody]loginUser loginUser)
        {
            //注意:推荐使用identityOptions。用户。使用此方法时，RequireUni queEmail应设置为true，否则如果用户有重复的电子邮件，商店可能会抛出。
            var user = await UserManager.FindByNameAsync(loginUser.UserName);
         
            if (user == null)
            {
                return NotFound(ResponseOutput.NotOk("您输入的用户名不存在"));
            }
 
            UserDto registerUser = new UserDto()
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PRACTIICALNAME = user.PRACTIICALNAME,
                SEX = user.SEX,
                PhoneNumber = user.PhoneNumber,
                BIRTHDATE = user.BIRTHDATE,
                NumberID = user.NumberID
            };

            var result = UserManager.PasswordHasher.VerifyHashedPassword(user, user.PasswordHash, loginUser.PassWord);

            if (result != PasswordVerificationResult.Success)
            {
                return Unauthorized(ResponseOutput.NotOk("您输入的密码不正确"));
            }

            var userClaims = await UserManager.GetClaimsAsync(user);
            var userRoles = await UserManager.GetRolesAsync(user);

            foreach(var roleItem in userRoles)
            {
                userClaims.Add(new Claim(ClaimTypes.Role, roleItem));
            }

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,loginUser.UserName),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email,user.Email)
            };

            claims.AddRange(userClaims);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtconfig.Key));
            var signCredentia = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwtToken = new JwtSecurityToken(
                issuer: _jwtconfig.Issuer,
                audience: _jwtconfig.Audienec,
                claims: claims,
                expires:DateTime.Now.AddMinutes(720),
                signingCredentials:signCredentia
                );

            return Ok(
                new
                {
                    meta = ResponseOutput.Ok(registerUser,"登录成功"),
                    token = new JwtSecurityTokenHandler().WriteToken(jwtToken),
                    expiration = TimeZoneInfo.ConvertTimeFromUtc(jwtToken.ValidTo,TimeZoneInfo.Local)
                });
        }

        /// <summary>
        ///  获取角色申明
        /// </summary>
        /// <param name="RoleId">角色Id</param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ManageUserClaims(string RoleId)
        {
            var role = await RoleManager.FindByIdAsync(RoleId);

            if(role == null)
            {
                return NotFound(ResponseOutput.NotOk($"无法找到ID为{RoleId}的角色"));
            }
            // UserManager服务中的GetClaimsAsync方法获取当前用户的所有声明
            var existingUserClaims = await RoleManager.GetClaimsAsync(role);

            var model = new UserClaimsViewModel
            {
                RoleId = RoleId
            };

            foreach(Claim claim in ClaimsStore.RoleClaims)
            {
                UserClaim userClaim = new UserClaim
                {
                    ClaimType = claim.Type,
                    ClaimValue = claim.Value
                };
                if (existingUserClaims.Any(c => c.Type == claim.Type))
                {
                    userClaim.IsSelected = true;
                }
                model.Cliams.Add(userClaim);
            }
            return Ok(ResponseOutput.Ok(model, "获取成功"));
        }

        /// <summary>
        ///  添加角色声明
        /// </summary>
        /// <param name="model">请输入添加的参数</param>
        /// <returns></returns>
        [HttpPost]
       public async Task<IActionResult> ManageUserClaims(UserClaimsViewModel model)
       {
            var role = await RoleManager.FindByIdAsync(model.RoleId);
            IdentityResult result = new IdentityResult();
            if (role == null)
            {
                return NotFound(ResponseOutput.NotOk($"无法找到ID为{model.RoleId}的角色"));
            }
            // 获取所有用户现有的声明并删除它们
            var claims = await RoleManager.GetClaimsAsync(role);

            for(int i = 0; i < claims.Count; i++)
            {
                 result = await RoleManager.RemoveClaimAsync(role, claims[i]);
                if (!result.Succeeded)
                {
                    return NotFound($"Error:{result.Errors.FirstOrDefault()?.Description}");
                }
                // 添加页面上选择的所有声明信息            
            }
           for(int i=0;i< model.Cliams.Count; i++)
            {
                if (model.Cliams[i].IsSelected)
                {
                   var cliam = new Claim(model.Cliams[i].ClaimType, model.Cliams[i].ClaimValue);

                    result = await RoleManager.AddClaimAsync(role, cliam);

                }
            }
            if (!result.Succeeded)
            {
                return NotFound($"Error:{result.Errors.FirstOrDefault()?.Description}");
            }
            return Ok(ResponseOutput.Ok("添加成功"));
       }

        /// <summary>
        ///  用户添加角色
        /// </summary>
        /// <param name="userid"></param>
        /// <param name="roleid"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> AddUserToRoleAsync(string userid,string roleid)
        {
            if(string.IsNullOrWhiteSpace(userid) || string.IsNullOrWhiteSpace(roleid))
            {
                return NotFound("参数不能为空");
            }
            var role = await RoleManager.FindByIdAsync(roleid);
            var user = await UserManager.FindByIdAsync(userid);

            if(user == null)
            {
                return NotFound(ResponseOutput.NotOk($"无法找到ID为{userid}的用户"));
            }
            if (role==null)
            {
                return NotFound(ResponseOutput.NotOk($"无法找到ID为{roleid}的角色"));
            }          
           var result = await UserManager.AddToRoleAsync(user, role.Name);

            if (!result.Succeeded)
            {
                return NotFound($"Error:{result.Errors.FirstOrDefault()?.Description}");

            }
            return Ok(ResponseOutput.Ok("添加成功"));
        }
    }
}
