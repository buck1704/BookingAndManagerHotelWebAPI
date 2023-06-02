using System;
using System.Collections.Generic;

namespace QuanLyKhachSanClient.Models
{
    public partial class Chitietdichvu
    {
        public int? MaCtdv { get; set; }
        public int? Mpdv { get; set; }
        public string? MaDV { get; set; }

        public virtual Phieudichvu MpdvNavigation { get; set; } = null!;
        public virtual Dichvu MaDVNavigation { get; set; } = null!;
    }
}
