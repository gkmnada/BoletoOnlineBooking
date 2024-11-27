using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Catalog.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CreateDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "categories",
                columns: table => new
                {
                    category_id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    slug_url = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_categories", x => x.category_id);
                });

            migrationBuilder.CreateTable(
                name: "inbox_state",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    message_id = table.Column<Guid>(type: "uuid", nullable: false),
                    consumer_id = table.Column<Guid>(type: "uuid", nullable: false),
                    lock_id = table.Column<Guid>(type: "uuid", nullable: false),
                    row_version = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    received = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    receive_count = table.Column<int>(type: "integer", nullable: false),
                    expiration_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    consumed = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    delivered = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    last_sequence_number = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_inbox_state", x => x.id);
                    table.UniqueConstraint("ak_inbox_state_message_id_consumer_id", x => new { x.message_id, x.consumer_id });
                });

            migrationBuilder.CreateTable(
                name: "outbox_state",
                columns: table => new
                {
                    outbox_id = table.Column<Guid>(type: "uuid", nullable: false),
                    lock_id = table.Column<Guid>(type: "uuid", nullable: false),
                    row_version = table.Column<byte[]>(type: "bytea", rowVersion: true, nullable: true),
                    created = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    delivered = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    last_sequence_number = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_outbox_state", x => x.outbox_id);
                });

            migrationBuilder.CreateTable(
                name: "movies",
                columns: table => new
                {
                    movie_id = table.Column<string>(type: "text", nullable: false),
                    movie_name = table.Column<string>(type: "text", nullable: false),
                    genre = table.Column<List<string>>(type: "text[]", nullable: false),
                    language = table.Column<List<string>>(type: "text[]", nullable: false),
                    duration = table.Column<string>(type: "text", nullable: false),
                    release_date = table.Column<string>(type: "text", nullable: false),
                    rating = table.Column<int>(type: "integer", nullable: false),
                    audience_score = table.Column<int>(type: "integer", nullable: false),
                    image_url = table.Column<string>(type: "text", nullable: false),
                    slug_url = table.Column<string>(type: "text", nullable: false),
                    category_id = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_movies", x => x.movie_id);
                    table.ForeignKey(
                        name: "fk_movies_categories_category_id",
                        column: x => x.category_id,
                        principalTable: "categories",
                        principalColumn: "category_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "outbox_message",
                columns: table => new
                {
                    sequence_number = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    enqueue_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    sent_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    headers = table.Column<string>(type: "text", nullable: true),
                    properties = table.Column<string>(type: "text", nullable: true),
                    inbox_message_id = table.Column<Guid>(type: "uuid", nullable: true),
                    inbox_consumer_id = table.Column<Guid>(type: "uuid", nullable: true),
                    outbox_id = table.Column<Guid>(type: "uuid", nullable: true),
                    message_id = table.Column<Guid>(type: "uuid", nullable: false),
                    content_type = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: false),
                    message_type = table.Column<string>(type: "text", nullable: false),
                    body = table.Column<string>(type: "text", nullable: false),
                    conversation_id = table.Column<Guid>(type: "uuid", nullable: true),
                    correlation_id = table.Column<Guid>(type: "uuid", nullable: true),
                    initiator_id = table.Column<Guid>(type: "uuid", nullable: true),
                    request_id = table.Column<Guid>(type: "uuid", nullable: true),
                    source_address = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    destination_address = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    response_address = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    fault_address = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    expiration_time = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_outbox_message", x => x.sequence_number);
                    table.ForeignKey(
                        name: "fk_outbox_message_inbox_state_inbox_message_id_inbox_consumer_",
                        columns: x => new { x.inbox_message_id, x.inbox_consumer_id },
                        principalTable: "inbox_state",
                        principalColumns: new[] { "message_id", "consumer_id" });
                    table.ForeignKey(
                        name: "fk_outbox_message_outbox_state_outbox_id",
                        column: x => x.outbox_id,
                        principalTable: "outbox_state",
                        principalColumn: "outbox_id");
                });

            migrationBuilder.CreateTable(
                name: "movie_casts",
                columns: table => new
                {
                    cast_id = table.Column<string>(type: "text", nullable: false),
                    cast_name = table.Column<string>(type: "text", nullable: false),
                    character = table.Column<string>(type: "text", nullable: false),
                    image_url = table.Column<string>(type: "text", nullable: false),
                    movie_id = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_movie_casts", x => x.cast_id);
                    table.ForeignKey(
                        name: "fk_movie_casts_movies_movie_id",
                        column: x => x.movie_id,
                        principalTable: "movies",
                        principalColumn: "movie_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "movie_crews",
                columns: table => new
                {
                    crew_id = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    title = table.Column<string>(type: "text", nullable: false),
                    image_url = table.Column<string>(type: "text", nullable: false),
                    movie_id = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_movie_crews", x => x.crew_id);
                    table.ForeignKey(
                        name: "fk_movie_crews_movies_movie_id",
                        column: x => x.movie_id,
                        principalTable: "movies",
                        principalColumn: "movie_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "movie_details",
                columns: table => new
                {
                    detail_id = table.Column<string>(type: "text", nullable: false),
                    image_url = table.Column<string>(type: "text", nullable: false),
                    video_url = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: false),
                    movie_id = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_movie_details", x => x.detail_id);
                    table.ForeignKey(
                        name: "fk_movie_details_movies_movie_id",
                        column: x => x.movie_id,
                        principalTable: "movies",
                        principalColumn: "movie_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "movie_images",
                columns: table => new
                {
                    image_id = table.Column<string>(type: "text", nullable: false),
                    image_url = table.Column<string>(type: "text", nullable: false),
                    movie_id = table.Column<string>(type: "text", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_movie_images", x => x.image_id);
                    table.ForeignKey(
                        name: "fk_movie_images_movies_movie_id",
                        column: x => x.movie_id,
                        principalTable: "movies",
                        principalColumn: "movie_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "ix_inbox_state_delivered",
                table: "inbox_state",
                column: "delivered");

            migrationBuilder.CreateIndex(
                name: "ix_movie_casts_movie_id",
                table: "movie_casts",
                column: "movie_id");

            migrationBuilder.CreateIndex(
                name: "ix_movie_crews_movie_id",
                table: "movie_crews",
                column: "movie_id");

            migrationBuilder.CreateIndex(
                name: "ix_movie_details_movie_id",
                table: "movie_details",
                column: "movie_id");

            migrationBuilder.CreateIndex(
                name: "ix_movie_images_movie_id",
                table: "movie_images",
                column: "movie_id");

            migrationBuilder.CreateIndex(
                name: "ix_movies_category_id",
                table: "movies",
                column: "category_id");

            migrationBuilder.CreateIndex(
                name: "ix_outbox_message_enqueue_time",
                table: "outbox_message",
                column: "enqueue_time");

            migrationBuilder.CreateIndex(
                name: "ix_outbox_message_expiration_time",
                table: "outbox_message",
                column: "expiration_time");

            migrationBuilder.CreateIndex(
                name: "ix_outbox_message_inbox_message_id_inbox_consumer_id_sequence_",
                table: "outbox_message",
                columns: new[] { "inbox_message_id", "inbox_consumer_id", "sequence_number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_outbox_message_outbox_id_sequence_number",
                table: "outbox_message",
                columns: new[] { "outbox_id", "sequence_number" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_outbox_state_created",
                table: "outbox_state",
                column: "created");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "movie_casts");

            migrationBuilder.DropTable(
                name: "movie_crews");

            migrationBuilder.DropTable(
                name: "movie_details");

            migrationBuilder.DropTable(
                name: "movie_images");

            migrationBuilder.DropTable(
                name: "outbox_message");

            migrationBuilder.DropTable(
                name: "movies");

            migrationBuilder.DropTable(
                name: "inbox_state");

            migrationBuilder.DropTable(
                name: "outbox_state");

            migrationBuilder.DropTable(
                name: "categories");
        }
    }
}
