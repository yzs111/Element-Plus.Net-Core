using AutoMapper;
using CW_ToyShopping.Common.Cache;
using CW_ToyShopping.Enity.UserModels;
using CW_ToyShopping.IRepository;
using CW_ToyShopping.IService;
using CW_ToyShopping.IService.PublicIService;
using CW_ToyShopping.IService.UserIService;
using CW_ToyShopping.Service.PublicService;
using CW_ToyShopping.Service.UserServices;
using Microsoft.AspNetCore.Identity;

namespace CW_ToyShopping.Service
{
    public class ServiceWrapper : IServiceWrapper
    {
        /// <summary>
        ///  封装好的仓储接口类
        /// </summary>
        private IRepositoryWrapper _repositoryWrapper { get; }
        /// <summary>
        /// 实体映射类
        /// </summary>
        private IMapper _mapper { get; }
        private ICache _cache { get; }

        #region Identity
        private UserManager<User> UserManager { get; }

        private RoleManager<Role> RoleManager { get; }
        #endregion


        #region Service
        UserService UserService = null;

        AuthorService authorService = null;

        RoleService RoleService = null;

        MenuService menuService = null;
        #endregion

        public ServiceWrapper(
            IRepositoryWrapper repositoryWrapper, 
            IMapper mapper, 
            ICache cache,
            UserManager<User> userManager,
            RoleManager<Role> roleManager)
        {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _cache = cache;
            UserManager = userManager;
            RoleManager = roleManager;
        }
        public IUserService IUserService => UserService ?? new UserService(UserManager, _mapper, _cache);

        public IRoleService IRoleService => RoleService ?? new RoleService(RoleManager, _mapper, _cache);

        public IAuthorService IAuthorService => authorService?? new AuthorService(_repositoryWrapper, _mapper, _cache);

        public IMenuService IMenuService => menuService?? new MenuService(_repositoryWrapper, _mapper, _cache);
    }
}
