using System.Threading.Tasks;
using AspNetBearerAuthentication.Extensions;
using AspNetBearerAuthentication.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;

namespace AspNetBearerAuthentication.Auth
{
    public sealed class BearerPolicyEvaluator : IPolicyEvaluator
    {
        private const string Scheme = "Bearer";

        private readonly IUserRepository _repository;

        private readonly ICalimsService _calimsService;

        public BearerPolicyEvaluator(IUserRepository repository, ICalimsService calimsService)
        {
            _repository = repository;
            _calimsService = calimsService;
        }

        public Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy _, HttpContext context)
        {
            var bearerToken = context.Request.Headers.GetBearerToken(); //get bearer token from extension method.

            var user = _repository.GetUser(bearerToken); //get user object from repository.
            
            var principal = _calimsService.CreateClaimsPrincipal(user); //call calims service to create some claims.
            
            if(principal == null) return Task.FromResult(AuthenticateResult.Fail("Invalid token"));

            var ticket = new AuthenticationTicket(principal, Scheme);

            var authenticateResult = AuthenticateResult.Success(ticket);

            return Task.FromResult(authenticateResult);
        }

        public Task<PolicyAuthorizationResult> AuthorizeAsync(AuthorizationPolicy _,
            AuthenticateResult authenticationResult, HttpContext context,
            object resource)
        {
            var authorizeResult = authenticationResult.Succeeded
                ? PolicyAuthorizationResult.Success()
                : PolicyAuthorizationResult.Challenge();

            return Task.FromResult(authorizeResult);
        }
    }
}