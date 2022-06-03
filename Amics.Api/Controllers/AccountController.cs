using Amics.Api.Infrastructure;
using Amics.Api.utils;
using Amics.Lookup.Models; 
using Lookup.Dapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Amics.web.Controllers
{
      public class AccountController : Controller
    {
        private readonly ILogger<AccountController> _logger;
        private readonly IAuthorizationPolicy _authPolicy;
        private readonly ILookupDbRepositoryService _lookupDbRepository;
        public AccountController(IAuthorizationPolicy authPolicy, ILogger<AccountController> logger, ILookupDbRepositoryService lookupDbRepo)
        {
            _authPolicy = authPolicy;
            _logger = logger;
            _lookupDbRepository = lookupDbRepo;
        }

        [HttpGet, Route("Login")]
        public async Task<bool> Login([FromQuery] string userName, [FromQuery] string password)
        {
            var decodedPassword = password.Base64StringDecode();
            var result =  _lookupDbRepository.ValidateUser(userName, decodedPassword);
            if (!result.Item1)
            {
                _logger.LogError($"Login Error: {userName}");
                return false;
            }
             
            var role = await _authPolicy.GetRole(userName);
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, role),
                new Claim("DB", result.Item2)
            };

            var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            return true;
        }   

        [HttpGet, Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return Redirect(Url.Content("~/"));
        }
 
        [HttpGet, Route("user"), ProducesResponseType(typeof(ApplicationUser), StatusCodes.Status200OK)]
        public async Task<ApplicationUser> GetUser()
        {
                var userId = User.Identity?.Name;
            if(string.IsNullOrEmpty(userId))
            {
                return await Task.FromResult<ApplicationUser>(null);
            }

            var userAgent = Request.Headers[HeaderNames.UserAgent].ToString();
            _logger.LogInformation($"[{userId}] User-Agent: {userAgent}");

            var user =  await _lookupDbRepository.GetUsersInfo(userId);
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var db = User.FindFirst("DB")?.Value;

            return user;

        }

        public IActionResult AccessDenied()
        {
            var currentUserId = User.Identity?.Name;
            var returnUri = Request.Query["ReturnUrl"];
            _logger.LogInformation($"User {currentUserId} tried to access {returnUri}, but failed");

            throw new UnauthorizedAccessException($"Not Authorzied Access to {returnUri}");
        }
    }

    
}
