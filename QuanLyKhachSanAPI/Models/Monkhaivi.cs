using System;
using System.Collections.Generic;

namespace QuanLyKhachSanAPI.Models
{
    public partial class MonkhaiviViewModel
    {
        public int Id { get; set; }
        public string TenMon { get; set; } = null!;
        public string Gia { get; set; } = null!;
        public string Img { get; set; } = null!;
    }
    public partial class MonkhaiviModel
    {
        public int Id { get; set; }
        public string TenMon { get; set; } = null!;
        public string Gia { get; set; } = null!;
        public IFormFile Img { get; set; } = null!;
    }
}
