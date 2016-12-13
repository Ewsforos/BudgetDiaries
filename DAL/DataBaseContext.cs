using DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace DAL
{
    public class DataBaseContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Transaction> Templates { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=DataBase.db");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>().Property(a => a.PK_ID).IsRequired();

            modelBuilder.Entity<Category>().Property(c => c.PK_ID).IsRequired();

            modelBuilder.Entity<Transaction>().Property(t => t.PK_ID).IsRequired();
            modelBuilder.Entity<Transaction>().Property(t => t.IsRepeatable).HasDefaultValue<bool>(false);
        }
    }
}
