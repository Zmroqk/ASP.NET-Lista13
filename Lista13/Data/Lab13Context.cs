using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lista13.Data
{
    public class Lab13Context : IdentityDbContext
    {
        public Lab13Context(DbContextOptions<Lab13Context> options)
            : base(options)
        {
        }

        public DbSet<Lista13.Models.Category> Category { get; set; }

        public DbSet<Lista13.Models.Article> Article { get; set; }
    }
}
