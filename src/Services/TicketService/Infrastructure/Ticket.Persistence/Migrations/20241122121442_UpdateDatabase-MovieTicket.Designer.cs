﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Ticket.Persistence.Context;

#nullable disable

namespace Ticket.Persistence.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20241122121442_UpdateDatabase-MovieTicket")]
    partial class UpdateDatabaseMovieTicket
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MassTransit.EntityFrameworkCoreIntegration.InboxState", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime?>("Consumed")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("ConsumerId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("Delivered")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("ExpirationTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long?>("LastSequenceNumber")
                        .HasColumnType("bigint");

                    b.Property<Guid>("LockId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("MessageId")
                        .HasColumnType("uuid");

                    b.Property<int>("ReceiveCount")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Received")
                        .HasColumnType("timestamp with time zone");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("bytea");

                    b.HasKey("Id");

                    b.HasIndex("Delivered");

                    b.ToTable("InboxState");
                });

            modelBuilder.Entity("MassTransit.EntityFrameworkCoreIntegration.OutboxMessage", b =>
                {
                    b.Property<long>("SequenceNumber")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("SequenceNumber"));

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<Guid?>("ConversationId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("CorrelationId")
                        .HasColumnType("uuid");

                    b.Property<string>("DestinationAddress")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<DateTime?>("EnqueueTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("ExpirationTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FaultAddress")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("Headers")
                        .HasColumnType("text");

                    b.Property<Guid?>("InboxConsumerId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("InboxMessageId")
                        .HasColumnType("uuid");

                    b.Property<Guid?>("InitiatorId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("MessageId")
                        .HasColumnType("uuid");

                    b.Property<string>("MessageType")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<Guid?>("OutboxId")
                        .HasColumnType("uuid");

                    b.Property<string>("Properties")
                        .HasColumnType("text");

                    b.Property<Guid?>("RequestId")
                        .HasColumnType("uuid");

                    b.Property<string>("ResponseAddress")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<DateTime>("SentTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("SourceAddress")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("SequenceNumber");

                    b.HasIndex("EnqueueTime");

                    b.HasIndex("ExpirationTime");

                    b.HasIndex("OutboxId", "SequenceNumber")
                        .IsUnique();

                    b.HasIndex("InboxMessageId", "InboxConsumerId", "SequenceNumber")
                        .IsUnique();

                    b.ToTable("OutboxMessage");
                });

            modelBuilder.Entity("MassTransit.EntityFrameworkCoreIntegration.OutboxState", b =>
                {
                    b.Property<Guid>("OutboxId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("Delivered")
                        .HasColumnType("timestamp with time zone");

                    b.Property<long?>("LastSequenceNumber")
                        .HasColumnType("bigint");

                    b.Property<Guid>("LockId")
                        .HasColumnType("uuid");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("bytea");

                    b.HasKey("OutboxId");

                    b.HasIndex("Created");

                    b.ToTable("OutboxState");
                });

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

                    b.Property<string>("city_id")
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

                    b.HasIndex("city_id");

                    b.ToTable("cinemas");
                });

            modelBuilder.Entity("Ticket.Domain.Entities.City", b =>
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
                    b.Property<string>("ticket_id")
                        .HasColumnType("text");

                    b.Property<DateTime>("created_at")
                        .HasColumnType("timestamp with time zone");

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

                    b.Property<string>("user_id")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("ticket_id");

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

                    b.Property<DateOnly>("session_date")
                        .HasColumnType("date");

                    b.Property<TimeOnly>("session_time")
                        .HasColumnType("time without time zone");

                    b.HasKey("id");

                    b.HasIndex("cinema_id");

                    b.HasIndex("hall_id");

                    b.ToTable("sessions");
                });

            modelBuilder.Entity("MassTransit.EntityFrameworkCoreIntegration.OutboxMessage", b =>
                {
                    b.HasOne("MassTransit.EntityFrameworkCoreIntegration.OutboxState", null)
                        .WithMany()
                        .HasForeignKey("OutboxId");

                    b.HasOne("MassTransit.EntityFrameworkCoreIntegration.InboxState", null)
                        .WithMany()
                        .HasForeignKey("InboxMessageId", "InboxConsumerId")
                        .HasPrincipalKey("MessageId", "ConsumerId");
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
