﻿// <auto-generated />
using System;
using ErrorCentral.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace ErrorCentral.Infrastructure.Migrations
{
    [DbContext(typeof(ErrorCentralContext))]
    partial class ErrorCentralContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("ErrorCentral.Domain.AggregatesModel.LogErrorAggregate.LogError", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnName("created_at")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Details")
                        .HasColumnName("details")
                        .HasColumnType("nvarchar(2000)")
                        .HasMaxLength(2000);

                    b.Property<int>("Environment")
                        .HasColumnName("e_environment")
                        .HasColumnType("int");

                    b.Property<bool>("Filed")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("filed")
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<int>("Level")
                        .HasColumnName("e_level")
                        .HasColumnType("int");

                    b.Property<bool>("Removed")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("removed")
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("Source")
                        .IsRequired()
                        .HasColumnName("source")
                        .HasColumnType("nvarchar(300)")
                        .HasMaxLength(300);

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnName("title")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnName("updated_at")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("UserId")
                        .HasColumnName("id_user")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("log_errors","dbo");
                });

            modelBuilder.Entity("ErrorCentral.Domain.AggregatesModel.UserAggregate.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("id")
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnName("created_at")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnName("email")
                        .HasColumnType("nvarchar(500)")
                        .HasMaxLength(500);

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnName("first_name")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnName("last_name")
                        .HasColumnType("nvarchar(100)")
                        .HasMaxLength(100);

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnName("password")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Removed")
                        .ValueGeneratedOnAdd()
                        .HasColumnName("removed")
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<DateTimeOffset>("UpdatedAt")
                        .HasColumnName("updated_at")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.ToTable("users","dbo");
                });

            modelBuilder.Entity("ErrorCentral.Domain.AggregatesModel.LogErrorAggregate.LogError", b =>
                {
                    b.HasOne("ErrorCentral.Domain.AggregatesModel.UserAggregate.User", "User")
                        .WithMany("LogErrors")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
