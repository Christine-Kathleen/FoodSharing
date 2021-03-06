using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FoodSharing.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPI.Authentication;
using static FoodSharing.Constants;

namespace WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class FoodsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FoodsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Foods
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Food>>> GetFoods()
        {
            return await _context.Foods.OrderByDescending(x=>x.TimePosted).
            Join(_context.Users, u => u.UserID, uir => uir.Id,
            (u, uir) => new { u, uir }).Select(m => new Food 
            { User = new ApplicationUser() { 
                UserName=  m.uir.UserName,
                FirstName=m.uir.FirstName,
                LastName=m.uir.LastName,
                Description= m.uir.Description,
                PasswordHash ="",
                ConcurrencyStamp="",
                SecurityStamp="",
                PhoneNumber=m.uir.PhoneNumber,
                Email=m.uir.Email,
                UserLocLatitude=m.uir.UserLocLatitude,
                UserLocLongitude=m.uir.UserLocLongitude,
                Id=m.uir.Id
            },
                FoodType=m.u.FoodType,
                Details=m.u.Details,
                FoodLocationLatitude=m.u.FoodLocationLatitude,
                FoodLocationLongitude=m.u.FoodLocationLongitude,
                ImageSource=m.u.ImageSource,
                ImageUrl=m.u.ImageUrl,
                Name=m.u.Name,
                TimePosted=m.u.TimePosted,
                UserID=m.u.UserID,
                FoodId=m.u.FoodId
            }).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Food>> GetFood(string id)
        {
            var food = await _context.Foods.FindAsync(id);

            if (food == null)
            {
                return NotFound();
            }

            return food;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutFood(int id, Food food)
        {
            if (id != food.FoodId)
            {
                return BadRequest();
            }

            _context.Entry(food).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = Status.Error, Message = APIMessages.ErrorOnUpdate });
            }

            return Ok(new Response { Status = Status.Success, Message = APIMessages.Success });
        }

        [HttpPost]
        public async Task<IActionResult> PostFood(Food food)
        {
            _context.Foods.Add(food);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (FoodExists(food.FoodId))
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = Status.Error, Message = APIMessages.ErrorAlreadyExists });

                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = Status.Error, Message = APIMessages.ErrorOnCreating });

                }
            }
            return Ok(new Response { Status = Status.Success, Message = APIMessages.Success });
        }

        // DELETE: api/Foods/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteFood(int id)
        {
            var food = await _context.Foods.FindAsync(id);
            if (food == null)
            {
                return NotFound();
            }

            _context.Foods.Remove(food);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = Status.Error, Message = APIMessages.ErrorOnDeletion });
            }

            return Ok(new Response { Status = Status.Success, Message = APIMessages.Success });
        }

        private bool FoodExists(int id)
        {
            return _context.Foods.Any(e => e.FoodId == id);
        }
    }
}
