﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PRM.Infrastructure.Persistence.MySQL;

namespace PRM.Infrastructure.Persistence.MySQL.Migrations
{
    [DbContext(typeof(PrmDbContext))]
    partial class PrmDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("PRM.Domain.Products.Product", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("DeleterId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("DeletionTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Description")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("LastModifierId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<decimal>("RentDailyLateFee")
                        .HasColumnType("decimal(65,30)");

                    b.Property<decimal>("RentDailyPrice")
                        .HasColumnType("decimal(65,30)");

                    b.Property<int>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.HasKey("Id");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("PRM.Domain.Products.ProductRentalHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Code")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("DeleterId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("DeletionTime")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("LastModifierId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<Guid>("ProductId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("RentId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("ProductId");

                    b.HasIndex("RentId");

                    b.ToTable("ProductRentalHistory");
                });

            modelBuilder.Entity("PRM.Domain.Renters.Renter", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("DeleterId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("DeletionTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("GovernmentRegistrationDocumentCode")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("LastModifierId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<byte[]>("PersonImage")
                        .HasColumnType("longblob");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Renters");
                });

            modelBuilder.Entity("PRM.Domain.Renters.RenterRentalHistory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Code")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("char(36)");

                    b.Property<Guid?>("DeleterId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("DeletionTime")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("LastModifierId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<Guid>("RentId")
                        .HasColumnType("char(36)");

                    b.Property<Guid>("RenterId")
                        .HasColumnType("char(36)");

                    b.HasKey("Id");

                    b.HasIndex("RentId");

                    b.HasIndex("RenterId");

                    b.ToTable("RenterRentalHistory");
                });

            modelBuilder.Entity("PRM.Domain.Rents.Rent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<string>("Code")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<DateTime>("CreationTime")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("CreatorId")
                        .HasColumnType("char(36)");

                    b.Property<decimal>("DailyLateFee")
                        .HasColumnType("decimal(65,30)");

                    b.Property<decimal>("DailyPrice")
                        .HasColumnType("decimal(65,30)");

                    b.Property<decimal>("DamageFee")
                        .HasColumnType("decimal(65,30)");

                    b.Property<Guid?>("DeleterId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime?>("DeletionTime")
                        .HasColumnType("datetime(6)");

                    b.Property<decimal>("Discount")
                        .HasColumnType("decimal(65,30)");

                    b.Property<DateTime>("EndDate")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime?>("LastModificationTime")
                        .HasColumnType("datetime(6)");

                    b.Property<Guid?>("LastModifierId")
                        .HasColumnType("char(36)");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<Guid>("RenterId")
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<bool>("WasProductDamaged")
                        .HasColumnType("tinyint(1)");

                    b.HasKey("Id");

                    b.ToTable("Rents");
                });

            modelBuilder.Entity("PRM.Domain.Products.ProductRentalHistory", b =>
                {
                    b.HasOne("PRM.Domain.Products.Product", null)
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PRM.Domain.Rents.Rent", null)
                        .WithMany()
                        .HasForeignKey("RentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("PRM.Domain.Renters.RenterRentalHistory", b =>
                {
                    b.HasOne("PRM.Domain.Rents.Rent", null)
                        .WithMany()
                        .HasForeignKey("RentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PRM.Domain.Renters.Renter", null)
                        .WithMany()
                        .HasForeignKey("RenterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
