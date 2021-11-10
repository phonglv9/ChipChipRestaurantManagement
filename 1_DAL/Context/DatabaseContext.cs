﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using _1_DAL.Models;

#nullable disable

namespace _1_DAL.Context
{
    public partial class DatabaseContext : DbContext
    {
        public DatabaseContext()
        {
        }

        public DatabaseContext(DbContextOptions<DatabaseContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BanAn> BanAns { get; set; }
        public virtual DbSet<CachCheBien> CachCheBiens { get; set; }
        public virtual DbSet<DanhMucFood> DanhMucFoods { get; set; }
        public virtual DbSet<DonVi> DonVis { get; set; }
        public virtual DbSet<Floor> Floors { get; set; }
        public virtual DbSet<HoaDon> HoaDons { get; set; }
        public virtual DbSet<HoaDonChiTiet> HoaDonChiTiets { get; set; }
        public virtual DbSet<MonAnChiTiet> MonAnChiTiets { get; set; }
        public virtual DbSet<NhanVien> NhanViens { get; set; }
        public virtual DbSet<NhanVienNh> NhanVienNhs { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<ThucDon> ThucDons { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=RATONLYNK\\SQLEXPRESS;Initial Catalog=QLNH;Persist Security Info=True;User ID=ratonlynkz;Password=123");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BanAn>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.Property(e => e.Busy).IsFixedLength(true);

                entity.HasOne(d => d.FloorNavigation)
                    .WithMany(p => p.BanAns)
                    .HasForeignKey(d => d.Floor)
                    .HasConstraintName("FK_BanAn_Floor");
            });

            modelBuilder.Entity<CachCheBien>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<DanhMucFood>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<DonVi>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<Floor>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<HoaDon>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.IdnhanVienNavigation)
                    .WithMany(p => p.HoaDons)
                    .HasForeignKey(d => d.IdnhanVien)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HoaDon_NhanVien");

                entity.HasOne(d => d.IdnhanVien1)
                    .WithMany(p => p.HoaDons)
                    .HasForeignKey(d => d.IdnhanVien)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HoaDon_NhanVienNH");

                entity.HasOne(d => d.IdtableNavigation)
                    .WithMany(p => p.HoaDons)
                    .HasForeignKey(d => d.Idtable)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HoaDon_BanAn");
            });

            modelBuilder.Entity<HoaDonChiTiet>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.IdbillNavigation)
                    .WithMany(p => p.HoaDonChiTiets)
                    .HasForeignKey(d => d.Idbill)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_HoaDonChiTiet_HoaDon");
            });

            modelBuilder.Entity<MonAnChiTiet>(entity =>
            {
                entity.Property(e => e.MaMon).ValueGeneratedNever();

                entity.Property(e => e.Anh).IsUnicode(false);

                entity.HasOne(d => d.IdcategoryNavigation)
                    .WithMany(p => p.MonAnChiTiets)
                    .HasForeignKey(d => d.Idcategory)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MonAnChiTiet_DanhMucFood");

                entity.HasOne(d => d.IdmethodNavigation)
                    .WithMany(p => p.MonAnChiTiets)
                    .HasForeignKey(d => d.Idmethod)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MonAnChiTiet_CachCheBien");

                entity.HasOne(d => d.IdunitNavigation)
                    .WithMany(p => p.MonAnChiTiets)
                    .HasForeignKey(d => d.Idunit)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MonAnChiTiet_DonVi");

                entity.HasOne(d => d.Idunit1)
                    .WithMany(p => p.MonAnChiTiets)
                    .HasForeignKey(d => d.Idunit)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MonAnChiTiet_HoaDonChiTiet");

                entity.HasOne(d => d.MaMonNavigation)
                    .WithOne(p => p.MonAnChiTiet)
                    .HasForeignKey<MonAnChiTiet>(d => d.MaMon)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MonAnChiTiet_ThucDon");
            });

            modelBuilder.Entity<NhanVien>(entity =>
            {
                entity.Property(e => e.MaNv).ValueGeneratedNever();

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.Password).IsUnicode(false);

                entity.Property(e => e.PhoneNo).IsUnicode(false);

                entity.HasOne(d => d.RoleNavigation)
                    .WithMany(p => p.NhanViens)
                    .HasForeignKey(d => d.Role)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NhanVien_Roles");
            });

            modelBuilder.Entity<NhanVienNh>(entity =>
            {
                entity.Property(e => e.MaNv).ValueGeneratedNever();

                entity.Property(e => e.Email).IsUnicode(false);

                entity.Property(e => e.Name).IsUnicode(false);

                entity.Property(e => e.Password).IsUnicode(false);

                entity.Property(e => e.PhoneNo).IsUnicode(false);

                entity.HasOne(d => d.RoleNavigation)
                    .WithMany(p => p.NhanVienNhs)
                    .HasForeignKey(d => d.Role)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_NhanVienNH_Roles");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            modelBuilder.Entity<ThucDon>(entity =>
            {
                entity.Property(e => e.Id).ValueGeneratedNever();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
