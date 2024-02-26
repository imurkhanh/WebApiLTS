using WA_1_1_ThemSuaXoa.Entity;

namespace WA_1_1_ThemSuaXoa.Interfaces
{
    public interface IHoaDonService
    {
        public IQueryable<HoaDon> LayHoaDon(string? keywords,
                                            int? year = null,
                                            int? month = null,
                                            DateTime? tuNgay = null,
                                            DateTime? denNgay = null,
                                            int? giaTu = null,
                                            int? giaDen = null,
                                            int pageSize = -1,
                                            int pageNumber = 1);
        public HoaDon ThemHoaDon(HoaDon hoaDon);
        public HoaDon SuaHoaDon(int hoaDonId,HoaDon hoaDon);
        public void XoaHoaDon (int hoaDonId);
        public string TaoMaGiaoDich();
    }
}
