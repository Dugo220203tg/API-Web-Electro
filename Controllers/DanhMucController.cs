﻿using API_Web_Shop_Electronic_TD.Data;
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
	public class DanhMucController : Controller
	{
		private readonly Hshop2023Context db;
		private readonly IDanhMuc DanhMucRepository;
		public DanhMucController(Hshop2023Context db, IDanhMuc DanhMucRepository)
		{
			this.db = db;
			this.DanhMucRepository = DanhMucRepository;
		}
		[HttpGet]
		public async Task<IActionResult> GetAll()
		{

			var danhmucs = await DanhMucRepository.GetAllAsync();
			var model = danhmucs.Select(s => s.ToDanhMucDo()).ToList();

			return Ok(model);
		}
		[HttpGet("{MaDanhMuc}")]
		public async Task<IActionResult> GetById([FromRoute] int MaDanhMuc)
		{
			try
			{
				if (!ModelState.IsValid)
					return BadRequest(ModelState);

				var danhmucs = await DanhMucRepository.GetByIdAsync(MaDanhMuc);

				if (danhmucs == null)
				{
					return NotFound(new ErrorResponse
					{
						Message = "Không tìm thấy dữ liệu",
						Errors = new List<string> { "Không tìm thấy thông tin với mã đã cho" }
					});
				}

				return Ok(danhmucs.ToDanhMucDo());

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
		public async Task<IActionResult> Post(DanhMucMD model)
		{
			try
			{
				if (!ModelState.IsValid)
					return BadRequest(ModelState);

				var createdModel = await DanhMucRepository.CreateAsync(model);
				return Ok(createdModel);
			}
			catch (Exception ex)
			{
				// Xử lý và thông báo lỗi tại đây
				return BadRequest("Đã xảy ra lỗi: " + ex.ToString());
			}
		}
		[HttpPut]
		[Route("{MaDanhMuc}")]
		public async Task<IActionResult> Update([FromRoute] int MaDanhMuc, [FromBody] DanhMucMD model)
		{
			try
			{
				if (!ModelState.IsValid)
					return BadRequest(ModelState);

				var Model = await DanhMucRepository.UpdateAsync(MaDanhMuc, model);
				if (Model == null)
				{
					return NotFound();
				}
				return Ok(Model.ToDanhMucDo());
			}
			catch (Exception ex)
			{
				// Xử lý và thông báo lỗi tại đây
				return BadRequest("Đã xảy ra lỗi: " + ex.ToString());
			}
		}
		[HttpDelete]
		[Route("{MaDanhMuc:int}")]
		public async Task<IActionResult> Delete([FromRoute] int MaDanhMuc)
		{
			// Kiểm tra tính hợp lệ của ModelState
			if (!ModelState.IsValid)
				return BadRequest(ModelState);

			// Xóa bản ghi từ bảng "HangHoas"
			var model = await DanhMucRepository.DeleteAsync(MaDanhMuc);

			// Nếu không tìm thấy bản ghi để xóa, trả về NotFound
			if (model == null)
				return NotFound();

			// Trả về phản hồi NoContent nếu xóa thành công
			return NoContent();
		}
	}
}