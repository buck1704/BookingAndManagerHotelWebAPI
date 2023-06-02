using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Constraints;
using Newtonsoft.Json;
using QuanLyKhachSanClient.Models;
using System.Globalization;
using System.Net;
using System.Text;
using System.Web;

namespace QuanLyKhachSanClient.Controllers
{
    public class hotelmanagerController : Controller
    {
        const string BASE_URL = "http://localhost:5071";
        private readonly IHttpClientFactory _factory;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public hotelmanagerController(IHttpClientFactory factory, IHttpContextAccessor httpContextAccessor)
        {
            _factory = factory;
            _httpContextAccessor = httpContextAccessor;
        }

        #region View
        public IActionResult DangNhap()
        {
            return View();
        }
        public IActionResult DangKy()
        {
            return View();
        }

        public ActionResult TrangChu()
        {
            return View();
        }

        public ActionResult allroom()
        {
            return View();
        }
        public ActionResult deluxroom()
        {
            return View();
        }
        public ActionResult executiveroom()
        {
            return View();
        }
        public ActionResult suiteroom()
        {
            return View();
        }
        public ActionResult service()
        {
            return View();
        }
        public ActionResult restaurant()
        {
            return View();
        }
        public ActionResult bar()
        {
            return View();
        }
        public ActionResult fitness()
        {
            return View();
        }
        public ActionResult washingcloses()
        {
            return View();
        }
        public ActionResult roomcheck()
        {
            return View();
        }
        public ActionResult page()
        {
            return View();
        }
        public ActionResult contact()
        {
            return View();
        }
        public ActionResult infor()
        {
            return View();
        }
        public ActionResult history()
        {
            return View();
        }

        public ActionResult roomstyle()
        {
            return View();
        }
        public IActionResult roomdeail()
        {
            return View();
        }
        public IActionResult payddn()
        {
            return View();
        }
        public IActionResult payb2ddn()
        {
            return View();
        }
        public ActionResult quenmatkhau()
        {
            return View();
        }
        #endregion

        #region TaiKhoan
        [HttpPost]
        public async Task<IActionResult> CheckLogin(string email, string password)
        {
            HttpClient client = _factory.CreateClient();
            var response = await client.GetAsync(BASE_URL + $"/api/HotelManager/login/{email}/{password}");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Message = "Tên đăng nhập hoặc mật khẩu sai! Vui lòng nhập lại ";
                return View("DangNhap");
            }

            var result = await response.Content.ReadFromJsonAsync<Quanlytaikhoan>();
            _httpContextAccessor.HttpContext.Session.SetInt32("IDkhachhang", result.Id);
            _httpContextAccessor.HttpContext.Session.SetString("HoTen", result.HoTen);
            _httpContextAccessor.HttpContext.Session.SetString("Email", result.Email);
            _httpContextAccessor.HttpContext.Session.SetString("Sdt", result.Sdt);
            _httpContextAccessor.HttpContext.Session.SetString("Cmnd", result.Cmnd);
            _httpContextAccessor.HttpContext.Session.SetString("PassWord", result.PassWord);
            ViewBag.Message = "Chào mừng bạn đến với BURNING HOTEL";
            return View("TrangChu");
        }
        [HttpPost]
        public async Task<IActionResult> CheckDangKy(Quanlytaikhoan addtaikhoan)
        {
            HttpClient client = _factory.CreateClient();
            var content = new StringContent(JsonConvert.SerializeObject(addtaikhoan), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(BASE_URL + $"/api/HotelManager/register", content);

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                dynamic errorObject = JsonConvert.DeserializeObject<dynamic>(errorResponse);
                string error = errorObject.error;
                ViewBag.ErrorMessage = HttpUtility.HtmlDecode(error);
                return View("DangKy");
            }
            ViewBag.SuccessMessage = "Đăng ký thành công!";
            return View("DangNhap");
        }

