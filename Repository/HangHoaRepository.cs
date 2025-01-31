﻿using API_Web_Shop_Electronic_TD.Data;
using API_Web_Shop_Electronic_TD.Interfaces;
using API_Web_Shop_Electronic_TD.Models;
using Microsoft.EntityFrameworkCore;
using API_Web_Shop_Electronic_TD.Mappers;
using System;


namespace API_Web_Shop_Electronic_TD.Repository
{
	public class HangHoaRepository : IHangHoaRepository
	{
		private readonly Hshop2023Context db;
		public HangHoaRepository(Hshop2023Context db)
		{
			this.db = db;
		}
		public async Task<List<HangHoa>> GetAllAsync()
		{
			return await db.HangHoas
				.Include(hh => hh.MaNccNavigation)  // Bao gồm nhà cung cấp (NhaCungCap)
				.Include(hh => hh.MaLoaiNavigation) // Bao gồm loại sản phẩm (LoaiSanPham)
				.ToListAsync();
		}

		public async Task<HangHoa> CreateAsync(CreateHangHoaMD model)
		{
			if (string.IsNullOrEmpty(model.TenHH))
			{
				throw new ArgumentException("Chưa nhập đủ thông tin: Tên hàng hóa không được để trống");
			}
			if (!model.MaLoai.HasValue || model.MaLoai <= 0)
			{
				throw new ArgumentException("Chưa nhập đủ thông tin: Mã loại không hợp lệ hoặc chưa được nhập");
			}
			if (string.IsNullOrEmpty(model.MaNCC))
			{
				throw new ArgumentException("Chưa nhập đủ thông tin: Mã nhà cung cấp không được để trống");
			}
			var existingLoai = await db.Loais.FirstOrDefaultAsync(l => l.MaLoai == model.MaLoai);
			if (existingLoai == null)
			{
				throw new ArgumentException($"Mã loại {model.MaLoai} không tồn tại trong hệ thống");
			}
			//var existingNCC = await db.NhaCungCaps.FirstOrDefaultAsync(l => l.MaNcc == model.MaNCC);
			//if (existingNCC == null)
			//{
			//	throw new ArgumentException($"Mã Nhà cung cấp {model.MaNCC} không tồn tại trong hệ thống");
			//}
			var Hanghoa = model.ToHangHoaCreateDTO(); // Sử dụng mapper để chuyển đổi từ KhachHangsMD sang KhachHang

			await db.HangHoas.AddAsync(Hanghoa);
			await db.SaveChangesAsync();

			return Hanghoa; // Trả về đối tượng KhachHang sau khi thêm vào cơ sở dữ liệu
		}

		public async Task<HangHoa?> DeleteAsync(int MaHh)
		{
			var HangHoaModel = await db.HangHoas.FirstOrDefaultAsync(x => x.MaHh == MaHh);
			if (HangHoaModel == null)
			{
				throw new KeyNotFoundException($"Không tìm thấy sản phẩm với mã {MaHh}");
			}
			db.HangHoas.Remove(HangHoaModel);
			await db.SaveChangesAsync();
			return HangHoaModel;
		}


		public async Task<HangHoa?> GetByIdAsync(int Mahh)
		{
			return await db.HangHoas.FindAsync(Mahh);
		}
		public async Task<HangHoa?> UpdateAsync(int Mahh, UpdateHangHoaMD Model)
		{
			if (string.IsNullOrEmpty(Model.TenHH))
			{
				throw new ArgumentException("Chưa nhập đủ thông tin: Tên hàng hóa không được để trống");
			}
			if (!Model.MaLoai.HasValue || Model.MaLoai <= 0)
			{
				throw new ArgumentException("Chưa nhập đủ thông tin: Mã loại không hợp lệ hoặc chưa được nhập");
			}
			if (string.IsNullOrEmpty(Model.MaNCC))
			{
				throw new ArgumentException("Chưa nhập đủ thông tin: Mã nhà cung cấp không được để trống");
			}
			var existingLoai = await db.Loais.FirstOrDefaultAsync(l => l.MaLoai == Model.MaLoai);
			if (existingLoai == null)
			{
				throw new ArgumentException($"Mã loại {Model.MaLoai} không tồn tại trong hệ thống");
			}
			var existingNCC = await db.NhaCungCaps.FirstOrDefaultAsync(l => l.MaNcc == Model.MaNCC);
			if (existingNCC == null)
			{
				throw new ArgumentException($"Mã Nhà cung cấp {Model.MaNCC} không tồn tại trong hệ thống");
			}
			// Lấy đối tượng HangHoa từ cơ sở dữ liệu
			var HangHoaModel = await db.HangHoas.FirstOrDefaultAsync(x => x.MaHh == Mahh);

			// Kiểm tra xem HangHoaModel có null không
			if (HangHoaModel == null)
			{
				throw new KeyNotFoundException($"Không tìm thấy sản phẩm với mã {Mahh}");
			}

			// Cập nhật thông tin của HangHoaModel từ dữ liệu được gửi từ client
			HangHoaModel.TenHh = Model.TenHH;
			HangHoaModel.Hinh = Model.Hinh;
			HangHoaModel.MoTa = Model.MoTa;
			HangHoaModel.MoTaDonVi = Model.MoTaDonVi;
			HangHoaModel.MaLoai = (int)Model.MaLoai;
			HangHoaModel.NgaySx = (DateOnly)Model.NgaySX;
			HangHoaModel.GiamGia = (double)Model.GiamGia;
			HangHoaModel.MaNcc = Model.MaNCC;
			HangHoaModel.DonGia = Model.DonGia;
			HangHoaModel.SoLuong = Model.SoLuong;

			// Lưu thay đổi vào cơ sở dữ liệu
			await db.SaveChangesAsync();

			// Trả về HangHoaModel đã được cập nhật
			return HangHoaModel;
		}

	}
}
