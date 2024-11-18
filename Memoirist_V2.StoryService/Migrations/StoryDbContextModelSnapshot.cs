﻿// <auto-generated />
using System;
using Memoirist_V2.StoryService.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Memoirist_V2.StoryService.Migrations
{
    [DbContext(typeof(StoryDbContext))]
    partial class StoryDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Memoirist_V2.StoryService.Models.Chapter", b =>
                {
                    b.Property<int>("ChapterId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ChapterId"));

                    b.Property<string>("ChapterContext")
                        .HasColumnType("text");

                    b.Property<string>("ChapterDateTime")
                        .HasColumnType("text");

                    b.Property<int?>("ChapterNumber")
                        .HasColumnType("integer");

                    b.Property<string>("ChapterTitle")
                        .HasColumnType("text");

                    b.Property<int>("StoryId")
                        .HasColumnType("integer");

                    b.HasKey("ChapterId");

                    b.HasIndex("StoryId");

                    b.ToTable("Chapters");
                });

            modelBuilder.Entity("Memoirist_V2.StoryService.Models.Comment", b =>
                {
                    b.Property<int>("CommentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CommentId"));

                    b.Property<string>("CommentContext")
                        .HasColumnType("text");

                    b.Property<string>("CommentDateTime")
                        .HasColumnType("text");

                    b.Property<int?>("CommentLike")
                        .HasColumnType("integer");

                    b.Property<int?>("CommentWriterId")
                        .HasColumnType("integer");

                    b.Property<string>("CommentWriterUsername")
                        .HasColumnType("text");

                    b.Property<int>("StoryId")
                        .HasColumnType("integer");

                    b.HasKey("CommentId");

                    b.HasIndex("StoryId");

                    b.ToTable("Comments");
                });

            modelBuilder.Entity("Memoirist_V2.StoryService.Models.Story", b =>
                {
                    b.Property<int?>("StoryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int?>("StoryId"));

                    b.Property<string>("StoryAuthor")
                        .HasColumnType("text");

                    b.Property<string>("StoryDateWrited")
                        .HasColumnType("text");

                    b.Property<string>("StoryIntroduction")
                        .HasColumnType("text");

                    b.Property<int?>("StoryLikes")
                        .HasColumnType("integer");

                    b.Property<string>("StoryName")
                        .HasColumnType("text");

                    b.Property<string>("StoryPicture")
                        .HasColumnType("text");

                    b.Property<bool>("TermsAndConditionsCheck")
                        .HasColumnType("boolean");

                    b.Property<int?>("WriterStoryId")
                        .HasColumnType("integer");

                    b.HasKey("StoryId");

                    b.ToTable("Stories");
                });

            modelBuilder.Entity("Memoirist_V2.StoryService.Models.Chapter", b =>
                {
                    b.HasOne("Memoirist_V2.StoryService.Models.Story", "Story")
                        .WithMany("Chapters")
                        .HasForeignKey("StoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Story");
                });

            modelBuilder.Entity("Memoirist_V2.StoryService.Models.Comment", b =>
                {
                    b.HasOne("Memoirist_V2.StoryService.Models.Story", "story")
                        .WithMany("StoryComment")
                        .HasForeignKey("StoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("story");
                });

            modelBuilder.Entity("Memoirist_V2.StoryService.Models.Story", b =>
                {
                    b.Navigation("Chapters");

                    b.Navigation("StoryComment");
                });
#pragma warning restore 612, 618
        }
    }
}
