using Microsoft.EntityFrameworkCore;
using WA_1_1_ThemSuaXoa.Entity;

namespace WA_1_1_ThemSuaXoa.Context
{
    public class AppDbContext:DbContext
    {
        public virtual DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }
        public virtual DbSet<HoaDon> HoaDons { get; set; }
        public virtual DbSet<KhachHang> KhachHangs { get; set; }
        public virtual DbSet<LoaiSanPham> LoaiSanPhams { get; set; }
        public virtual DbSet<SanPham> SanPhams { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-V5E7153\\SQLEXPRESS;Database=WA_1_1;Trusted_Connection = True;TrustServerCertificate=True");
        }
    }
}