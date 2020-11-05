using Common;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebdScadaBackend.Models
{
    public class PointContext : DbContext
    {
        public DbSet<PointItem> Points { get; set; }
        public DbSet<ConfigItem> ConfigItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public PointContext(DbContextOptions options): base(options)
        {
            this.ChangeTracker.LazyLoadingEnabled = false;
        }
    }
}
