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
    public class ReviewsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ReviewsController(ApplicationDbContext context)
        {
            _context = context;
        }

        //// GET: api/Reviews
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Review>>> GetReviews()
        //{
        //    return await _context.Reviews.OrderBy(x => x.SendTime).
        //   Join(_context.Users, u => u.ReviewerUserId, uir => uir.Id,
        //   (u, uir) => new { u, uir }).Select(m => new Review
        //   {
        //       ReviewerId = new ApplicationUser()
        //       {
        //           UserName = m.uir.UserName,
        //           PasswordHash = "",
        //           ConcurrencyStamp = "",
        //           SecurityStamp = "",
        //           PhoneNumber = m.uir.PhoneNumber,
        //           Email = m.uir.Email,
        //           UserLocLatitude = m.uir.UserLocLatitude,
        //           UserLocLongitude = m.uir.UserLocLongitude
        //       },
        //       ReviewContent = m.u.ReviewContent,
        //       SendTime = m.u.SendTime,
        //       ReviewId = m.u.ReviewId,
        //       ReviewerUserId = m.u.ReviewerUserId,
        //       ReviewedUserId=m.u.ReviewedUserId
        //   }
        //   )
        //   .ToListAsync();
        //}

        // GET: api/Reviews/5
        [HttpGet("{id}")]
        public async Task<ActionResult<IEnumerable<Review>>> GetReview(string id)
        {
            var review =  await _context.Reviews.Where(x => x.ReviewedUserId == id).OrderBy(x => x.SendTime).
           Join(_context.Users, u => u.ReviewerUserId, uir => uir.Id,
           (u, uir) => new { u, uir }).Select(m => new Review
           {
               ReviewerId = new ApplicationUser()
               {
                   UserName = m.uir.UserName,
                   PasswordHash = "",
                   ConcurrencyStamp = "",
                   SecurityStamp = "",
                   PhoneNumber = m.uir.PhoneNumber,
                   Email = m.uir.Email,
                   UserLocLatitude = m.uir.UserLocLatitude,
                   UserLocLongitude = m.uir.UserLocLongitude,
                   FirstName=m.uir.FirstName,
                   LastName=m.uir.LastName,
                   Id=m.uir.Id
               },
               ReviewContent = m.u.ReviewContent,
               SendTime = m.u.SendTime,
               ReviewId = m.u.ReviewId,
               ReviewerUserId = m.u.ReviewerUserId,
               ReviewedUserId=m.u.ReviewedUserId
           }
           )
           .ToListAsync();
            return review;
        }

        // PUT: api/Reviews/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutReview(int id, Review review)
        {
            if (id != review.ReviewId)
            {
                return BadRequest();
            }

            _context.Entry(review).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ReviewExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Reviews
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Review>> PostReview(Review review)
        {
            _context.Reviews.Add(review);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                if (ReviewExists(review.ReviewId))
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

        // DELETE: api/Reviews/5
        [HttpPost]
        [Route("DeleteReview")]
        public async Task<IActionResult> DeleteReview(int id)
        {
            var review = await _context.Reviews.FindAsync(id);
            if (review == null)
            {
                return NotFound();
            }

            _context.Reviews.Remove(review);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ReviewExists(int id)
        {
            return _context.Reviews.Any(e => e.ReviewId == id);
        }
    }
}
