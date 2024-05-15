﻿// <auto-generated />
using System;
using LocationCheck.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LocationCheck.Data.Migrations
{
    [DbContext(typeof(LocationCheckDb))]
    partial class LocationCheckDbModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("LocationCheck.Data.Entities.ApiUserEntity", b =>
                {
                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<Guid>("ApiKey")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Username");

                    b.HasIndex("ApiKey");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("ApiUsers");
                });
#pragma warning restore 612, 618
        }
    }
}
