using System;
using System.Collections.Generic;

namespace QuanLyKhachSanClient.Models
{
    public partial class Phong
    {
        public Phong()
        {
            Chitietphongs = new HashSet<Chitietphong>();
        }

        public int Id { get; set; }
        public string LoaiPhong { get; set; } = null!;

        public virtual ICollection<Chitietphong> Chitietphongs { get; set; }
    }
}