        public async Task<IActionResult> Doimatkhau(string sdt, string mkm, string nlmk)
        {
            HttpClient client = _factory.CreateClient();

            // tạo object để truyền lên body request
            var data = new { sdt = sdt, mkm = mkm, nlmk = nlmk };

            // truyền data lên body request
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"{BASE_URL}/api/HotelManager/doimatkhau/{sdt}/{mkm}/{nlmk}", content);



            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                dynamic errorObject = JsonConvert.DeserializeObject<dynamic>(errorResponse);
                string error = errorObject.error;
                ViewBag.ErrorMessage = HttpUtility.HtmlDecode(error);
                return View("quenmatkhau");
            }
            else if (response.StatusCode == HttpStatusCode.OK)
            {
                ViewBag.SuccessMessage = "Đổi mật khẩu thành công!";
                return View("DangNhap");
            }

            else
            {
                // xử lý trường hợp lỗi không mong muốn
                ViewBag.ErrorMessage = "Lỗi không mong muốn đã xảy ra";
                return View("quenmatkhau");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Suathongtincanhan(string ten, string sdt, string email, string cmnd, string pass)
        {
            HttpClient client = _factory.CreateClient();

            // tạo object để truyền lên body request
            var data = new { ten = ten, sdt = sdt, email = email, cmnd = cmnd, pass = pass };

            // truyền data lên body request
            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await client.PutAsync($"{BASE_URL}/api/HotelManager/SuaTaiKhoan/{ten}/{sdt}/{email}/{cmnd}/{pass}", content);


            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                dynamic errorObject = JsonConvert.DeserializeObject<dynamic>(errorResponse);
                string error = errorObject.error;
                ViewBag.ErrorMessage = HttpUtility.HtmlDecode(error);
                return View("infor");
            }
            else if (response.StatusCode == HttpStatusCode.OK)
            {
                ViewBag.SuccessMessage = "Cập nhập thông tin thành công!";
                return View("DangNhap");
            }
            else
            {
                // xử lý trường hợp lỗi không mong muốn
                ViewBag.ErrorMessage = "Lỗi không mong muốn đã xảy ra";
                return View("infor");
            }
        } 
        #endregion

        [HttpPost]
        public async Task<IActionResult> GetRoom(int category, int nguoi, string ngayden, string ngaydi, string ngaydenrs, string ngaydirs, int checknguoi, int checkphong, string btn, string submit)
        {
            HttpClient client = _factory.CreateClient();
            var response = new HttpResponseMessage();

            if (btn != null)
            {
                response = await client.GetAsync(BASE_URL + $"/api/HotelManager/GetPhong/{category}/{nguoi}");
                ViewBag.ngayden = ngayden;
                ViewBag.ngaydi = ngaydi;
                ViewBag.Nguoi = nguoi;
                ViewBag.Phong = category;
                ViewBag.ButtonClicked = "btn";
                _httpContextAccessor.HttpContext.Session.SetString("NgayDen", ngayden);
                _httpContextAccessor.HttpContext.Session.SetString("NgayDi", ngaydi);
            }


            else
            {
                response = await client.GetAsync(BASE_URL + $"/api/HotelManager/GetPhong/{checkphong}/{checknguoi}");
                ViewBag.ngaydenrs = ngaydenrs;
                ViewBag.ngaydirs = ngaydirs;
                ViewBag.checknguoi = checknguoi;
                ViewBag.checkphong = checkphong;
                ViewBag.ButtonClicked = "submit";
                _httpContextAccessor.HttpContext.Session.SetString("NgayDen", ngaydenrs);
                _httpContextAccessor.HttpContext.Session.SetString("NgayDi", ngaydirs);
            }

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Message = "Rất tiếc đã hết phòng";
                return View("TrangChu");
            }
            var result = await response.Content.ReadFromJsonAsync<List<Chitietphong>>();
            var phongs = new List<Chitietphong>();
            foreach (var item in result)
            {
                var phong = new Chitietphong()
                {
                    Id = item.Id,
                    TenPhong = item.TenPhong,
                    NguoiMax = item.NguoiMax,
                    Img = item.Img,
                    GiaPhong = item.GiaPhong
                };
                phongs.Add(phong);
            }
            ViewBag.Room = phongs;
            return View("roomstyle", phongs);


        }

        [HttpPost]
        public async Task<IActionResult> GetRoomar(int categoryar, int roomar, string submitar, string ngaydenar, string ngaydiar)
        {
            HttpClient client = _factory.CreateClient();
            var response = await client.GetAsync(BASE_URL + $"/api/HotelManager/GetPhong/{categoryar}/{roomar}");


            ViewBag.NgayDenar = ngaydenar;
            ViewBag.NgayDiar = ngaydiar;
            ViewBag.roomar = roomar;
            ViewBag.categoryar = categoryar;
            _httpContextAccessor.HttpContext.Session.SetString("NgayDen", ngaydenar);
            _httpContextAccessor.HttpContext.Session.SetString("NgayDi", ngaydiar);
            ViewBag.ButtonClicked = "submitar";

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Message = "Rất tiếc đã hết phòng";
                return View("TrangChu");
            }
            var result = await response.Content.ReadFromJsonAsync<List<Chitietphong>>();
            var phongs = new List<Chitietphong>();
            foreach (var item in result)
            {
                var phong = new Chitietphong()
                {
                    Id = item.Id,
                    TenPhong = item.TenPhong,
                    NguoiMax = item.NguoiMax,
                    Img = item.Img,
                    GiaPhong = item.GiaPhong
                };
                phongs.Add(phong);
            }
            ViewBag.phong = phongs;

            return View("allroom", phongs);

        }
        [HttpGet]
        public async Task<IActionResult> GetAllChiTietPhong()
        {
            HttpClient client = _factory.CreateClient();
            var response = await client.GetAsync(BASE_URL + $"/api/HotelManager/GetAllChiTietPhong");

            if (!response.IsSuccessStatusCode)
            {
                ViewData["error"] = "Lỗi hiển thị phòng";
                return View("TrangChu");
            }
            var result = await response.Content.ReadFromJsonAsync<List<Chitietphong>>();
            var phongar = new List<Chitietphong>();
            foreach (var item in result)
            {
                var phong = new Chitietphong()
                {
                    Id = item.Id,
                    TenPhong = item.TenPhong,
                    NguoiMax = item.NguoiMax,
                    Img = item.Img,
                    GiaPhong = item.GiaPhong
                };
                phongar.Add(phong);
            }
            ViewBag.Phongar = phongar;
            // Truyền danh sách phòng sang view
            return View("allroom", phongar);

        }
        [HttpGet]
        public async Task<IActionResult> GetChiTietPhong(int Id)
        {
            HttpClient client = _factory.CreateClient();
            var response = await client.GetAsync(BASE_URL + $"/api/HotelManager/chitietphong/{Id}");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Message = "Lỗi hiển thị phòng ";
                return View("roomstyle");
            }
            var result = await response.Content.ReadFromJsonAsync<Chitietphong>();
            _httpContextAccessor.HttpContext.Session.SetInt32("ID_loaiPhong", result.IdPhong);
            _httpContextAccessor.HttpContext.Session.SetInt32("Id_Phong", result.Id);
            _httpContextAccessor.HttpContext.Session.SetString("TenPhong", result.TenPhong);
            _httpContextAccessor.HttpContext.Session.SetInt32("NguoiMax", result.NguoiMax);
            _httpContextAccessor.HttpContext.Session.SetInt32("GiaPhong", result.GiaPhong);
            _httpContextAccessor.HttpContext.Session.SetString("LoaiGiuong", result.LoaiGiuong);
            _httpContextAccessor.HttpContext.Session.SetString("Img", result.Img);
            _httpContextAccessor.HttpContext.Session.SetString("DienTich", result.DienTich);
            _httpContextAccessor.HttpContext.Session.SetString("TamNhin", result.TamNhin);
            _httpContextAccessor.HttpContext.Session.SetString("MoTa", result.Mota);
            _httpContextAccessor.HttpContext.Session.SetInt32("TinhTrang", result.TinhTrang);

            return View("roomdeail", result);
        }

        [HttpPost]
        public IActionResult thanhtoan(int TongTienDichVu, string MaDichVu)
        {
            string ngayden = HttpContext.Session.GetString("NgayDen");
            string ngaydi = HttpContext.Session.GetString("NgayDi");
            DateTime ngayDen = DateTime.ParseExact(ngayden, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);
            DateTime ngayDi = DateTime.ParseExact(ngaydi, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);
            int soNgay = (int)(ngayDi - ngayDen).TotalDays;
            int giaPhong = HttpContext.Session.GetInt32("GiaPhong") ?? 0;
            int TienPhong = soNgay * giaPhong;
            int ThanhTien = TongTienDichVu + TienPhong;
            int GiamGia = 0;
            if (ThanhTien > 5000000)
            {
                GiamGia = Convert.ToInt32(ThanhTien * 0.01);
            }
            int SoTienTra = ThanhTien - GiamGia;

            ViewBag.tongtiendichvu = TongTienDichVu;
            ViewBag.ngayo = soNgay;
            ViewBag.tienphong = TienPhong;
            ViewBag.thanhtien = ThanhTien;
            ViewBag.giamgia = GiamGia;
            ViewBag.sotienphaitra = SoTienTra;
            _httpContextAccessor.HttpContext.Session.SetInt32("SoTienTra", SoTienTra);
            _httpContextAccessor.HttpContext.Session.SetInt32("TongTienDichVu", TongTienDichVu);
            string MaDichVuArray = string.Join(",", MaDichVu); 
            HttpContext.Session.SetString("MaDichVuArray", MaDichVuArray);

            return View("payb2ddn");
        }
        [HttpPost]
        public async Task<IActionResult> ThemPhieuDatPhong(string thanhtoan, bool Huyphong=false)
        {
            int Id = HttpContext.Session.GetInt32("Id_Phong") ?? 1;
            string ngayden = HttpContext.Session.GetString("NgayDen");
            string ngaydi = HttpContext.Session.GetString("NgayDi");
            DateTime ngayDen = DateTime.ParseExact(ngayden, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);
            DateTime ngayDi = DateTime.ParseExact(ngaydi, "yyyy-MM-ddTHH:mm", CultureInfo.InvariantCulture);
            //thêm phiếu đặt phòng

            int idkhachhang = HttpContext.Session.GetInt32("IDkhachhang") ?? 1;
            string hoten = HttpContext.Session.GetString("HoTen");
            string sdt = HttpContext.Session.GetString("Sdt");
            string email = HttpContext.Session.GetString("Email");
            string cmnd = HttpContext.Session.GetString("Cmnd");
            string pass = HttpContext.Session.GetString("PassWord");
            int idloaiphong = HttpContext.Session.GetInt32("ID_loaiPhong") ?? 1;
            string tenphong = HttpContext.Session.GetString("TenPhong");
            int nguoimax = HttpContext.Session.GetInt32("NguoiMax")??1;
            string loaigiuong = HttpContext.Session.GetString("LoaiGiuong");
            int giaphong = HttpContext.Session.GetInt32("GiaPhong")??0;
            int tinhtrang = HttpContext.Session.GetInt32("TinhTrang")??0;
            string img = HttpContext.Session.GetString("Img");
            string dientich = HttpContext.Session.GetString("DienTich");
            string tamnhin = HttpContext.Session.GetString("TamNhin");
            string mota = HttpContext.Session.GetString("MoTa");
            var khachHang = new Quanlytaikhoan
            {
                
                Id=idkhachhang,
                HoTen =hoten,
                Sdt = sdt,
                Email = email,
                Cmnd = cmnd,
                PassWord = pass
            };

            var phong = new Chitietphong
            {
                Id= idloaiphong,
                IdPhong =Id,
                TenPhong = tenphong,
                NguoiMax = nguoimax,
                LoaiGiuong = loaigiuong,
                GiaPhong = giaphong,
                TinhTrang = tinhtrang,
                Img = img,
                DienTich = dientich,
                TamNhin = tamnhin,
                Mota = mota
            };

            var phieudatphong = new Phieudatphong()
            {
                Idphong = Id,
                IdKh = HttpContext.Session.GetInt32("IDkhachhang") ?? 0,
                PhuongThucThanhToan = thanhtoan,
                NgayDen = ngayDen,
                NgayDi = ngayDi,
                NgayTt = DateTime.Now,
                IdKhNavigation = khachHang,
                IdphongNavigation = phong
            };

            HttpClient client = _factory.CreateClient();
            var content = new StringContent(JsonConvert.SerializeObject(phieudatphong), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(BASE_URL + "/api/HotelManager/themphieudatphong", content);
            string errorContent = await response.Content.ReadAsStringAsync();
            if (!response.IsSuccessStatusCode)
            {
                ViewData["error"] = "Lỗi thêm chi tiết thanh toán";
                return View("payb2ddn");
            }
            var responseContent = await response.Content.ReadAsStringAsync();
            var successMessage = JsonConvert.DeserializeObject<dynamic>(responseContent);
            ViewBag.Message = successMessage.message.ToString();
            ViewBag.MaPhong = successMessage.maPhong;

            //thêm phiếu dịch vụ
           
            var phieudichvu = new Phieudichvu
            {
                MaDp = ViewBag.MaPhong,
                TongTien = HttpContext.Session.GetInt32("TongTienDichVu") ?? 1,
                MaDpNavigation=phieudatphong
            };
           
            var content3 = new StringContent(JsonConvert.SerializeObject(phieudichvu), Encoding.UTF8, "application/json");
            var dvresponse = await client.PostAsync(BASE_URL + "/api/HotelManager/themphieudichvu", content3);
            if (!dvresponse.IsSuccessStatusCode)
            {
                ViewData["error"] = "Lỗi thêm dịch vụ phòng";
                return View("payb2ddn");
            }
            var responseContent2 = await dvresponse.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<dynamic>(responseContent2);
            int mapdv = result.maPhieuDichVu;


            //thêm chi tiết dịch vụ
            string MaDichVuArray = HttpContext.Session.GetString("MaDichVuArray");
            var ctdv = new List<Chitietdichvu>();
            if (!string.IsNullOrEmpty(MaDichVuArray))
            {
                string[] maDichVuArray = JsonConvert.DeserializeObject<string[]>(MaDichVuArray);

                var chitietdichvuList = new List<Chitietdichvu>();

                foreach (var maDichVu in maDichVuArray)
                {
                    var responsedv = await client.GetAsync(BASE_URL + "/api/HotelManager/GetDichVu/" + maDichVu);
                    if (responsedv.IsSuccessStatusCode)
                    {
                        var contentdv = await responsedv.Content.ReadFromJsonAsync<Dichvu>();

                        var dichVu = new Dichvu
                        {
                            Id = contentdv.Id,
                            TenDichVu = contentdv.TenDichVu,
                            DonGia = contentdv.DonGia,
                        };
                    
                        var chitietdichvu = new Chitietdichvu()
                        {
                            Mpdv = mapdv,
                            MaDV = maDichVu,
                            MpdvNavigation = phieudichvu,
                            MaDVNavigation = dichVu
                        };

                        chitietdichvuList.Add(chitietdichvu);
                    }
                }


                var content4 = new StringContent(JsonConvert.SerializeObject(chitietdichvuList), Encoding.UTF8, "application/json");
                var ctdvresponse = await client.PostAsync(BASE_URL + "/api/HotelManager/themchitietdichvu", content4);
                var errorContent2 = await ctdvresponse.Content.ReadAsStringAsync();

                if (!ctdvresponse.IsSuccessStatusCode)
                {
                    ViewData["error"] = "Lỗi thêm chi tiết dịch vụ phòng";
                    return View("payb2ddn");
                }
            }


            // cập nhập phòng

            var data = new { Id = Id };
            var content2 = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var phongResponse = await client.PutAsync($"{BASE_URL}/api/HotelManager/CapNhapPhong/{Id}?Huyphong=false", content2);
            if (!phongResponse.IsSuccessStatusCode)
            {
                ViewData["error"] = "Lỗi Cập nhập phòng rồi. duma";
                return View("payb2ddn");
            }

            return View("TrangChu");
        }

        [HttpPost]
        public async Task<IActionResult> thongtinphong(int ma, string sdt)
        {
            if (string.IsNullOrEmpty(ma.ToString()) || string.IsNullOrEmpty(sdt))
            {
                ViewBag.Error = "Vui lòng nhập đầy đủ thông tin!";
                return View("roomcheck");
            }
            HttpClient client = _factory.CreateClient();
            var response = await client.GetAsync(BASE_URL + $"/api/HotelManager/GetKiemTraPhong/{ma}/{sdt}");

            if (!response.IsSuccessStatusCode)
            {
                ViewBag.Error = "Thông tin sai";
                return View("roomcheck");
            }
           
            try
            {
                var result = await response.Content.ReadAsStringAsync();
                dynamic thongtinphong = JsonConvert.DeserializeObject<dynamic>(result);

                return View("thongtinphong", thongtinphong);
            }

            catch (Exception ex)
            {
                ViewBag.Error = "Cíu tao: " + ex.Message;
                ViewBag.JsonResponse = await response.Content.ReadAsStringAsync();
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> thongtinphong2(int ma, string sdt)
        {
           
            HttpClient client = _factory.CreateClient();
            var response = await client.GetAsync(BASE_URL + $"/api/HotelManager/GetKiemTraPhong/{ma}/{sdt}");

            if (!response.IsSuccessStatusCode)
            {
                ViewData["error"] = "Thông tin sai";
                return View("roomcheck");
            }

            try
            {
                var result = await response.Content.ReadAsStringAsync();
                dynamic thongtinphong = JsonConvert.DeserializeObject<dynamic>(result);

                return View("thongtinphong", thongtinphong);
            }

            catch (Exception ex)
            {
                ViewBag.Error = "Cíu tao: " + ex.Message;
                ViewBag.JsonResponse = await response.Content.ReadAsStringAsync();
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> Lichsudatphong(int Id)
        {
            HttpClient client = _factory.CreateClient();
            var response = await client.GetAsync(BASE_URL + $"/api/HotelManager/Lichsuphong/{Id}");
            if (!response.IsSuccessStatusCode)
            {
                ViewData["error"] = "Lỗi hiển thị lịch sử phòng";
                return View("TrangChu");
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<dynamic>>(jsonString);
            if (result != null)
            {
                var phongs = new List<dynamic>();

                foreach (var item in result)
                {
                    dynamic phong = new System.Dynamic.ExpandoObject();
                    phong.MaDatPhong=item.maDp;
                    phong.TenPhong = item.tenPhong;
                    phong.GiaPhong = item.giaPhong;

                    phong.TenDichVu = new List<string>();

                    foreach (var dichvu in item.dichVu)
                    {
                        string tenDichVu = dichvu.tenDichVu;
                        int donGia = dichvu.donGia;
                        phong.TenDichVu.Add(tenDichVu + " : " + donGia); 
                    }
                    phong.TienPhong=item.tienPhong;
                    phong.NgayDen = item.ngayDen;
                    phong.NgayDi = item.ngayDi;
                    phong.NgayTt = item.ngayTt;
                    phong.PhuongThucThanhToan = item.phuongThucThanhToan;
                    phong.Img = item.img;

                    phongs.Add(phong);
                }

                return View("history", phongs);
            }


            return NotFound();

        }


        [HttpGet]
        public async Task<IActionResult> HuyDatPhong(int MaDP, int Id, bool Huyphong=true)
        {
            HttpClient client = _factory.CreateClient();
            var response = await client.DeleteAsync(BASE_URL + $"/api/HotelManager/HuyDatPhong/{MaDP}");

            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                ViewData["error"] = errorContent;
                return View("thongtinphong");
            }
            var data = new { Id = Id };
            var content2 = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var phongResponse = await client.PutAsync($"{BASE_URL}/api/HotelManager/CapNhapPhong/{Id}?Huyphong=true", content2);
            ViewBag.Message = "Hủy Phòng Thành Công!";
            return View("TrangChu");
        }

        [HttpGet]
        public async Task<IActionResult> GetMonAn()
        {
            HttpClient client = _factory.CreateClient();
            var response = await client.GetAsync(BASE_URL + "/api/admin/GetMonChinh");
            var response1 = await client.GetAsync(BASE_URL + "/api/admin/GetMonKhaiVi");
            var response2 = await client.GetAsync(BASE_URL + "/api/admin/GetMonNoiBat");
            var response3 = await client.GetAsync(BASE_URL + "/api/admin/GetMonTrangMieng");
            var response4 = await client.GetAsync(BASE_URL + "/api/admin/GetDoUong");
            var response5 = await client.GetAsync(BASE_URL + "/api/admin/GetBufet");

            if (!response.IsSuccessStatusCode || !response1.IsSuccessStatusCode || !response2.IsSuccessStatusCode || !response3.IsSuccessStatusCode || !response4.IsSuccessStatusCode || !response.IsSuccessStatusCode)
            {
                ViewData["error"] = "Lỗi hiển thị thông tin";
                return View("TrangChu");
            }

            var result = await response.Content.ReadFromJsonAsync<List<MonchinhViewModel>>();
            var result1 = await response1.Content.ReadFromJsonAsync<List<MonkhaiviViewModel>>();
            var result2 = await response2.Content.ReadFromJsonAsync<List<MonnoibatViewModel>>();
            var result3 = await response3.Content.ReadFromJsonAsync<List<MontrangmiengViewModel>>();
            var result4 = await response4.Content.ReadFromJsonAsync<List<DouongViewModel>>();
            var result5 = await response5.Content.ReadFromJsonAsync<List<SetbuffetViewModel>>();

            var monchinhs = new List<MonchinhViewModel>();
            var monkhaivis = new List<MonkhaiviViewModel>();
            var monnoibats = new List<MonnoibatViewModel>();
            var montrangmiengs = new List<MontrangmiengViewModel>();
            var dounongs = new List<DouongViewModel>();
            var buffets = new List<SetbuffetViewModel>();

            foreach (var item in result)
            {
                var mc = new MonchinhViewModel()
                {
                    Id = item.Id,
                    TenMon = item.TenMon,
                    Gia = item.Gia,
                    Img = item.Img
                };
                monchinhs.Add(mc);
            }

            foreach (var item in result1)
            {
                var mkv = new MonkhaiviViewModel()
                {
                    Id = item.Id,
                    TenMon = item.TenMon,
                    Gia = item.Gia,
                    Img = item.Img
                };
                monkhaivis.Add(mkv);
            }

            foreach (var item in result2)
            {
                var mnb = new MonnoibatViewModel()
                {
                    Id = item.Id,
                    TenMon = item.TenMon,
                    Gia = item.Gia,
                    Img = item.Img
                };
                monnoibats.Add(mnb);
            }

            foreach (var item in result3)
            {
                var mtm = new MontrangmiengViewModel()
                {
                    Id = item.Id,
                    TenMon = item.TenMon,
                    Gia = item.Gia,
                    Img = item.Img
                };
                montrangmiengs.Add(mtm);
            }

            foreach (var item in result4)
            {
                var du = new DouongViewModel()
                {
                    Id = item.Id,
                    TenDoUong = item.TenDoUong,
                    Gia = item.Gia,
                    Img = item.Img
                };
                dounongs.Add(du);
            }
            foreach (var item in result5)
            {
                var du = new SetbuffetViewModel()
                {
                    Id = item.Id,
                    TenSet = item.TenSet,
                    Gia = item.Gia,
                    Img = item.Img
                };
                buffets.Add(du);
            }

            ViewBag.MonChinh = monchinhs;
            ViewBag.MonKhaiVi = monkhaivis;
            ViewBag.MonNoiBat = monnoibats;
            ViewBag.MonTrangMieng = montrangmiengs;
            ViewBag.DoUong = dounongs;
            ViewBag.Buffets = buffets;
            // Truyền danh sách món ăn sang view
            return View("restaurant");


        }

    }
}
