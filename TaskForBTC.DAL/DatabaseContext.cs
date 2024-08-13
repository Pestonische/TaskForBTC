using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using TaskForBTC.Repositories.Items;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace TaskForBTC.DAL
{   //Класс подключения и развертывания датасетов.
    public class DatabaseContext : IdentityDbContext<UserItem>
    {
        //Датасет событий.
        public DbSet<EventItem> EventItems { get; set; }
        //Датасет пользователей.
        public DbSet<UserItem> UserItems { get; set; }
        public DatabaseContext(DbContextOptions<DatabaseContext> options, IHttpContextAccessor httpContextAccessor) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(EventItem).Assembly);
            //Добавление проверки, чтобы количество пользователей в бд не превышало максимального указанного значения.
            modelBuilder.Entity<EventItem>(entity => entity.HasCheckConstraint("ValidNunberOfUsers", "NumberOfUsers <= MaxNumberOfUsers"));
            
        }
    }
}
