using System.ComponentModel.DataAnnotations;

namespace QuanLyKhachSanAPI.Models
{
    public partial class SetbuffetViewModel
    {
        [Key]
        public int Id { get; set; }
        public string TenSet { get; set; } = null!;
        public string Gia { get; set; } = null!;
        public string Img { get; set; } = null!;
    }

    public partial class SetbuffetModel
    {
        public int Id { get; set; }
        public string TenSet { get; set; } = null!;
        public string Gia { get; set; } = null!;
        public IFormFile Image { get; set; } = null!;
    }
}
