using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Amics.Api.Infrastructure
{
    public interface IAuthorizationPolicy
    {
        Task<string> GetRole(string userName);
        Task<ICollection<Claim>> GetClaims(string userName);        

    }
    public class AuthorizationPolicy: IAuthorizationPolicy
    {
        private readonly ILogger<AuthorizationPolicy> _logger;

        public AuthorizationPolicy(ILogger<AuthorizationPolicy> logger)
        {
            _logger = logger;
        }

        public async Task<string> GetRole(string userName)
        {
            return await Task.FromResult(AppRoles.Basic);
        }

        public async Task<ICollection<Claim>> GetClaims(string userName)
        {
            var role = await GetRole(userName);
            var claimList = new List<Claim>();
            claimList.Add(new Claim(ClaimTypes.Role, role));

            _logger.LogDebug(JsonConvert.SerializeObject(claimList));
            return await Task.FromResult(claimList.ToList());
        }
    }

    public static class AppRoles
    {
        public const string Basic = nameof(Basic);
        public const string Admin = nameof(Admin);
        public const string WebMaster = nameof(WebMaster);
    }

    public static class AuthPolicies
    {
        public const string CanView = nameof(CanView);  
        public const string AtleaseBasicUser  = nameof(AtleaseBasicUser);
        public const string AtleastAdmin = nameof(AtleastAdmin);
        public const string SuperUser = nameof(SuperUser);
    }
}
