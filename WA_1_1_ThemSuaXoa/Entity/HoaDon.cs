namespace WA_1_1_ThemSuaXoa.Entity
{
    public class HoaDon
    {
        public int Id { get; set; }
        public int? KhachHangId { get; set; }
        public string TenHoaDon { get; set; }
        public string? MaGiaoDich { get; set; }
        public string GhiChu {  get; set; }
        public DateTime ThoiGianTao { get; set; }
        public DateTime? ThoiGianCapNhat { get; set; }
        public double? TongTien {  get; set; }
        public virtual KhachHang? KhachHang { get; set; }
        public virtual IEnumerable<ChiTietHoaDon>? ChiTietHoaDons { get; set; }
    }
}
