﻿// <auto-generated />
using Constanze.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Constanze.Infrastructure.Migrations
{
    [DbContext(typeof(ConstanzeContext))]
    [Migration("20230604174616_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Constanze.Core.Entities.AppUser", b =>
                {
                    b.Property<int>("Key")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Key"));

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Id")
                        .HasColumnType("bigint");

                    b.Property<string>("LanguageCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LastName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Username")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Key");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Constanze.Core.Entities.Conversation", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"));

                    b.Property<long>("ChatId")
                        .HasColumnType("bigint");

                    b.Property<bool>("IsClosed")
                        .HasColumnType("bit");

                    b.Property<int>("UserKey")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserKey");

                    b.ToTable("Conversations");
                });

            modelBuilder.Entity("Constanze.Core.Entities.DialogMessage", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Bot")
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "bot");

                    b.Property<long>("ConversationId")
                        .HasColumnType("bigint");

                    b.Property<string>("User")
                        .HasColumnType("nvarchar(max)")
                        .HasAnnotation("Relational:JsonPropertyName", "user");

                    b.HasKey("Id");

                    b.HasIndex("ConversationId");

                    b.ToTable("DialogMessages");
                });

            modelBuilder.Entity("Constanze.Core.Entities.Conversation", b =>
                {
                    b.HasOne("Constanze.Core.Entities.AppUser", "User")
                        .WithMany("Conversations")
                        .HasForeignKey("UserKey")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Constanze.Core.Entities.DialogMessage", b =>
                {
                    b.HasOne("Constanze.Core.Entities.Conversation", "Conversation")
                        .WithMany("DialogMessages")
                        .HasForeignKey("ConversationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Conversation");
                });

            modelBuilder.Entity("Constanze.Core.Entities.AppUser", b =>
                {
                    b.Navigation("Conversations");
                });

            modelBuilder.Entity("Constanze.Core.Entities.Conversation", b =>
                {
                    b.Navigation("DialogMessages");
                });
#pragma warning restore 612, 618
        }
    }
}