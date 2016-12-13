using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using DAL;

namespace DAL.Migrations
{
    [DbContext(typeof(DataBaseContext))]
    [Migration("20161030144415_FirstMigration")]
    partial class FirstMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-preview1-22509");

            modelBuilder.Entity("DAL.Entities.Account", b =>
                {
                    b.Property<Guid>("PK_ID")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Balance");

                    b.Property<string>("Currency");

                    b.Property<bool>("IsDefault");

                    b.Property<string>("Name");

                    b.Property<string>("Number");

                    b.Property<string>("Type");

                    b.HasKey("PK_ID");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("DAL.Entities.Category", b =>
                {
                    b.Property<Guid>("PK_ID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("IconPath");

                    b.Property<bool>("IsDefault");

                    b.Property<string>("Name");

                    b.HasKey("PK_ID");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("DAL.Entities.Transaction", b =>
                {
                    b.Property<Guid>("PK_ID")
                        .ValueGeneratedOnAdd();

                    b.Property<Guid?>("AccountPK_ID");

                    b.Property<Guid?>("CategoryPK_ID");

                    b.Property<DateTime>("Date");

                    b.Property<DateTime?>("DateFrom");

                    b.Property<DateTime?>("DateTo");

                    b.Property<Guid>("FK_Account");

                    b.Property<Guid>("FK_Categoty");

                    b.Property<bool>("IsRepeatable")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<string>("Note");

                    b.Property<string>("RepeatType");

                    b.Property<string>("Title");

                    b.Property<double>("Value");

                    b.HasKey("PK_ID");

                    b.HasIndex("AccountPK_ID");

                    b.HasIndex("CategoryPK_ID");

                    b.ToTable("Transaction");
                });

            modelBuilder.Entity("DAL.Entities.Transaction", b =>
                {
                    b.HasOne("DAL.Entities.Account")
                        .WithMany("Transactions")
                        .HasForeignKey("AccountPK_ID");

                    b.HasOne("DAL.Entities.Category")
                        .WithMany("Transactions")
                        .HasForeignKey("CategoryPK_ID");
                });
        }
    }
}
