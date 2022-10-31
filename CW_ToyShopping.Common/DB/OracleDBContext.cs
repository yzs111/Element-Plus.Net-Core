using CW_ToyShopping.Enity.AdminModels.MenuModels;
using CW_ToyShopping.Enity.PublicModels;
using CW_ToyShopping.Enity.UserModels;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CW_ToyShopping.DB
{
    public class OracleDBContext: IdentityDbContext<User, Role,string>
    {
        public OracleDBContext(DbContextOptions<OracleDBContext> options) : base(options)
        {

        }
        public virtual DbSet<Author> Author { get; set; }

        public virtual DbSet<Book> Book { get; set; }

        public virtual DbSet<Menu> Menus { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*#region 为索引添加序列
             modelBuilder.Entity<Menu>(entity =>
              {
                  //entity.ToTable("SYS_MENU");

                 entity.Property(o => o.MENUID).ForOracleUseSequenceHiLo("SQ_SYS_MENU");
              });
            #endregion

            if (this.Database.IsOracle())
            {
                modelBuilder.HasDefaultSchema("YZS");
            }
            */
            base.OnModelCreating(modelBuilder);
        }
    }
}
