using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using QuanLyKhachSanAPI.Models;
using System.Linq;
using System.Runtime.Intrinsics.Arm;
using static System.Net.WebRequestMethods;

namespace QuanLyKhachSanAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelManagerController : Controller
    {
        BTL_WebHotelManagerContext ctx;

        public HotelManagerController(BTL_WebHotelManagerContext ctx)
        {
            this.ctx = ctx;
        }

        #region Tài Khoản
        [HttpGet("login/{email}/{password}")]
        public async Task<ActionResult<Quanlytaikhoan>> CheckLogin(string email, string password)
        {
            var result = await ctx.Quanlytaikhoans.FirstOrDefaultAsync(em => (em.Email == email || em.Sdt == email) && em.PassWord == password);

            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        [HttpPost("register")]
        public async Task<IActionResult> CheckDangKy(Quanlytaikhoan addtaikhoan)
        {
            if (await ctx.Quanlytaikhoans.AnyAsync(q => q.Email == addtaikhoan.Email))
            {
                return BadRequest(new { error = "Email đã được sử dụng" });
            }

            if (await ctx.Quanlytaikhoans.AnyAsync(q => q.Sdt == addtaikhoan.Sdt))
            {
                return BadRequest(new { error = "Số điện thoại đã được sử dụng" });
            }
            if (string.IsNullOrEmpty(addtaikhoan.HoTen) || string.IsNullOrEmpty(addtaikhoan.Sdt) || string.IsNullOrEmpty(addtaikhoan.Email) || string.IsNullOrEmpty(addtaikhoan.Cmnd))
            {
                return BadRequest(new { error = "Vui lòng nhập đầy dủ thông tin!" });
            }

            var dangky = new Quanlytaikhoan()
            {
                HoTen = addtaikhoan.HoTen,
                Sdt = addtaikhoan.Sdt,
                Email = addtaikhoan.Email,
                Cmnd = addtaikhoan.Cmnd,
                PassWord = addtaikhoan.PassWord
            };
            await ctx.Quanlytaikhoans.AddAsync(dangky);
            await ctx.SaveChangesAsync();
            return Ok(dangky);
        }

        [HttpPut("suataikhoan/{ten}/{sdt}/{email}/{cmnd}/{pass}")]

        public async Task<IActionResult> SuaTaiKhoan(string ten, string sdt, string email, string cmnd, string pass)
        {
            var cn = await ctx.Quanlytaikhoans.Where(em => em.Sdt == sdt || em.Email == email).ToListAsync();

            if (cn.Count > 1)
            {
                return BadRequest(new { error = "Số điện thoại hoặc email đã tồn tại" });
            }

            else
            {
                cn[0].HoTen = ten;
                cn[0].Sdt = sdt;
                cn[0].Email = email;
                cn[0].Cmnd = cmnd;
                cn[0].PassWord = pass;
                await ctx.SaveChangesAsync();
                return Ok(cn);
            }
        }

        [HttpPut("doimatkhau/{sdt}/{mkm}/{nlmk}")]
        public async Task<IActionResult> Doimatkhau(string sdt, string mkm, string nlmk)
        {
            var account = await ctx.Quanlytaikhoans.FirstOrDefaultAsync(q => q.Sdt == sdt);

            if (account == null)
            {
                return BadRequest(new { error = "Vui lòng nhập lại số điện thoại" });
            }

            if (mkm != nlmk)
            {
                return BadRequest(new { error = "Mật khẩu mới và nhập lại mật khẩu không giống nhau, vui lòng nhập lại." });
            }

            account.PassWord = mkm;

            await ctx.SaveChangesAsync();

            return Ok();
        }
        #endregion

        #region Dịch Vụ
        [HttpGet("GetAllDichVu")]
        public async Task<ActionResult<Dichvu>> GetAllDichVu()
        {
            var dichvu = await ctx.Dichvus.ToListAsync();

            return Ok(dichvu);

        }

        [HttpGet("GetDichVu/{Id}")]
        public async Task<IActionResult> GetDichVu(string Id)
        {
            var result = await ctx.Dichvus.FindAsync(Id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }
        #endregion

        #region Phòng
        [HttpGet("GetAllPhong")]
        public async Task<ActionResult<Phong>> GetAllPhong()
        {
            var phong = await ctx.Phongs.ToListAsync();

            return Ok(phong);

        }


        [HttpGet("chitietphong/{Id}")]
        public async Task<IActionResult> GetChiTietPhong(int Id)
        {
            var result = await ctx.Chitietphongs.FindAsync(Id);
            if (result == null)
            {
                return NotFound();
            }
            return Ok(result);
        }

        [HttpGet("GetPhong/{maphong}/{soluongnguoi}")]
        public IActionResult GetRooms(int maphong, int soluongnguoi)
        {
            var rooms = (from a in ctx.Phongs
                         join b in ctx.Chitietphongs on a.Id equals b.IdPhong
                         where a.Id == maphong && b.NguoiMax >= soluongnguoi && b.TinhTrang == 0
                         select b).ToList();

            if (rooms.Count() == 0)
            {
                return NotFound();
            }

            return Ok(rooms);
        }



        [HttpGet("GetAllChiTietPhong")]
        public async Task<ActionResult<Chitietphong>> GetAllChiTietPhong()
        {
            var chitietphong = await ctx.Chitietphongs.ToListAsync();

            return Ok(chitietphong);

        }
        #endregion

        #region Phiếu đặt phòng
        [HttpPost("themphieudatphong")]
        public async Task<IActionResult> Phieudatphong(Phieudatphong cttt)
        {
            var phieudatphong = new Phieudatphong()
            {
                Idphong = cttt.Idphong,
                IdKh = cttt.IdKh,
                PhuongThucThanhToan = cttt.PhuongThucThanhToan,
                NgayDen = cttt.NgayDen,
                NgayDi = cttt.NgayDi,
                NgayTt = cttt.NgayTt
            };
            var khachHang = await ctx.Quanlytaikhoans.FindAsync(cttt.IdKh);
            if (khachHang != null)
            {
                phieudatphong.IdKhNavigation = khachHang;
            }
            var idphong = await ctx.Chitietphongs.FindAsync(cttt.Idphong);
            if (khachHang != null)
            {
                phieudatphong.IdphongNavigation = idphong;
            }
            await ctx.Phieudatphongs.AddAsync(phieudatphong);
            await ctx.SaveChangesAsync();
            int maPhong = phieudatphong.MaDp;
            return Ok(new { Message = "Chúc bạn có nghỉ vui vẻ", MaPhong = maPhong });

        }
        #endregion

        #region Phiếu dịch vụ
        [HttpPost("themphieudichvu")]
        public async Task<IActionResult> PhieuDichVu(Phieudichvu pdv)
        {
            var phieudichvu = new Phieudichvu()
            {

                MaDp = pdv.MaDp,
                TongTien = pdv.TongTien
            };

            await ctx.Phieudichvus.AddAsync(phieudichvu);
            await ctx.SaveChangesAsync();
            int maPDV = phieudichvu.Mpdv;
            return Ok(new { MaPhieuDichVu = maPDV });

        }
        #endregion

        #region Chi tiết dịch vụ

        [HttpPost("themchitietdichvu")]
        public async Task<IActionResult> ChitietDichVu(List<Chitietdichvu> ctdvList)
        {

            foreach (var ctdv in ctdvList)
            {
                var chitietdichvu = new Chitietdichvu()
                {
                    Mpdv = ctdv.Mpdv,
                    MaDV = ctdv.MaDV
                };
                var Mpdv = await ctx.Phieudichvus.FindAsync(ctdv.Mpdv);
                if (Mpdv != null)
                {
                    chitietdichvu.MpdvNavigation = Mpdv;
                }
                var MaDV = await ctx.Dichvus.FindAsync(ctdv.MaDV);
                if (MaDV != null)
                {
                    chitietdichvu.MaDVNavigation = MaDV;
                }

                ctx.Chitietdichvus.Add(chitietdichvu);
            }

            await ctx.SaveChangesAsync();

            return Ok();
        }

        #endregion


        [HttpPut("CapNhapPhong/{Id}")]
        public async Task<IActionResult> CapNhapPhong(int Id, bool Huyphong=true)
        {
            var cn = await ctx.Chitietphongs.FindAsync(Id);

            if (cn == null)
            {
                return NotFound();
            }
            if (Huyphong == true)
            {
                cn.TinhTrang = 0;
            }
            else
            {
                cn.TinhTrang = 1;
            }

            await ctx.SaveChangesAsync();
            return Ok();
        }

        #region Kiểm tra phòng
        [HttpGet("GetKiemTraPhong/{ma}/{sdt}")]
        public IActionResult GetPhieuDatPhong(int ma, string sdt)
        {
            var phieudatphong = (from pdp in ctx.Phieudatphongs
                                 join tk in ctx.Quanlytaikhoans on pdp.IdKh equals tk.Id
                                 join ctp in ctx.Chitietphongs on pdp.Idphong equals ctp.Id
                                 join pdv in ctx.Phieudichvus on pdp.MaDp equals pdv.MaDp
                                 join p in ctx.Phongs on ctp.IdPhong equals p.Id
                                 where tk.Sdt == sdt && pdp.MaDp == ma
                                 select new
                                 {
                                     pdp.MaDp,
                                     tk.HoTen,
                                     tk.Sdt,
                                     tk.Email,
                                     ctp.Id,
                                     ctp.TenPhong,
                                     ctp.Img,
                                     p.LoaiPhong,
                                     ctp.GiaPhong,
                                     pdp.NgayDen,
                                     pdp.NgayDi,
                                     pdp.PhuongThucThanhToan,
                                     SoTienPhaiTra = (pdp.NgayDi - pdp.NgayDen).TotalDays * ctp.GiaPhong + pdv.TongTien
                                 }).FirstOrDefault();

            if (phieudatphong == null)
            {
                return NotFound();
            }

            return Ok(phieudatphong);
        }
        #endregion

        [HttpGet("Lichsuphong/{Id}")]
        public IActionResult Lichsuphong(int Id)
        {

            var rooms = (from a in ctx.Phieudatphongs
                         join b in ctx.Chitietphongs on a.Idphong equals b.Id
                         join c in ctx.Phieudichvus on a.MaDp equals c.MaDp
                         join d in ctx.Chitietdichvus on c.Mpdv equals d.Mpdv into ctdv
                         from cate2 in ctdv.DefaultIfEmpty()
                         join e in ctx.Dichvus on cate2.MaDV equals e.Id into dv
                         from cate3 in dv.DefaultIfEmpty()
                         where a.IdKh == Id
                         group new { cate3.TenDichVu, cate3.DonGia } by new
                         {
                             a.MaDp,
                             b.TenPhong,
                             b.GiaPhong,
                             a.NgayDen,
                             a.NgayDi,
                             a.NgayTt,
                             a.PhuongThucThanhToan,
                             b.Img
                         } into g
                         select new
                         {
                             g.Key.MaDp,
                             g.Key.TenPhong,
                             g.Key.NgayDen,
                             g.Key.NgayDi,
                             g.Key.NgayTt,
                             g.Key.PhuongThucThanhToan,
                             g.Key.Img,
                             TienPhong = (g.Key.NgayDi - g.Key.NgayDen).TotalDays * g.Key.GiaPhong,
                             DichVu = g.Select(x => new
                             {
                                 TenDichVu = x.TenDichVu != null ? x.TenDichVu : "",
                                 DonGia = x.DonGia != null ? x.DonGia : 0
                             }).ToList()
                         }).ToList();

            if (rooms.Count() == 0)
            {
                return NotFound();
            }
            return Ok(rooms);
        }

        [HttpDelete("HuyDatPhong/{MaDP}")]

        public async Task<IActionResult> Delete(int MaDP)
        {
            
            var phieuDatPhong = await ctx.Phieudatphongs.FirstOrDefaultAsync(em => em.MaDp == MaDP);
            if (phieuDatPhong == null)
            {
                return NotFound();
            }
            ctx.Phieudatphongs.Remove(phieuDatPhong);

            var phieuDichVu = await ctx.Phieudichvus.FirstOrDefaultAsync(em => em.MaDp == MaDP);
            ctx.Phieudichvus.Remove(phieuDichVu);

            var chiTietDichVuList = await ctx.Chitietdichvus.Where(cd => cd.Mpdv == phieuDichVu.Mpdv).ToListAsync();
            ctx.Chitietdichvus.RemoveRange(chiTietDichVuList);

            await ctx.SaveChangesAsync();

            return Ok();
        }


    }

}
