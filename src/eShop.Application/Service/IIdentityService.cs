using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShop.Application.Service
{
    public interface IIdentityService
    {
        Task<string> GetUsernameAsync(string userId);
    }
}
