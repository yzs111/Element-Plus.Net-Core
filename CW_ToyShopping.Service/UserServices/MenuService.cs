using AutoMapper;
using CW_ToyShopping.Common.Cache;
using CW_ToyShopping.Common.Helpers;
using CW_ToyShopping.Common.Helpers.Output;
using CW_ToyShopping.Enity.AdminModels.MenuModels;
using CW_ToyShopping.IRepository;
using CW_ToyShopping.IRepository.UserlRepository;
using CW_ToyShopping.IService.UserIService;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace CW_ToyShopping.Service.UserServices
{
    public class MenuService : IMenuService
    {
        private IRepositoryWrapper _menuepository { get; }
        private IMapper _mapper { get; }
        private ICache _cache { get; }
        public MenuService(IRepositoryWrapper menuepository, IMapper mapper, ICache cache)
        {
            _menuepository = menuepository;
            _mapper = mapper;
            _cache = cache;
        }
        public async Task<IResponseOutput> MenuDtos()
        {
            var menu = await _menuepository.Menu.GetAllAsync();

            var menuDtos = _mapper.Map<List<MenuDto>>(menu);

            //List<MenuDto> menuDtos = DTreeJSONHelper.GetTypeOfWorkforTree(menu, new List<MenuDto>(), 0);

            var TreeList =  DTreeJSONHelper.CreateTree<MenuDto>(menuDtos, item => item.MENUID, item => item.PID);

            return ResponseOutput.Ok(TreeList, "查询成功");
        }

        public async Task<IResponseOutput> CreateMenu(MenuDto menuDto)
        {
           

            var menu = _mapper.Map<Menu>(menuDto);

            _menuepository.Menu.Create(menu);

            bool IsEnalb =  await _menuepository.Menu.SaveAsync();

            if (IsEnalb)
            {
                return ResponseOutput.Ok("创建成功");
            }
            return ResponseOutput.NotOk("创建失败");
        }

        public async Task<IResponseOutput> UpdateMenu(MenuDto menuDto)
        {
            var menu = _mapper.Map<Menu>(menuDto);

            _menuepository.Menu.Update(menu);

            bool IsEnalb = await _menuepository.Menu.SaveAsync();

            if (IsEnalb)
            {
                return ResponseOutput.Ok("修改成功");
            }
            return ResponseOutput.NotOk("修改失败");
        }

        public async Task<IResponseOutput> DeteleMenu(int MenuId)
        {
            var menu = await _menuepository.Menu.GetAllAsync();

            menu = menu.Where(x => x.MENUID == MenuId || x.PID == MenuId).ToList();

            _menuepository.Menu.DeleteList(menu);

            bool IsEnalb = await _menuepository.Menu.SaveAsync();

            if (IsEnalb)
            {
                return ResponseOutput.Ok("删除成功");
            }
            return ResponseOutput.NotOk("删除失败");
        }
    }
}
