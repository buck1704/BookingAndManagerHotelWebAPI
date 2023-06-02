using System;
using System.Collections.Generic;

namespace QuanLyKhachSanAPI.Models
{
    public partial class Taikhoanadmin
    {
        public int Id { get; set; }
        public string HoTen { get; set; } = null!;
        public string Sdt { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PassWord { get; set; } = null!;
    }
}
