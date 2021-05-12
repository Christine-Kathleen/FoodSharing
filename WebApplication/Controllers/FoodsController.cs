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
    //[Authorize]

    public class FoodsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public FoodsController(ApplicationDbContext context)
        {
            _context = context;
            //AddTestData();
        }

        public async void AddTestData()
        {
            var food = new Food();
            food.Name = "Cake";
            food.Details = "Vanilla";
            food.FoodType = TypeOfFood.FromStore;
            food.AnnouncementAvailability = Availability.Available;
            food.User = _context.Users.First();
            food.UserID = food.User.Id;
            _context.Foods.Add(food);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                //if (!FoodExists(id))
                //{
                //    return NotFound();
                //}
                //else
                //{
                //    throw;
                //}
            }
        }


        // GET: api/Foods
        [HttpGet]
        //[Authorize]
        public async Task<ActionResult<IEnumerable<Food>>> GetFoods()
        {
            return  _context.Foods.OrderByDescending(x=>x.TimePosted).
         Join(_context.Users, u => u.UserID, uir => uir.Id,
         (u, uir) => new { u, uir }).Select(m => new Food { User = new ApplicationUser() { UserName=  m.uir.UserName,PasswordHash="",ConcurrencyStamp="",SecurityStamp="", PhoneNumber=m.uir.PhoneNumber,Email=m.uir.Email, UserLocLatitude=m.uir.UserLocLatitude, UserLocLongitude=m.uir.UserLocLongitude },FoodType=m.u.FoodType,AnnouncementAvailability=m.u.AnnouncementAvailability, Details=m.u.Details, FoodLocationLatitude=m.u.FoodLocationLatitude, FoodLocationLongitude=m.u.FoodLocationLongitude, ImageSource=m.u.ImageSource, ImageUrl=m.u.ImageUrl, Name=m.u.Name, TimePosted=m.u.TimePosted, UserID=m.u.UserID, FoodId=m.u.FoodId }).ToList();
           // return await _context.Foods.Include(x => x.User).ToListAsync();
            //var dbdatastored= from entry in Food join ApplicationUser in this.Use
            //_context.Foods.Join()
            //return await (from Food in _context.Foods join ApplicationUser in _context.Users on Food.UserID equals ApplicationUser.Id into tmp from m in tmp.DefaultIfEmpty()
            //              select new Food
            //              {
            //                  AnnouncementAvailability=tmp.AnnouncementAvailability,

            //              }
            //              )
           //return await _context.Foods.Include("Food.User").ToListAsync();
            //using (var context = new ApplicationDbContext())
            //{
            //    var studentName = ctx.Students.SqlQuery("Select studentid, studentname, standardId from Student where studentname='Bill'")
            //    var foods = context.Foods.att
            //                        .Where(s => s.FirstName == "Bill")
            //                        .FirstOrDefault<Student>();

            //    context.Entry(ApplicationUser).Reference(s => s.).Load(); // loads StudentAddress
            //    context.Entry(student).Collection(s => s.StudentCourses).Load(); // loads Courses collection 
            //}

            //await _context.Users.ToListAsync();
            //_context.Foods.
            //List<Food> foodlist=
           // return await _context.Foods.OrderByDescending(x=>x.TimePosted).ToListAsync();
            //foreach (var item in foodlist)
            //{
            //    item.User=_context.Users.find
            //}
        }

        // GET: api/Foods/5
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

        // PUT: api/Foods/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize]
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

        // POST: api/Foods
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize]
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

        //[HttpPatch]
        //[Route("UpdateFood")]
        //[Authorize]
        //public async Task<ActionResult<Food>> UpdateFood(Food food)
        //{
        //    _context.Foods.Add(food);
        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = Status.Error, Message = APIMessages.ErrorOnDeletion });
        //    }

        //    return Ok(new Response { Status = Status.Success, Message = APIMessages.Success });

        //    return CreatedAtAction("GetFood", new { id = food.FoodId }, food);
        //}

        // DELETE: api/Foods/5
        [HttpPost]
        [Route("DeleteFood")]
        [Authorize]
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
