using CW_ToyShopping.DB;
using CW_ToyShopping.IRepository;
using CW_ToyShopping.IRepository.PublicIRepository;
using CW_ToyShopping.IRepository.UserlRepository;
using CW_ToyShopping.Repository.PublicRepository;
using CW_ToyShopping.Repository.UserRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace CW_ToyShopping.Repository
{
   public class RepositoryWrapper: IRepositoryWrapper
    {
        private IAuthorRepository authorRepository = null;

        private IBookRepository bookRepository = null;

        private IMenuepository menuRepository = null;

        private OracleDBContext _mysqlDBContext { get; }
        public RepositoryWrapper(OracleDBContext mysqlDBContext)
        {
            _mysqlDBContext = mysqlDBContext;
        }
        public IAuthorRepository Author => authorRepository ?? new AuthorRepository(_mysqlDBContext);
        public IBookRepository book => bookRepository ?? new BookRepository(_mysqlDBContext);
        public IMenuepository Menu => menuRepository ?? new Menuepository(_mysqlDBContext);
    }
}
