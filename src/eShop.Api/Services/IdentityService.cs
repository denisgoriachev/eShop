using eShop.Application.Service;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace eShop.Api.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IdentityService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public Task<string> GetUsernameAsync(string userId)
        {
            return Task.FromResult(_httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name));
        }
    }
}
