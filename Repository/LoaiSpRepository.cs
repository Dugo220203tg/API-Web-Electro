﻿using API_Web_Shop_Electronic_TD.Data;
using API_Web_Shop_Electronic_TD.DTOs;
using API_Web_Shop_Electronic_TD.Interfaces;
using API_Web_Shop_Electronic_TD.Mappers;
using API_Web_Shop_Electronic_TD.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace API_Web_Shop_Electronic_TD.Repository
{
	public class LoaiSpRepository : ILoaiSp
	{
		private readonly Hshop2023Context db;
		public LoaiSpRepository(Hshop2023Context db)
		{
			this.db = db;
		}

		public async Task<Loai> CreateAsync(LoaiSpMD model)
		{
			if (string.IsNullOrEmpty(model.TenLoai))
			{
				throw new ArgumentException("Chưa nhập đủ thông tin: Tên Loại không được để trống");
			}
			var Loai = model.ToLoaiDTO(); // Sử dụng mapper để chuyển đổi từ KhachHangsMD sang KhachHang

			await db.Loais.AddAsync(Loai);
			await db.SaveChangesAsync();

			return Loai;
		}

		public async Task<Loai?> DeleteAsync(int MaLoai)
		{
			var LoaiModel = await db.Loais.FirstOrDefaultAsync(x => x.MaLoai == MaLoai);
			if (LoaiModel == null)
			{
				throw new KeyNotFoundException($"Không tìm thấy loại sản phẩm với mã {MaLoai}");
			}
			db.Loais.Remove(LoaiModel);
			await db.SaveChangesAsync();
			return LoaiModel;
		}

		public async Task<List<Loai>> GetAllAsync()
		{
			return await db.Loais.ToListAsync();
		}

		public async Task<Loai?> GetByIdAsync(int MaLoai)
		{
			return await db.Loais.FindAsync(MaLoai);
		}
		public async Task<Loai?> GetByNameAsync(string TenLoai)
		{
			var data = await db.Loais.FirstOrDefaultAsync(x => x.TenLoai.Contains(TenLoai));
			if (data == null)
			{
				return null;
			}
			return data;

		}
		public async Task<Loai?> UpdateAsync(int MaLoai, LoaiSpMD model)
		{
			if (string.IsNullOrEmpty(model.TenLoai))
			{
				throw new ArgumentException("Chưa nhập đủ thông tin: Tên loại không được để trống");
			}
			// Lấy đối tượng HangHoa từ cơ sở dữ liệu
			var LoaiModel = await db.Loais.FirstOrDefaultAsync(x => x.MaLoai == MaLoai);

			// Kiểm tra xem HangHoaModel có null không
			if (LoaiModel == null)
			{
				throw new KeyNotFoundException($"Không tìm thấy loại sản phẩm với mã {MaLoai}");
			}

			// Cập nhật thông tin của HangHoaModel từ dữ liệu được gửi từ client
			LoaiModel.TenLoai = model.TenLoai;
			LoaiModel.Hinh = model.Hinh;
			LoaiModel.MoTa = model.Mota;

			// Lưu thay đổi vào cơ sở dữ liệu
			await db.SaveChangesAsync();

			// Trả về HangHoaModel đã được cập nhật
			return LoaiModel;
		}
	}
}
