using AutoMapper;
using CW_ToyShopping.Enity.UserModels;
using CW_ToyShopping.IService.UserIService;
using System.Threading.Tasks;
using System.Linq;
using CW_ToyShopping.Common.Cache;
using Microsoft.AspNetCore.Identity;
using CW_ToyShopping.Common.Helpers.Output;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using CW_ToyShopping.IRepository.UserlRepository;
using CW_ToyShopping.IRepository;
using CW_ToyShopping.Common.Helpers.Page;
using CW_ToyShopping.Enity.AdminModels.UserModels;

namespace CW_ToyShopping.Service.UserServices
{
   public class UserService: IUserService
    {
        private IMapper _mapper { get; }
        private ICache _cache { get; }
        private UserManager<User> UserManager { get; }

        private string[] CaCheKey = new string[] { };
        public UserService(UserManager<User> userManager,IMapper mapper, ICache cache)
        {
            UserManager = userManager;
            _mapper = mapper;
            _cache = cache;
        }
        public IResponseOutput GetUserList(PageInput<User> input)
        {
            //var CaceName = "GetUserData" + input.PageIndex;
            //CaCheKey.Append(CaceName);

            var UserName = input.Filter?.UserName;

             var users = UserManager.Users.ToList()
                .Skip(input.PageSize * (input.PageIndex - 1))
                .Take(input.PageSize)
                .OrderByDescending(x => x.CREATEDATE)
                .ToList();

            if (!string.IsNullOrWhiteSpace(UserName))
            {
                users = users.Where(x => x.UserName.ToLower().Contains(UserName.ToLower())).ToList();
            };

            var UserList = _mapper.Map<List<UserDto>>(users);

            #region 缓存时间设置
            MemoryCacheEntryOptions options = new MemoryCacheEntryOptions();
            options.AbsoluteExpiration = DateTime.Now.AddMinutes(720);
            options.Priority = CacheItemPriority.Normal;
            #endregion


            //if (!_cache.Exists(CaceName))
            //{
            //    _cache.Set(CaceName, UserList, options);
            //}

            var data = new PageOutput<UserDto>()
            {
                Total = UserManager.Users.Count(),
                List = UserList,
                PageIndex = input.PageIndex,
                PageSize = input.PageSize
            };
            return ResponseOutput.Ok(data);
        }

        public async Task<IResponseOutput> CreateUsers(UserDto registerUser)
        {
            // 初始化实例
            var user = new User();

            #region 给实体赋值
            user.SEX = registerUser.SEX;
            user.Email = registerUser.Email;
            user.UserName = registerUser.UserName;
            user.PhoneNumber = registerUser.PhoneNumber;
            user.PRACTIICALNAME = registerUser.PRACTIICALNAME;
            user.BRIEF = registerUser.BRIEF;
            user.NumberID = registerUser.NumberID;
            user.NATION = registerUser.NATION;
            user.OCCUPATION = registerUser.OCCUPATION;
            user.AGE = registerUser.AGE;
            user.AREA = registerUser.AREA;
            user.IMAGEURL = registerUser.IMAGEURL;
            user.COUNTRY = registerUser.COUNTRY;
            user.PROVINCE = registerUser.PROVINCE;
            user.CITY = registerUser.CITY;
            user.PASSWORD = registerUser.Password;
            user.TOWN = registerUser.TOWN;
            user.BIRTHDATE = registerUser.BIRTHDATE;
            user.CREATEPERSON = registerUser.CREATEPERSON;
            user.CREATEDATE = DateTime.Now;
            user.UPDATEPERSON = registerUser.UPDATEPERSON;
            user.UPDATEDATE = DateTime.Now;
            #endregion

            IdentityResult result = await UserManager.CreateAsync(user, registerUser.Password);
            
            if (result.Succeeded)
            {
                _cache.Del(CaCheKey);
                return ResponseOutput.Ok(user,"创建成功");
            }
            else
            {
                return ResponseOutput.NotOk($"Error:{result.Errors.FirstOrDefault()?.Description}");
            }
        }

