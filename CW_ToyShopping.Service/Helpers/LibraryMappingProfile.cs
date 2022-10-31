using AutoMapper;
using CW_ToyShopping.Enity.PublicModels;
using CW_ToyShopping.Enity.UserModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace CW_ToyShopping.Service.Helpers
{
   public class LibraryMappingProfile:Profile
    {
        public LibraryMappingProfile()
        {

            CreateMap<Author, AuthirDto>();


            CreateMap<Book, BookDto>();

            CreateMap<AuthirDto, Author>();

            // CreateMap<ClaimsStore, UserClaimsViewModel>();

        }
    }
}
