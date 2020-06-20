namespace TodoApp.Migrations
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using TodoApp.Models;

    internal sealed class Configuration : DbMigrationsConfiguration<TodoApp.Models.TodoesContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
            AutomaticMigrationDataLossAllowed = true;
            ContextKey = "TodoApp.Models.TodoesContext";
        }

        protected override void Seed(TodoApp.Models.TodoesContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method
            //  to avoid creating duplicate seed data.

            User admin = new User()
            {
                Id = 1,
                UserName = "admin",
                Password = "password",
                Roles = new List<Role>()
            };

            ///下記処理は検証に使用していた為、不要
            //User kimura = new User()
            //{
            //    Id = 2,
            //    UserName = "kimura",
            //    Password = "password",
            //    Roles = new List<Role>()
            //};

            Role administrators = new Role()
            {
                Id = 1,
                RoleName = "Administrators",
                Users = new List<User>()
            };

            Role users = new Role()
            {
                Id = 2,
                RoleName = "Users",
                Users = new List<User>()
            };

            //adminのパスワードを取得する為にインスタンス生成しておく
            var membershipProvider = new CustomMembershipProvider();
            //adminのパスワードをハッシュ化し、取得する
            admin.Password = membershipProvider.GeneratePasswordHash(admin.UserName, admin.Password);

            admin.Roles.Add(administrators);
            administrators.Users.Add(admin);
            ///下記処理も検証に使用ていた為、不要
            //kimura.Roles.Add(users);
            //users.Users.Add(kimura);

            //下記の第二引数のkimuraを削除
            context.Users.AddOrUpdate(user => user.Id, new User[] { admin });
            context.Roles.AddOrUpdate(role => role.Id, new Role[] { administrators, users });
        }
    }
}
