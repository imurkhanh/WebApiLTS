using Microsoft.EntityFrameworkCore;
using WA_1_1_ThemSuaXoa.Context;
using WA_1_1_ThemSuaXoa.Entity;
using WA_1_1_ThemSuaXoa.Interfaces;

namespace WA_1_1_ThemSuaXoa.Services
{
    public class HoaDonService : IHoaDonService
    {
        private readonly AppDbContext appDbContext;
        public HoaDonService()
        {
            appDbContext = new AppDbContext();
        }

        public IQueryable<HoaDon> LayHoaDon(string? keywords,
                                            int? year = null,
                                            int? month = null,
                                            DateTime? tuNgay = null,
                                            DateTime? denNgay = null,
                                            int? giaTu = null,
                                            int? giaDen = null,
                                            int pageSize = -1,
                                            int pageNumber = 1)
        {
            var query = appDbContext.HoaDons.Include(x=>x.ChiTietHoaDons)
                                            .OrderByDescending(x => x.ThoiGianTao)
                                            .AsQueryable();
            if(pageSize > 0)
            {
                query = query.Skip(pageSize*(pageNumber-1)).Take(pageSize).AsQueryable();
            }
            if(!string.IsNullOrEmpty(keywords))
            {
                query = query.Where(x => x.TenHoaDon.ToLower().Contains(keywords.ToLower()) || x.MaGiaoDich.ToLower().Contains(keywords.ToLower()));
            }    
            if (year.HasValue)
            {
                query = query.Where(x=>x.ThoiGianTao.Year== year);
            }
            if (month.HasValue)
            {
                query = query.Where(x => x.ThoiGianTao.Month == month);
            }
            if (tuNgay.HasValue)
            {
                query = query.Where(x => x.ThoiGianTao.Date == tuNgay.Value.Date);
            }
            if (denNgay.HasValue)
            {
                query = query.Where(x => x.ThoiGianTao.Date == denNgay.Value.Date);
            }
            if (giaTu.HasValue)
            {
                query = query.Where(x => x.TongTien >= giaTu);
            }
            if (giaDen.HasValue)
            {
                query = query.Where(x => x.TongTien <= giaTu);
            }
            return query;
        }

        public HoaDon SuaHoaDon(int hoaDonId, HoaDon hoaDon)
        {
            if (appDbContext.HoaDons.Any(x => x.Id == hoaDonId))
            {
                using (var trans = appDbContext.Database.BeginTransaction())
                {
                    if (hoaDon.ChiTietHoaDons == null || hoaDon.ChiTietHoaDons.Count() == 0)
                    {
                        var lstCTHDHienTai = appDbContext.ChiTietHoaDons.Where(x => x.HoaDonId == hoaDon.Id);
                        appDbContext.RemoveRange(lstCTHDHienTai);
                        appDbContext.SaveChanges();
                    }
                    else
                    {
                        var lstCTHDHienTai = appDbContext.ChiTietHoaDons.Where(x => x.HoaDonId == hoaDon.Id).ToList();
                        var lstCTHDDelete = new List<ChiTietHoaDon>();
                        foreach (var chiTiet in lstCTHDHienTai)
                        {
                            if (!hoaDon.ChiTietHoaDons.Any(x => x.Id == chiTiet.Id))
                            {
                                lstCTHDDelete.Add(chiTiet);
                            }
                            else
                            {
                                var chiTietMoi = hoaDon.ChiTietHoaDons.FirstOrDefault(x => x.Id == chiTiet.Id);
                                chiTiet.SanPhamId = chiTietMoi.SanPhamId;
                                chiTiet.SoLuong = chiTietMoi.SoLuong;
                                chiTiet.DVT = chiTietMoi.DVT;
                                var sanPham = appDbContext.SanPhams.FirstOrDefault(z => z.Id == chiTietMoi.SanPhamId);
                                chiTiet.ThanhTien = sanPham.GiaThanh * chiTietMoi.SoLuong;
                                appDbContext.Update(chiTiet);
                                appDbContext.SaveChanges();
                            }
                        } 
                        appDbContext.RemoveRange(lstCTHDDelete);
                        appDbContext.SaveChanges();
                        foreach (var chiTiet in hoaDon.ChiTietHoaDons)
                        {
                            if (!lstCTHDHienTai.Any(x => x.Id == chiTiet.Id))
                            {
                                chiTiet.HoaDonId = hoaDon.Id;
                                var sanPham = appDbContext.SanPhams.FirstOrDefault(z => z.Id == chiTiet.SanPhamId);
                                chiTiet.ThanhTien = sanPham.GiaThanh * chiTiet.SoLuong;
                                appDbContext.Add(chiTiet);
                                appDbContext.SaveChanges();
                            }
                        }
                    }
                    var tongTienMoi = appDbContext.ChiTietHoaDons.Where(x => x.HoaDonId == hoaDon.Id).Sum(x => x.ThanhTien);
                    hoaDon.TongTien = tongTienMoi;
                    hoaDon.ThoiGianCapNhat = DateTime.Now;
                    hoaDon.ChiTietHoaDons = null;
                    appDbContext.Update(hoaDon);
                    appDbContext.SaveChanges();
                    trans.Commit();
                    return hoaDon;
                }
            }
            else
            {
                throw new Exception("Hoa don khong ton tai!");
            }
        }

