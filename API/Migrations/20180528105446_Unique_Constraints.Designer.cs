﻿// <auto-generated />
using API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace API.Migrations
{
    [DbContext(typeof(Context))]
    [Migration("20180528105446_Unique_Constraints")]
    partial class Unique_Constraints
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011");

            modelBuilder.Entity("API.Models.Exercise", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<byte[]>("Recording")
                        .IsRequired();

                    b.HasKey("ID");

                    b.ToTable("Exercises");
                });

            modelBuilder.Entity("API.Models.ExercisePlanning", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Amount");

                    b.Property<DateTime>("Date");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<bool>("IsComplete");

                    b.Property<long>("UserExercise_ID");

                    b.HasKey("ID");

                    b.HasIndex("UserExercise_ID");

                    b.ToTable("ExercisePlannings");
                });

            modelBuilder.Entity("API.Models.ExerciseResult", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Date");

                    b.Property<int>("Duration");

                    b.Property<string>("Result")
                        .IsRequired();

                    b.Property<int>("Score");

                    b.Property<long>("UserExercise_ID");

                    b.HasKey("ID");

                    b.HasIndex("UserExercise_ID");

                    b.ToTable("ExerciseResults");
                });

            modelBuilder.Entity("API.Models.Tokens.RefreshToken", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("Revoked");

                    b.Property<string>("Token");

                    b.Property<long>("User_ID");

                    b.HasKey("ID");

                    b.HasIndex("Token")
                        .IsUnique();

                    b.HasIndex("User_ID");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("API.Models.User", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<char>("Gender");

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<int>("UserGroup_ID");

                    b.HasKey("ID");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("UserGroup_ID");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("API.Models.UserExercise", b =>
                {
                    b.Property<long>("ID")
                        .ValueGeneratedOnAdd();

                    b.Property<long>("Exercise_ID");

                    b.Property<long>("User_ID");

                    b.HasKey("ID");

                    b.HasIndex("Exercise_ID");

                    b.HasIndex("User_ID");

                    b.ToTable("UserExercises");
                });

            modelBuilder.Entity("API.Models.UserGroup", b =>
                {
                    b.Property<int>("ID");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("Type")
                        .IsRequired();

                    b.HasKey("ID");

                    b.ToTable("UserGroups");
                });

            modelBuilder.Entity("API.Models.UserPhysio", b =>
                {
                    b.Property<long>("User_ID");

                    b.Property<long>("Physio_ID");

                    b.HasKey("User_ID", "Physio_ID");

                    b.HasIndex("Physio_ID");

                    b.ToTable("UserPhysios");
                });

            modelBuilder.Entity("API.Models.ExercisePlanning", b =>
                {
                    b.HasOne("API.Models.UserExercise", "UserExercise")
                        .WithMany()
                        .HasForeignKey("UserExercise_ID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("API.Models.ExerciseResult", b =>
                {
                    b.HasOne("API.Models.UserExercise", "UserExercise")
                        .WithMany()
                        .HasForeignKey("UserExercise_ID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("API.Models.Tokens.RefreshToken", b =>
                {
                    b.HasOne("API.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("User_ID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("API.Models.User", b =>
                {
                    b.HasOne("API.Models.UserGroup", "UserGroup")
                        .WithMany()
                        .HasForeignKey("UserGroup_ID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("API.Models.UserExercise", b =>
                {
                    b.HasOne("API.Models.Exercise", "Exercise")
                        .WithMany()
                        .HasForeignKey("Exercise_ID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("API.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("User_ID")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("API.Models.UserPhysio", b =>
                {
                    b.HasOne("API.Models.User", "Physio")
                        .WithMany()
                        .HasForeignKey("Physio_ID")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("API.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("User_ID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
