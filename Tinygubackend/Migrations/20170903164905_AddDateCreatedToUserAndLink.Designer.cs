using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Tinygubackend;
using Tinygubackend.Contexts;

namespace Tinygubackend.Migrations
{
    [DbContext(typeof(TinyguContext))]
    [Migration("20170903164905_AddDateCreatedToUserAndLink")]
    partial class AddDateCreatedToUserAndLink
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("Tinygubackend.Models.Link", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<string>("LongUrl");

                    b.Property<int?>("OwnerId");

                    b.Property<string>("ShortUrl");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Links");
                });

            modelBuilder.Entity("Tinygubackend.Models.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<string>("Name");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Tinygubackend.Models.Link", b =>
                {
                    b.HasOne("Tinygubackend.Models.User", "Owner")
                        .WithMany("Links")
                        .HasForeignKey("OwnerId");
                });
        }
    }
}
