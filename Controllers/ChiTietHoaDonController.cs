using API_Web_Shop_Electronic_TD.Data;
using API_Web_Shop_Electronic_TD.DTOs;
using API_Web_Shop_Electronic_TD.Interfaces;
using API_Web_Shop_Electronic_TD.Mappers;
using API_Web_Shop_Electronic_TD.Models;
using API_Web_Shop_Electronic_TD.Repository;
using Microsoft.AspNetCore.Mvc;

namespace API_Web_Shop_Electronic_TD.Controllers
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	public class ChiTietHoaDonController : Controller
	{
		private readonly Hshop2023Context db;
		private readonly ICtHoaDon ChiTietHoaDonRepository;
		public ChiTietHoaDonController(Hshop2023Context db, ICtHoaDon ChiTietHoaDonRepository)
		{
			this.db = db;
			this.ChiTietHoaDonRepository = (ICtHoaDon?)ChiTietHoaDonRepository;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{

			var ctHoaDon = await ChiTietHoaDonRepository.GetAllAsync();
			var model = ctHoaDon.Select(s => s.ToCtHoaDonDo()).ToList();

			return Ok(model);
		}
		[HttpGet("{MaCt}")]
		public async Task<IActionResult> GetById([FromRoute] int MaCt)
		{
			try
			{
				if (!ModelState.IsValid)
					return BadRequest(ModelState);

				var Model = await ChiTietHoaDonRepository.GetByIdAsync(MaCt);

				if (Model == null)
				{
					return NotFound(new ErrorResponse
					{
						Message = "Không tìm thấy dữ liệu",
						Errors = new List<string> { "Không tìm thấy thông tin với mã đã cho" }
					});
				}

				return Ok(Model.ToCtHoaDonDo());

			}

			catch (Exception ex)
			{
				return BadRequest(new ErrorResponse
				{
					Message = "Đã xảy ra lỗi",
					Errors = new List<string> { "Lỗi không xác định: " + ex.Message }
				});
			}
		}

		[HttpPost]
		public async Task<IActionResult> Post(ChiTietHoaDonMD model)
		{
			try
			{
				if (!ModelState.IsValid)
					return BadRequest(ModelState);

				var createdModel = await ChiTietHoaDonRepository.CreateAsync(model);
				return Ok(createdModel);
			}
			catch (Exception ex)
			{
				// Xử lý và thông báo lỗi tại đây
				return BadRequest("Đã xảy ra lỗi: " + ex.ToString());
			}
		}
		[HttpPut]
		[Route("{MaLoai}")]
		public async Task<IActionResult> Update([FromRoute] int MaCt, [FromBody] ChiTietHoaDonMD model)
		{
			try
			{
				if (!ModelState.IsValid)
					return BadRequest(ModelState);

				var Model = await ChiTietHoaDonRepository.UpdateAsync(MaCt, model);
				if (Model == null)
				{
					return NotFound();
				}
				return Ok(Model.ToCtHoaDonDo());
			}
			catch (Exception ex)
			{
				// Xử lý và thông báo lỗi tại đây
				return BadRequest("Đã xảy ra lỗi: " + ex.ToString());
			}
		}
		[HttpDelete]
		[Route("{MaLoai:int}")]
		public async Task<IActionResult> Delete([FromRoute] int MaCt)
		{
			// Kiểm tra tính hợp lệ của ModelState
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			// Xóa bản ghi từ bảng "HangHoas"
			var Model = await ChiTietHoaDonRepository.DeleteAsync(MaCt);

			// Nếu không tìm thấy bản ghi để xóa, trả về NotFound
			if (Model == null)
				return NotFound();

			// Trả về phản hồi NoContent nếu xóa thành công
			return NoContent();
		}
	}
}
