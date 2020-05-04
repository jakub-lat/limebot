﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using PotatoBot.Models;

namespace PotatoBot.Migrations
{
    [DbContext(typeof(GuildContext))]
    partial class GuildContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "5.0.0-preview.3.20181.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("DAL.Models.GuildMember", b =>
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

            modelBuilder.Entity("DAL.Models.ReactionRole", b =>
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

            modelBuilder.Entity("PotatoBot.Models.GuildData", b =>
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

                    b.Property<int>("RequiredXPToLevelUp")
                        .HasColumnType("integer");

                    b.Property<string>("WelcomeMessage")
                        .HasColumnType("text");

                    b.Property<decimal>("WelcomeMessagesChannel")
                        .HasColumnType("numeric(20,0)");

                    b.HasKey("Id");

                    b.ToTable("Guilds");
                });

            modelBuilder.Entity("PotatoBot.Models.MutedUser", b =>
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

            modelBuilder.Entity("PotatoBot.Models.Warn", b =>
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

            modelBuilder.Entity("DAL.Models.GuildMember", b =>
                {
                    b.HasOne("PotatoBot.Models.GuildData", "Guild")
                        .WithMany("Members")
                        .HasForeignKey("GuildId");
                });

            modelBuilder.Entity("PotatoBot.Models.Warn", b =>
                {
                    b.HasOne("PotatoBot.Models.GuildData", "Guild")
                        .WithMany("Warns")
                        .HasForeignKey("GuildId");
                });
#pragma warning restore 612, 618
        }
    }
}
