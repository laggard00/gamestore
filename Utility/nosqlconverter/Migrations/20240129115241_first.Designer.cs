﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using nosqlconverter.DBContexts;

#nullable disable

namespace nosqlconverter.Migrations
{
    [DbContext(typeof(NorthwindDbContext))]
    [Migration("20240129115241_first")]
    partial class first
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("nosqlconverter.SQLOrders", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<Guid>("CustomerId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("char(36)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("NoSqlCustomerId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("NoSqlOrderId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("SQLOrders");
                });
#pragma warning restore 612, 618
        }
    }
}