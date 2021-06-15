using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace testProject2.Models
{
    public partial class GPcontext : DbContext
    {
        public GPcontext()
            : base("name=GPcontext")
        {
        }

        public virtual DbSet<catalog> catalogs { get; set; }
        public virtual DbSet<comment> comments { get; set; }
        public virtual DbSet<project> projects { get; set; }
        public virtual DbSet<project_likes> project_likes { get; set; }
        public virtual DbSet<role> roles { get; set; }
        public virtual DbSet<user> users { get; set; }
        public virtual DbSet<login> logins { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<catalog>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<catalog>()
                .HasMany(e => e.projects)
                .WithOptional(e => e.catalog)
                .HasForeignKey(e => e.cat_id)
                .WillCascadeOnDelete();

            modelBuilder.Entity<comment>()
                .Property(e => e.com_content)
                .IsUnicode(false);

            modelBuilder.Entity<project>()
                .Property(e => e.title)
                .IsUnicode(false);

            modelBuilder.Entity<project>()
                .Property(e => e.bref)
                .IsUnicode(false);

            modelBuilder.Entity<project>()
                .Property(e => e.description)
                .IsUnicode(false);

            modelBuilder.Entity<project>()
                .Property(e => e.prototype)
                .IsUnicode(false);

            modelBuilder.Entity<project>()
                .HasMany(e => e.comments)
                .WithOptional(e => e.project)
                .HasForeignKey(e => e.proj_id)
                .WillCascadeOnDelete();

            modelBuilder.Entity<project>()
                .HasMany(e => e.project_likes)
                .WithRequired(e => e.project)
                .HasForeignKey(e => e.proj_id)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<role>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<role>()
                .HasMany(e => e.users)
                .WithOptional(e => e.role)
                .HasForeignKey(e => e.role_id);

            modelBuilder.Entity<user>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.name)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.address)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.phone)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.password)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .Property(e => e.graduation_year)
                .IsUnicode(false);

            modelBuilder.Entity<user>()
                .HasMany(e => e.comments)
                .WithOptional(e => e.user)
                .HasForeignKey(e => e.user_id)
                .WillCascadeOnDelete();

            modelBuilder.Entity<user>()
                .HasMany(e => e.project_likes)
                .WithRequired(e => e.user)
                .HasForeignKey(e => e.user_id);

            modelBuilder.Entity<user>()
                .HasOptional(e => e.login)
                .WithRequired(e => e.user)
                .WillCascadeOnDelete();

            modelBuilder.Entity<login>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<login>()
                .Property(e => e.password)
                .IsUnicode(false);
        }
    }
}
