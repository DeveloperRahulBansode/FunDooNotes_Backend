using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAcessLayer.Entity;
using Microsoft.EntityFrameworkCore;

namespace DataAcessLayer.Context
{
    public class UserDBContext : DbContext
    {
        public UserDBContext(DbContextOptions<UserDBContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Notes> Notes { get; set; }
        public DbSet<Label> Labels { get; set; }
        public DbSet<NoteLabel> NoteLabels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ✅ Unique Email Constraint for User
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // ✅ User-Notes (One-to-Many)
            modelBuilder.Entity<Notes>()
                .HasOne(n => n.User)
                .WithMany(u => u.Notes)
                .HasForeignKey(n => n.UserID)
                .OnDelete(DeleteBehavior.Cascade); // ✅ If a User is deleted, all Notes will also be deleted.

            // ✅ Many-to-Many Relationship (Notes <-> Labels)
            modelBuilder.Entity<NoteLabel>()
                .HasKey(nl => new { nl.NotesId, nl.LabelId }); // ✅ Composite Primary Key

            modelBuilder.Entity<NoteLabel>()
                .HasOne(nl => nl.Note)
                .WithMany(n => n.NoteLabels)
                .HasForeignKey(nl => nl.NotesId)
                .OnDelete(DeleteBehavior.NoAction); // Prevent cascading delete on Notes

            modelBuilder.Entity<NoteLabel>()
            .HasOne(nl => nl.Note)
            .WithMany(n => n.NoteLabels)
            .HasForeignKey(nl => nl.NotesId)
            .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<NoteLabel>()
                .HasOne(nl => nl.Label)
                .WithMany(l => l.NoteLabels)
                .HasForeignKey(nl => nl.LabelId)
                .OnDelete(DeleteBehavior.Cascade); // If a Label is deleted, its NoteLabels will be deleted
        }





    }
}
