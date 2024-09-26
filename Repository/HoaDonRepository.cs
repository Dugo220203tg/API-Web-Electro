using API_Web_Shop_Electronic_TD.Data;
using API_Web_Shop_Electronic_TD.Interfaces;
using API_Web_Shop_Electronic_TD.Mappers;
using API_Web_Shop_Electronic_TD.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace API_Web_Shop_Electronic_TD.Repository
{
	public class HoaDonRepository : IHoaDon
	{
		private readonly Hshop2023Context db;
		public HoaDonRepository(Hshop2023Context db)
		{
			this.db = db;
		}
		public async Task<HoaDon?> DeleteAsync(int MaHd)
		{
			var Model = await db.HoaDons
				.Include(h => h.ChiTietHds) // Load ChiTietHoaDon records
				.FirstOrDefaultAsync(x => x.MaHd == MaHd);

			if (Model == null)
			{
				throw new KeyNotFoundException($"Không tìm thấy Hóa đơn với mã {MaHd}");
			}

			db.HoaDons.Remove(Model);
			await db.SaveChangesAsync(); // Cascade delete will handle ChiTietHoaDon records

			return Model;
		}


		public async Task<List<HoaDon>> GetAllAsync()
		{
			return await db.HoaDons
				.Include(h => h.MaTrangThaiNavigation)
				.Include(h => h.ChiTietHds)
				.ThenInclude(ct => ct.MaHhNavigation) // Include navigation property
				.ToListAsync();
		}

		public async Task<HoaDon?> GetByIdAsync(int MaHd)
		{
			return await db.HoaDons
				.Include(h => h.MaTrangThaiNavigation)
				.Include(h => h.ChiTietHds)
				//.Include(h => h.MaKhNavigation)
				.ThenInclude(ct => ct.MaHhNavigation)
				.FirstOrDefaultAsync(h => h.MaHd == MaHd);
		}

		public async Task<HoaDon?> UpdateAsync(int MaHd, HoaDonMD model)
		{
			// Lấy đối tượng HoaDon từ cơ sở dữ liệu
			var hoaDon = await db.HoaDons
				.Include(h => h.ChiTietHds) // Load related ChiTietHoaDons
				.FirstOrDefaultAsync(x => x.MaHd == MaHd);

			// Kiểm tra xem HoaDon có tồn tại hay không
			if (hoaDon == null)
			{
				throw new KeyNotFoundException($"Không tìm thấy Hóa đơn với mã {MaHd}");
			}

			// Cập nhật thông tin của HoaDon
			hoaDon.MaKh = model.MaKH;
			hoaDon.NgayDat = model.NgayDat;
			hoaDon.HoTen = model.HoTen;
			hoaDon.DiaChi = model.DiaChi;
			hoaDon.CachVanChuyen = model.CachVanChuyen;
			hoaDon.PhiVanChuyen = (double)(double?)model.PhiVanChuyen;
			hoaDon.MaTrangThai = model.MaTrangThai;
			hoaDon.DienThoai = model.DienThoai;

			// Cập nhật thông tin trong bảng ChiTietHoaDon
			// Xóa những chi tiết không còn trong model
			var chiTietToRemove = hoaDon.ChiTietHds
				.Where(ct => !model.ChiTietHds.Any(m => m.MaCT == ct.MaCt))
				.ToList();
			db.ChiTietHds.RemoveRange(chiTietToRemove);

			// Cập nhật hoặc thêm chi tiết hóa đơn
			foreach (var chiTietModel in model.ChiTietHds)
			{
				var chiTiet = hoaDon.ChiTietHds.FirstOrDefault(ct => ct.MaCt == chiTietModel.MaCT);

				if (chiTiet != null) // Nếu chi tiết đã tồn tại, cập nhật
				{
					chiTiet.MaHh = chiTietModel.MaHH;
					chiTiet.SoLuong = chiTietModel.SoLuong;
					chiTiet.DonGia = chiTietModel.DonGia;
					chiTiet.MaGiamGia = chiTietModel.MaGiamGia;
				}
				else // Nếu chi tiết chưa tồn tại, thêm mới
				{
					hoaDon.ChiTietHds.Add(new ChiTietHd
					{
						MaHd = hoaDon.MaHd,
						MaHh = chiTietModel.MaHH,
						SoLuong = chiTietModel.SoLuong,
						DonGia = chiTietModel.DonGia,
						MaGiamGia = chiTietModel.MaGiamGia,
					});
				}
			}

			// Lưu các thay đổi vào cơ sở dữ liệu
			await db.SaveChangesAsync();

			// Trả về hóa đơn đã được cập nhật
			return hoaDon;
		}


		public async Task<HoaDon> CreateAsync(HoaDonMD model)
		{
			if (string.IsNullOrEmpty(model.MaKH))
			{
				throw new ArgumentException("Mã khách hàng không hợp lệ hoặc chưa được nhập");
			}

			if (string.IsNullOrEmpty(model.HoTen))
			{
				throw new ArgumentException("Tên khách hàng chưa được nhập");
			}
			var hoadon = model.ToHoaDonDTO();
			await db.HoaDons.AddAsync(hoadon);
			await db.SaveChangesAsync();

			return hoadon;
		}
	}
}
