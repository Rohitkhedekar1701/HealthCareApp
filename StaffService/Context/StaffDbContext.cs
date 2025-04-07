using Microsoft.EntityFrameworkCore;

using StaffService.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace StaffService.Context
{
    public class StaffDbContext : DbContext 
    {
        public StaffDbContext(DbContextOptions<StaffDbContext> options): base(options) { }

        public DbSet<Staff> Staffs => Set<Staff>(); 
    }
}
