﻿// <auto-generated />
using System;
using Memoirist_V2.PostService.DataContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Memoirist_V2.PostService.Migrations
{
    [DbContext(typeof(PostDbContext))]
    partial class PostDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Memoirist_V2.PostService.Models.CommentPost", b =>
                {
                    b.Property<int>("CommentPostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CommentPostId"));

                    b.Property<string>("CommentContext")
                        .HasColumnType("text");

                    b.Property<int?>("CommentLike")
                        .HasColumnType("integer");

                    b.Property<string>("CommentWriterAvatar")
                        .HasColumnType("text");

                    b.Property<int?>("CommentWriterId")
                        .HasColumnType("integer");

                    b.Property<string>("CommentWriterName")
                        .HasColumnType("text");

                    b.Property<int?>("PostId")
                        .HasColumnType("integer");

                    b.HasKey("CommentPostId");

                    b.HasIndex("PostId");

                    b.ToTable("CommentPosts");
                });

            modelBuilder.Entity("Memoirist_V2.PostService.Models.Post", b =>
                {
                    b.Property<int>("PostId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("PostId"));

                    b.Property<string>("PostContext")
                        .HasColumnType("text");

                    b.Property<string>("PostDateTime")
                        .HasColumnType("text");

                    b.Property<int?>("PostLike")
                        .HasColumnType("integer");

                    b.Property<string>("PostName")
                        .HasColumnType("text");

                    b.Property<string>("PostWriterAvatar")
                        .HasColumnType("text");

                    b.Property<int?>("PostWriterId")
                        .HasColumnType("integer");

                    b.Property<string>("PostWriterName")
                        .HasColumnType("text");

                    b.HasKey("PostId");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("Memoirist_V2.PostService.Models.CommentPost", b =>
                {
                    b.HasOne("Memoirist_V2.PostService.Models.Post", null)
                        .WithMany("ListCommentPost")
                        .HasForeignKey("PostId");
                });

            modelBuilder.Entity("Memoirist_V2.PostService.Models.Post", b =>
                {
                    b.Navigation("ListCommentPost");
                });
#pragma warning restore 612, 618
        }
    }
}
