﻿// <auto-generated />
using System;
using LocationCheck.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace LocationCheck.Data.Migrations
{
    [DbContext(typeof(LocationCheckDb))]
    [Migration("20240520154001_Add_RequestResponseLog_model")]
    partial class Add_RequestResponseLog_model
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("LocationCheck.Data.Entities.ApiUserEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<bool>("Active")
                        .HasColumnType("bit");

                    b.Property<Guid>("ApiKey")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ApiKey");

                    b.HasIndex("Username")
                        .IsUnique();

                    b.ToTable("ApiUsers");
                });

            modelBuilder.Entity("LocationCheck.Data.Entities.RequestResponseLog", b =>
                {
                    b.Property<Guid>("RequestId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("ApiUserEntityId")
                        .HasColumnType("int");

                    b.Property<string>("Request")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Response")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("RequestId");

                    b.HasIndex("ApiUserEntityId");

                    b.ToTable("RequestResponseLogs");
                });

            modelBuilder.Entity("LocationCheck.Data.Entities.RequestResponseLog", b =>
                {
                    b.HasOne("LocationCheck.Data.Entities.ApiUserEntity", "ApiUserEntity")
                        .WithMany()
                        .HasForeignKey("ApiUserEntityId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApiUserEntity");
                });
#pragma warning restore 612, 618
        }
    }
}
