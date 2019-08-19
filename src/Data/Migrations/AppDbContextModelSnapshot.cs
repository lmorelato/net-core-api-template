﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Template.Data.Context;

namespace Template.Data.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasMaxLength(256)
                        .IsUnicode(false);

                    b.Property<string>("ClaimValue")
                        .HasMaxLength(256)
                        .IsUnicode(false);

                    b.Property<int>("RoleId");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("IdentityRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ClaimType")
                        .HasMaxLength(256)
                        .IsUnicode(false);

                    b.Property<string>("ClaimValue")
                        .HasMaxLength(256)
                        .IsUnicode(false);

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("IdentityUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.Property<string>("ProviderKey")
                        .HasMaxLength(256)
                        .IsUnicode(false);

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(256)
                        .IsUnicode(false);

                    b.Property<string>("ProviderDisplayName")
                        .HasMaxLength(256)
                        .IsUnicode(false);

                    b.Property<int>("UserId");

                    b.HasKey("ProviderKey", "LoginProvider");

                    b.HasAlternateKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("IdentityUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<int>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("IdentityUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.Property<int>("UserId");

                    b.Property<string>("LoginProvider")
                        .HasMaxLength(256)
                        .IsUnicode(false);

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .IsUnicode(false);

                    b.Property<string>("Value")
                        .HasMaxLength(256)
                        .IsUnicode(false);

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("IdentityUserTokens");
                });

            modelBuilder.Entity("Template.Data.Entities.AccessLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("Date");

                    b.Property<string>("IpAddress")
                        .HasMaxLength(20)
                        .IsUnicode(false);

                    b.Property<decimal?>("Latitude")
                        .HasColumnType("decimal(12,8)");

                    b.Property<decimal?>("Longitude")
                        .HasColumnType("decimal(12,8)");

                    b.Property<int>("UserId");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AccessLogs");
                });

            modelBuilder.Entity("Template.Data.Entities.AuditLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Changed")
                        .HasMaxLength(4096);

                    b.Property<DateTime>("Created");

                    b.Property<int>("Kind");

                    b.Property<int>("ModifiedById");

                    b.Property<DateTime>("ModifiedOn");

                    b.Property<string>("RowId")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("TableName")
                        .IsRequired()
                        .HasMaxLength(128);

                    b.HasKey("Id");

                    b.ToTable("AuditLog");
                });

            modelBuilder.Entity("Template.Data.Entities.Identity.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasMaxLength(256)
                        .IsUnicode(false);

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .IsUnicode(false);

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("IdentityRoles");
                });

            modelBuilder.Entity("Template.Data.Entities.Identity.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasMaxLength(256)
                        .IsUnicode(false);

                    b.Property<DateTime>("CreatedOn")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GetUtcDate()");

                    b.Property<string>("Culture")
                        .IsRequired()
                        .HasMaxLength(10)
                        .IsUnicode(false);

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .IsUnicode(false);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<string>("FullName")
                        .HasMaxLength(128)
                        .IsUnicode(false);

                    b.Property<bool>("IsDeleted");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<DateTime>("ModifiedOn")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("GetUtcDate()");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .IsUnicode(false);

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .IsUnicode(false);

                    b.Property<string>("PasswordHash")
                        .HasMaxLength(256)
                        .IsUnicode(false);

                    b.Property<string>("PhoneNumber")
                        .HasMaxLength(128)
                        .IsUnicode(false);

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp")
                        .HasMaxLength(256)
                        .IsUnicode(false);

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .IsUnicode(false);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("IdentityUsers");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<int>", b =>
                {
                    b.HasOne("Template.Data.Entities.Identity.Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<int>", b =>
                {
                    b.HasOne("Template.Data.Entities.Identity.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<int>", b =>
                {
                    b.HasOne("Template.Data.Entities.Identity.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<int>", b =>
                {
                    b.HasOne("Template.Data.Entities.Identity.Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("Template.Data.Entities.Identity.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<int>", b =>
                {
                    b.HasOne("Template.Data.Entities.Identity.User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });

            modelBuilder.Entity("Template.Data.Entities.AccessLog", b =>
                {
                    b.HasOne("Template.Data.Entities.Identity.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Restrict);
                });
#pragma warning restore 612, 618
        }
    }
}