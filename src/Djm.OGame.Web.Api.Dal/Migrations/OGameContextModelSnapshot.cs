﻿// <auto-generated />
using Djm.OGame.Web.Api.Dal;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace Djm.OGame.Web.Api.Dal.Migrations
{
    [DbContext(typeof(OGameContext))]
    partial class OGameContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Djm.OGame.Web.Api.Dal.Entities.Pin", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("OwnerId");

                    b.Property<int>("TargetId");

                    b.Property<int>("UniverseId");

                    b.HasKey("Id");

                    b.HasIndex("UniverseId", "OwnerId", "TargetId")
                        .IsUnique();

                    b.ToTable("Pins");
                });

            modelBuilder.Entity("Djm.OGame.Web.Api.Dal.Entities.Player", b =>
                {
                    b.Property<int>("Id");

                    b.Property<int>("UniverseId");

                    b.Property<string>("ProfilePicturePath");

                    b.HasKey("Id", "UniverseId");

                    b.HasIndex("Id", "UniverseId")
                        .IsUnique();

                    b.ToTable("Players");
                });

            modelBuilder.Entity("Djm.OGame.Web.Api.Dal.Entities.Univers", b =>
                {
                    b.Property<int>("Id");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Univers");
                });
#pragma warning restore 612, 618
        }
    }
}