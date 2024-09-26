using API_Web_Shop_Electronic_TD.Data;
using API_Web_Shop_Electronic_TD.Interfaces;
using API_Web_Shop_Electronic_TD.Mappers;
using API_Web_Shop_Electronic_TD.Models;
using Microsoft.EntityFrameworkCore;

namespace API_Web_Shop_Electronic_TD.Repository
{
	public class ChiTietHoaDonRepository : ICtHoaDon
	{
		private readonly Hshop2023Context db;
		public ChiTietHoaDonRepository(Hshop2023Context db)
		{
			this.db = db;
		}
		async Task<ChiTietHd> ICtHoaDon.CreateAsync(ChiTietHoaDonMD model)
		{
			if (model.MaHH == 0)
			{
				throw new ArgumentException("Mã hàng hóa không hợp lệ hoặc chưa được nhập");
			}

			var chitiethoadon = model.ToCtHoaDonDTO();
			await db.ChiTietHds.AddAsync(chitiethoadon);
			await db.SaveChangesAsync();

			return chitiethoadon;
		}

		async Task<ChiTietHd?> ICtHoaDon.DeleteAsync(int MaCt)
		{
			var Model = await db.ChiTietHds.FirstOrDefaultAsync(x => x.MaCt == MaCt);
			if (Model == null)
			{
				throw new KeyNotFoundException($"Không tìm thấy loại sản phẩm với mã {MaCt}");
			}
			db.ChiTietHds.Remove(Model);
			await db.SaveChangesAsync();
			return Model;
		}

		async Task<List<ChiTietHd>> ICtHoaDon.GetAllAsync()
		{
			return await db.ChiTietHds
				.Include(ct => ct.MaHdNavigation)
					.ThenInclude(hd => hd.MaTrangThaiNavigation)
				.Include(ct => ct.MaHhNavigation)
				.ToListAsync();
		}

		async Task<ChiTietHd?> ICtHoaDon.GetByIdAsync(int MaCt)
		{
			return await db.ChiTietHds.FindAsync(MaCt);
		}

		async Task<ChiTietHd?> ICtHoaDon.UpdateAsync(int MaCt, ChiTietHoaDonMD model)
		{

			// Lấy đối tượng HangHoa từ cơ sở dữ liệu
			var Model = await db.ChiTietHds.FirstOrDefaultAsync(x => x.MaCt == MaCt);

			// Kiểm tra xem HangHoaModel có null không
			if (Model == null)
			{
				throw new KeyNotFoundException($"Không tìm thấy loại sản phẩm với mã {MaCt}");
			}

			// Cập nhật thông tin của HangHoaModel từ dữ liệu được gửi từ client
			Model.MaHd = model.MaHD;
			Model.MaHh = model.MaHH;
			Model.DonGia = model.DonGia;
			Model.SoLuong = model.SoLuong;
			Model.MaGiamGia = model.MaGiamGia;

			// Lưu thay đổi vào cơ sở dữ liệu
			await db.SaveChangesAsync();

			// Trả về HangHoaModel đã được cập nhật
			return Model;
		}
	}
}
