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
    public class adminController : Controller
    {
        const string BASE_URL = "http://localhost:5071";
        IHttpClientFactory factory;
        private readonly IHttpClientFactory _factory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public adminController(IHttpClientFactory factory, IHttpContextAccessor httpContextAccessor)
        {
            _factory = factory;
            _httpContextAccessor = httpContextAccessor;
        }
        
        public IActionResult DangNhap()
        {
            return View();
        }

        public IActionResult QuanLyDatPhong()
        {
            return View();
        }
        public IActionResult User_Create()
        {
            return View();
        }

        public IActionResult Room_Create()
        {
            return View();
        }

        public IActionResult Restaurant_Index()
        {
            return View();
        }

        public IActionResult Restaurant_Buffet_Create()
        {
            return View();
        }
        public IActionResult Restaurant_DoUong_Create()
        {
            return View();
        }
        public IActionResult Restaurant_MonChinh_Create()
        {
            return View();
        }
        public IActionResult Restaurant_MonKhaiVi_Create()
        {
            return View();
        }
        public IActionResult Restaurant_MonNoiBat_Create()
        {
            return View();
        }
        public IActionResult Restaurant_MonTrangMieng_Create()
        {
            return View();
        }
        public IActionResult Index()
        {
            return View();
        }



        [HttpPost]
        public async Task<IActionResult> CheckLogin(string email, string password)
        {
            HttpClient client = _factory.CreateClient();
            var response = await client.GetAsync(BASE_URL + $"/api/admin/login/{email}/{password}");

            if (!response.IsSuccessStatusCode)
            {
                ViewData["error"] = "Tài khoản hoặc mật khẩu sai";
                return View("DangNhap");
            }
            return View("Index");
        }

        #region index

        public async Task<IActionResult> User_Index()
        {
            HttpClient client = _factory.CreateClient();
            var response = await client.GetAsync(BASE_URL + "/api/admin/GetUser/");

            if (!response.IsSuccessStatusCode)
            {
                ViewData["error"] = "Lỗi hiển thị phòng";
                return View("User_Index");
            }

            var users = await response.Content.ReadFromJsonAsync<List<Quanlytaikhoan>>();
            return View("User_Index", users);
        }

        public async Task<IActionResult> Room_Index()
        {
            HttpClient client = _factory.CreateClient();
            var response = await client.GetAsync(BASE_URL + $"/api/admin/GetRoom/");

            if (!response.IsSuccessStatusCode)
            {
                ViewData["error"] = "Lỗi hiển thị phòng";
                return View();
            }

            var result = await response.Content.ReadFromJsonAsync<List<Chitietphong>>();

            return View("Room_Index", result);
        }

        public async Task<IActionResult> Restaurant_Buffet_Index()
        {
            HttpClient client = _factory.CreateClient();
            var response = await client.GetAsync(BASE_URL + $"/api/admin/GetBufet/");

            if (!response.IsSuccessStatusCode)
            {
                ViewData["error"] = "Lỗi hiển thị phòng";
                return View();
            }

            var result = await response.Content.ReadFromJsonAsync<List<SetbuffetViewModel>>();

            return View("Restaurant_Buffet_Index", result);
        }
        public async Task<IActionResult> Restaurant_DoUong_Index()
        {
            HttpClient client = _factory.CreateClient();
            var response = await client.GetAsync(BASE_URL + $"/api/admin/GetRestaurant_DoUong/");

            if (!response.IsSuccessStatusCode)
            {
                ViewData["error"] = "Lỗi hiển thị phòng";
                return View();
            }

            var result = await response.Content.ReadFromJsonAsync<List<DouongViewModel>>();

            return View("Restaurant_DoUong_Index", result);
        }
        public async Task<IActionResult> Restaurant_MonChinh_Index()
        {
            HttpClient client = _factory.CreateClient();
            var response = await client.GetAsync(BASE_URL + $"/api/admin/GetMonChinh/");

            if (!response.IsSuccessStatusCode)
            {
                ViewData["error"] = "Lỗi hiển thị phòng";
                return View();
            }

            var result = await response.Content.ReadFromJsonAsync<List<MonchinhViewModel>>();

            return View("Restaurant_MonChinh_Index", result);
        }
        public async Task<IActionResult> Restaurant_MonKhaiVi_Index()
        {
            HttpClient client = _factory.CreateClient();
            var response = await client.GetAsync(BASE_URL + $"/api/admin/GetRestaurant_MonKhaiVi/");

            if (!response.IsSuccessStatusCode)
            {
                ViewData["error"] = "Lỗi hiển thị phòng";
                return View();
            }

            var result = await response.Content.ReadFromJsonAsync<List<MonkhaiviViewModel>>();

            return View("Restaurant_MonKhaiVi_Index", result);
        }
        public async Task<IActionResult> Restaurant_MonNoiBat_Index()
        {
            HttpClient client = _factory.CreateClient();
            var response = await client.GetAsync(BASE_URL + $"/api/admin/GetRestaurant_MonNoiBat");

            if (!response.IsSuccessStatusCode)
            {
                ViewData["error"] = "Lỗi hiển thị phòng";
                return View();
            }

            var result = await response.Content.ReadFromJsonAsync<List<MonnoibatViewModel>>();

            return View("Restaurant_MonNoiBat_Index", result);
        }
        public async Task<IActionResult> Restaurant_MonTrangMieng_Index()
        {
            HttpClient client = _factory.CreateClient();
            var response = await client.GetAsync(BASE_URL + $"/api/admin/GetMonTrangMieng");

            if (!response.IsSuccessStatusCode)
            {
                ViewData["error"] = "Lỗi hiển thị phòng";
                return View();
            }

            var result = await response.Content.ReadFromJsonAsync<List<MontrangmiengViewModel>>();

            return View("Restaurant_MonTrangMieng_Index", result);
        }
        #endregion

        #region user
        // thêm người dùng
        [HttpPost]
        public async Task<IActionResult> SignUp(Quanlytaikhoan addtaikhoan)
        {
            HttpClient client = _factory.CreateClient();
            var content = new StringContent(JsonConvert.SerializeObject(addtaikhoan), Encoding.UTF8, "application/json");
            var response = await client.PostAsync(BASE_URL + $"/api/admin/SignUp", content);

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var errorResponse = await response.Content.ReadAsStringAsync();
                dynamic errorObject = JsonConvert.DeserializeObject<dynamic>(errorResponse);
                string error = errorObject.error;
                ViewBag.Message = HttpUtility.HtmlDecode(error);
                return View("User_Create");
            }
            ViewBag.Message = "Thành công!";
            return View("User_Create");
        }

        // Lấy thông tin người dùng để chỉnh sửa
        public async Task<IActionResult> User_Edit(int id)
        {
            HttpClient client = _factory.CreateClient();
            var response = await client.GetAsync(BASE_URL + $"/api/admin/GetUser/{id}");
            if (!response.IsSuccessStatusCode)
            {
                ViewData["error"] = "Lỗi hiển thị thông tin người dùng";
                return View("User_Index");
            }
            var result = await response.Content.ReadFromJsonAsync<List<Quanlytaikhoan>>();
            if (result == null || result.Count == 0)
            {
                ViewData["error"] = "Không tìm thấy thông tin người dùng";
                return View("User_Index");
            }
            var item = result[0];
            var user = new Quanlytaikhoan
            {
                Id = item.Id,
                HoTen = item.HoTen,
                Sdt = item.Sdt,
                Email = item.Email,
                Cmnd = item.Cmnd,
                PassWord = item.PassWord
            };
            return View(user);
        }
        // cập nhật thông tin
        [HttpPost]
        public async Task<IActionResult> User_Put(int id, string hoten, string sdt, string email, string cmnd, string password)
        {
            var data = new List<object>
                {
                    new { id, hoten, sdt, email, cmnd, password }
                };

            var jsonContent = JsonConvert.SerializeObject(data);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, BASE_URL + $"/api/admin/User_Update/{id}/{hoten}/{sdt}/{email}/{cmnd}/{password}");
            request.Content = content;

            var client = _factory.CreateClient();
            var response = await client.SendAsync(request);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("User_Index");
            }
            else
            {
                ViewData["error"] = "Lỗi cập nhật thông tin khách hàng";
                return View("User_Edit");
            }
        }
        // lấy thông tin người dùng để xoá
        public async Task<IActionResult> User_Delete(int id)
        {
            HttpClient client = _factory.CreateClient();
            var response = await client.GetAsync(BASE_URL + $"/api/admin/GetUser/{id}");
            if (!response.IsSuccessStatusCode)
            {
                ViewData["error"] = "Lỗi hiển thị thông tin người dùng";
                return View("User_Delete");
            }
            var result = await response.Content.ReadFromJsonAsync<List<Quanlytaikhoan>>();
            if (result == null || result.Count == 0)
            {
                ViewData["error"] = "Không tìm thấy thông tin người dùng";
                return View("User_Delete");
            }
            var item = result[0];
            var user = new Quanlytaikhoan
            {
                Id = item.Id,
                HoTen = item.HoTen,
                Sdt = item.Sdt,
                Email = item.Email,
                Cmnd = item.Cmnd,
                PassWord = item.PassWord
            };
            return View(user);
        }
        // xoá người dùng
        [HttpPost]
        public async Task<IActionResult> User_Delete_Conf(int id)
        {
            using (var client = _factory.CreateClient())
            {
                var response = await client.DeleteAsync(BASE_URL + $"/api/admin/DeleteUser/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    ViewData["error"] = "Xóa thất bại";
                    return View("User_Index");
                }
                return RedirectToAction("User_Index");
            }
        }

        #endregion

        #region room
        // thêm phòng
        [HttpPost]
        public async Task<IActionResult> Room_Add(Img_Chitietphong room, IFormFile imgFile)
        {
            using (var httpClient = _factory.CreateClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    // Thêm thông tin phòng vào content
                    content.Add(new StringContent(room.IdPhong.ToString()), "IdPhong");
                    content.Add(new StringContent(room.TenPhong), "TenPhong");
                    content.Add(new StringContent(room.NguoiMax.ToString()), "NguoiMax");
                    content.Add(new StringContent(room.LoaiGiuong), "LoaiGiuong");
                    content.Add(new StringContent(room.GiaPhong.ToString()), "GiaPhong");
                    content.Add(new StringContent(room.TinhTrang.ToString()), "SoLuongPhong");
                    content.Add(new StringContent(room.DienTich.ToString()), "DienTich");
                    content.Add(new StringContent(room.TamNhin), "TamNhin");
                    content.Add(new StringContent(room.Mota), "Mota");

                    // Thêm tệp tin ảnh vào content
                    if (imgFile != null)
                    {
                        using (var stream = new MemoryStream())
                        {
                            await imgFile.CopyToAsync(stream);
                            stream.Seek(0, SeekOrigin.Begin);
                            byte[] imageData = stream.ToArray();
                            content.Add(new ByteArrayContent(imageData), "Image", imgFile.FileName);
                        }
                    }
                    using (var response = await httpClient.PostAsync(BASE_URL + $"/api/admin/Create_Room", content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Room_Index");
                        }
                        else
                        {
                            string errorContent = await response.Content.ReadAsStringAsync();
                            ViewData["error"] = "Lỗi thêm phòng mới" + errorContent;
                            return View("Room_Create");
                        }
                    }
                }
            }
        }

        // Lấy thông tin phòng để chỉnh sửa
        public async Task<IActionResult> Room_Edit(int id)
        {
            HttpClient client = _factory.CreateClient();
            var response = await client.GetAsync(BASE_URL + $"/api/admin/GetRoom/{id}");
            if (!response.IsSuccessStatusCode)
            {
                ViewData["error"] = "Lỗi hiển thị thông tin người dùng";
                return View("Room_Index");
            }
            var result = await response.Content.ReadFromJsonAsync<List<Chitietphong>>();
            if (result == null || result.Count == 0)
            {
                ViewData["error"] = "Không tìm thấy thông tin người dùng";
                return View("Room_Index");
            }
            var item = result[0];
            var user = new Chitietphong
            {
                Id = item.Id,
                IdPhong = item.IdPhong,
                TenPhong = item.TenPhong,
                NguoiMax = item.NguoiMax,
                LoaiGiuong = item.LoaiGiuong,
                GiaPhong = item.GiaPhong,
                TinhTrang = item.TinhTrang,
                Img = item.Img,
                DienTich = item.DienTich,
                TamNhin = item.TamNhin,
                Mota = item.Mota
            };
            return View(user);
        }

        // cập nhật thông tin
        [HttpPost]
        public async Task<IActionResult> Room_Put(Img_Chitietphong room, IFormFile imgFile)
        {
            using (var httpClient = _factory.CreateClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    // Thêm thông tin phòng vào content
                    content.Add(new StringContent(room.Id.ToString()), "Id");
                    content.Add(new StringContent(room.IdPhong.ToString()), "IdPhong");
                    content.Add(new StringContent(room.TenPhong), "TenPhong");
                    content.Add(new StringContent(room.NguoiMax.ToString()), "NguoiMax");
                    content.Add(new StringContent(room.LoaiGiuong), "LoaiGiuong");
                    content.Add(new StringContent(room.GiaPhong.ToString()), "GiaPhong");
                    content.Add(new StringContent(room.TinhTrang.ToString()), "SoLuongPhong");
                    content.Add(new StringContent(room.DienTich.ToString()), "DienTich");
                    content.Add(new StringContent(room.TamNhin), "TamNhin");
                    content.Add(new StringContent(room.Mota), "Mota");

                    // Thêm tệp tin ảnh vào content
                    if (imgFile != null)
                    {
                        using (var stream = new MemoryStream())
                        {
                            await imgFile.CopyToAsync(stream);
                            stream.Seek(0, SeekOrigin.Begin);
                            byte[] imageData = stream.ToArray();
                            content.Add(new ByteArrayContent(imageData), "Image", imgFile.FileName);
                        }
                    }
                    using (var response = await httpClient.PostAsync(BASE_URL + $"/api/admin/Room_Update", content))
                    {
                        var client = _factory.CreateClient();
                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Room_Index");
                        }
                        else
                        {
                            string errorContent = await response.Content.ReadAsStringAsync();
                            ViewData["error"] = "Lỗi thêm phòng mới" + errorContent;
                            return View("Room_Create");
                        }
                    }

                }
            }
        }


        // lấy thông tin người dùng để xoá
        public async Task<IActionResult> Room_Delete(int id)
        {
            HttpClient client = _factory.CreateClient();
            var response = await client.GetAsync(BASE_URL + $"/api/admin/GetRoom/{id}");
            if (!response.IsSuccessStatusCode)
            {
                ViewData["error"] = "Lỗi hiển thị thông tin phòng";
                return View("Room_Delete");
            }
            var result = await response.Content.ReadFromJsonAsync<List<Chitietphong>>();
            if (result == null || result.Count == 0)
            {
                ViewData["error"] = "Không tìm thấy thông tin phòng";
                return View("Room_Delete");
            }
            var item = result[0];
            var room = new Chitietphong
            {
                Id = item.Id,
                IdPhong = item.IdPhong,
                TenPhong = item.TenPhong,
                NguoiMax = item.NguoiMax,
                LoaiGiuong = item.LoaiGiuong,
                GiaPhong = item.GiaPhong,
                TinhTrang = item.TinhTrang,
                Img = item.Img,
                DienTich = item.DienTich,
                TamNhin = item.TamNhin,
                Mota = item.Mota
            };
            return View(room);
        }

        // xoá phòng
        [HttpPost]
        public async Task<IActionResult> Room_Delete_Conf(int id)
        {
            using (var client = _factory.CreateClient())
            {
                var response = await client.DeleteAsync(BASE_URL + $"/api/admin/DeleteRoom/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    ViewData["error"] = "Xóa thất bại";
                    return View("Room_Index");
                }
                return RedirectToAction("Room_Index");
            }
        }

        #endregion

        #region buffet

        [HttpPost]
        public async Task<IActionResult> Restaurant_Buffet_Add(SetbuffetModel model, IFormFile imgFile)
        {
            using (var httpClient = _factory.CreateClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    content.Add(new StringContent(model.TenSet), "TenSet");
                    content.Add(new StringContent(model.Gia.ToString()), "Gia");
                    // Thêm tệp tin ảnh vào content
                    if (imgFile != null)
                    {
                        using (var stream = new MemoryStream())
                        {
                            await imgFile.CopyToAsync(stream);
                            stream.Seek(0, SeekOrigin.Begin);
                            byte[] imageData = stream.ToArray();
                            content.Add(new ByteArrayContent(imageData), "Image", imgFile.FileName);
                        }
                    }
                    using (var response = await httpClient.PostAsync(BASE_URL + $"/api/admin/Create_Buffet", content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Restaurant_Buffet_Index");
                        }
                        else
                        {
                            string errorContent = await response.Content.ReadAsStringAsync();
                            ViewData["error"] = "Lỗi thêm buffet mới:" + errorContent;
                            return View("Restaurant_Buffet_Create");
                        }
                    }
                }
            }
        }

        public async Task<IActionResult> Restaurant_Buffet_Edit(int id)
        {
            HttpClient client = _factory.CreateClient();
            var response = await client.GetAsync(BASE_URL + $"/api/admin/GetBuffet/{id}");
            if (!response.IsSuccessStatusCode)
            {
                ViewData["error"] = "Lỗi hiển thị thông tin người dùng";
                return View("Restaurant_Buffet_Index");
            }
            var result = await response.Content.ReadFromJsonAsync<List<SetbuffetViewModel>>();
            if (result == null || result.Count == 0)
            {
                ViewData["error"] = "Không tìm thấy thông tin người dùng";
                return View("Restaurant_Buffet_Index");
            }
            var item = result[0];
            var buffet = new SetbuffetViewModel
            {
                Id = item.Id,
                Img = item.Img,
                TenSet = item.TenSet,
                Gia = item.Gia
            };
            return View(buffet);
        }

        // cập nhật thông tin
        [HttpPost]
        public async Task<IActionResult> Restaurant_Buffet_Update(SetbuffetModel room, IFormFile imgFile)
        {
            using (var httpClient = _factory.CreateClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    // Thêm thông tin phòng vào content
                    content.Add(new StringContent(room.Id.ToString()), "Id");
                    content.Add(new StringContent(room.TenSet.ToString()), "TenSet");
                    content.Add(new StringContent(room.Gia), "Gia");

                    // Thêm tệp tin ảnh vào content
                    if (imgFile != null)
                    {
                        using (var stream = new MemoryStream())
                        {
                            await imgFile.CopyToAsync(stream);
                            stream.Seek(0, SeekOrigin.Begin);
                            byte[] imageData = stream.ToArray();
                            content.Add(new ByteArrayContent(imageData), "Image", imgFile.FileName);
                        }
                    }
                    using (var response = await httpClient.PutAsync(BASE_URL + $"/api/admin/Buffet_Update", content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Restaurant_Buffet_Index");
                        }
                        else
                        {
                            string errorContent = await response.Content.ReadAsStringAsync();
                            ViewData["error"] = "Lỗi thêm phòng mới" + errorContent;
                            return View("Restaurant_Buffet_Create");
                        }
                    }

                }
            }
        }

        public async Task<IActionResult> Restaurant_Buffet_Delete(int id)
        {
            HttpClient client = _factory.CreateClient();
            var response = await client.GetAsync(BASE_URL + $"/api/admin/GetBuffet/{id}");
            if (!response.IsSuccessStatusCode)
            {
                ViewData["error"] = "Lỗi hiển thị thông tin món";
                return View("Restaurant_Buffet_Delete");
            }
            var result = await response.Content.ReadFromJsonAsync<List<SetbuffetViewModel>>();
            if (result == null || result.Count == 0)
            {
                ViewData["error"] = "Không tìm thấy thông tin phòng";
                return View("Restaurant_Buffet_Delete");
            }
            var item = result[0];
            var buffet = new SetbuffetViewModel
            {
                Id = item.Id,
                Img = item.Img,
                TenSet = item.TenSet,
                Gia = item.Gia
            };
            return View(buffet);
        }

        // xoá phòng
        [HttpPost]
        public async Task<IActionResult> Buffet_Delete_Conf(int id)
        {
            using (var client = _factory.CreateClient())
            {
                var response = await client.DeleteAsync(BASE_URL + $"/api/admin/DeleteBuffet/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    ViewData["error"] = "Xóa thất bại";
                    return View("Restaurant_Buffet_Index");
                }
                return RedirectToAction("Restaurant_Buffet_Index");
            }
        }

        #endregion

        #region douong

        #endregion
        #region monnoibat
        [HttpPost]
        public async Task<IActionResult> Restaurant_MonNoiBat_Add(MonnoibatModel model, IFormFile imgFile)
        {
            using (var httpClient = _factory.CreateClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    content.Add(new StringContent(model.TenMon), "TenMon");
                    content.Add(new StringContent(model.Gia.ToString()), "Gia");

                    // Thêm tệp tin ảnh vào content
                    if (imgFile != null)
                    {
                        using (var stream = new MemoryStream())
                        {
                            await imgFile.CopyToAsync(stream);
                            stream.Seek(0, SeekOrigin.Begin);
                            byte[] imageData = stream.ToArray();
                            content.Add(new ByteArrayContent(imageData), "Img", imgFile.FileName);
                        }
                    }
                    using (var response = await httpClient.PostAsync(BASE_URL + $"/api/admin/Create_MonNoiBat", content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Restaurant_MonNoiBat_Index");
                        }
                        else
                        {
                            string errorContent = await response.Content.ReadAsStringAsync();
                            ViewData["error"] = "Lỗi thêm món  mới:" + errorContent;
                            return View("Restaurant_MonNoiBat_Create");
                        }
                    }
                }
            }
        }
        public async Task<IActionResult> Restaurant_MonNoiBat_Edit(int id)
        {
            HttpClient client = _factory.CreateClient();
            var response = await client.GetAsync(BASE_URL + $"/api/admin/GetMonNoiBat/{id}");
            if (!response.IsSuccessStatusCode)
            {
                ViewData["error"] = "Lỗi hiển thị thông tin người dùng";
                return View("Restaurant_MonNoiBat_Index");
            }
            var result = await response.Content.ReadFromJsonAsync<List<MonnoibatViewModel>>();
            if (result == null || result.Count == 0)
            {
                ViewData["error"] = "Không tìm thấy thông tin người dùng";
                return View("Restaurant_MonNoiBat_Index");
            }
            var item = result[0];
            var monnoibat = new MonnoibatViewModel
            {
                Id = item.Id,
                Img = item.Img,
                TenMon = item.TenMon,
                Gia = item.Gia
            };
            return View(monnoibat);
        }
        // cập nhật thông tin
        [HttpPost]
        public async Task<IActionResult> Restaurant_MonNoiBat_Update(MonnoibatModel room, IFormFile imgFile)
        {
            using (var httpClient = _factory.CreateClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    // Thêm thông tin phòng vào content
                    content.Add(new StringContent(room.Id.ToString()), "Id");
                    content.Add(new StringContent(room.TenMon.ToString()), "TenMon");
                    content.Add(new StringContent(room.Gia), "Gia");

                    // Thêm tệp tin ảnh vào content
                    if (imgFile != null)
                    {
                        using (var stream = new MemoryStream())
                        {
                            await imgFile.CopyToAsync(stream);
                            stream.Seek(0, SeekOrigin.Begin);
                            byte[] imageData = stream.ToArray();
                            content.Add(new ByteArrayContent(imageData), "Img", imgFile.FileName);
                        }
                    }
                    using (var response = await httpClient.PutAsync(BASE_URL + $"/api/admin/MonNoiBat_Update", content))
                    {

                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Restaurant_MonNoiBat_Index");
                        }
                        else
                        {
                            string errorContent = await response.Content.ReadAsStringAsync();
                            ViewData["error"] = "Lỗi thêm món " + errorContent;
                            return View("Restaurant_MonNoiBat_Edit");
                        }
                    }

                }
            }
        }
        public async Task<IActionResult> Restaurant_MonNoiBat_Delete(int id)
        {
            HttpClient client = _factory.CreateClient();
            var response = await client.GetAsync(BASE_URL + $"/api/admin/GetMonNoiBat/{id}");
            if (!response.IsSuccessStatusCode)
            {
                ViewData["error"] = "Lỗi hiển thị thông tin món";
                return View("Restaurant_MonNoiBat_Delete");
            }
            var result = await response.Content.ReadFromJsonAsync<List<MonnoibatViewModel>>();
            if (result == null || result.Count == 0)
            {
                ViewData["error"] = "Không tìm thấy thông tin món";
                return View("Restaurant_MonNoiBat_Delete");
            }
            var item = result[0];
            var monnoibat = new MonnoibatViewModel
            {
                Id = item.Id,
                Img = item.Img,
                TenMon = item.TenMon,
                Gia = item.Gia
            };
            return View(monnoibat);
        }

        // xoá phòng
        [HttpPost]
        public async Task<IActionResult> MonNoiBat_Delete_Conf(int id)
        {
            using (var client = _factory.CreateClient())
            {
                var response = await client.DeleteAsync(BASE_URL + $"/api/admin/DeleteMonNoiBat/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    ViewData["error"] = "Xóa thất bại";
                    return View("Restaurant_MonNoiBat_Index");
                }
                return RedirectToAction("Restaurant_MonNoiBat_Index");
            }
        }
        #endregion

        #region montrangmieng
        [HttpPost]
        public async Task<IActionResult> Restaurant_MonTrangMieng_Add(MontrangmiengModel model, IFormFile imgFile)
        {
            using (var httpClient = _factory.CreateClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    content.Add(new StringContent(model.TenMon), "TenMon");
                    content.Add(new StringContent(model.Gia.ToString()), "Gia");

                    // Thêm tệp tin ảnh vào content
                    if (imgFile != null)
                    {
                        using (var stream = new MemoryStream())
                        {
                            await imgFile.CopyToAsync(stream);
                            stream.Seek(0, SeekOrigin.Begin);
                            byte[] imageData = stream.ToArray();
                            content.Add(new ByteArrayContent(imageData), "Img", imgFile.FileName);
                        }
                    }
                    using (var response = await httpClient.PostAsync(BASE_URL + $"/api/admin/Create_MonTrangMieng", content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Restaurant_MonTrangMieng_Index");
                        }
                        else
                        {
                            string errorContent = await response.Content.ReadAsStringAsync();
                            ViewData["error"] = "Lỗi thêm món   mới:" + errorContent;
                            return View("Restaurant_MonTrangMieng_Create");
                        }
                    }
                }
            }
        }
        public async Task<IActionResult> Restaurant_MonTrangMieng_Edit(int id)
        {
            HttpClient client = _factory.CreateClient();
            var response = await client.GetAsync(BASE_URL + $"/api/admin/GetMonTrangMieng/{id}");
            if (!response.IsSuccessStatusCode)
            {
                ViewData["error"] = "Lỗi hiển thị thông tin người dùng";
                return View("Restaurant_MonTrangMieng_Index");
            }
            var result = await response.Content.ReadFromJsonAsync<List<MontrangmiengViewModel>>();
            if (result == null || result.Count == 0)
            {
                ViewData["error"] = "Không tìm thấy thông tin người dùng";
                return View("Restaurant_MonTrangMieng_Index");
            }
            var item = result[0];
            var montrangmieng = new MontrangmiengViewModel
            {
                Id = item.Id,
                Img = item.Img,
                TenMon = item.TenMon,
                Gia = item.Gia
            };
            return View(montrangmieng);
        }
        // cập nhật thông tin
        [HttpPost]
        public async Task<IActionResult> Restaurant_MonTrangMieng_Update(MontrangmiengModel room, IFormFile imgFile)
        {
            using (var httpClient = _factory.CreateClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    // Thêm thông tin phòng vào content
                    content.Add(new StringContent(room.Id.ToString()), "Id");
                    content.Add(new StringContent(room.TenMon.ToString()), "TenMon");
                    content.Add(new StringContent(room.Gia), "Gia");

                    // Thêm tệp tin ảnh vào content
                    if (imgFile != null)
                    {
                        using (var stream = new MemoryStream())
                        {
                            await imgFile.CopyToAsync(stream);
                            stream.Seek(0, SeekOrigin.Begin);
                            byte[] imageData = stream.ToArray();
                            content.Add(new ByteArrayContent(imageData), "Img", imgFile.FileName);
                        }
                    }
                    using (var response = await httpClient.PostAsync(BASE_URL + $"/api/admin/MonTrangMieng_Update", content))
                    {

                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Restaurant_MonTrangMieng_Index");
                        }
                        else
                        {
                            string errorContent = await response.Content.ReadAsStringAsync();
                            ViewData["error"] = "Lỗi thêm món " + errorContent;
                            return View("Restaurant_MonTrangMieng_Edit");
                        }
                    }

                }
            }
        }
        public async Task<IActionResult> Restaurant_MonTrangMieng_Delete(int id)
        {
            HttpClient client = _factory.CreateClient();
            var response = await client.GetAsync(BASE_URL + $"/api/admin/GetMonTrangMieng/{id}");
            if (!response.IsSuccessStatusCode)
            {
                ViewData["error"] = "Lỗi hiển thị thông tin món";
                return View("Restaurant_MonTrangMieng_Delete");
            }
            var result = await response.Content.ReadFromJsonAsync<List<MontrangmiengViewModel>>();
            if (result == null || result.Count == 0)
            {
                ViewData["error"] = "Không tìm thấy thông tin món";
                return View("Restaurant_MonTrangMieng_Delete");
            }
            var item = result[0];
            var montrangmieng = new MontrangmiengViewModel
            {
                Id = item.Id,
                Img = item.Img,
                TenMon = item.TenMon,
                Gia = item.Gia
            };
            return View(montrangmieng);
        }

        // xoá phòng
        [HttpPost]
        public async Task<IActionResult> MonTrangMieng_Delete_Conf(int id)
        {
            using (var client = _factory.CreateClient())
            {
                var response = await client.DeleteAsync(BASE_URL + $"/api/admin/DeleteMonTrangMieng/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    ViewData["error"] = "Xóa thất bại";
                    return View("Restaurant_MonTrangMieng_Index");
                }
                return RedirectToAction("Restaurant_MonTrangMieng_Index");
            }
        }
        #endregion

        #region monchinh
        [HttpPost]
        public async Task<IActionResult> Restaurant_MonChinh_Add(MonchinhModel model, IFormFile imgFile)
        {
            using (var httpClient = _factory.CreateClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    content.Add(new StringContent(model.TenMon), "TenMon");
                    content.Add(new StringContent(model.Gia.ToString()), "Gia");

                    // Thêm tệp tin ảnh vào content
                    if (imgFile != null)
                    {
                        using (var stream = new MemoryStream())
                        {
                            await imgFile.CopyToAsync(stream);
                            stream.Seek(0, SeekOrigin.Begin);
                            byte[] imageData = stream.ToArray();
                            content.Add(new ByteArrayContent(imageData), "Img", imgFile.FileName);
                        }
                    }
                    using (var response = await httpClient.PostAsync(BASE_URL + $"/api/admin/Create_MonChinh", content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Restaurant_MonChinh_Index");
                        }
                        else
                        {
                            string errorContent = await response.Content.ReadAsStringAsync();
                            ViewData["error"] = "Lỗi thêm món chính  mới:" + errorContent;
                            return View("Restaurant_MonChinh_Create");
                        }
                    }
                }
            }
        }
        public async Task<IActionResult> Restaurant_MonChinh_Edit(int id)
        {
            HttpClient client = _factory.CreateClient();
            var response = await client.GetAsync(BASE_URL + $"/api/admin/GetMonChinh/{id}");
            if (!response.IsSuccessStatusCode)
            {
                ViewData["error"] = "Lỗi hiển thị thông tin người dùng";
                return View("Restaurant_MonChinh_Index");
            }
            var result = await response.Content.ReadFromJsonAsync<List<MonchinhViewModel>>();
            if (result == null || result.Count == 0)
            {
                ViewData["error"] = "Không tìm thấy thông tin người dùng";
                return View("Restaurant_MonChinh_Index");
            }
            var item = result[0];
            var monchinh = new MonchinhViewModel
            {
                Id = item.Id,
                Img = item.Img,
                TenMon = item.TenMon,
                Gia = item.Gia
            };
            return View(monchinh);
        }
        // cập nhật thông tin
        [HttpPost]
        public async Task<IActionResult> Restaurant_MonChinh_Update(MonchinhModel room, IFormFile imgFile)
        {
            using (var httpClient = _factory.CreateClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    // Thêm thông tin phòng vào content
                    content.Add(new StringContent(room.Id.ToString()), "Id");
                    content.Add(new StringContent(room.TenMon.ToString()), "TenMon");
                    content.Add(new StringContent(room.Gia), "Gia");

                    // Thêm tệp tin ảnh vào content
                    if (imgFile != null)
                    {
                        using (var stream = new MemoryStream())
                        {
                            await imgFile.CopyToAsync(stream);
                            stream.Seek(0, SeekOrigin.Begin);
                            byte[] imageData = stream.ToArray();
                            content.Add(new ByteArrayContent(imageData), "Img", imgFile.FileName);
                        }
                    }
                    using (var response = await httpClient.PutAsync(BASE_URL + $"/api/admin/MonChinh_Update", content))
                    {
                        var client = _factory.CreateClient();
                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Restaurant_MonChinh_Index");
                        }
                        else
                        {
                            string errorContent = await response.Content.ReadAsStringAsync();
                            ViewData["error"] = "Lỗi thêm món chính " + errorContent;
                            return View("Restaurant_MonChinh_Edit");
                        }
                    }

                }
            }
        }
        public async Task<IActionResult> Restaurant_MonChinh_Delete(int id)
        {
            HttpClient client = _factory.CreateClient();
            var response = await client.GetAsync(BASE_URL + $"/api/admin/GetMonchinh/{id}");
            if (!response.IsSuccessStatusCode)
            {
                ViewData["error"] = "Lỗi hiển thị thông tin món";
                return View("Restaurant_MonChinh_Delete");
            }
            var result = await response.Content.ReadFromJsonAsync<List<MonchinhViewModel>>();
            if (result == null || result.Count == 0)
            {
                ViewData["error"] = "Không tìm thấy thông tin món";
                return View("Restaurant_Buffet_Delete");
            }
            var item = result[0];
            var monchinh = new MonchinhViewModel
            {
                Id = item.Id,
                Img = item.Img,
                TenMon = item.TenMon,
                Gia = item.Gia
            };
            return View(monchinh);
        }

        // xoá phòng
        [HttpPost]
        public async Task<IActionResult> MonChinh_Delete_Conf(int id)
        {
            using (var client = _factory.CreateClient())
            {
                var response = await client.DeleteAsync(BASE_URL + $"/api/admin/DeleteMonChinh/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    ViewData["error"] = "Xóa thất bại";
                    return View("Restaurant_MonChinh_Index");
                }
                return RedirectToAction("Restaurant_MonChinh_Index");
            }
        }
        #endregion

        #region monkhaivi
        [HttpPost]
        public async Task<IActionResult> Restaurant_MonKhaiVi_Add(MonkhaiviModel model, IFormFile imgFile)
        {
            using (var httpClient = _factory.CreateClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    content.Add(new StringContent(model.TenMon), "TenMon");
                    content.Add(new StringContent(model.Gia.ToString()), "Gia");

                    // Thêm tệp tin ảnh vào content
                    if (imgFile != null)
                    {
                        using (var stream = new MemoryStream())
                        {
                            await imgFile.CopyToAsync(stream);
                            stream.Seek(0, SeekOrigin.Begin);
                            byte[] imageData = stream.ToArray();
                            content.Add(new ByteArrayContent(imageData), "Img", imgFile.FileName);
                        }
                    }
                    using (var response = await httpClient.PostAsync(BASE_URL + $"/api/admin/Create_MonKhaiVi", content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Restaurant_MonKhaiVi_Index");
                        }
                        else
                        {
                            string errorContent = await response.Content.ReadAsStringAsync();
                            ViewData["error"] = "Lỗi thêm món   mới:" + errorContent;
                            return View("Restaurant_MonKhaiVi_Create");
                        }
                    }
                }
            }
        }
        public async Task<IActionResult> Restaurant_MonKhaiVi_Edit(int id)
        {
            HttpClient client = _factory.CreateClient();
            var response = await client.GetAsync(BASE_URL + $"/api/admin/GetMonKhaiVi/{id}");
            if (!response.IsSuccessStatusCode)
            {
                ViewData["error"] = "Lỗi hiển thị thông tin người dùng";
                return View("Restaurant_MonKhaiVi_Index");
            }
            var result = await response.Content.ReadFromJsonAsync<List<MonkhaiviViewModel>>();
            if (result == null || result.Count == 0)
            {
                ViewData["error"] = "Không tìm thấy thông tin người dùng";
                return View("Restaurant_MonKhaiVi_Index");
            }
            var item = result[0];
            var monkhaivi = new MonkhaiviViewModel
            {
                Id = item.Id,
                Img = item.Img,
                TenMon = item.TenMon,
                Gia = item.Gia
            };
            return View(monkhaivi);
        }
        // cập nhật thông tin
        [HttpPost]
        public async Task<IActionResult> Restaurant_MonKhaiVi_Update(MonkhaiviModel room, IFormFile imgFile)
        {
            using (var httpClient = _factory.CreateClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    // Thêm thông tin phòng vào content
                    content.Add(new StringContent(room.Id.ToString()), "Id");
                    content.Add(new StringContent(room.TenMon.ToString()), "TenMon");
                    content.Add(new StringContent(room.Gia), "Gia");

                    // Thêm tệp tin ảnh vào content
                    if (imgFile != null)
                    {
                        using (var stream = new MemoryStream())
                        {
                            await imgFile.CopyToAsync(stream);
                            stream.Seek(0, SeekOrigin.Begin);
                            byte[] imageData = stream.ToArray();
                            content.Add(new ByteArrayContent(imageData), "Img", imgFile.FileName);
                        }
                    }
                    using (var response = await httpClient.PostAsync(BASE_URL + $"/api/admin/MonKhaiVi_Update", content))
                    {
                        var client = _factory.CreateClient();
                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Restaurant_MonKhaiVi_Index");
                        }
                        else
                        {
                            string errorContent = await response.Content.ReadAsStringAsync();
                            ViewData["error"] = "Lỗi thêm món khai vị " + errorContent;
                            return View("Restaurant_MonKhaiVi_Edit");
                        }
                    }

                }
            }
        }
        public async Task<IActionResult> Restaurant_MonKhaiVi_Delete(int id)
        {
            HttpClient client = _factory.CreateClient();
            var response = await client.GetAsync(BASE_URL + $"/api/admin/GetMonKhaiVi/{id}");
            if (!response.IsSuccessStatusCode)
            {
                ViewData["error"] = "Lỗi hiển thị thông tin món";
                return View("Restaurant_MonKhaiVi_Delete");
            }
            var result = await response.Content.ReadFromJsonAsync<List<MonkhaiviViewModel>>();
            if (result == null || result.Count == 0)
            {
                ViewData["error"] = "Không tìm thấy thông tin món";
                return View("Restaurant_MonKhaiVi_Delete");
            }
            var item = result[0];
            var monkhaivi = new MonkhaiviViewModel
            {
                Id = item.Id,
                Img = item.Img,
                TenMon = item.TenMon,
                Gia = item.Gia
            };
            return View(monkhaivi);
        }

        // xoá phòng
        [HttpPost]
        public async Task<IActionResult> MonKhaiVi_Delete_Conf(int id)
        {
            using (var client = _factory.CreateClient())
            {
                var response = await client.DeleteAsync(BASE_URL + $"/api/admin/DeleteMonKhaiVi/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    ViewData["error"] = "Xóa thất bại";
                    return View("Restaurant_MonKhaiVi_Index");
                }
                return RedirectToAction("Restaurant_MonKhaiVi_Index");
            }
        }
        #endregion

        #region douong
        [HttpPost]
        public async Task<IActionResult> Restaurant_DoUong_Add(DouongModel model, IFormFile imgFile)
        {
            using (var httpClient = _factory.CreateClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    content.Add(new StringContent(model.TenDoUong), "TenDoUong");
                    content.Add(new StringContent(model.Gia.ToString()), "Gia");

                    // Thêm tệp tin ảnh vào content
                    if (imgFile != null)
                    {
                        using (var stream = new MemoryStream())
                        {
                            await imgFile.CopyToAsync(stream);
                            stream.Seek(0, SeekOrigin.Begin);
                            byte[] imageData = stream.ToArray();
                            content.Add(new ByteArrayContent(imageData), "Img", imgFile.FileName);
                        }
                    }
                    using (var response = await httpClient.PostAsync(BASE_URL + $"/api/admin/Create_DoUong", content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Restaurant_DoUong_Index");
                        }
                        else
                        {
                            string errorContent = await response.Content.ReadAsStringAsync();
                            ViewData["error"] = "Lỗi thêm đồ uống:" + errorContent;
                            return View("Restaurant_DoUong_Create");
                        }
                    }
                }
            }
        }
        public async Task<IActionResult> Restaurant_DoUong_Edit(int id)
        {
            HttpClient client = _factory.CreateClient();
            var response = await client.GetAsync(BASE_URL + $"/api/admin/GetDoUong/{id}");
            if (!response.IsSuccessStatusCode)
            {
                ViewData["error"] = "Lỗi hiển thị thông tin người dùng";
                return View("Restaurant_DoUong_Index");
            }
            var result = await response.Content.ReadFromJsonAsync<List<DouongViewModel>>();
            if (result == null || result.Count == 0)
            {
                ViewData["error"] = "Không tìm thấy thông tin người dùng";
                return View("Restaurant_DoUong_Index");
            }
            var item = result[0];
            var douong = new DouongViewModel
            {
                Id = item.Id,
                Img = item.Img,
                TenDoUong = item.TenDoUong,
                Gia = item.Gia
            };
            return View("Restaurant_DoUong_Edit", douong);
        }
        // cập nhật thông tin
        [HttpPost]
        public async Task<IActionResult> Restaurant_DoUong_Update(DouongModel room, IFormFile imgFile)
        {

            using (HttpClient httpClient = _factory.CreateClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    // Thêm thông tin phòng vào content

                    content.Add(new StringContent(room.Id.ToString()), "Id");
                    content.Add(new StringContent(room.TenDoUong.ToString()), "TenDoUong");
                    content.Add(new StringContent(room.Gia.ToString()), "Gia");

                    // Thêm tệp tin ảnh vào content
                    if (imgFile != null)
                    {
                        using (var stream = new MemoryStream())
                        {
                            await imgFile.CopyToAsync(stream);
                            stream.Seek(0, SeekOrigin.Begin);
                            byte[] imageData = stream.ToArray();
                            content.Add(new ByteArrayContent(imageData), "Img", imgFile.FileName);
                        }
                    }

                    using (var response = await httpClient.PutAsync($"{BASE_URL}/api/admin/DoUong_Update", content))
                    {
                        if (response.IsSuccessStatusCode)
                        {
                            return RedirectToAction("Restaurant_DoUong_Index");
                        }
                        else
                        {
                            string errorContent = await response.Content.ReadAsStringAsync();
                            ViewData["error"] = "Lỗi cập nhập đồ uống " + errorContent;
                            return View("Restaurant_DoUong_Edit");
                        }
                    }

                }
            }
        }
        public async Task<IActionResult> Restaurant_DoUong_Delete(int id)
        {
            HttpClient client = _factory.CreateClient();
            var response = await client.GetAsync(BASE_URL + $"/api/admin/GetDoUong/{id}");
            if (!response.IsSuccessStatusCode)
            {
                ViewData["error"] = "Lỗi hiển thị thông tin đồ uống";
                return View("Restaurant_DoUong_Delete");
            }
            var result = await response.Content.ReadFromJsonAsync<List<DouongViewModel>>();
            if (result == null || result.Count == 0)
            {
                ViewData["error"] = "Không tìm thấy thông tin đồ uống";
                return View("Restaurant_DoUong_Delete");
            }
            var item = result[0];
            var douong = new DouongViewModel
            {
                Id = item.Id,
                Img = item.Img,
                TenDoUong = item.TenDoUong,
                Gia = item.Gia
            };
            return View(douong);
        }

        // xoá phòng
        [HttpPost]
        public async Task<IActionResult> DoUong_Delete_Conf(int id)
        {
            using (var client = _factory.CreateClient())
            {
                var response = await client.DeleteAsync(BASE_URL + $"/api/admin/DeleteDoUong/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    ViewBag.error = "Xóa thất bại";
                    return View("Restaurant_DoUong_Index");
                }
                return RedirectToAction("Restaurant_DoUong_Delete");
            }
        }

        #endregion
        [HttpGet]
        public async Task<IActionResult> QuanLyDatPhongd()
        {
            HttpClient client = _factory.CreateClient();
            var response = await client.GetAsync(BASE_URL + $"/api/Admin/QuanLyDatPhong");
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
                    phong.Id = item.id;
                    phong.HoTen=item.hoTen;
                    phong.Sdt = item.sdt;
                    phong.Email=item.email;
                    phong.TenPhong = item.tenPhong;
                 
                    phong.TenDichVu = new List<string>();

                    foreach (var dichvu in item.dichVu)
                    {
                        string tenDichVu = dichvu.tenDichVu;
                        phong.TenDichVu.Add(tenDichVu);
                    }
                    phong.NgayDen = item.ngayDen;
                    phong.NgayDi = item.ngayDi;
                    phong.NgayTt = item.ngayTt;
                    phong.PhuongThucThanhToan = item.phuongThucThanhToan;
                    phong.ThanhTien=item.thanhTien;

                    phongs.Add(phong);
                }

                return View("QuanLyDatPhong", phongs);
            }


            return NotFound();

        }
        [HttpPost]
        public async Task<IActionResult> TimKiemDatPhong(string timkiem)
        {
            HttpClient client = _factory.CreateClient();
            var response = await client.GetAsync(BASE_URL + $"/api/Admin/TimKiemDatPhong/{timkiem}");
            if (!response.IsSuccessStatusCode)
            {
                ViewBag.error = "Lỗi hiển thị";
                return View("QuanLyDatPhong");
            }

            var jsonString = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<dynamic>>(jsonString);
            if (result != null)
            {
                var phongs = new List<dynamic>();

                foreach (var item in result)
                {
                    dynamic phong = new System.Dynamic.ExpandoObject();
                    phong.Id = item.id;
                    phong.HoTen = item.hoTen;
                    phong.Sdt = item.sdt;
                    phong.Email = item.email;
                    phong.TenPhong = item.tenPhong;

                    phong.TenDichVu = new List<string>();

                    foreach (var dichvu in item.dichVu)
                    {
                        string tenDichVu = dichvu.tenDichVu;
                        phong.TenDichVu.Add(tenDichVu);
                    }
                    phong.NgayDen = item.ngayDen;
                    phong.NgayDi = item.ngayDi;
                    phong.NgayTt = item.ngayTt;
                    phong.PhuongThucThanhToan = item.phuongThucThanhToan;
                    phong.ThanhTien = item.thanhTien;

                    phongs.Add(phong);
                }

                return View("QuanLyDatPhong", phongs);
            }
            else
            {
                ViewBag.error = "Không tìm thấy kết quả!";
                return View("QuanLyDatPhong");
            }

           
        }
    }
}
