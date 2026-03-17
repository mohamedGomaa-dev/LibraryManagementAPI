using Library.Models.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DataAccess.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Book> Books { get; set; }
        public DbSet<BookCopy> BookCopies { get; set; }
        public DbSet<BorrowingRecord> BorrowingRecords { get; set; }
        public DbSet<Fine> Fines { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Settings> Settings { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<BorrowingRecord>()
        .HasOne(b => b.Fine)
        .WithOne(f => f.BorrowingRecord)
        .HasForeignKey<Fine>(f => f.BorrowingRecordId);
            // 1. حل تحذير الـ Decimal (تحديد 18 رقم صحيح ورقمين عشريين)
            modelBuilder.Entity<Fine>()
                .Property(f => f.FineAmount)
                .HasColumnType("decimal(18,2)");

            // 2. حل مشكلة الـ Cascade Delete
            // نخبر EF Core ألا يقوم بحذف الغرامة تلقائياً إذا تم حذف المستخدم
            modelBuilder.Entity<Fine>()
                .HasOne(f => f.User)
                .WithMany(u => u.Fines) // إذا كان لديك قائمة Fines داخل كلاس User اكتبها هنا، وإلا اتركها فارغة
                .HasForeignKey(f => f.UserId)
                .OnDelete(DeleteBehavior.NoAction); // هذا هو السطر السحري الذي يحل المشكلة!
        }
    }
}
