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

            modelBuilder.Entity("Djm.OGame.Web.Api.Dal.Entities.Article", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("AuthorEmail");

                    b.Property<int>("HtmlContentId");

                    b.Property<string>("Image");

                    b.Property<DateTime>("LastEdit");

                    b.Property<string>("Preview");

                    b.Property<DateTime>("PublishDate");

                    b.Property<string>("Title")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Articles");
                });

            modelBuilder.Entity("Djm.OGame.Web.Api.Dal.Entities.ArticleContent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("HtmlContent")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("ArticlesContents");
                });

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
                    b.Property<string>("EmailAddress")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("AllowNotifications");

                    b.Property<string>("Name");

                    b.Property<int>("OGameId");

                    b.Property<string>("Password");

                    b.Property<string>("ProfilePicturePath");

                    b.Property<string>("Role");

                    b.Property<byte[]>("Salt");

                    b.Property<int>("UniverseId");

                    b.HasKey("EmailAddress");

                    b.HasIndex("UniverseId", "OGameId")
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
