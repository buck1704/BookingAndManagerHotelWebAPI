
namespace QuanLyKhachSanClient.Models
{
    public partial class MonchinhViewModel
    {
        public int Id { get; set; }
        public string TenMon { get; set; } = null!;
        public string Gia { get; set; } = null!;
        public string Img { get; set; } = null!;
    }
    public partial class MonchinhModel
    {
        public int Id { get; set; }
        public string TenMon { get; set; } = null!;
        public string Gia { get; set; } = null!;
        public IFormFile Img { get; set; } = null!;
    }
}
