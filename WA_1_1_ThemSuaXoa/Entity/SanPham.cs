namespace WA_1_1_ThemSuaXoa.Entity
{
    public class SanPham
    {
        public int Id { get; set; }
        public int LoaiSanPhamId { get; set; }
        public string TenSanPham { get; set; }
        public double GiaThanh { get; set; }
        public string MoTa { get; set; }
        public DateTime NgayHetHan { get; set; }
        public bool DaHetHan { get;set; }
        public int? SoLuongTonKho {  get; set; }
        public string KyHieuSanPham { get; set; }
        public virtual LoaiSanPham? LoaiSanPham { get; set; }
    }
}
