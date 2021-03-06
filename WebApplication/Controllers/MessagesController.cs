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
    public class MessagesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public MessagesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult<IEnumerable<Message>>> GetMessages(string userId)
        {
            return await _context.Messages.OrderBy(x => x.SendTime).Where(x => x.ReceiverUserId == userId || x.SenderUserId == userId).
          Join(_context.Users,
                msg => msg.ReceiverUserId,
                user => user.Id,
                (msg, user) => new
                {
                    ReceiverId = new ApplicationUser()
                    {
                        UserName = user.UserName,
                        PasswordHash = "",
                        ConcurrencyStamp = "",
                        SecurityStamp = "",
                        PhoneNumber = user.PhoneNumber,
                        Email = user.Email,
                        UserLocLatitude = user.UserLocLatitude,
                        UserLocLongitude = user.UserLocLongitude,
                        Id = user.Id
                    },
                    msg.Content, msg.MessageId, msg.SendTime, msg.State, msg.SenderUserId, msg.ReceiverUserId
                }).Join(_context.Users, msg => msg.SenderUserId, uir2 => uir2.Id,
          (u, uir2) => new { u, uir2 }).Select(m => new Message
          {
              SenderId = new ApplicationUser()
              {
                  UserName = m.uir2.UserName,
                  PasswordHash = "",
                  ConcurrencyStamp = "",
                  SecurityStamp = "",
                  PhoneNumber = m.uir2.PhoneNumber,
                  Email = m.uir2.Email,
                  UserLocLatitude = m.uir2.UserLocLatitude,
                  UserLocLongitude = m.uir2.UserLocLongitude,
                  Id = m.uir2.Id
              },
              ReceiverId = m.u.ReceiverId,
              Content = m.u.Content,
              MessageId = m.u.MessageId,
              SendTime = m.u.SendTime,
              State = m.u.State,
              SenderUserId = m.u.SenderUserId,
              ReceiverUserId = m.u.ReceiverUserId
          }
          ).ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult> PostMessage(Message message)
        {
            _context.Messages.Add(message);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (MessageExists(message.MessageId))
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

        [HttpPost]
        [Route("DeleteMessage")]

        public async Task<IActionResult> DeleteMessage(int id)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message == null)
            {
                  return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = Status.Error, Message = APIMessages.ErrorOnNotFound });
            }

            _context.Messages.Remove(message);

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

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateMessage(int id)
        {
            var message = await _context.Messages.FindAsync(id);
            if (message == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = Status.Error, Message = APIMessages.ErrorOnNotFound });
            }

            message.State = MessageState.Seen;
            _context.Entry(message).State = EntityState.Modified;

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
        private bool MessageExists(int id)
        {
            return _context.Messages.Any(e => e.MessageId == id);
        }
    }
}
