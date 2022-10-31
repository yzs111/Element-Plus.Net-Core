using AutoMapper;
using CW_ToyShopping.Enity.AdminModels.MenuModels;
using CW_ToyShopping.Enity.PublicModels;
using CW_ToyShopping.Enity.UserModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CW_ToyShopping.Service.Helpers
{
    public class UserMappingProfile: Profile
    {
        public UserMappingProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(d => d.UserId, o => o.MapFrom(s => s.Id));

            CreateMap<UserDto, User>()
                 .ForMember(d => d.Id, o => o.MapFrom(s => s.UserId));

            CreateMap<Role, RoleDto>()
                .ForMember(d => d.RoleId, o => o.MapFrom(s => s.Id));

            CreateMap<RoleDto, Role>()
                 .ForMember(d => d.Id, o => o.MapFrom(s => s.RoleId));

            CreateMap<Menu, MenuDto>();

            CreateMap<MenuDto, Menu>();

        }
    }
}
