using Microsoft.EntityFrameworkCore;
using LocationCheck.Data.Entities;
using LocationCheck.Data.Models;
using Newtonsoft.Json;

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
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Username)
                      .IsUnique();
                entity.Property(e => e.Username)
                      .IsRequired();
                entity.HasIndex(e => e.ApiKey);                
            });

            modelBuilder.Entity<RequestResponseLogEntity>(entity =>
            {
                entity.HasKey(e => e.RequestId);
                entity.HasOne<ApiUserEntity>(e => e.ApiUserEntity);
                entity.Property(e => e.Request)
                    .HasConversion(
                        x => FromObject(x),
                        x => ToObject<RequestLog>(x) 
                    );
                entity.Property(e => e.Response)
                    .HasConversion(
                        x => FromObject((x ?? null)!),
                        x => ToNullableObject<ResponseLog>(x) 
                    );
            });

        }

        public DbSet<ApiUserEntity> ApiUsers { get; set; }
        public DbSet<RequestResponseLogEntity> RequestResponseLogs { get; set; }

        private T ToObject<T>(string objectString) where T : class, new()
        {
            return JsonConvert.DeserializeObject<T>(objectString) ?? new T();
        }
        
        private T? ToNullableObject<T>(string objectString) where T : class
        {
            return JsonConvert.DeserializeObject<T>(objectString);
        }

        private string FromObject<T>(T stringToSerialize) where T : class
        {
            return JsonConvert.SerializeObject(stringToSerialize);
        }
    }
    
    
}