        public string TaoMaGiaoDich()
        {
            var res = DateTime.Now.ToString("yyyyMMdd") + "_";
            var countSoGiaoDichHomNay = appDbContext.HoaDons.Count(x => x.ThoiGianTao.Date == DateTime.Now.Date);
            if (countSoGiaoDichHomNay > 0)
            {
                int temp = countSoGiaoDichHomNay + 1;
                if (temp < 10)
                {
                    return res + "00" + temp.ToString();
                }
                else if (temp < 100)
                {
                    return res + "0" + temp.ToString();
                }
                else return res + temp.ToString();
            }
            else { return res + "001"; }
        }

        public HoaDon ThemHoaDon(HoaDon hoaDon)
        {
            using (var trans = appDbContext.Database.BeginTransaction())
            {
                hoaDon.ThoiGianTao = DateTime.Now;
                hoaDon.MaGiaoDich = TaoMaGiaoDich();
                var lstChiTietHoaDon = hoaDon.ChiTietHoaDons;
                hoaDon.ChiTietHoaDons = null;
                appDbContext.Add(hoaDon);
                appDbContext.SaveChanges();
                foreach (var chiTiet in lstChiTietHoaDon)
                {
                    if (appDbContext.SanPhams.Any(x => x.Id == chiTiet.SanPhamId))
                    {
                        chiTiet.HoaDonId = hoaDon.Id;
                        var sanPham = appDbContext.SanPhams.FirstOrDefault(x => x.Id == chiTiet.SanPhamId);
                        chiTiet.ThanhTien = chiTiet.SoLuong * sanPham.GiaThanh;
                        appDbContext.Add(chiTiet);
                        appDbContext.SaveChanges();

                    }
                    else throw new Exception("San pham khong ton tai");

                }
                hoaDon.TongTien = lstChiTietHoaDon.Sum(x => x.ThanhTien);
                appDbContext.SaveChanges();
                trans.Commit();
                return hoaDon;
            }
        }

        public void XoaHoaDon(int hoaDonId)
        {
            if (appDbContext.HoaDons.Any(x => x.Id == hoaDonId))
            {
                using (var trans = appDbContext.Database.BeginTransaction())
                {
                    var lstCTHDHienTai = appDbContext.ChiTietHoaDons.Where(x => x.HoaDonId == hoaDonId);
                    appDbContext.RemoveRange(lstCTHDHienTai);
                    appDbContext.SaveChanges();
                    var hoaDon = appDbContext.HoaDons.Find(hoaDonId);
                    appDbContext.Remove(hoaDon);
                    appDbContext.SaveChanges();
                    trans.Commit();

                }
            }
            else
            {
                throw new Exception("Hoa don khong ton tai!");
            }
        }
    }
}
