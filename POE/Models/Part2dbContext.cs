using System;
using Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using POE.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

#nullable disable

namespace POE.Models
{
    public partial class Part2dbContext : DbContext
    {
        public Part2dbContext()
        {
        }

        public Part2dbContext(DbContextOptions<Part2dbContext> options)
            : base(options)
        {
        }

        public DbSet<LecturerClaim> LecturerClaims { get; set; }
        public DbSet<AdminUsers> adminUsers { get; set; }
        public DbSet<Lecturer> Lecturers { get; set; }
       





        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=ClaimeDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            }

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<LecturerClaim>(entity =>
            {
                entity.HasKey(e => e.Id);

                entity.Property(e => e.Id)
                    .ValueGeneratedNever(); // Assuming GUID is manually set

                entity.Property(e => e.LecturerName)
                    .IsRequired()
                    .HasMaxLength(100); // Specify max length if required

                entity.Property(e => e.HoursWorked)
                    .IsRequired()
                    .HasColumnType("decimal(5,2)"); // Define precision

                entity.Property(e => e.HourlyRate)
                    .IsRequired()
                    .HasColumnType("decimal(7,2)"); // Define precision

                entity.Property(e => e.TotalAmount)
                    .IsRequired()
                    .HasColumnType("decimal(10,2)"); // Define precision

                entity.Property(e => e.Notes)
                    .HasMaxLength(500); // Optional with a max length

                entity.Property(e => e.SubmissionDate)
                    .IsRequired()
                    .HasColumnType("datetime");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasDefaultValue("Pending");

                entity.Property(e => e.DocumentPath)
                    .HasMaxLength(255)
                    .IsRequired(false); // Allow null values
            });
;


