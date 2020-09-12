﻿// <auto-generated />

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace LimeBot.DAL.Migrations
{
    [DbContext(typeof(GuildContext))]
    [Migration("20200504095622_LevelUpMessage")]
    partial class LevelUpMessage
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "5.0.0-preview.3.20181.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("LimeBot.DAL.Models.GuildMember", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<decimal?>("GuildId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<DateTime>("LastMessaged")
                        .HasColumnType("timestamp without time zone");

                    b.Property<decimal>("UserId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<int>("XP")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("GuildId");

                    b.ToTable("Members");
                });

            modelBuilder.Entity("LimeBot.DAL.Models.ReactionRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("Emoji")
                        .HasColumnType("text");

                    b.Property<decimal>("GuildId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("MessageId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("MessageJumpLink")
                        .HasColumnType("text");

                    b.Property<decimal>("RoleId")
                        .HasColumnType("numeric(20,0)");

                    b.HasKey("Id");

                    b.ToTable("ReactionRoles");
                });

            modelBuilder.Entity("LimeBot.Models.GuildData", b =>
                {
                    b.Property<decimal>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal[]>("AutoRolesOnJoin")
                        .HasColumnType("numeric[]");

                    b.Property<bool>("EnableLevelUpMessage")
                        .HasColumnType("boolean");

                    b.Property<bool>("EnableLeveling")
                        .HasColumnType("boolean");

                    b.Property<bool>("EnableMessageLogs")
                        .HasColumnType("boolean");

                    b.Property<bool>("EnableModLogs")
                        .HasColumnType("boolean");

                    b.Property<bool>("EnableWelcomeMessages")
                        .HasColumnType("boolean");

                    b.Property<string>("LeaveMessage")
                        .HasColumnType("text");

                    b.Property<string>("LevelUpMessage")
                        .HasColumnType("text");

                    b.Property<decimal>("MessageLogsChannel")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("ModLogsChannel")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal>("MutedRoleId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("Prefix")
                        .HasColumnType("text");

                    b.Property<bool>("ReactionRolesNotifyDM")
                        .HasColumnType("boolean");

                    b.Property<string>("WelcomeMessage")
                        .HasColumnType("text");

                    b.Property<decimal>("WelcomeMessagesChannel")
                        .HasColumnType("numeric(20,0)");

                    b.HasKey("Id");

                    b.ToTable("Guilds");
                });

            modelBuilder.Entity("LimeBot.Models.MutedUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<decimal>("GuildId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<DateTime>("Time")
                        .HasColumnType("timestamp without time zone");

                    b.Property<decimal>("UserId")
                        .HasColumnType("numeric(20,0)");

                    b.HasKey("Id");

                    b.ToTable("MutedUsers");
                });

            modelBuilder.Entity("LimeBot.Models.Warn", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<decimal>("AuthorId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<decimal?>("GuildId")
                        .HasColumnType("numeric(20,0)");

                    b.Property<string>("Reason")
                        .HasColumnType("text");

                    b.Property<decimal>("UserId")
                        .HasColumnType("numeric(20,0)");

                    b.HasKey("Id");

                    b.HasIndex("GuildId");

                    b.ToTable("Warns");
                });

            modelBuilder.Entity("LimeBot.DAL.Models.GuildMember", b =>
                {
                    b.HasOne("LimeBot.Models.GuildData", "Guild")
                        .WithMany("Members")
                        .HasForeignKey("GuildId");
                });

            modelBuilder.Entity("LimeBot.Models.Warn", b =>
                {
                    b.HasOne("LimeBot.Models.GuildData", "Guild")
                        .WithMany("Warns")
                        .HasForeignKey("GuildId");
                });
#pragma warning restore 612, 618
        }
    }
}