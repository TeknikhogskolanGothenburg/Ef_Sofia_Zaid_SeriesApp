﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using SeriesApp.Data;
using System;

namespace SeriesApp.Data.Migrations
{
    [DbContext(typeof(SeriesContext))]
    [Migration("20180403194546_Init")]
    partial class Init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("SeriesApp.Domain.Actor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("Birthday");

                    b.Property<int>("CountryId");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("Actors");
                });

            modelBuilder.Entity("SeriesApp.Domain.Country", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("CountryCode");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("SeriesApp.Domain.Episode", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("ReleaseDate");

                    b.Property<int>("SeasonNumber");

                    b.Property<int>("SeriesId");

                    b.HasKey("Id");

                    b.HasIndex("SeriesId");

                    b.ToTable("Episodes");
                });

            modelBuilder.Entity("SeriesApp.Domain.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Genres");
                });

            modelBuilder.Entity("SeriesApp.Domain.ProductionCompany", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CountryId");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.HasIndex("CountryId");

                    b.ToTable("ProductionCompanies");
                });

            modelBuilder.Entity("SeriesApp.Domain.Series", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ProductionCompanyId");

                    b.Property<string>("Title");

                    b.HasKey("Id");

                    b.HasIndex("ProductionCompanyId");

                    b.ToTable("Series");
                });

            modelBuilder.Entity("SeriesApp.Domain.Actor", b =>
                {
                    b.HasOne("SeriesApp.Domain.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SeriesApp.Domain.Episode", b =>
                {
                    b.HasOne("SeriesApp.Domain.Series", "Series")
                        .WithMany()
                        .HasForeignKey("SeriesId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SeriesApp.Domain.ProductionCompany", b =>
                {
                    b.HasOne("SeriesApp.Domain.Country", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("SeriesApp.Domain.Series", b =>
                {
                    b.HasOne("SeriesApp.Domain.ProductionCompany", "ProductionCompany")
                        .WithMany()
                        .HasForeignKey("ProductionCompanyId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
