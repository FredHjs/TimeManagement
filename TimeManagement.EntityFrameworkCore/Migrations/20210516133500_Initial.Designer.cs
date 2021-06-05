﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TimeManagement.EntityFrameworkCore;

namespace TimeManagement.Migrations
{
    [DbContext(typeof(TimeManagementDbContext))]
    [Migration("20210516133500_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.6")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("TimeManagement.Entities.Course", b =>
                {
                    b.Property<string>("ID")
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<string>("Discription")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<double>("Grade")
                        .HasMaxLength(10)
                        .HasColumnType("float");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("TermID")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.HasKey("ID");

                    b.HasIndex("Name");

                    b.ToTable("Courses");
                });

            modelBuilder.Entity("TimeManagement.Entities.Event", b =>
                {
                    b.Property<string>("ID")
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<string>("CourseID")
                        .IsRequired()
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<string>("Description")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("RepeatDays")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("Time")
                        .HasColumnType("datetime2");

                    b.HasKey("ID");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("TimeManagement.Entities.Term", b =>
                {
                    b.Property<string>("ID")
                        .HasMaxLength(36)
                        .HasColumnType("nvarchar(36)");

                    b.Property<DateTime>("EndTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<DateTime>("StartTime")
                        .HasColumnType("datetime2");

                    b.Property<int>("TermNumber")
                        .HasMaxLength(5)
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("TermNumber");

                    b.ToTable("Terms");
                });

            modelBuilder.Entity("TimeManagement.Entities.Course", b =>
                {
                    b.HasOne("TimeManagement.Entities.Term", "Term")
                        .WithMany("Courses")
                        .HasForeignKey("ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Term");
                });

            modelBuilder.Entity("TimeManagement.Entities.Event", b =>
                {
                    b.HasOne("TimeManagement.Entities.Course", "Course")
                        .WithMany("Events")
                        .HasForeignKey("ID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Course");
                });

            modelBuilder.Entity("TimeManagement.Entities.Course", b =>
                {
                    b.Navigation("Events");
                });

            modelBuilder.Entity("TimeManagement.Entities.Term", b =>
                {
                    b.Navigation("Courses");
                });
#pragma warning restore 612, 618
        }
    }
}
