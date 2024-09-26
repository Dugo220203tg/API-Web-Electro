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
	public class DanhGiaSpController : Controller
	{

		private readonly Hshop2023Context db;
		private readonly IDanhGiaSp DanhGiaSpRepository;
		public DanhGiaSpController(Hshop2023Context db, IDanhGiaSp DanhGiaSpRepository)
		{
			this.db = db;
			this.DanhGiaSpRepository = DanhGiaSpRepository;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{

			var danhgias = await DanhGiaSpRepository.GetAllAsync();
			var model = danhgias.Select(s => s.ToDanhGiaDo()).ToList();

			return Ok(model);
		}
		[HttpGet("{MaDg}")]
		public async Task<IActionResult> GetById([FromRoute] int MaDg)
		{
			try
			{
				if (!ModelState.IsValid)
					return BadRequest(ModelState);

				var danhgias = await DanhGiaSpRepository.GetByIdAsync(MaDg);

				if (danhgias == null)
				{
					return NotFound(new ErrorResponse
					{
						Message = "Không tìm thấy dữ liệu",
						Errors = new List<string> { "Không tìm thấy thông tin với mã đã cho" }
					});
				}

				return Ok(danhgias.ToDanhGiaDo());

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
		public async Task<IActionResult> Post(DanhGiaSpMD model)
		{
			try
			{
				if (!ModelState.IsValid)
					return BadRequest(ModelState);

				var createdModel = await DanhGiaSpRepository.CreateAsync(model);
				return Ok(createdModel);
			}
			catch (Exception ex)
			{
				// Xử lý và thông báo lỗi tại đây
				return BadRequest("Đã xảy ra lỗi: " + ex.ToString());
			}
		}
		[HttpPut]
		[Route("{MaDg}")]
		public async Task<IActionResult> Update([FromRoute] int MaDg, [FromBody] DanhGiaSpMD model)
		{
			try
			{
				if (!ModelState.IsValid)
					return BadRequest(ModelState);

				var Model = await DanhGiaSpRepository.UpdateAsync(MaDg, model);
				if (Model == null)
				{
					return NotFound();
				}
				return Ok(Model.ToDanhGiaDo());
			}
			catch (Exception ex)
			{
				// Xử lý và thông báo lỗi tại đây
				return BadRequest("Đã xảy ra lỗi: " + ex.ToString());
			}
		}
		[HttpDelete]
		[Route("{MaDg:int}")]
		public async Task<IActionResult> Delete([FromRoute] int MaDg)
		{
			// Kiểm tra tính hợp lệ của ModelState
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			// Xóa bản ghi từ bảng "HangHoas"
			var Model = await DanhGiaSpRepository.DeleteAsync(MaDg);

			// Nếu không tìm thấy bản ghi để xóa, trả về NotFound
			if (Model == null)
				return NotFound();

			// Trả về phản hồi NoContent nếu xóa thành công
			return NoContent();
		}
	}
}
