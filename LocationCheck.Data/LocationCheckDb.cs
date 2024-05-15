using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using LocationCheck.Data.Entities;

namespace LocationCheck.Data
{
    public class LocationCheckDb : DbContext
    {   

        public LocationCheckDb(DbContextOptions<LocationCheckDb> options)
        : base(options)
        {}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ApiUserEntity>(entity =>
            {                
                entity.HasKey(e => e.Username);
                entity.HasIndex(e => e.Username)
                      .IsUnique();
                entity.Property(e => e.Username)
                      .IsRequired();
                entity.HasIndex(e => e.ApiKey);                
            });

            
        }

        public DbSet<ApiUserEntity> ApiUsers { get; set; }

    }
}
