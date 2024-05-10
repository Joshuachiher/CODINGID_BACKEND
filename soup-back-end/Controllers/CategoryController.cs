using soup_back_end.Data;
using soup_back_end.Models;
using soup_back_end.DTOs.Category;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Reflection.Metadata.BlobBuilder;
using Microsoft.AspNetCore.Authorization;

namespace soup_back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryData _categoryData;

        public CategoryController(Data.CategoryData categoryData)
        {
            _categoryData = categoryData;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                List<Category> categories = _categoryData.GetAll();
                return Ok(categories);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetById")]
        [Authorize]
        public IActionResult GetById(string category_id)
        {
            Category? category = _categoryData.GetById(category_id);

            if (category == null)
            {
                return NotFound("Data Not Found");
            }

            return Ok(category);
        }

        [HttpGet("GetByName")]
        [Authorize]
        public IActionResult GetByName(string category_Name)
        {
            Category? category = _categoryData.GetByName(category_Name);

            if (category == null)
            {
                return NotFound("Data Not Found");
            }

            return Ok(category);
        }

        [HttpGet("GetCourseByCategoryId")]
        [Authorize]
        public IActionResult GetCourseByCategoryId(string categoryId)
        {
            Course? course = _categoryData.GetCourseByCategoryId(categoryId);

            if (course == null)
            {
                return NotFound("Data Not Found");
            }

            return Ok(course);
        }

        [HttpPost]
        public IActionResult Post([FromBody] CategoryDTO categoryDto)
        {

            if (categoryDto == null)
                return BadRequest("Data Should be Inputed");

            Category category = new Category
            {
                category_id = categoryDto.category_id,
                category_name = categoryDto.category_name,
                Description = categoryDto.Description,
                img = categoryDto.img,
                Created = DateTime.Now,
                Updated = DateTime.Now,
            };

            bool result = _categoryData.Insert(category);

            if (result)
            {
                return StatusCode(201, category.category_id);
            }
            else
            {
                return StatusCode(500, "error occur");
            }

        }

        [HttpPut]
        public IActionResult Put(string id, [FromBody] CategoryDTO categoryDto)
        {

            if (categoryDto == null)
                return BadRequest("Data Should be Inputed");

            Category category = new Category
            {
                category_id = categoryDto.category_id,
                category_name = categoryDto.category_name,
                Description = categoryDto.Description,
                img = categoryDto.img,
                Updated = DateTime.Now,
            };

            bool result = _categoryData.Update(id, category);

            if (result)
            {
                return NoContent();
            }
            else
            {
                return StatusCode(500, "error occur");
            }

        }

        [HttpDelete]
        public IActionResult Delete(string id)
        {
            bool result = _categoryData.Delete(id);

            if (result)
            {
                return NoContent();
            }
            else
            {
                return StatusCode(500, "error occur");
            }
        }
    }
}
