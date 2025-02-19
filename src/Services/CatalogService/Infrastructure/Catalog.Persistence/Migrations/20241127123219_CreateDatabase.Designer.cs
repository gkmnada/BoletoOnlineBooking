﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Catalog.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Catalog.Persistence.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20241127123219_CreateDatabase")]
    partial class CreateDatabase
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Catalog.Domain.Entities.Category", b =>
                {
                    b.Property<string>("CategoryID")
                        .HasColumnType("text")
                        .HasColumnName("category_id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("is_active");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("SlugURL")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("slug_url");

                    b.HasKey("CategoryID")
                        .HasName("pk_categories");

                    b.ToTable("categories", (string)null);
                });

            modelBuilder.Entity("Catalog.Domain.Entities.Movie", b =>
                {
                    b.Property<string>("MovieID")
                        .HasColumnType("text")
                        .HasColumnName("movie_id");

                    b.Property<int>("AudienceScore")
                        .HasColumnType("integer")
                        .HasColumnName("audience_score");

                    b.Property<string>("CategoryID")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("category_id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Duration")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("duration");

                    b.PrimitiveCollection<List<string>>("Genre")
                        .IsRequired()
                        .HasColumnType("text[]")
                        .HasColumnName("genre");

                    b.Property<string>("ImageURL")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("image_url");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("is_active");

                    b.PrimitiveCollection<List<string>>("Language")
                        .IsRequired()
                        .HasColumnType("text[]")
                        .HasColumnName("language");

                    b.Property<string>("MovieName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("movie_name");

                    b.Property<int>("Rating")
                        .HasColumnType("integer")
                        .HasColumnName("rating");

                    b.Property<string>("ReleaseDate")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("release_date");

                    b.Property<string>("SlugURL")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("slug_url");

                    b.HasKey("MovieID")
                        .HasName("pk_movies");

                    b.HasIndex("CategoryID")
                        .HasDatabaseName("ix_movies_category_id");

                    b.ToTable("movies", (string)null);
                });

            modelBuilder.Entity("Catalog.Domain.Entities.MovieCast", b =>
                {
                    b.Property<string>("CastID")
                        .HasColumnType("text")
                        .HasColumnName("cast_id");

                    b.Property<string>("CastName")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("cast_name");

                    b.Property<string>("Character")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("character");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("ImageURL")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("image_url");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("is_active");

                    b.Property<string>("MovieID")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("movie_id");

                    b.HasKey("CastID")
                        .HasName("pk_movie_casts");

                    b.HasIndex("MovieID")
                        .HasDatabaseName("ix_movie_casts_movie_id");

                    b.ToTable("movie_casts", (string)null);
                });

            modelBuilder.Entity("Catalog.Domain.Entities.MovieCrew", b =>
                {
                    b.Property<string>("CrewID")
                        .HasColumnType("text")
                        .HasColumnName("crew_id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("ImageURL")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("image_url");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("is_active");

                    b.Property<string>("MovieID")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("movie_id");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("title");

                    b.HasKey("CrewID")
                        .HasName("pk_movie_crews");

                    b.HasIndex("MovieID")
                        .HasDatabaseName("ix_movie_crews_movie_id");

                    b.ToTable("movie_crews", (string)null);
                });

            modelBuilder.Entity("Catalog.Domain.Entities.MovieDetail", b =>
                {
                    b.Property<string>("DetailID")
                        .HasColumnType("text")
                        .HasColumnName("detail_id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("ImageURL")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("image_url");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("is_active");

                    b.Property<string>("MovieID")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("movie_id");

                    b.Property<string>("VideoURL")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("video_url");

                    b.HasKey("DetailID")
                        .HasName("pk_movie_details");

                    b.HasIndex("MovieID")
                        .HasDatabaseName("ix_movie_details_movie_id");

                    b.ToTable("movie_details", (string)null);
                });

            modelBuilder.Entity("Catalog.Domain.Entities.MovieImage", b =>
                {
                    b.Property<string>("ImageID")
                        .HasColumnType("text")
                        .HasColumnName("image_id");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_at");

                    b.Property<string>("ImageURL")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("image_url");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("is_active");

                    b.Property<string>("MovieID")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("movie_id");

                    b.HasKey("ImageID")
                        .HasName("pk_movie_images");

                    b.HasIndex("MovieID")
                        .HasDatabaseName("ix_movie_images_movie_id");

                    b.ToTable("movie_images", (string)null);
                });

            modelBuilder.Entity("MassTransit.EntityFrameworkCoreIntegration.InboxState", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime?>("Consumed")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("consumed");

                    b.Property<Guid>("ConsumerId")
                        .HasColumnType("uuid")
                        .HasColumnName("consumer_id");

                    b.Property<DateTime?>("Delivered")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("delivered");

                    b.Property<DateTime?>("ExpirationTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("expiration_time");

                    b.Property<long?>("LastSequenceNumber")
                        .HasColumnType("bigint")
                        .HasColumnName("last_sequence_number");

                    b.Property<Guid>("LockId")
                        .HasColumnType("uuid")
                        .HasColumnName("lock_id");

                    b.Property<Guid>("MessageId")
                        .HasColumnType("uuid")
                        .HasColumnName("message_id");

                    b.Property<int>("ReceiveCount")
                        .HasColumnType("integer")
                        .HasColumnName("receive_count");

                    b.Property<DateTime>("Received")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("received");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("bytea")
                        .HasColumnName("row_version");

                    b.HasKey("Id")
                        .HasName("pk_inbox_state");

                    b.HasAlternateKey("MessageId", "ConsumerId")
                        .HasName("ak_inbox_state_message_id_consumer_id");

                    b.HasIndex("Delivered")
                        .HasDatabaseName("ix_inbox_state_delivered");

                    b.ToTable("inbox_state", (string)null);
                });

            modelBuilder.Entity("MassTransit.EntityFrameworkCoreIntegration.OutboxMessage", b =>
                {
                    b.Property<long>("SequenceNumber")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("sequence_number");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("SequenceNumber"));

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("body");

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("content_type");

                    b.Property<Guid?>("ConversationId")
                        .HasColumnType("uuid")
                        .HasColumnName("conversation_id");

                    b.Property<Guid?>("CorrelationId")
                        .HasColumnType("uuid")
                        .HasColumnName("correlation_id");

                    b.Property<string>("DestinationAddress")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("destination_address");

                    b.Property<DateTime?>("EnqueueTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("enqueue_time");

                    b.Property<DateTime?>("ExpirationTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("expiration_time");

                    b.Property<string>("FaultAddress")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("fault_address");

                    b.Property<string>("Headers")
                        .HasColumnType("text")
                        .HasColumnName("headers");

                    b.Property<Guid?>("InboxConsumerId")
                        .HasColumnType("uuid")
                        .HasColumnName("inbox_consumer_id");

                    b.Property<Guid?>("InboxMessageId")
                        .HasColumnType("uuid")
                        .HasColumnName("inbox_message_id");

                    b.Property<Guid?>("InitiatorId")
                        .HasColumnType("uuid")
                        .HasColumnName("initiator_id");

                    b.Property<Guid>("MessageId")
                        .HasColumnType("uuid")
                        .HasColumnName("message_id");

                    b.Property<string>("MessageType")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("message_type");

                    b.Property<Guid?>("OutboxId")
                        .HasColumnType("uuid")
                        .HasColumnName("outbox_id");

                    b.Property<string>("Properties")
                        .HasColumnType("text")
                        .HasColumnName("properties");

                    b.Property<Guid?>("RequestId")
                        .HasColumnType("uuid")
                        .HasColumnName("request_id");

                    b.Property<string>("ResponseAddress")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("response_address");

                    b.Property<DateTime>("SentTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("sent_time");

                    b.Property<string>("SourceAddress")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("source_address");

                    b.HasKey("SequenceNumber")
                        .HasName("pk_outbox_message");

                    b.HasIndex("EnqueueTime")
                        .HasDatabaseName("ix_outbox_message_enqueue_time");

                    b.HasIndex("ExpirationTime")
                        .HasDatabaseName("ix_outbox_message_expiration_time");

                    b.HasIndex("OutboxId", "SequenceNumber")
                        .IsUnique()
                        .HasDatabaseName("ix_outbox_message_outbox_id_sequence_number");

                    b.HasIndex("InboxMessageId", "InboxConsumerId", "SequenceNumber")
                        .IsUnique()
                        .HasDatabaseName("ix_outbox_message_inbox_message_id_inbox_consumer_id_sequence_");

                    b.ToTable("outbox_message", (string)null);
                });

            modelBuilder.Entity("MassTransit.EntityFrameworkCoreIntegration.OutboxState", b =>
                {
                    b.Property<Guid>("OutboxId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("outbox_id");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<DateTime?>("Delivered")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("delivered");

                    b.Property<long?>("LastSequenceNumber")
                        .HasColumnType("bigint")
                        .HasColumnName("last_sequence_number");

                    b.Property<Guid>("LockId")
                        .HasColumnType("uuid")
                        .HasColumnName("lock_id");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("bytea")
                        .HasColumnName("row_version");

                    b.HasKey("OutboxId")
                        .HasName("pk_outbox_state");

                    b.HasIndex("Created")
                        .HasDatabaseName("ix_outbox_state_created");

                    b.ToTable("outbox_state", (string)null);
                });

            modelBuilder.Entity("Catalog.Domain.Entities.Movie", b =>
                {
                    b.HasOne("Catalog.Domain.Entities.Category", "Category")
                        .WithMany("Movies")
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_movies_categories_category_id");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("Catalog.Domain.Entities.MovieCast", b =>
                {
                    b.HasOne("Catalog.Domain.Entities.Movie", "Movie")
                        .WithMany("MovieCasts")
                        .HasForeignKey("MovieID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_movie_casts_movies_movie_id");

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("Catalog.Domain.Entities.MovieCrew", b =>
                {
                    b.HasOne("Catalog.Domain.Entities.Movie", "Movie")
                        .WithMany("MovieCrews")
                        .HasForeignKey("MovieID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_movie_crews_movies_movie_id");

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("Catalog.Domain.Entities.MovieDetail", b =>
                {
                    b.HasOne("Catalog.Domain.Entities.Movie", "Movie")
                        .WithMany("MovieDetails")
                        .HasForeignKey("MovieID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_movie_details_movies_movie_id");

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("Catalog.Domain.Entities.MovieImage", b =>
                {
                    b.HasOne("Catalog.Domain.Entities.Movie", "Movie")
                        .WithMany("MovieImages")
                        .HasForeignKey("MovieID")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_movie_images_movies_movie_id");

                    b.Navigation("Movie");
                });

            modelBuilder.Entity("MassTransit.EntityFrameworkCoreIntegration.OutboxMessage", b =>
                {
                    b.HasOne("MassTransit.EntityFrameworkCoreIntegration.OutboxState", null)
                        .WithMany()
                        .HasForeignKey("OutboxId")
                        .HasConstraintName("fk_outbox_message_outbox_state_outbox_id");

                    b.HasOne("MassTransit.EntityFrameworkCoreIntegration.InboxState", null)
                        .WithMany()
                        .HasForeignKey("InboxMessageId", "InboxConsumerId")
                        .HasPrincipalKey("MessageId", "ConsumerId")
                        .HasConstraintName("fk_outbox_message_inbox_state_inbox_message_id_inbox_consumer_");
                });

            modelBuilder.Entity("Catalog.Domain.Entities.Category", b =>
                {
                    b.Navigation("Movies");
                });

            modelBuilder.Entity("Catalog.Domain.Entities.Movie", b =>
                {
                    b.Navigation("MovieCasts");

                    b.Navigation("MovieCrews");

                    b.Navigation("MovieDetails");

                    b.Navigation("MovieImages");
                });
#pragma warning restore 612, 618
        }
    }
}
