﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Ticket.Persistence.Context;

#nullable disable

namespace Ticket.Persistence.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    partial class ApplicationContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Ticket.Domain.Entities.Category", b =>
                {
                    b.Property<string>("id")
                        .HasColumnType("text");

                    b.Property<DateTime>("created_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("is_active")
                        .HasColumnType("boolean");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.ToTable("categories");
                });

            modelBuilder.Entity("Ticket.Domain.Entities.Cinema", b =>
                {
                    b.Property<string>("id")
                        .HasColumnType("text");

                    b.Property<string>("address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("city_id")
                        .HasColumnType("integer");

                    b.Property<DateTime>("created_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("is_active")
                        .HasColumnType("boolean");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.HasIndex("city_id");

                    b.ToTable("cinemas");
                });

            modelBuilder.Entity("Ticket.Domain.Entities.City", b =>
                {
                    b.Property<int>("city_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("city_id"));

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("city_id");

                    b.ToTable("cities");
                });

            modelBuilder.Entity("Ticket.Domain.Entities.Hall", b =>
                {
                    b.Property<string>("id")
                        .HasColumnType("text");

                    b.Property<int>("capacity")
                        .HasColumnType("integer");

                    b.Property<string>("cinema_id")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("created_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("is_active")
                        .HasColumnType("boolean");

                    b.Property<string>("name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.HasIndex("cinema_id");

                    b.ToTable("halls");
                });

            modelBuilder.Entity("Ticket.Domain.Entities.MovieTicket", b =>
                {
                    b.Property<string>("id")
                        .HasColumnType("text");

                    b.Property<DateTime>("created_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("is_active")
                        .HasColumnType("boolean");

                    b.Property<decimal>("price")
                        .HasColumnType("numeric");

                    b.Property<string>("seat_id")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("session_id")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("status")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.HasIndex("seat_id");

                    b.HasIndex("session_id");

                    b.ToTable("movie_tickets");
                });

            modelBuilder.Entity("Ticket.Domain.Entities.Pricing", b =>
                {
                    b.Property<string>("id")
                        .HasColumnType("text");

                    b.Property<string>("category_id")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("created_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("is_active")
                        .HasColumnType("boolean");

                    b.Property<decimal>("price")
                        .HasColumnType("numeric");

                    b.Property<string>("session_id")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.HasIndex("category_id");

                    b.HasIndex("session_id");

                    b.ToTable("pricings");
                });

            modelBuilder.Entity("Ticket.Domain.Entities.Seat", b =>
                {
                    b.Property<string>("id")
                        .HasColumnType("text");

                    b.Property<DateTime>("created_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("hall_id")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("is_active")
                        .HasColumnType("boolean");

                    b.Property<int>("number")
                        .HasColumnType("integer");

                    b.Property<string>("row")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("id");

                    b.HasIndex("hall_id");

                    b.ToTable("seats");
                });

            modelBuilder.Entity("Ticket.Domain.Entities.Session", b =>
                {
                    b.Property<string>("id")
                        .HasColumnType("text");

                    b.Property<string>("cinema_id")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("created_at")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("hall_id")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("is_active")
                        .HasColumnType("boolean");

                    b.Property<string>("movie_id")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<DateTime>("session_date")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("id");

                    b.HasIndex("cinema_id");

                    b.HasIndex("hall_id");

                    b.ToTable("sessions");
                });

            modelBuilder.Entity("Ticket.Domain.Entities.Cinema", b =>
                {
                    b.HasOne("Ticket.Domain.Entities.City", "city")
                        .WithMany("cinemas")
                        .HasForeignKey("city_id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("city");
                });

            modelBuilder.Entity("Ticket.Domain.Entities.Hall", b =>
                {
                    b.HasOne("Ticket.Domain.Entities.Cinema", "cinema")
                        .WithMany("halls")
                        .HasForeignKey("cinema_id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("cinema");
                });

            modelBuilder.Entity("Ticket.Domain.Entities.MovieTicket", b =>
                {
                    b.HasOne("Ticket.Domain.Entities.Seat", "seat")
                        .WithMany("movie_tickets")
                        .HasForeignKey("seat_id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Ticket.Domain.Entities.Session", "session")
                        .WithMany("movie_tickets")
                        .HasForeignKey("session_id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("seat");

                    b.Navigation("session");
                });

            modelBuilder.Entity("Ticket.Domain.Entities.Pricing", b =>
                {
                    b.HasOne("Ticket.Domain.Entities.Category", "category")
                        .WithMany("pricings")
                        .HasForeignKey("category_id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Ticket.Domain.Entities.Session", "session")
                        .WithMany("pricings")
                        .HasForeignKey("session_id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("category");

                    b.Navigation("session");
                });

            modelBuilder.Entity("Ticket.Domain.Entities.Seat", b =>
                {
                    b.HasOne("Ticket.Domain.Entities.Hall", "hall")
                        .WithMany("seats")
                        .HasForeignKey("hall_id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("hall");
                });

            modelBuilder.Entity("Ticket.Domain.Entities.Session", b =>
                {
                    b.HasOne("Ticket.Domain.Entities.Cinema", "cinema")
                        .WithMany("sessions")
                        .HasForeignKey("cinema_id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("Ticket.Domain.Entities.Hall", "hall")
                        .WithMany("sessions")
                        .HasForeignKey("hall_id")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("cinema");

                    b.Navigation("hall");
                });

            modelBuilder.Entity("Ticket.Domain.Entities.Category", b =>
                {
                    b.Navigation("pricings");
                });

            modelBuilder.Entity("Ticket.Domain.Entities.Cinema", b =>
                {
                    b.Navigation("halls");

                    b.Navigation("sessions");
                });

            modelBuilder.Entity("Ticket.Domain.Entities.City", b =>
                {
                    b.Navigation("cinemas");
                });

            modelBuilder.Entity("Ticket.Domain.Entities.Hall", b =>
                {
                    b.Navigation("seats");

                    b.Navigation("sessions");
                });

            modelBuilder.Entity("Ticket.Domain.Entities.Seat", b =>
                {
                    b.Navigation("movie_tickets");
                });

            modelBuilder.Entity("Ticket.Domain.Entities.Session", b =>
                {
                    b.Navigation("movie_tickets");

                    b.Navigation("pricings");
                });
#pragma warning restore 612, 618
        }
    }
}
