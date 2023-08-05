﻿// <auto-generated />
using System;
using ChatAppBackEnd.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ChatAppBackEnd.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20230730041056_dateofbirth")]
    partial class dateofbirth
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.20")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ChatAppBackEnd.Models.DatabaseModels.ChatRoom", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ChatRoomType")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("CreatorId")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("CreatorId");

                    b.ToTable("ChatRooms");
                });

            modelBuilder.Entity("ChatAppBackEnd.Models.DatabaseModels.Message", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ChatRoomId")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("MessageText")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ReplyToMessageId")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("SenderId")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("ChatRoomId");

                    b.HasIndex("ReplyToMessageId");

                    b.HasIndex("SenderId");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("ChatAppBackEnd.Models.DatabaseModels.User", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("Bio")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("DisplayName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Gender")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhotoUrl")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("ChatAppBackEnd.Models.DatabaseModels.UserChatRoom", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ChatRoomId")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("LastMessageReadId")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.HasIndex("ChatRoomId");

                    b.HasIndex("LastMessageReadId");

                    b.HasIndex("UserId");

                    b.ToTable("UserChatRooms");
                });

            modelBuilder.Entity("ChatAppBackEnd.Models.DatabaseModels.UserRelationship", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("CreatedTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("InitiatorUserId")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("RelationshipType")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("TargetUserId")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("UpdatedTime")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("InitiatorUserId");

                    b.HasIndex("TargetUserId");

                    b.ToTable("UserRelationships");
                });

            modelBuilder.Entity("ChatAppBackEnd.Models.DatabaseModels.ChatRoom", b =>
                {
                    b.HasOne("ChatAppBackEnd.Models.DatabaseModels.User", "Creator")
                        .WithMany()
                        .HasForeignKey("CreatorId");

                    b.Navigation("Creator");
                });

            modelBuilder.Entity("ChatAppBackEnd.Models.DatabaseModels.Message", b =>
                {
                    b.HasOne("ChatAppBackEnd.Models.DatabaseModels.ChatRoom", "ChatRoom")
                        .WithMany()
                        .HasForeignKey("ChatRoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ChatAppBackEnd.Models.DatabaseModels.Message", "ReplyToMessage")
                        .WithMany()
                        .HasForeignKey("ReplyToMessageId");

                    b.HasOne("ChatAppBackEnd.Models.DatabaseModels.User", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ChatRoom");

                    b.Navigation("ReplyToMessage");

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("ChatAppBackEnd.Models.DatabaseModels.UserChatRoom", b =>
                {
                    b.HasOne("ChatAppBackEnd.Models.DatabaseModels.ChatRoom", "ChatRoom")
                        .WithMany()
                        .HasForeignKey("ChatRoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ChatAppBackEnd.Models.DatabaseModels.Message", "LastMessageRead")
                        .WithMany()
                        .HasForeignKey("LastMessageReadId")
                        .OnDelete(DeleteBehavior.Restrict);

                    b.HasOne("ChatAppBackEnd.Models.DatabaseModels.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ChatRoom");

                    b.Navigation("LastMessageRead");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ChatAppBackEnd.Models.DatabaseModels.UserRelationship", b =>
                {
                    b.HasOne("ChatAppBackEnd.Models.DatabaseModels.User", "InitiatorUser")
                        .WithMany()
                        .HasForeignKey("InitiatorUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("ChatAppBackEnd.Models.DatabaseModels.User", "TargetUser")
                        .WithMany()
                        .HasForeignKey("TargetUserId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("InitiatorUser");

                    b.Navigation("TargetUser");
                });
#pragma warning restore 612, 618
        }
    }
}
