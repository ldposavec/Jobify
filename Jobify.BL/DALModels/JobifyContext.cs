using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Jobify;

namespace Jobify.BL.DALModels;

public partial class JobifyContext : DbContext
{
    public JobifyContext()
    {
    }

    public JobifyContext(DbContextOptions<JobifyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Admin> Admins { get; set; }

    public virtual DbSet<Employer> Employers { get; set; }

    public virtual DbSet<Firm> Firms { get; set; }

    public virtual DbSet<JobAd> JobAds { get; set; }

    public virtual DbSet<JobApp> JobApps { get; set; }

    public virtual DbSet<JobOffer> JobOffers { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserType> UserTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //=> optionsBuilder.UseNpgsql("name=ConnectionStrings:AppConnStr");
        //=> optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Username=postgres;Password=postgres;Database=Jobify");
        => optionsBuilder.UseNpgsql("Host=truly-universal-spitz.data-1.use1.tembo.io;Port=5432;Username=postgres;Password=y2csSQsBqEyAi4c6").UseLazyLoadingProxies();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("admin_pkey");

            entity.ToTable("admin");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Admins)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("admin_user_id_fkey");
        });

        modelBuilder.Entity<Employer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("employer_pkey");

            entity.ToTable("employer");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.FirmId).HasColumnName("firm_id");
            entity.Property(e => e.Position)
                .HasMaxLength(100)
                .HasColumnName("position");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.Firm).WithMany(p => p.Employers)
                .HasForeignKey(d => d.FirmId)
                .HasConstraintName("employer_firm_id_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Employers)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("employer_user_id_fkey");
        });

        modelBuilder.Entity<Firm>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("firm_pkey");

            entity.ToTable("firm");

            entity.HasIndex(e => e.Oib, "firm_oib_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Address)
                .HasMaxLength(255)
                .HasColumnName("address");
            entity.Property(e => e.FirmName)
                .HasMaxLength(200)
                .HasColumnName("firm_name");
            entity.Property(e => e.Industry)
                .HasMaxLength(100)
                .HasColumnName("industry");
            entity.Property(e => e.Oib)
                .HasMaxLength(11)
                .HasColumnName("oib");
        });

        modelBuilder.Entity<JobAd>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("job_ad_pkey");

            entity.ToTable("job_ad");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.EmployerId).HasColumnName("employer_id");
            entity.Property(e => e.Salary)
                .HasPrecision(12, 2)
                .HasColumnName("salary");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.Title)
                .HasMaxLength(255)
                .HasColumnName("title");

            entity.HasOne(d => d.Employer).WithMany(p => p.JobAds)
                .HasForeignKey(d => d.EmployerId)
                .HasConstraintName("job_ad_employer_id_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.JobAds)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("job_ad_status_id_fkey");
        });

        modelBuilder.Entity<JobApp>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("job_app_pkey");

            entity.ToTable("job_app");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.CvFilepath)
                .HasMaxLength(255)
                .HasColumnName("cv_filepath");
            entity.Property(e => e.JobAdId).HasColumnName("job_ad_id");
            entity.Property(e => e.StatusId).HasColumnName("status_id");
            entity.Property(e => e.StudentId).HasColumnName("student_id");

            entity.HasOne(d => d.JobAd).WithMany(p => p.JobApps)
                .HasForeignKey(d => d.JobAdId)
                .HasConstraintName("job_app_job_ad_id_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.JobApps)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("job_app_status_id_fkey");

            entity.HasOne(d => d.Student).WithMany(p => p.JobApps)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("job_app_student_id_fkey");
        });

        modelBuilder.Entity<JobOffer>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("job_offer_pkey");

            entity.ToTable("job_offer");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Date)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("date");
            entity.Property(e => e.JobAppId).HasColumnName("job_app_id");
            entity.Property(e => e.StatusId).HasColumnName("status_id");

            entity.HasOne(d => d.JobApp).WithMany(p => p.JobOffers)
                .HasForeignKey(d => d.JobAppId)
                .HasConstraintName("job_offer_job_app_id_fkey");

            entity.HasOne(d => d.Status).WithMany(p => p.JobOffers)
                .HasForeignKey(d => d.StatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("job_offer_status_id_fkey");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("notification_pkey");

            entity.ToTable("notification");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("created_at");
            entity.Property(e => e.Message).HasColumnName("message");
            entity.Property(e => e.JobAppId).HasColumnName("job_app_id");
            entity.Property(e => e.UserId).HasColumnName("user_id");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("notification_user_id_fkey");
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("status_pkey");

            entity.ToTable("status");

            entity.HasIndex(e => e.Name, "status_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("student_pkey");

            entity.ToTable("student");

            entity.HasIndex(e => e.Jmbag, "student_jmbag_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.AverageGrade)
                .HasPrecision(3, 2)
                .HasColumnName("average_grade");
            entity.Property(e => e.Jmbag)
                .HasMaxLength(10)
                .HasColumnName("jmbag");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.YearOfStudy).HasColumnName("year_of_study");

            entity.HasOne(d => d.User).WithMany(p => p.Students)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("student_user_id_fkey");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_pkey");

            entity.ToTable("user");

            entity.HasIndex(e => e.Mail, "user_mail_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.IsEmailVerified)
                .HasDefaultValue(false)
                .HasColumnName("is_email_verified");
            entity.Property(e => e.Mail)
                .HasMaxLength(150)
                .HasColumnName("mail");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .HasColumnName("password");
            entity.Property(e => e.Surname)
                .HasMaxLength(100)
                .HasColumnName("surname");
            entity.Property(e => e.UserTypeId).HasColumnName("user_type_id");

            entity.HasOne(d => d.UserType).WithMany(p => p.Users)
                .HasForeignKey(d => d.UserTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("user_user_type_id_fkey");
        });

        modelBuilder.Entity<UserType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("user_type_pkey");

            entity.ToTable("user_type");

            entity.HasIndex(e => e.Name, "user_type_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
