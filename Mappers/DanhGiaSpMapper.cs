using API_Web_Shop_Electronic_TD.Data;
using API_Web_Shop_Electronic_TD.Models;

namespace API_Web_Shop_Electronic_TD.Mappers
{
	public static class DanhGiaSpMapper
	{
		public static DanhGiaSpMD ToDanhGiaDo(this DanhGiaSp Model)
		{
			return new DanhGiaSpMD
			{
				Sao = (int)Model.Sao,
				MaHH = Model.MaHh,
				Ngay = (DateTime)Model.Ngay,
				NoiDung = Model.NoiDung,
				TrangThai = (int)Model.TrangThai,
				MaKH = Model.MaKh
			};
		}
		public static DanhGiaSp ToDanhGiaDTO(this DanhGiaSpMD Model)
		{

			return new DanhGiaSp
			{
				Sao = Model.Sao,
				MaHh = Model.MaHH,
				Ngay = Model.Ngay,
				NoiDung = Model.NoiDung,
				TrangThai = Model.TrangThai,
			};
		}
	}
}