        public async Task<IResponseOutput> EditUser(UserDto registerUser)
        {
            IdentityResult result = new IdentityResult();
            var user = await UserManager.FindByIdAsync(registerUser.UserId);
            if(user == null)
            {
                return ResponseOutput.NotOk($"无法找到ID为{registerUser.UserId}的用户");
            }

            #region 给实体赋值
            user.SEX = registerUser.SEX;
            user.NumberID = registerUser.NumberID;
            user.Email = registerUser.Email;
            user.UserName = registerUser.UserName;
            user.PhoneNumber = registerUser.PhoneNumber;
            user.PRACTIICALNAME = registerUser.PRACTIICALNAME;
            user.NATION = registerUser.NATION;
            user.OCCUPATION = registerUser.OCCUPATION;
            user.AGE = registerUser.AGE;
            user.AREA = registerUser.AREA;
            user.IMAGEURL = registerUser.IMAGEURL;
            user.COUNTRY = registerUser.COUNTRY;
            user.PROVINCE = registerUser.PROVINCE;
            user.BRIEF = registerUser.BRIEF;
            user.CITY = registerUser.CITY;
            user.TOWN = registerUser.TOWN;
            user.BIRTHDATE = registerUser.BIRTHDATE;
            user.CREATEPERSON = registerUser.CREATEPERSON;
            user.UPDATEPERSON = registerUser.UPDATEPERSON;
            user.UPDATEDATE = DateTime.Now;
            #endregion

            result = await UserManager.UpdateAsync(user);
            if (result.Succeeded)
            {
               
                _cache.Del(CaCheKey);
                return ResponseOutput.Ok(user,"修改成功");
            }
            else
            {
                return ResponseOutput.NotOk($"Error:{result.Errors.FirstOrDefault()?.Description}");
            } 
        }

        public async Task<IResponseOutput> DeteleUser(string id)
        {
            var user = await UserManager.FindByIdAsync(id);

            if(user == null)
            {
                return ResponseOutput.NotOk($"无法找到ID为{id}的用户");
            }
            IdentityResult result = await UserManager.DeleteAsync(user);
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
        public async Task<IResponseOutput> GetUserDetial(string UserId)
        {
            var user = await UserManager.FindByIdAsync(UserId);

            if (user == null)
            {
                return ResponseOutput.NotOk($"无法找到ID为{UserId}的用户");
            }
            var UserDto = _mapper.Map<UserDto>(user);
            UserDto.Password = user.PASSWORD;

            return ResponseOutput.Ok(UserDto, "查询成功");
        }

        public async Task<IResponseOutput> Updateload(string id,string Url)
        {
            var user = await UserManager.FindByIdAsync(id);
            if (user == null)
            {
                return ResponseOutput.NotOk($"无法找到ID为{id}的用户");
            }
            user.IMAGEURL = Url;
            var result = await UserManager.UpdateAsync(user);

            
            if (result.Succeeded)
            {

                _cache.Del(CaCheKey);
                return ResponseOutput.Ok(user, "图片上传成功");
            }
            else
            {
                return ResponseOutput.NotOk($"Error:{result.Errors.FirstOrDefault()?.Description}");
            }
        }

        public async Task<IResponseOutput> UpdatePassWord(ResetPasswordDto resetPasswordDto)
        {
            var user = await UserManager.FindByNameAsync(resetPasswordDto.UserName);
             
            if (user == null)
            {
                return ResponseOutput.NotOk($"无法找到{resetPasswordDto.UserName}的用户");
            }
            user.PASSWORD = resetPasswordDto.Password;
            var aa = await UserManager.GeneratePasswordResetTokenAsync(user);

            var result = await UserManager.ResetPasswordAsync(user, aa, resetPasswordDto.Password);
         
            if (result.Succeeded)
            {
                return ResponseOutput.Ok("密码修改成功");
            }
            else
            {
                return ResponseOutput.NotOk($"Error:{result.Errors.FirstOrDefault()?.Description}");
            }
        }

        public Task<IResponseOutput> WXSentMessage(string context)
        {
            throw new NotImplementedException();
        }
    }
}
