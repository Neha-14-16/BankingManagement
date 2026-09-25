using System.Data.Entity;
using BankingManagement.Models;

namespace BankingManagement.Data
{
    public class BankingDbContext : DbContext
    {
        public BankingDbContext()
            : base(ConnectionString.GetConnectionString())
        {
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Customer> Customers { get; set; }

        public DbSet<Account> Accounts { get; set; }

        public DbSet<Beneficiary> Beneficiaries { get; set; }

        public DbSet<BankingTransaction> BankingTransactions { get; set; }

        public DbSet<PaymentTransaction> PaymentTransactions { get; set; }

        public DbSet<BankBranch> BankBranches { get; set; }

        public DbSet<AccountTransaction> AccountTransactions { get; set; }


        protected override void OnModelCreating(
            DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            // Customer -> User
            modelBuilder.Entity<Customer>()
                .HasRequired(c => c.User)
                .WithMany()
                .HasForeignKey(c => c.UserId)
                .WillCascadeOnDelete(false);


            // Customer -> Account
            modelBuilder.Entity<Account>()
                .HasRequired(a => a.Customer)
                .WithMany()
                .HasForeignKey(a => a.CustomerId)
                .WillCascadeOnDelete(false);


            // Customer -> Beneficiary
            modelBuilder.Entity<Beneficiary>()
                .HasRequired(b => b.Customer)
                .WithMany()
                .HasForeignKey(b => b.CustomerId)
                .WillCascadeOnDelete(false);


            // Account -> PaymentTransaction
            modelBuilder.Entity<PaymentTransaction>()
                .HasRequired(p => p.Account)
                .WithMany()
                .HasForeignKey(p => p.AccountId)
                .WillCascadeOnDelete(false);


            // BankingTransaction -> FromAccount
            modelBuilder.Entity<BankingTransaction>()
                .HasRequired(t => t.FromAccount)
                .WithMany()
                .HasForeignKey(t => t.FromAccountId)
                .WillCascadeOnDelete(false);


            // BankingTransaction -> ToAccount
            modelBuilder.Entity<BankingTransaction>()
                .HasRequired(t => t.ToAccount)
                .WithMany()
                .HasForeignKey(t => t.ToAccountId)
                .WillCascadeOnDelete(false);


            // Account -> AccountTransaction
            modelBuilder.Entity<AccountTransaction>()
                .HasRequired(t => t.Account)
                .WithMany()
                .HasForeignKey(t => t.AccountId)
                .WillCascadeOnDelete(false);
        }
    }
}