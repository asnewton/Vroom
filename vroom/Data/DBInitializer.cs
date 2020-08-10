using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using vroom.AppDbContext;
using vroom.Helpers;
using vroom.Models;

namespace vroom.Data
{
    public class DBInitializer : IDBInitializer
    {

        public DBInitializer(VroomDbContext db,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _db = db;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        private readonly VroomDbContext _db;
        private readonly UserManager<IdentityUser> _userManager;
        public readonly RoleManager<IdentityRole> _roleManager;

        public void Initialize()
        {
            // if migration exists then update database
            if (_db.Database.GetPendingMigrations().Count() > 0)
            {
                _db.Database.Migrate();
            }

            // Exit if Admin role already exists
            if (_db.Roles.Any(x => x.Name == Roles.Admin)) return;

            // Create Admin role
            _roleManager.CreateAsync(new IdentityRole(Roles.Admin)).GetAwaiter().GetResult();

            // Create default Admin user
            _userManager.CreateAsync(new ApplicationUser()
            {
                UserName = "Admin",
                Email = "admin@gmail.com",
                EmailConfirmed = true
            }, "Admin@123").GetAwaiter().GetResult();

            // Assign role to admin user
            _userManager.AddToRoleAsync(_userManager.FindByNameAsync("Admin").GetAwaiter().GetResult(),
                Roles.Admin).GetAwaiter().GetResult();
        }
    }
}
