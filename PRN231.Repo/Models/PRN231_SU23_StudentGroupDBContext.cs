﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PRN231.Repo.Models
{
    public partial class PRN231_SU23_StudentGroupDBContext : DbContext
    {
        public PRN231_SU23_StudentGroupDBContext()
        {
        }

        public PRN231_SU23_StudentGroupDBContext(DbContextOptions<PRN231_SU23_StudentGroupDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Student> Students { get; set; } = null!;
        public virtual DbSet<StudentGroup> StudentGroups { get; set; } = null!;
        public virtual DbSet<UserRole> UserRoles { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=.;Uid=sa;Pwd=Password.1;Database=PRN231_SU23_StudentGroupDB;Persist Security Info=True;Encrypt=False");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Student>(entity =>
            {
                entity.ToTable("Student");

                entity.Property(e => e.DateOfBirth).HasColumnType("datetime");

                entity.Property(e => e.Email).HasMaxLength(250);

                entity.Property(e => e.FullName).HasMaxLength(250);

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Students)
                    .HasForeignKey(d => d.GroupId)
                    .HasConstraintName("FK__Student__GroupId__3C69FB99");
            });

            modelBuilder.Entity<StudentGroup>(entity =>
            {
                entity.ToTable("StudentGroup");

                entity.Property(e => e.Code).HasMaxLength(10);

                entity.Property(e => e.GroupName).HasMaxLength(250);
            });

            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.ToTable("UserRole");

                entity.HasIndex(e => e.Username, "UQ__UserRole__536C85E4D479F026")
                    .IsUnique();

                entity.Property(e => e.Passphrase)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.UserRole1).HasColumnName("UserRole");

                entity.Property(e => e.Username).HasMaxLength(250);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
