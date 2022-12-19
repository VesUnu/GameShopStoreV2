﻿using GameShopStoreV2.Application.ItemServices.Categories;
using GameShopStoreV2.Core.Items.Categories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace GameShopStoreV2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(
            ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _categoryService.GetAll();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var category = await _categoryService.GetById(id);
            return Ok(category);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> CreateGenre(CategoryCreate request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _categoryService.CreateCategory(request);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }

        [HttpPut]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> EditGenre(CategoryEdit request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var result = await _categoryService.EditCategory(request);
            if (!result.IsSuccess)
            {
                return BadRequest(result);
            }
            return Ok(result);
        }
    }
}

