// Application Database Context
namespace pg_onion.Infrastructure.Data
{
    using Domain.Entities;
    using Microsoft.EntityFrameworkCore;

    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<FinancialAccount> FinancialAccounts { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<ItemOfExpenses> ItemOfExpenses { get; set; }
        public DbSet<Budget> Budgets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User Configuration
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Email).IsRequired().HasMaxLength(255);
                entity.Property(e => e.FullName).IsRequired().HasMaxLength(255);
                entity.HasIndex(e => e.Email).IsUnique();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
                
                entity.HasMany(e => e.FinancialAccounts)
                    .WithOne(a => a.User)
                    .HasForeignKey(a => a.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Financial Account Configuration
            modelBuilder.Entity<FinancialAccount>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.AccountName).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Currency).IsRequired().HasMaxLength(3);
                entity.Property(e => e.Balance).HasPrecision(18, 2);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
                
                entity.HasOne(e => e.User)
                    .WithMany(u => u.FinancialAccounts)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.Transactions)
                    .WithOne(t => t.FinancialAccount)
                    .HasForeignKey(t => t.AccountId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasMany(e => e.Budgets)
                    .WithOne(b => b.FinancialAccount)
                    .HasForeignKey(b => b.FinancialAccountId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Transaction Configuration
            modelBuilder.Entity<Transaction>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Amount).HasPrecision(18, 2).IsRequired();
                entity.Property(e => e.TransactionDate).IsRequired();
                entity.Property(e => e.Description).HasMaxLength(500);
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
                
                entity.HasOne(e => e.FinancialAccount)
                    .WithMany(a => a.Transactions)
                    .HasForeignKey(e => e.AccountId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.ItemOfExpenses)
                    .WithMany(i => i.Transactions)
                    .HasForeignKey(e => e.ItemOfExpensesId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => new { e.AccountId, e.TransactionDate });
            });

            // ItemOfExpenses Configuration
            modelBuilder.Entity<ItemOfExpenses>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(255);
                entity.Property(e => e.Type).IsRequired();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
                
                entity.HasMany(e => e.Transactions)
                    .WithOne(t => t.ItemOfExpenses)
                    .HasForeignKey(t => t.ItemOfExpensesId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasMany(e => e.Budgets)
                    .WithOne(b => b.ItemOfExpenses)
                    .HasForeignKey(b => b.ItemOfExpensesId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => e.Type);
            });

            // Budget Configuration
            modelBuilder.Entity<Budget>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Amount).HasPrecision(18, 2).IsRequired();
                entity.Property(e => e.SpentAmount).HasPrecision(18, 2).IsRequired().HasDefaultValue(0);
                entity.Property(e => e.StartDate).IsRequired();
                entity.Property(e => e.EndDate).IsRequired();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("GETUTCDATE()");
                entity.Property(e => e.UpdatedAt).HasDefaultValueSql("GETUTCDATE()");
                
                entity.HasOne(e => e.FinancialAccount)
                    .WithMany(a => a.Budgets)
                    .HasForeignKey(e => e.FinancialAccountId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(e => e.ItemOfExpenses)
                    .WithMany(i => i.Budgets)
                    .HasForeignKey(e => e.ItemOfExpensesId)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(e => new { e.ItemOfExpensesId, e.StartDate, e.EndDate });
            });
        }
    }
}
