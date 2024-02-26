using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WA_1_1_ThemSuaXoa.Entity;
using WA_1_1_ThemSuaXoa.Interfaces;
using WA_1_1_ThemSuaXoa.Services;

namespace WA_1_1_ThemSuaXoa.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class HoaDonController : ControllerBase
    {
        private readonly IHoaDonService hoaDonService;
        public HoaDonController()
        {
            hoaDonService = new HoaDonService();
        }

        [HttpGet]
        public IActionResult LayHoaDon(string? keywords,
                                       int? year = null,
                                       int? month = null,
                                       DateTime? tuNgay = null,
                                       DateTime? denNgay = null,
                                       int? giaTu = null,
                                       int? giaDen = null,
                                       int pageSize = -1,
                                       int pageNumber = 1)
        {
            var res = hoaDonService.LayHoaDon(keywords,year,month,tuNgay,denNgay,giaTu,giaDen,pageSize,pageNumber);
            return Ok(res);
        }

        [HttpPost]
        public IActionResult ThemHoaDon(HoaDon hoaDon)
        {
            var res = hoaDonService.ThemHoaDon(hoaDon);
            return Ok(res);
        }
        
        [HttpPut("{hoaDonId}")]
        public IActionResult SuaHoaDon(int hoaDonId,HoaDon hoaDon)
        {
            var res = hoaDonService.SuaHoaDon(hoaDonId,hoaDon);
            return Ok(res);
        }
        
        [HttpDelete("{hoaDonId}")]
        public IActionResult XoaHoaDon(int hoaDonId)
        {
            hoaDonService.XoaHoaDon(hoaDonId);
            return Ok();
        }
    }
}
