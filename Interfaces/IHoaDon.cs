﻿using API_Web_Shop_Electronic_TD.Data;
using API_Web_Shop_Electronic_TD.Models;

namespace API_Web_Shop_Electronic_TD.Interfaces
{
	public interface IHoaDon
	{
		Task<List<HoaDon>> GetAllAsync();
		Task<HoaDon?> GetByIdAsync(int MaHd);
		Task<HoaDon> CreateAsync(HoaDonMD model);
		Task<HoaDon?> UpdateAsync(int MaHd, HoaDonMD model);
		Task<HoaDon?> DeleteAsync(int MaHd);
	}
	public interface ICtHoaDon
	{
		Task<List<ChiTietHd>> GetAllAsync();
		Task<ChiTietHd?> GetByIdAsync(int MaCt);
		Task<ChiTietHd> CreateAsync(ChiTietHoaDonMD model);
		Task<ChiTietHd?> UpdateAsync(int MaCt, ChiTietHoaDonMD model);
		Task<ChiTietHd?> DeleteAsync(int MaCt);
	}
}