            modelBuilder.Entity<AdminUsers>(entity =>
            {
                entity.HasKey(e => e.UserId);  // Primary key

                entity.Property(e => e.UserId)
                    .ValueGeneratedNever();  // GUID is manually generated

                entity.Property(e => e.UserNames)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.UserPassword)
                    .IsRequired()
                    .HasMaxLength(50);


            });
            modelBuilder.Entity<Lecturer>(entity =>
            {
                modelBuilder.Entity<Lecturer>()
                  .HasKey(l => l.LecturerId);

                modelBuilder.Entity<Lecturer>()
                    .Property(l => l.LecturerId)
                    .ValueGeneratedNever();

                entity.Property(e => e.Name)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.Email)
                      .IsRequired()
                      .HasMaxLength(100);

                entity.Property(e => e.ContactNumber)
                      .HasMaxLength(20);

                entity.Property(e => e.Address)
                      .HasMaxLength(200);
            });






            ////modelBuilder.Entity<User>(entity =>
            ////{
            ////    entity.HasKey(e => e.UserId)
            ////        .HasName("PK__Authoris__CB9A1CFF3EF736A0");

            ////    entity.Property(e => e.UserId)
            ////        .ValueGeneratedNever()
            ////        .HasColumnName("userId");

            ////    entity.Property(e => e.UserNames)
            ////        .IsRequired()
            ////        .HasMaxLength(20)
            ////        .HasColumnName("user_names");

            ////    entity.Property(e => e.UserPassword)
            ////        .IsRequired()
            ////        .HasMaxLength(20)
            ////        .HasColumnName("user_password");
            ////});

            ////modelBuilder.Entity<Donation>(entity =>
            ////{
            ////    entity.HasKey(e => e.DonationId)
            ////        .HasName("PK__Donation__C5082EFB3256748D");

            ////    entity.Property(e => e.DonationId)
            ////        .ValueGeneratedNever();  // GUID, so no automatic generation

            ////    entity.Property(e => e.UserId)  // 'Guid' type for userId
            ////        .HasColumnName("userId");

            ////    entity.Property(e => e.DonationDate)
            ////        .HasColumnType("datetime");

            ////    entity.Property(e => e.DonationDescription)
            ////        .IsRequired()
            ////        .HasMaxLength(800);

            ////    entity.Property(e => e.DonationDonor)
            ////        .IsRequired()
            ////        .HasMaxLength(50);

            ////    entity.HasOne(d => d.User)
            ////        .WithMany(p => p.DonationOfGoods)
            ////        .HasForeignKey(d => d.UserId)  // Ensure it's GUID for the FK
            ////        .OnDelete(DeleteBehavior.ClientSetNull)
            ////        .HasConstraintName("FK_DonationOfGoods_UserId");
            ////});

            ////modelBuilder.Entity<VolunteerUser>(entity =>
            ////{
            ////    entity.HasKey(e => e.VolunteerId);  // Primary key

            ////    entity.Property(e => e.VolunteerId)
            ////        .ValueGeneratedNever();  // GUID is manually generated

            ////    entity.Property(e => e.UserNames)
            ////        .IsRequired()
            ////        .HasMaxLength(50);

            ////    entity.Property(e => e.UserPassword)
            ////        .IsRequired()
            ////        .HasMaxLength(50);


            ////});

            ////modelBuilder.Entity<MyTask>(entity =>
            ////{
            ////    // Primary Key
            ////    entity.HasKey(e => e.TaskId)
            ////          .HasName("PK_MyTasks");

            ////    // TaskId is a GUID
            ////    entity.Property(e => e.TaskId)
            ////          .IsRequired()
            ////          .ValueGeneratedNever();  // GUID is generated elsewhere, not by the database

            ////    // DisasterId is a foreign key to ActiveDisasters table
            ////    entity.Property(e => e.DisasterId)
            ////          .IsRequired();

            ////    // TaskType with a max length of 50 characters
            ////    entity.Property(e => e.TaskType)
            ////          .IsRequired()
            ////          .HasMaxLength(50);

            ////    // DateCreated
            ////    entity.Property(e => e.DateCreated)
            ////          .IsRequired();

            ////    // Status with a max length of 50 characters
            ////    entity.Property(e => e.Status)
            ////          .IsRequired()
            ////          .HasMaxLength(50);

            ////    // Foreign key relation to ActiveDisasters
            ////    entity.HasOne(t => t.Incident)
            ////          .WithMany(ad => ad.MyTasks)  // Assuming ActiveDisaster has a collection of MyTasks
            ////          .HasForeignKey(t => t.DisasterId)
            ////          .OnDelete(DeleteBehavior.Cascade)
            ////          .HasConstraintName("FK_Tasks_ActiveDisasters");
            ////});
            ////modelBuilder.Entity<VolunteerTask>(entity =>
            ////{

            ////    // Define the primary key
            ////    entity.HasKey(e => e.VolunteerTaskId)
            ////        .HasName("PK_VolunteerTasks");

            ////    // Set the Status property configuration
            ////    entity.Property(e => e.Status)
            ////        .IsRequired()
            ////        .HasMaxLength(50);

            ////    // Define foreign key relationship with VolunteerUser
            ////    entity.HasOne(vt => vt.VolunteerUser)
            ////        .WithMany(v => v.VolunteerTasks)  // One VolunteerUser can have many VolunteerTasks
            ////        .HasForeignKey(vt => vt.VolunteerId)
            ////        .OnDelete(DeleteBehavior.ClientSetNull)
            ////        .HasConstraintName("FK_VolunteerTasks_VolunteerUsers");

            ////    // Define foreign key relationship with Task
            ////    entity.HasOne(vt => vt.MyTask)
            ////        .WithMany(t => t.VolunteerTasks)  // One Task can have many VolunteerTasks
            ////        .HasForeignKey(vt => vt.TaskId)
            ////        .OnDelete(DeleteBehavior.ClientSetNull)
            ////        .HasConstraintName("FK_VolunteerTasks_Tasks");
            ////});




            //modelBuilder.Entity<DonationOfGoodsCategory>(entity =>
            //{
            //    entity.HasKey(e => e.DonationCategoryId)
            //        .HasName("PK__Donation__4EA6D65E62F14EE5");

            //    entity.ToTable("DonationOfGoodsCategory");

            //    entity.Property(e => e.DonationCategoryId).ValueGeneratedNever();

            //    entity.Property(e => e.DonationCategoryName).HasMaxLength(50);
            //});

            //modelBuilder.Entity<DonationsOfMoney>(entity =>
            //{
            //    entity.HasKey(e => e.DonationId)
            //        .HasName("PK__Donation__F7F4F4135D8AF1F5");

            //    entity.ToTable("DonationsOfMoney");

            //    entity.Property(e => e.DonationId)
            //        .ValueGeneratedNever()
            //        .HasColumnName("donationId");

            //    entity.Property(e => e.DonatedDate).HasColumnType("date");

            //    entity.Property(e => e.DonationDonor)
            //        .IsRequired()
            //        .HasMaxLength(30);
            //});

            //modelBuilder.Entity<PurchasesOfGood>(entity =>
            //{
            //    entity.HasKey(e => e.PurchaseId)
            //        .HasName("PK__Purchase__6B0A6BBE0038B372");

            //    entity.Property(e => e.PurchaseId).ValueGeneratedNever();

            //    entity.Property(e => e.DateOfPurchase).HasColumnType("datetime");

            //    entity.Property(e => e.DonationOfGoodsCategoryId).HasColumnName("DonationOfGoodsCategoryID");

            //    entity.HasOne(d => d.Disaster)
            //        .WithMany(p => p.PurchasesOfGoods)
            //        .HasForeignKey(d => d.DisasterId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK__Purchases__Disas__6D0D32F4");

            //    entity.HasOne(d => d.DonationOfGoodsCategory)
            //        .WithMany(p => p.PurchasesOfGoods)
            //        .HasForeignKey(d => d.DonationOfGoodsCategoryId)
            //        .OnDelete(DeleteBehavior.ClientSetNull)
            //        .HasConstraintName("FK__Purchases__Donat__6C190EBB");
            //});

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
