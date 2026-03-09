using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nutrilife.DataAccessLayer.utilities
{
    public class RoleSeedData : ISeedData
    {

        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleSeedData(RoleManager<IdentityRole> roleManager) 
        {
            _roleManager = roleManager;
        }

        async Task ISeedData.DataSeed()
        {
            string[] roles = ["User", "Nutritionist", "Admin"];

            if (!await _roleManager.Roles.AnyAsync()) // اذا في قيمة وحدة عالاقل ترجع ترو
            {
                foreach (var role in roles)
                {
                    await _roleManager.CreateAsync(new IdentityRole(role)); // لما نرفع الكود على سيرفر. في اول مرة بتم تشغيله فيها رح يضيف هاي الرولز على الداتا بيس
                }
            }


            
        }
    }
}
