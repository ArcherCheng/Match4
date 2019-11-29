using System;
using MatchApi.Helper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MatchApi.Data
{
    public partial class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AppDataLog> AppDataLog { get; set; }
        public virtual DbSet<AppKeyValue> AppKeyValue { get; set; }
        public virtual DbSet<AppUser> AppUser { get; set; }
        public virtual DbSet<AppUserLog> AppUserLog { get; set; }
        public virtual DbSet<Liker> Liker { get; set; }
        public virtual DbSet<Member> Member { get; set; }
        public virtual DbSet<MemberCondition> MemberCondition { get; set; }
        public virtual DbSet<MemberDetail> MemberDetail { get; set; }
        public virtual DbSet<MemberPhoto> MemberPhoto { get; set; }
        public virtual DbSet<Message> Message { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                string connString = AppSettingsHelper.Configuration["ConnectionStrings:DefaultConnection"];
                optionsBuilder.UseSqlServer(connString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<AppDataLog>(entity =>
            {
                entity.Property(e => e.TableName)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.WriteIp).HasMaxLength(30);

                entity.Property(e => e.WriteTime).HasColumnType("datetime");

                entity.Property(e => e.WriteUser).HasMaxLength(30);
            });

            modelBuilder.Entity<AppKeyValue>(entity =>
            {
                entity.HasIndex(e => new { e.AppGroup, e.AppKey })
                    .HasName("AppKeyValue_Uindex1")
                    .IsUnique();

                entity.Property(e => e.AppGroup)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.AppKey)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.AppValue)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.WriteIp).HasMaxLength(30);

                entity.Property(e => e.WriteTime).HasColumnType("datetime");

                entity.Property(e => e.WriteUser).HasMaxLength(30);
            });

            modelBuilder.Entity<AppUser>(entity =>
            {
                entity.HasIndex(e => e.Email)
                    .HasName("AppUser_Idx1")
                    .IsUnique();

                entity.HasIndex(e => e.Phone)
                    .HasName("AppUser_Idx2")
                    .IsUnique();

                entity.HasIndex(e => e.UserName)
                    .HasName("AppUser_Idx3")
                    .IsUnique();

                entity.Property(e => e.Birthday).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.IsInWork)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.LoginDate).HasColumnType("datetime");

                entity.Property(e => e.MainPhotoUrl).HasMaxLength(250);

                entity.Property(e => e.PasswordHash).HasMaxLength(2000);

                entity.Property(e => e.PasswordSalt).HasMaxLength(2000);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.UserRole).HasMaxLength(30);

                entity.Property(e => e.WriteIp).HasMaxLength(30);

                entity.Property(e => e.WriteTime).HasColumnType("datetime");

                entity.Property(e => e.WriteUser).HasMaxLength(30);
            });

            modelBuilder.Entity<AppUserLog>(entity =>
            {
                entity.Property(e => e.Destination).HasMaxLength(255);

                entity.Property(e => e.Method).HasMaxLength(30);

                entity.Property(e => e.QueryString).HasMaxLength(255);

                entity.Property(e => e.Refer).HasMaxLength(255);

                entity.Property(e => e.WriteIp).HasMaxLength(30);

                entity.Property(e => e.WriteTime).HasColumnType("datetime");

                entity.Property(e => e.WriteUser).HasMaxLength(30);
            });

            modelBuilder.Entity<Liker>(entity =>
            {
                entity.HasKey(e => new { e.SendId, e.LikerId })
                    .HasName("liker_Pkey");

                entity.Property(e => e.AddedDate).HasColumnType("datetime");

                entity.Property(e => e.DeleteDate).HasColumnType("datetime");

                entity.Property(e => e.WriteIp).HasMaxLength(30);

                entity.Property(e => e.WriteTime).HasColumnType("datetime");

                entity.Property(e => e.WriteUser).HasMaxLength(30);

                entity.HasOne(d => d.LikerNavigation)
                    .WithMany(p => p.LikerLikerNavigation)
                    .HasForeignKey(d => d.LikerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Liker_LikerMe");

                entity.HasOne(d => d.Send)
                    .WithMany(p => p.LikerSend)
                    .HasForeignKey(d => d.SendId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Liker_MyLiker");
            });

            modelBuilder.Entity<Member>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("Member_Pkey");

                entity.HasIndex(e => e.Email)
                    .HasName("Member_index1")
                    .IsUnique();

                entity.HasIndex(e => e.Phone)
                    .HasName("Member_index2")
                    .IsUnique();

                entity.Property(e => e.ActiveDate).HasColumnType("datetime");

                entity.Property(e => e.Blood).HasMaxLength(2);

                entity.Property(e => e.City).HasMaxLength(30);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.JobType).HasMaxLength(30);

                entity.Property(e => e.LoginDate).HasColumnType("datetime");

                entity.Property(e => e.MainPhotoUrl).HasMaxLength(250);

                entity.Property(e => e.NickName)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.Property(e => e.PasswordHash).HasMaxLength(2000);

                entity.Property(e => e.PasswordSalt).HasMaxLength(2000);

                entity.Property(e => e.Phone)
                    .IsRequired()
                    .HasMaxLength(15);

                entity.Property(e => e.Religion).HasMaxLength(30);

                entity.Property(e => e.School).HasMaxLength(30);

                entity.Property(e => e.Star).HasMaxLength(30);

                entity.Property(e => e.Subjects).HasMaxLength(30);

                entity.Property(e => e.UserRoles).HasMaxLength(30);

                entity.Property(e => e.WriteIp).HasMaxLength(30);

                entity.Property(e => e.WriteTime).HasColumnType("datetime");

                entity.Property(e => e.WriteUser).HasMaxLength(30);
            });

            modelBuilder.Entity<MemberCondition>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("MemberCondition_Pkey");

                entity.Property(e => e.UserId).ValueGeneratedNever();

                entity.Property(e => e.BloodInclude).HasMaxLength(15);

                entity.Property(e => e.CityInclude).HasMaxLength(120);

                entity.Property(e => e.JobTypeInclude).HasMaxLength(120);

                entity.Property(e => e.ReligionInclude).HasMaxLength(120);

                entity.Property(e => e.StarInclude).HasMaxLength(120);

                entity.Property(e => e.WriteIp).HasMaxLength(30);

                entity.Property(e => e.WriteTime).HasColumnType("datetime");

                entity.Property(e => e.WriteUser).HasMaxLength(30);

                entity.HasOne(d => d.User)
                    .WithOne(p => p.MemberCondition)
                    .HasForeignKey<MemberCondition>(d => d.UserId)
                    .HasConstraintName("MemberCondition_Member");
            });

            modelBuilder.Entity<MemberDetail>(entity =>
            {
                entity.HasKey(e => e.UserId)
                    .HasName("MemberDetail_Pkey");

                entity.Property(e => e.WriteTime).HasColumnType("datetime");

                entity.Property(e => e.WriteIp).HasMaxLength(30);

                entity.Property(e => e.WriteUser).HasMaxLength(30);

                entity.HasOne(d => d.User)
                    .WithOne(p => p.MemberDetail)
                    .HasForeignKey<MemberDetail>(d => d.UserId)
                    .HasConstraintName("MemberDetail_Member");
            });

            modelBuilder.Entity<MemberPhoto>(entity =>
            {
                entity.Property(e => e.AddedDate).HasColumnType("datetime");

                entity.Property(e => e.Descriptions).HasMaxLength(250);

                entity.Property(e => e.PhotoUrl).HasMaxLength(250);

                entity.Property(e => e.PublicId).HasMaxLength(250);

                entity.Property(e => e.WriteIp).HasMaxLength(30);

                entity.Property(e => e.WriteTime).HasColumnType("datetime");

                entity.Property(e => e.WriteUser).HasMaxLength(30);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.MemberPhoto)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("MemberPhoto_Member");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasIndex(e => new { e.RecipientId, e.SenderId })
                    .HasName("message_index2");

                entity.HasIndex(e => new { e.SenderId, e.RecipientId })
                    .HasName("message_index1");

                entity.Property(e => e.Contents)
                    .IsRequired()
                    .HasMaxLength(1000);

                entity.Property(e => e.ReadDate).HasColumnType("datetime");

                entity.Property(e => e.SendDate).HasColumnType("datetime");

                entity.Property(e => e.WriteIp).HasMaxLength(30);

                entity.Property(e => e.WriteTime).HasColumnType("datetime");

                entity.Property(e => e.WriteUser).HasMaxLength(30);

                entity.HasOne(d => d.Recipient)
                    .WithMany(p => p.MessageRecipient)
                    .HasForeignKey(d => d.RecipientId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Message_Recipient");

                entity.HasOne(d => d.Sender)
                    .WithMany(p => p.MessageSender)
                    .HasForeignKey(d => d.SenderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("Message_Sender");
            });
        }
    }
}
