using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuanLyKhachSanAPI.Models
{
    public partial class MonnoibatViewModel
    {
        [Key]
        public int Id { get; set; }
        public string TenMon { get; set; } = null!;
        public string Gia { get; set; } = null!;
        public string Img { get; set; } = null!;
    }

    public partial class MonnoibatModel
    {
        public int Id { get; set; }
        public string TenMon { get; set; } = null!;
        public string Gia { get; set; } = null!;
        public IFormFile Img { get; set; } = null!;
    }
}
