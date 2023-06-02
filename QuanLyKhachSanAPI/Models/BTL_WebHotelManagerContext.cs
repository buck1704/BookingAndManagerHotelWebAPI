using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace QuanLyKhachSanAPI.Models
{
    public partial class BTL_WebHotelManagerContext : DbContext
    {
        public BTL_WebHotelManagerContext()
        {
        }

        public BTL_WebHotelManagerContext(DbContextOptions<BTL_WebHotelManagerContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Chitietdichvu> Chitietdichvus { get; set; } = null!;
        public virtual DbSet<Chitietphong> Chitietphongs { get; set; } = null!;
        public virtual DbSet<Dichvu> Dichvus { get; set; } = null!;
        public virtual DbSet<DouongViewModel> Douongs { get; set; } = null!;
        public virtual DbSet<MonchinhViewModel> Monchinhs { get; set; } = null!;
        public virtual DbSet<MonkhaiviViewModel> Monkhaivis { get; set; } = null!;
        public virtual DbSet<MonnoibatViewModel> Monnoibats { get; set; } = null!;
        public virtual DbSet<MontrangmiengViewModel> Montrangmiengs { get; set; } = null!;
        public virtual DbSet<Phieudatphong> Phieudatphongs { get; set; } = null!;
        public virtual DbSet<Phieudichvu> Phieudichvus { get; set; } = null!;
        public virtual DbSet<Phong> Phongs { get; set; } = null!;
        public virtual DbSet<Quanlytaikhoan> Quanlytaikhoans { get; set; } = null!;
        public virtual DbSet<SetbuffetViewModel> Setbuffets { get; set; } = null!;
        public virtual DbSet<Taikhoanadmin> Taikhoanadmins { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=IdeaPad\\SQLEXPRESS;Database=BTL_WebHotelManager;User=sa;Password=sa;");

            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Chitietdichvu>(entity =>
            {
                entity.HasKey(e => e.MaCtdv)
                    .HasName("PK__chitietd__1E4E40E62D5E259F");

                entity.ToTable("chitietdichvu");

                entity.Property(e => e.MaCtdv).HasColumnName("MaCTDV");

                entity.Property(e => e.Mpdv).HasColumnName("MPDV");
                entity.Property(e => e.MaDV).HasColumnName("MaDV");

                entity.HasOne(d => d.MpdvNavigation)
                    .WithMany(p => p.Chitietdichvus)
                    .HasForeignKey(d => d.Mpdv)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("malk5");
                entity.HasOne(d => d.MaDVNavigation)
                    .WithMany(p => p.Chitietdichvus)
                    .HasForeignKey(d => d.MaDV)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("malk6");
            });

            modelBuilder.Entity<Chitietphong>(entity =>
            {
                entity.ToTable("chitietphong");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.DienTich).HasMaxLength(50);

                entity.Property(e => e.IdPhong).HasColumnName("ID_Phong");

                entity.Property(e => e.Img)
                    .HasMaxLength(500)
                    .HasColumnName("IMG");

                entity.Property(e => e.LoaiGiuong).HasMaxLength(50);

                entity.Property(e => e.Mota).HasMaxLength(500);

                entity.Property(e => e.TamNhin).HasMaxLength(50);

                entity.Property(e => e.TenPhong).HasMaxLength(50);

                entity.HasOne(d => d.IdPhongNavigation)
                    .WithMany(p => p.Chitietphongs)
                    .HasForeignKey(d => d.IdPhong)
                    .HasConstraintName("malk1");
            });

            modelBuilder.Entity<Dichvu>(entity =>
            {
                entity.ToTable("dichvu");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.TenDichVu).HasMaxLength(50);
            });

            modelBuilder.Entity<DouongViewModel>(entity =>
            {
                entity.ToTable("douong");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Gia).HasMaxLength(50);

                entity.Property(e => e.Img)
                    .HasMaxLength(50)
                    .HasColumnName("IMG");

                entity.Property(e => e.TenDoUong).HasMaxLength(50);
            });

            modelBuilder.Entity<MonchinhViewModel>(entity =>
            {
                entity.ToTable("monchinh");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Gia).HasMaxLength(50);

                entity.Property(e => e.Img)
                    .HasMaxLength(50)
                    .HasColumnName("IMG");

                entity.Property(e => e.TenMon).HasMaxLength(50);
            });

            modelBuilder.Entity<MonkhaiviViewModel>(entity =>
            {
                entity.ToTable("monkhaivi");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Gia).HasMaxLength(50);

                entity.Property(e => e.Img)
                    .HasMaxLength(50)
                    .HasColumnName("IMG");

                entity.Property(e => e.TenMon).HasMaxLength(50);
            });

            modelBuilder.Entity<MonnoibatViewModel>(entity =>
            {
                entity.ToTable("monnoibat");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Gia).HasMaxLength(50);

                entity.Property(e => e.Img)
                    .HasMaxLength(50)
                    .HasColumnName("IMG");

                entity.Property(e => e.TenMon).HasMaxLength(50);
            });

            modelBuilder.Entity<MontrangmiengViewModel>(entity =>
            {
                entity.ToTable("montrangmieng");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Gia).HasMaxLength(50);

                entity.Property(e => e.Img)
                    .HasMaxLength(50)
                    .HasColumnName("IMG");

                entity.Property(e => e.TenMon).HasMaxLength(50);
            });

            modelBuilder.Entity<Phieudatphong>(entity =>
            {
                entity.HasKey(e => e.MaDp)
                    .HasName("PK__phieudat__27258669C2D43C0C");

                entity.ToTable("phieudatphong");

                entity.Property(e => e.MaDp).HasColumnName("MaDP");

                entity.Property(e => e.IdKh).HasColumnName("ID_KH");

                entity.Property(e => e.Idphong).HasColumnName("IDPhong");

                entity.Property(e => e.NgayDen).HasColumnType("datetime");

                entity.Property(e => e.NgayDi).HasColumnType("datetime");

                entity.Property(e => e.NgayTt).HasColumnType("datetime");

                entity.Property(e => e.PhuongThucThanhToan).HasMaxLength(100);

                entity.HasOne(d => d.IdKhNavigation)
                    .WithMany(p => p.Phieudatphongs)
                    .HasForeignKey(d => d.IdKh)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("malk2");

                entity.HasOne(d => d.IdphongNavigation)
                    .WithMany(p => p.Phieudatphongs)
                    .HasForeignKey(d => d.Idphong)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("malk3");
            });

            modelBuilder.Entity<Phieudichvu>(entity =>
            {
                entity.HasKey(e => e.Mpdv)
                    .HasName("PK__phieudic__6FF4522F13B0B995");

                entity.ToTable("phieudichvu");

                entity.Property(e => e.Mpdv).HasColumnName("MPDV");

                entity.Property(e => e.MaDp).HasColumnName("MaDP");

                entity.HasOne(d => d.MaDpNavigation)
                    .WithMany(p => p.Phieudichvus)
                    .HasForeignKey(d => d.MaDp)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("malk4");
            });

            modelBuilder.Entity<Phong>(entity =>
            {
                entity.ToTable("phong");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.LoaiPhong).HasMaxLength(50);
            });

            modelBuilder.Entity<Quanlytaikhoan>(entity =>
            {
                entity.ToTable("quanlytaikhoan");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cmnd)
                    .HasMaxLength(50)
                    .HasColumnName("CMND");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.HoTen).HasMaxLength(100);

                entity.Property(e => e.PassWord).HasMaxLength(50);

                entity.Property(e => e.Sdt)
                    .HasMaxLength(50)
                    .HasColumnName("SDT");
            });

            modelBuilder.Entity<SetbuffetViewModel>(entity =>
            {
                entity.ToTable("setbuffet");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Gia).HasMaxLength(50);

                entity.Property(e => e.Img)
                    .HasMaxLength(50)
                    .HasColumnName("IMG");

                entity.Property(e => e.TenSet).HasMaxLength(50);
            });

            modelBuilder.Entity<Taikhoanadmin>(entity =>
            {
                entity.ToTable("taikhoanadmin");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Email).HasMaxLength(50);

                entity.Property(e => e.HoTen).HasMaxLength(50);

                entity.Property(e => e.PassWord).HasMaxLength(50);

                entity.Property(e => e.Sdt)
                    .HasMaxLength(50)
                    .HasColumnName("SDT");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
