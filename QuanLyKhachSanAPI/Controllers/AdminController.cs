using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QuanLyKhachSanAPI.Models;

namespace QuanLyKhachSanAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        BTL_WebHotelManagerContext ctx;

        public AdminController(BTL_WebHotelManagerContext ctx)
        {
            this.ctx = ctx;
        }
        public static string url_anh = "C:\\Users\\84336\\source\\repos\\QuanLyKhachSanAPI\\QuanLyKhachSanAPI\\QuanLyKhachSanClient\\wwwroot\\img\\new_pic";
        [HttpGet("login/{email}/{password}")]
        public async Task<ActionResult<Taikhoanadmin>> CheckLogin(string email, string password)
        {
            var result = await ctx.Taikhoanadmins.FirstOrDefaultAsync(em => (em.Email == email || em.Sdt == email) && em.PassWord == password);

            if (result == null)
            {
                return NotFound();
            }
            return result;
        }

        #region user

        [HttpGet("GetUser")]
        public async Task<ActionResult<Quanlytaikhoan>> GetUser()
        {
            var thongtin_user = await ctx.Quanlytaikhoans.ToListAsync();
            return Ok(thongtin_user);
        }

        [HttpGet("GetUser/{Id}")]
        public async Task<ActionResult<List<Quanlytaikhoan>>> GetUser(int id)
        {
            var result = await ctx.Quanlytaikhoans.Where(ee => ee.Id == id).ToListAsync();
            if (result == null)
            {
                return NotFound("Không tìm thấy khách hàng có id = " + id);
            }
            return Ok(result);
        }

        [HttpPost("User_Update/{id}/{hoten}/{sdt}/{email}/{cmnd}/{password}")]
        public async Task<bool> UpdateById(int id, string hoten, string sdt, string email, string cmnd, string password)
        {
            var taikhoan = await ctx.Quanlytaikhoans.SingleOrDefaultAsync(t => t.Id == id);
            if (taikhoan == null)
            {
                return false;
            }
            taikhoan.HoTen = hoten;
            taikhoan.Sdt = sdt;
            taikhoan.Email = email;
            taikhoan.Cmnd = cmnd;
            taikhoan.PassWord = password;
            try
            {
                return await ctx.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }

        [HttpPost("SignUp")]
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

        [HttpDelete("DeleteUser/{id}")]
        public async Task<ActionResult<bool>> DeleteUser(int id)
        {
            using (var ctx = new BTL_WebHotelManagerContext())
            {
                var taikhoan = await ctx.Quanlytaikhoans.FindAsync(id);
                if (taikhoan == null)
                    return NotFound();

                ctx.Quanlytaikhoans.Remove(taikhoan);
                var isDeleted = await ctx.SaveChangesAsync() > 0;
                return Ok(isDeleted);
            }
        }
        #endregion

        #region room

        [HttpPost("Create_Room")]
        public async Task<IActionResult> PostWithImageAsync([FromForm] Img_Chitietphong model)
        {
            var findP = ctx.Chitietphongs.Find(model.Id);
            if (findP != null)
            {
                return Ok("đã tồn tại phòng này");
            }
            else
            {
                var addroom = new Chitietphong
                {
                    IdPhong = model.IdPhong,
                    TenPhong = model.TenPhong,
                    NguoiMax = model.NguoiMax,
                    LoaiGiuong = model.LoaiGiuong,
                    GiaPhong = model.GiaPhong,
                    TinhTrang = model.TinhTrang,
                    DienTich = model.DienTich,
                    TamNhin = model.TamNhin,
                    Mota = model.Mota
                };
                if (model.Image.Length > 0)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), url_anh, model.Image.FileName);

                    using (var stream = System.IO.File.Create(path))
                    {
                        await model.Image.CopyToAsync(stream);
                    }
                    addroom.Img = "~/img/more_pic/" + model.Image.FileName;
                }
                else
                {
                    addroom.Img = "";
                }
                ctx.Chitietphongs.Add(addroom);
                ctx.SaveChanges();
                return Ok(addroom);
            }
        }

        [HttpGet("GetRoom")]
        public async Task<ActionResult<Chitietphong>> GetRoom()
        {
            var thongtin_room = await ctx.Chitietphongs.ToListAsync();
            ctx.Database.CloseConnection();
            return Ok(thongtin_room);
        }

        [HttpGet("GetRoom/{Id}")]
        public async Task<ActionResult<List<Chitietphong>>> GetRoom(int id)
        {
            var result = await ctx.Chitietphongs.Where(ee => ee.Id == id).ToListAsync();
            if (result == null)
            {
                return NotFound("Không tìm thấy phòng có id = " + id);
            }
            return Ok(result);
        }

        [HttpPost("Room_Update")]
        public async Task<bool> UpdateRoom([FromForm] Img_Chitietphong model)
        {
            var findP = await ctx.Chitietphongs.SingleOrDefaultAsync(t => t.Id == model.Id);
            if (findP == null)
            {
                return false;
            }
            findP.IdPhong = model.IdPhong;
            findP.TenPhong = model.TenPhong;
            findP.NguoiMax = model.NguoiMax;
            findP.LoaiGiuong = model.LoaiGiuong;
            findP.GiaPhong = model.GiaPhong;
            findP.TinhTrang = model.TinhTrang;
            findP.DienTich = model.DienTich;
            findP.TamNhin = model.TamNhin;
            findP.Mota = model.Mota;

            if (model.Image.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), url_anh,
                    model.Image.FileName);
                using (var stream = System.IO.File.Create(path))
                {
                    await model.Image.CopyToAsync(stream);
                }
                findP.Img = "~/img/more_pic/" + model.Image.FileName;
            }
            else
            {
                findP.Img = "";
            }
            try
            {
                return await ctx.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }

        [HttpDelete("DeleteRoom/{id}")]
        public async Task<ActionResult<bool>> DeleteRoom(int id)
        {
            using (var ctx = new BTL_WebHotelManagerContext())
            {
                var _deleteroom = await ctx.Chitietphongs.FindAsync(id);
                if (_deleteroom == null)
                    return NotFound();

                ctx.Chitietphongs.Remove(_deleteroom);
                var isDeleted = await ctx.SaveChangesAsync() > 0;
                return Ok(isDeleted);
            }
        }

        #endregion

        #region get restaurant
        [HttpGet("GetRestaurant_Buffet")]
        public async Task<ActionResult<SetbuffetViewModel>> GetRestaurant_Buffet()
        {

            return Ok(await ctx.Setbuffets.ToListAsync());
        }

        [HttpGet("GetRestaurant_DoUong")]
        public async Task<ActionResult<DouongViewModel>> GetRestaurant_DoUong()
        {
            return Ok(await ctx.Douongs.ToListAsync());
        }

        [HttpGet("GetRestaurant_MonChinh")]
        public async Task<ActionResult<MonchinhViewModel>> GetRestaurant_MonChinh()
        {
            return Ok(await ctx.Monchinhs.ToListAsync());
        }

        [HttpGet("GetRestaurant_MonKhaiVi")]
        public async Task<ActionResult<MonkhaiviViewModel>> GetRestaurant_MonKhaiVi()
        {
            return Ok(await ctx.Monnoibats.ToListAsync());
        }

        [HttpGet("GetRestaurant_MonNoiBat")]
        public async Task<ActionResult<MonnoibatViewModel>> GetRestaurant_MonNoiBat()
        {
            return Ok(await ctx.Monnoibats.ToListAsync());
        }

        [HttpGet("GetRestaurant_MonTrangMieng")]
        public async Task<ActionResult<MontrangmiengViewModel>> GetRestaurant_MonTrangMieng()
        {
            return Ok(await ctx.Montrangmiengs.ToListAsync());
        }
        #endregion

        #region buffet

        [HttpPost("Create_Buffet")]
        public async Task<IActionResult> Create_Buffet([FromForm] SetbuffetModel model)
        {
            if (await ctx.Setbuffets.AnyAsync(q => q.Id == model.Id))
            {
                return BadRequest();
            }
            else
            {
                var add = new SetbuffetViewModel
                {
                    TenSet = model.TenSet,
                    Gia = model.Gia
                };
                if (model.Image.Length > 0)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), url_anh, model.Image.FileName);

                    using (var stream = System.IO.File.Create(path))
                    {
                        await model.Image.CopyToAsync(stream);
                    }
                    add.Img = "~/img/more_pic/" + model.Image.FileName;
                }
                else
                {
                    add.Img = "";
                }
                ctx.Setbuffets.Add(add);
                ctx.SaveChanges();
                return Ok(add);
            }
        }

        [HttpGet("GetBufet")]
        public async Task<ActionResult<SetbuffetViewModel>> GetBuffet()
        {
            using (var ctx = new BTL_WebHotelManagerContext())
            {
                var thongtin_buffet = await ctx.Setbuffets.ToListAsync();
                return Ok(thongtin_buffet);
            }

        }

        [HttpGet("GetBuffet/{id}")]
        public async Task<ActionResult<List<SetbuffetViewModel>>> GetBuffet(int id)
        {
            var result = await ctx.Setbuffets.Where(ee => ee.Id == id).ToListAsync();
            if (result == null)
            {
                return NotFound("Không tìm thấy phòng có id = " + id);
            }
            return Ok(result);
        }

        [HttpPut("Buffet_Update")]
        public async Task<bool> UpdateBuffet([FromForm] SetbuffetModel model)
        {
            var findP = await ctx.Setbuffets.SingleOrDefaultAsync(t => t.Id == model.Id);
            if (findP == null)
            {
                return false;
            }
            findP.TenSet = model.TenSet;
            findP.Gia = model.Gia;

            if (model.Image.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), url_anh,
                    model.Image.FileName);
                using (var stream = System.IO.File.Create(path))
                {
                    await model.Image.CopyToAsync(stream);
                }
                findP.Img = "~/img/new_pic/" + model.Image.FileName;
            }
            else
            {
                findP.Img = "";
            }
            try
            {
                return await ctx.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }

        [HttpDelete("DeleteBuffet/{id}")]
        public async Task<ActionResult<bool>> DeleteBuffet(int id)
        {
            using (var ctx = new BTL_WebHotelManagerContext())
            {
                var _deleteroom = await ctx.Setbuffets.FindAsync(id);
                if (_deleteroom == null)
                    return NotFound();

                ctx.Setbuffets.Remove(_deleteroom);
                var isDeleted = await ctx.SaveChangesAsync() > 0;
                return Ok(isDeleted);
            }
        }

        #endregion

        #region monchinh
        [HttpPost("Create_MonChinh")]
        public async Task<IActionResult> Create_MonChinh([FromForm] MonchinhModel model)
        {
            if (await ctx.Monchinhs.AnyAsync(q => q.Id == model.Id))
            {
                return BadRequest();
            }
            else
            {
                var add = new MonchinhViewModel
                {
                    TenMon = model.TenMon,
                    Gia = model.Gia
                };
                if (model.Img.Length > 0)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), url_anh, model.Img.FileName);

                    using (var stream = System.IO.File.Create(path))
                    {
                        await model.Img.CopyToAsync(stream);
                    }
                    add.Img = "~/img/more_pic/" + model.Img.FileName;
                }
                else
                {
                    add.Img = "";
                }
                ctx.Monchinhs.Add(add);
                ctx.SaveChanges();
                return Ok(add);
            }
        }
        [HttpGet("GetMonChinh")]
        public async Task<ActionResult<MonchinhViewModel>> GetMonChinh()
        {
            var thongtin_monchinh = await ctx.Monchinhs.ToListAsync();
            return Ok(thongtin_monchinh);
        }

        [HttpGet("GetMonChinh/{id}")]
        public async Task<ActionResult<List<SetbuffetViewModel>>> GetMonChinh(int id)
        {
            var result = await ctx.Monchinhs.Where(ee => ee.Id == id).ToListAsync();
            if (result == null)
            {
                return NotFound("Không tìm thấy phòng có id = " + id);
            }
            return Ok(result);
        }
        [HttpPost("MonChinh_Update")]
        public async Task<bool> Update_MonChinh([FromForm] MonchinhModel model)
        {
            var findP = await ctx.Monchinhs.SingleOrDefaultAsync(t => t.Id == model.Id);
            if (findP == null)
            {
                return false;
            }
            findP.TenMon = model.TenMon;
            findP.Gia = model.Gia;

            if (model.Img.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), url_anh, model.Img.FileName);
                using (var stream = System.IO.File.Create(path))
                {
                    await model.Img.CopyToAsync(stream);
                }
                findP.Img = "~/img/new_pic/" + model.Img.FileName;
            }
            else
            {
                findP.Img = "";
            }
            try
            {
                return await ctx.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }

        [HttpDelete("DeleteMonChinh/{id}")]
        public async Task<ActionResult<bool>> DeleteMonChinh(int id)
        {
            using (var ctx = new BTL_WebHotelManagerContext())
            {
                var _deletemonchinh = await ctx.Monchinhs.FindAsync(id);
                if (_deletemonchinh == null)
                    return NotFound();

                ctx.Monchinhs.Remove(_deletemonchinh);
                var isDeleted = await ctx.SaveChangesAsync() > 0;
                return Ok(isDeleted);
            }
        }
        #endregion

        #region monkhaivi
        [HttpPost("Create_MonKhaiVi")]
        public async Task<IActionResult> Create_MonKhaiVi([FromForm] MonkhaiviModel model)
        {
            if (await ctx.Monkhaivis.AnyAsync(q => q.Id == model.Id))
            {
                return BadRequest();
            }
            else
            {
                var add = new MonkhaiviViewModel
                {
                    TenMon = model.TenMon,
                    Gia = model.Gia
                };
                if (model.Img.Length > 0)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), url_anh, model.Img.FileName);

                    using (var stream = System.IO.File.Create(path))
                    {
                        await model.Img.CopyToAsync(stream);
                    }
                    add.Img = "~/img/more_pic/" + model.Img.FileName;
                }
                else
                {
                    add.Img = "";
                }
                ctx.Monkhaivis.Add(add);
                ctx.SaveChanges();
                return Ok(add);
            }
        }
        [HttpGet("GetMonKhaiVi")]
        public async Task<ActionResult<MonkhaiviViewModel>> GetMonKhaiVi()
        {
            using (var ctx = new BTL_WebHotelManagerContext())
            {
                var thongtin_monkhaivi = await ctx.Monkhaivis.ToListAsync();
                return Ok(thongtin_monkhaivi);
            }
        }

        [HttpGet("GetMonKhaiVi/{id}")]
        public async Task<ActionResult<List<MonkhaiviViewModel>>> GetMonKhaiVi(int id)
        {
            var result = await ctx.Monkhaivis.Where(ee => ee.Id == id).ToListAsync();
            if (result == null)
            {
                return NotFound("Không tìm thấy Món khai vj có id = " + id);
            }
            return Ok(result);
        }
        [HttpPost("MonKhaiVi_Update")]
        public async Task<bool> Update_MonKhaiVi([FromForm] MonkhaiviModel model)
        {
            var findP = await ctx.Monkhaivis.SingleOrDefaultAsync(t => t.Id == model.Id);
            if (findP == null)
            {
                return false;
            }
            findP.TenMon = model.TenMon;
            findP.Gia = model.Gia;

            if (model.Img.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), url_anh, model.Img.FileName);
                using (var stream = System.IO.File.Create(path))
                {
                    await model.Img.CopyToAsync(stream);
                }
                findP.Img = "~/img/new_pic/" + model.Img.FileName;
            }
            else
            {
                findP.Img = "";
            }
            try
            {
                return await ctx.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }

        [HttpDelete("DeleteMonKhaiVi/{id}")]
        public async Task<ActionResult<bool>> DeleteMonKhaiVi(int id)
        {
            using (var ctx = new BTL_WebHotelManagerContext())
            {
                var _deletemonkhaivi = await ctx.Monkhaivis.FindAsync(id);
                if (_deletemonkhaivi == null)
                    return NotFound();

                ctx.Monkhaivis.Remove(_deletemonkhaivi);
                var isDeleted = await ctx.SaveChangesAsync() > 0;
                return Ok(isDeleted);
            }
        }
        #endregion

        #region monnoibat
        [HttpPost("Create_MonNoiBat")]
        public async Task<IActionResult> Create_MonNoiBat([FromForm] MonnoibatModel model)
        {
            if (await ctx.Monnoibats.AnyAsync(q => q.Id == model.Id))
            {
                return BadRequest();
            }
            else
            {
                var add = new MonnoibatViewModel
                {
                    TenMon = model.TenMon,
                    Gia = model.Gia
                };
                if (model.Img.Length > 0)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), url_anh, model.Img.FileName);

                    using (var stream = System.IO.File.Create(path))
                    {
                        await model.Img.CopyToAsync(stream);
                    }
                    add.Img = "~/img/more_pic/" + model.Img.FileName;
                }
                else
                {
                    add.Img = "";
                }
                ctx.Monnoibats.Add(add);
                ctx.SaveChanges();
                return Ok(add);
            }
        }
        [HttpGet("GetMonNoiBat")]
        public async Task<ActionResult<MonnoibatViewModel>> GetMonNoiBat()
        {
            using (var ctx = new BTL_WebHotelManagerContext())
            {
                var thongtin_monnoibat = await ctx.Monnoibats.ToListAsync();
                return Ok(thongtin_monnoibat);
            }

        }

        [HttpGet("GetMonNoiBat/{id}")]
        public async Task<ActionResult<List<MonnoibatViewModel>>> GetMonNoiBat(int id)
        {
            var result = await ctx.Monnoibats.Where(ee => ee.Id == id).ToListAsync();
            if (result == null)
            {
                return NotFound("Không tìm thấy món có id = " + id);
            }
            return Ok(result);
        }
        [HttpPost("MonNoiBat_Update")]
        public async Task<bool> Update_MonNoiBat([FromForm] MonnoibatModel model)
        {
            var findP = await ctx.Monnoibats.SingleOrDefaultAsync(t => t.Id == model.Id);
            if (findP == null)
            {
                return false;
            }
            findP.TenMon = model.TenMon;
            findP.Gia = model.Gia;

            if (model.Img.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), url_anh, model.Img.FileName);
                using (var stream = System.IO.File.Create(path))
                {
                    await model.Img.CopyToAsync(stream);
                }
                findP.Img = "~/img/new_pic/" + model.Img.FileName;
            }
            else
            {
                findP.Img = "";
            }
            try
            {
                return await ctx.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }

        [HttpDelete("DeleteMonNoiBat/{id}")]
        public async Task<ActionResult<bool>> DeleteMonNoiBat(int id)
        {
            using (var ctx = new BTL_WebHotelManagerContext())
            {
                var _deletemonnoibat = await ctx.Monnoibats.FindAsync(id);
                if (_deletemonnoibat == null)
                    return NotFound();

                ctx.Monnoibats.Remove(_deletemonnoibat);
                var isDeleted = await ctx.SaveChangesAsync() > 0;
                return Ok(isDeleted);
            }
        }


        #endregion

        #region montrangmieng
        [HttpPost("Create_MonTrangMieng")]
        public async Task<IActionResult> Create_MonTrangMieng([FromForm] MontrangmiengModel model)
        {
            if (await ctx.Montrangmiengs.AnyAsync(q => q.Id == model.Id))
            {
                return BadRequest();
            }
            else
            {
                var add = new MontrangmiengViewModel
                {
                    TenMon = model.TenMon,
                    Gia = model.Gia
                };
                if (model.Img.Length > 0)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), url_anh, model.Img.FileName);

                    using (var stream = System.IO.File.Create(path))
                    {
                        await model.Img.CopyToAsync(stream);
                    }
                    add.Img = "~/img/more_pic/" + model.Img.FileName;
                }
                else
                {
                    add.Img = "";
                }
                ctx.Montrangmiengs.Add(add);
                ctx.SaveChanges();
                return Ok(add);
            }
        }
        [HttpGet("GetMonTrangMieng")]
        public async Task<ActionResult<MontrangmiengViewModel>> GetMonTrangMieng()
        {
            using (var ctx = new BTL_WebHotelManagerContext())
            {
                var thongtin_montrangmieng = await ctx.Montrangmiengs.ToListAsync();
                return Ok(thongtin_montrangmieng);
            }

        }

        [HttpGet("GetMonTrangMieng/{id}")]
        public async Task<ActionResult<List<MontrangmiengViewModel>>> GetMonTrangMieng(int id)
        {
            var result = await ctx.Montrangmiengs.Where(ee => ee.Id == id).ToListAsync();
            if (result == null)
            {
                return NotFound("Không tìm thấy món có id = " + id);
            }
            return Ok(result);
        }
        [HttpPost("MonTrangMieng_Update")]
        public async Task<bool> Update_MonTrangMieng([FromForm] MontrangmiengModel model)
        {
            var findP = await ctx.Montrangmiengs.SingleOrDefaultAsync(t => t.Id == model.Id);
            if (findP == null)
            {
                return false;
            }
            findP.TenMon = model.TenMon;
            findP.Gia = model.Gia;

            if (model.Img.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), url_anh, model.Img.FileName);
                using (var stream = System.IO.File.Create(path))
                {
                    await model.Img.CopyToAsync(stream);
                }
                findP.Img = "~/img/new_pic/" + model.Img.FileName;
            }
            else
            {
                findP.Img = "";
            }
            try
            {
                return await ctx.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }

        [HttpDelete("DeleteMonTrangMieng/{id}")]
        public async Task<ActionResult<bool>> DeleteMonTrangMieng(int id)
        {
            using (var ctx = new BTL_WebHotelManagerContext())
            {
                var _deletetrangmieng = await ctx.Montrangmiengs.FindAsync(id);
                if (_deletetrangmieng == null)
                    return NotFound();

                ctx.Montrangmiengs.Remove(_deletetrangmieng);
                var isDeleted = await ctx.SaveChangesAsync() > 0;
                return Ok(isDeleted);
            }
        }

        #endregion

        #region douong
        [HttpPost("Create_DoUong")]
        public async Task<IActionResult> Create_DoUong([FromForm] DouongModel model)
        {
            if (await ctx.Douongs.AnyAsync(q => q.Id == model.Id))
            {
                return BadRequest();
            }
            else
            {
                var add = new DouongViewModel
                {
                    TenDoUong = model.TenDoUong,
                    Gia = model.Gia
                };
                if (model.Img.Length > 0)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), url_anh, model.Img.FileName);

                    using (var stream = System.IO.File.Create(path))
                    {
                        await model.Img.CopyToAsync(stream);
                    }
                    add.Img = "~/img/more_pic/" + model.Img.FileName;
                }
                else
                {
                    add.Img = "";
                }
                ctx.Douongs.Add(add);
                ctx.SaveChanges();
                return Ok(add);
            }
        }
        [HttpGet("GetDoUong")]
        public async Task<ActionResult<DouongViewModel>> GetDoUong()
        {
            using (var ctx = new BTL_WebHotelManagerContext())
            {
                var thongtin_douong = await ctx.Douongs.ToListAsync();
                return Ok(thongtin_douong);
            }
        }

        [HttpGet("GetDoUong/{id}")]
        public async Task<ActionResult<List<DouongViewModel>>> GetDoUong(int id)
        {
            var result = await ctx.Douongs.Where(ee => ee.Id == id).ToListAsync();
            if (result == null)
            {
                return NotFound("Không tìm thấy đồ uống có id = " + id);
            }
            return Ok(result);
        }
        [HttpPut("DoUong_Update")]
        public async Task<bool> Update_DoUong([FromForm] DouongModel model)
        {
            var findP = await ctx.Douongs.SingleOrDefaultAsync(t => t.Id == model.Id);
            if (findP == null)
            {
                return false;
            }
            findP.TenDoUong = model.TenDoUong;
            findP.Gia = model.Gia;

            if (model.Img.Length > 0)
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), url_anh, model.Img.FileName);
                using (var stream = System.IO.File.Create(path))
                {
                    await model.Img.CopyToAsync(stream);
                }
                findP.Img = "~/img/new_pic/" + model.Img.FileName;
            }
            else
            {
                findP.Img = "";
            }
            try
            {
                return await ctx.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }

        [HttpDelete("DeleteDoUong/{id}")]
        public async Task<ActionResult<bool>> DeleteDoUong(int id)
        {
            using (var ctx = new BTL_WebHotelManagerContext())
            {
                var _deletedouong = await ctx.Douongs.FindAsync(id);
                if (_deletedouong == null)
                    return NotFound();

                ctx.Douongs.Remove(_deletedouong);
                var isDeleted = await ctx.SaveChangesAsync() > 0;
                return Ok(isDeleted);
            }
        }

        #endregion

        [HttpGet("QuanLyDatPhong")]
        public IActionResult Lichsuphong()
        {
            var rooms = (from a in ctx.Phieudatphongs
                         join f in ctx.Quanlytaikhoans on a.IdKh equals f.Id
                         join b in ctx.Chitietphongs on a.Idphong equals b.Id
                         join c in ctx.Phieudichvus on a.MaDp equals c.MaDp
                         join d in ctx.Chitietdichvus on c.Mpdv equals d.Mpdv into ctdv
                         from cate2 in ctdv.DefaultIfEmpty()
                         join e in ctx.Dichvus on cate2.MaDV equals e.Id into dv
                         from cate3 in dv.DefaultIfEmpty()
                         group new { cate3.TenDichVu, cate3.DonGia } by new
                         {

                             a.MaDp,
                             b.TenPhong,
                             b.GiaPhong,
                             a.NgayDen,
                             a.NgayDi,
                             a.NgayTt,
                             a.PhuongThucThanhToan,
                             c.TongTien,
                             f.Id,
                             f.HoTen,
                             f.Email,
                             f.Sdt
                         } into g
                         select new
                         {
                             g.Key.MaDp,
                             g.Key.TenPhong,
                             g.Key.NgayDen,
                             g.Key.NgayDi,
                             g.Key.NgayTt,
                             g.Key.PhuongThucThanhToan,
                             g.Key.TongTien,
                             g.Key.Id,
                             g.Key.HoTen,
                             g.Key.Email,
                             g.Key.Sdt,
                             ThanhTien = (g.Key.NgayDi - g.Key.NgayDen).TotalDays * g.Key.GiaPhong+g.Key.TongTien,
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

      [HttpGet("TimKiemDatPhong/{timkiem}")]
        public IActionResult TimKiemDatPhong(string timkiem)
        {
            var rooms = (from a in ctx.Phieudatphongs
                         join f in ctx.Quanlytaikhoans on a.IdKh equals f.Id
                         join b in ctx.Chitietphongs on a.Idphong equals b.Id
                         join c in ctx.Phieudichvus on a.MaDp equals c.MaDp
                         join d in ctx.Chitietdichvus on c.Mpdv equals d.Mpdv into ctdv
                         from cate2 in ctdv.DefaultIfEmpty()
                         join e in ctx.Dichvus on cate2.MaDV equals e.Id into dv
                         from cate3 in dv.DefaultIfEmpty()
                         where f.HoTen.Contains(timkiem) || f.Sdt.Contains(timkiem)
                         group new { cate3.TenDichVu, cate3.DonGia } by new
                         {
                             a.MaDp,
                             a.NgayDen,
                             a.NgayDi,
                             a.NgayTt,
                             a.PhuongThucThanhToan,
                             b.TenPhong,
                             b.GiaPhong,
                             c.TongTien,
                             f.Id,
                             f.HoTen,
                             f.Email , 
                             f.Sdt
                         } into g
                         select new
                         {
                             g.Key.Id,
                             g.Key.HoTen,
                             g.Key.Email,
                             g.Key.Sdt,
                             g.Key.MaDp,
                             g.Key.TenPhong,
                             g.Key.GiaPhong,
                             g.Key.NgayDen,
                             g.Key.NgayDi,
                             g.Key.NgayTt,
                             g.Key.TongTien,
                             g.Key.PhuongThucThanhToan,
                        
                             ThanhTien = (g.Key.NgayDi - g.Key.NgayDen).TotalDays * g.Key.GiaPhong + g.Key.TongTien,
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
    }
}