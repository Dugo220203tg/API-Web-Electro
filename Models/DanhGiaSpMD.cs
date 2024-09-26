using System.ComponentModel.DataAnnotations;

namespace API_Web_Shop_Electronic_TD.Models
{
	public class DanhGiaSpMD
	{
		[Key]
		//public int MaDg { get; set; }
		public string MaKH { get; set; }
		public string NoiDung { get; set; }
		public int Sao {  get; set; }
		public DateTime Ngay { get; set; }
		[Key]
		public int MaHH { get; set; }
		public int TrangThai { get; set; }
	}
}
