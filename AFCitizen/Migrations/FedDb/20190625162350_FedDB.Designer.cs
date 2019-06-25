﻿// <auto-generated />
using AFCitizen.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AFCitizen.Migrations.FedDb
{
    [DbContext(typeof(FedDbContext))]
    [Migration("20190625162350_FedDB")]
    partial class FedDB
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.4-servicing-10062")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AFCitizen.Models.Block", b =>
                {
                    b.Property<string>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("AuthorityLevel");

                    b.Property<string>("Document");

                    b.Property<string>("From");

                    b.Property<string>("Hash");

                    b.Property<string>("PreviousHash");

                    b.Property<string>("To");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.ToTable("Blocks");
                });
#pragma warning restore 612, 618
        }
    }
}
