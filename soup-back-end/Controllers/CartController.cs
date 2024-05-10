using soup_back_end.Data;
using soup_back_end.Models;
using soup_back_end.DTOs.Cart;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Reflection.Metadata.BlobBuilder;
using Microsoft.AspNetCore.Authorization;
using soup_back_end.DTOs.Course;

namespace soup_back_end.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : Controller
    {
        private readonly CartData _cartData;

        public CartController(Data.CartData cartData)
        {
            _cartData = cartData;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            try
            {
                List<Cart> cart = _cartData.GetAll();
                return Ok(cart);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetByUserId")]
        public IActionResult GetByUserId(Guid userid)
        {
            List<Cart> cart = _cartData.GetByUserId(userid);

            if (cart == null)
            {
                return NotFound("Data Not Found");
            }

            return Ok(cart);
        }

        [HttpGet("GetItemAmount")]
        public IActionResult GetItemAmount(Guid userid)
        {
            List<Cart> cart = _cartData.GetByUserId(userid);

            if (cart == null)
            {
                return NotFound("Data Not Found");
            }

            return Ok(cart);
        }

        [HttpGet("GetTotalPrice")]
        public IActionResult GetTotalPrice(Guid userid)
        {
            List<Cart> cart = _cartData.GetByUserId(userid);

            if (cart == null)
            {
                return NotFound("Data Not Found");
            }

            return Ok(cart);
        }

        [HttpPost]
        public IActionResult Post([FromBody] CartDTO cartDto)
        {

            if (cartDto == null)
                return BadRequest("Data Should be Inputed");

            Cart cart = new Cart
            {
                cartId = Guid.NewGuid(),
                courseId = cartDto.courseId,
                categoryId = cartDto.categoryId,
                scheduleId = cartDto.scheduleId,
                userId = cartDto.userId
            };

            bool result = _cartData.Insert(cart);

            if (result)
            {
                return StatusCode(201, cart.cartId);
            }
            else
            {
                return StatusCode(500, "error occur");
            }

        }

        [HttpPut("UpdateIsSelected")]
        public IActionResult Put(Guid userId,bool isSelected, [FromBody] CartDTO cartDto)
        {

            if (cartDto == null)
                return BadRequest("Data Should be Inputed");

            Cart cart = new Cart
            {
                courseId = cartDto.courseId,
                categoryId = cartDto.categoryId,
                scheduleId = cartDto.scheduleId,
                userId = cartDto.userId,
                isSelected = cartDto.isSelected
            };

            bool result = _cartData.UpdateIsSelected(userId, isSelected);

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
            bool result = _cartData.Delete(id);

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
