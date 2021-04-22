using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Authentication;

namespace FoodSharing.Services
{
    public interface IUserService
    {
        Task<IEnumerable<ApplicationUser>> GetPeopleAsync();
    }
}
