﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SpredMedia.Notification.Infrastructure;

#nullable disable

namespace SpredMedia.Notification.Infrastructure.Migrations
{
    [DbContext(typeof(NotificationDbContext))]
    [Migration("20230624234402_myMigration01")]
    partial class myMigration01
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("SpredMedia.Notification.Model.Entity.Email", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("RecipientAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SenderAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Subject")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<byte[]>("body")
                        .HasColumnType("varbinary(max)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Emails");
                });

            modelBuilder.Entity("SpredMedia.Notification.Model.Entity.SMS", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("RecipientNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SenderNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Subject")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<byte[]>("body")
                        .HasColumnType("varbinary(max)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Sms");
                });

            modelBuilder.Entity("SpredMedia.Notification.Model.Entity.User", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("EmailAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("UserName")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("SpredMedia.Notification.Model.Entity.Email", b =>
                {
                    b.HasOne("SpredMedia.Notification.Model.Entity.User", "User")
                        .WithMany("Email")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SpredMedia.Notification.Model.Entity.SMS", b =>
                {
                    b.HasOne("SpredMedia.Notification.Model.Entity.User", "User")
                        .WithMany("SMS")
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("SpredMedia.Notification.Model.Entity.User", b =>
                {
                    b.Navigation("Email");

                    b.Navigation("SMS");
                });
#pragma warning restore 612, 618
        }
    }
}
