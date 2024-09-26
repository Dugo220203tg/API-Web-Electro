using API_Web_Shop_Electronic_TD.Data;
using API_Web_Shop_Electronic_TD.Interfaces;
using API_Web_Shop_Electronic_TD.Mappers;
using API_Web_Shop_Electronic_TD.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace API_Web_Shop_Electronic_TD.Repository
{
	public class DanhGiaSpRepository : IDanhGiaSp
	{
		private readonly Hshop2023Context db;
		public DanhGiaSpRepository(Hshop2023Context db)
		{
			this.db = db;
		}
		public async Task<DanhGiaSp> CreateAsync(DanhGiaSpMD model)
		{
			var danhgia = model.ToDanhGiaDTO(); // Sử dụng mapper để chuyển đổi từ KhachHangsMD sang KhachHang

			await db.DanhGiaSps.AddAsync(danhgia);
			await db.SaveChangesAsync();

			return danhgia;
		}

		public async Task<DanhGiaSp?> DeleteAsync(int MaDg)
		{
			var Model = await db.DanhGiaSps.FirstOrDefaultAsync(x => x.MaDg == MaDg);
			if (Model == null)
			{
				throw new KeyNotFoundException($"Không tìm thấy loại sản phẩm với mã {MaDg}");
			}
			db.DanhGiaSps.Remove(Model);
			await db.SaveChangesAsync();
			return Model;
		}

		public async Task<List<DanhGiaSp>> GetAllAsync()
		{
			return await db.DanhGiaSps.ToListAsync();
		}

		public async Task<DanhGiaSp?> GetByIdAsync(int MaDg)
		{
			return await db.DanhGiaSps.FindAsync(MaDg);
		}

		public async Task<DanhGiaSp?> UpdateAsync(int MaDg, DanhGiaSpMD model)
		{
			// Lấy đối tượng HangHoa từ cơ sở dữ liệu
			var DanhGiaModel = await db.DanhGiaSps.FirstOrDefaultAsync(x => x.MaDg == MaDg);

			// Kiểm tra xem HangHoaModel có null không
			if (DanhGiaModel == null)
			{
				throw new KeyNotFoundException($"Không tìm thấy loại sản phẩm với mã {MaDg}");
			}

			// Cập nhật thông tin của HangHoaModel từ dữ liệu được gửi từ client
			DanhGiaModel.Sao = model.Sao;
			DanhGiaModel.NoiDung = model.NoiDung;
			DanhGiaModel.TrangThai = model.TrangThai;

			// Lưu thay đổi vào cơ sở dữ liệu
			await db.SaveChangesAsync();

			// Trả về HangHoaModel đã được cập nhật
			return DanhGiaModel;
		}
	}
}
