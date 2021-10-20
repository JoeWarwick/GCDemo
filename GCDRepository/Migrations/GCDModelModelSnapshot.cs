﻿// <auto-generated />
using System;
using GCDRepository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GCDRepository.Migrations
{
    [DbContext(typeof(GCDModel))]
    partial class GCDModelModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.10");

            modelBuilder.Entity("GCDRepository.ToDoItem", b =>
                {
                    b.Property<int>("TodoItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("Added")
                        .HasColumnType("TEXT");

                    b.Property<bool>("Checked")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("ScheduledBy")
                        .HasColumnType("TEXT");

                    b.Property<string>("Title")
                        .HasColumnType("TEXT");

                    b.HasKey("TodoItemId");

                    b.ToTable("ToDoItems");
                });
#pragma warning restore 612, 618
        }
    }
}