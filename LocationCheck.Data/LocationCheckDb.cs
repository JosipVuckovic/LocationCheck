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

            modelBuilder.Entity<PlaceBasicDataEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.ExternalIdentifier).IsRequired();
                entity.HasIndex(e => e.ExternalIdentifier);
            });
            
            modelBuilder.Entity<UserFavoritePlaceEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.PlaceId).IsRequired();
                entity.Property(e => e.UserId).IsRequired();
                entity.HasOne<ApiUserEntity>(e => e.User);
                entity.HasOne<PlaceBasicDataEntity>(e => e.Place);
            });
        }

        public DbSet<ApiUserEntity> ApiUsers { get; set; }
        public DbSet<RequestResponseLogEntity> RequestResponseLogs { get; set; }
        public DbSet<PlaceBasicDataEntity> PlaceBasicData { get; set; }
        public DbSet<UserFavoritePlaceEntity> FavoritePlaces { get; set; }

        private static T ToObject<T>(string objectString) where T : class, new()
        {
            return JsonConvert.DeserializeObject<T>(objectString) ?? new T();
        }
        
        private static T? ToNullableObject<T>(string objectString) where T : class
        {
            return JsonConvert.DeserializeObject<T>(objectString);
        }

        private static string FromObject<T>(T stringToSerialize) where T : class
        {
            return JsonConvert.SerializeObject(stringToSerialize);
        }
    }
    
    
}
