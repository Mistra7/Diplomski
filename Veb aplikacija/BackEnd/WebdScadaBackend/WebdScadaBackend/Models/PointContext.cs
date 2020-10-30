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
        public DbSet<BasePointItem> Points { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BasePointItem>().HasDiscriminator(p => p.Type).HasValue(PointType.HR_LONG);
            modelBuilder.Entity<AnalogInput>().HasDiscriminator(p => p.Type).HasValue(PointType.ANALOG_INPUT);
            modelBuilder.Entity<AnalogOutput>().HasDiscriminator(p => p.Type).HasValue(PointType.ANALOG_OUTPUT);
            modelBuilder.Entity<DigitalInput>().HasDiscriminator(p => p.Type).HasValue(PointType.DIGITAL_INPUT);
            modelBuilder.Entity<DigitalOutput>().HasDiscriminator(p => p.Type).HasValue(PointType.DIGITAL_OUTPUT);
            base.OnModelCreating(modelBuilder);
        }

        public PointContext(DbContextOptions options): base(options)
        {

        }
    }
}
