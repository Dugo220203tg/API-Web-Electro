using API_Web_Shop_Electronic_TD.Data;
using API_Web_Shop_Electronic_TD.Models;

namespace API_Web_Shop_Electronic_TD.Mappers
{
	public static class ChiTietHoaDonMapper
	{
		public static ChiTietHoaDonMD ToCtHoaDonDo(this ChiTietHd Model)
		{
			return new ChiTietHoaDonMD
			{
				MaCT = Model.MaCt,
				MaHD = Model.MaHd,
				MaHH = Model.MaHh,
				DonGia = Model.MaHhNavigation?.DonGia ?? 0,
				SoLuong = Model.SoLuong,
				MaGiamGia = (int)Model.MaGiamGia,
				TenHangHoa = Model.MaHhNavigation?.TenHh ?? string.Empty
			};
		}

		public static ChiTietHd ToCtHoaDonDTO(this ChiTietHoaDonMD Model)
		{
			return new ChiTietHd
			{
				MaCt = Model.MaCT,
				MaHd = Model.MaHD,
				MaHh = Model.MaHH,
				DonGia = Model.DonGia,
				SoLuong = Model.SoLuong,
				MaGiamGia = Model.MaGiamGia,
				MaHhNavigation = new HangHoa
				{
					TenHh = Model.TenHangHoa
				}
			};
		}
	}
}