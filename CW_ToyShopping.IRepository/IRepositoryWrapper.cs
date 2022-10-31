using CW_ToyShopping.IRepository.PublicIRepository;
using CW_ToyShopping.IRepository.UserlRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace CW_ToyShopping.IRepository
{
   public interface IRepositoryWrapper
    {
        IBookRepository book { get; }

        IAuthorRepository Author{ get; }

        IMenuepository Menu { get; }
    }
}
