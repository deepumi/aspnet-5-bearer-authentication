# ASP.NET Core 5.0 Bearer Authentication.

## The main goal is to migrate ASP.NET WEB API 2.0 Bearer authentication to ASP.NET 5.0 Core authorization interfaces.

https://deepumi.wordpress.com/2020/05/30/asp-net-core-5-0-bearer-authentication/

The repository implement custom Bearer Authentication using ASP.NET core IAsyncAuthorizationFilter & IPolicyEvaluator interfaces. Read more about on my [blog post](https://deepumi.wordpress.com/2020/05/30/asp-net-core-5-0-bearer-authentication/)


## Implment IPolicyEvaluator interface.
```csharp
public sealed class BearerPolicyEvaluator : IPolicyEvaluator
{
	private const string Scheme = "Bearer";

	public Task<AuthenticateResult> AuthenticateAsync(AuthorizationPolicy _, HttpContext context)
	{
		if (!context.Request.Headers.ContainsKey("Authorization"))
			return Task.FromResult(AuthenticateResult.Fail("No Authorization header found!"));

		string authHeader = context.Request.Headers["Authorization"];

		string bearerToken = authHeader?.Replace("Bearer ", string.Empty);

		if (!string.Equals(bearerToken, "authToken", StringComparison.Ordinal))
		{
			return Task.FromResult(AuthenticateResult.Fail("Invalid token"));
		}

		var claims = new[]
		{
			new Claim(ClaimTypes.NameIdentifier, "1000"),
			new Claim(ClaimTypes.Name, "Deepu Madhusoodanan")
		};

		var identity = new ClaimsIdentity(claims, Scheme);

		var principal = new ClaimsPrincipal(identity);

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

```

## Implement IAsyncAuthorizationFilter interace.

```csharp
public sealed class BearerAuthorizeFilter : IAsyncAuthorizationFilter
{
    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        if (context?.HttpContext?.Request?.Headers == null) throw new ArgumentNullException(nameof(context));

        if (!context.HttpContext.Request.Headers.ContainsKey("Authorization"))
            context.Result = CreateUnauthorized();

        var policyEvaluator = context.HttpContext.RequestServices.GetRequiredService<IPolicyEvaluator>();
        var authenticateResult = await policyEvaluator.AuthenticateAsync(default, context.HttpContext);
        var authorizeResult = await policyEvaluator.AuthorizeAsync(default, authenticateResult, context.HttpContext, context);

        if (authorizeResult.Challenged)
        {
            context.Result = CreateUnauthorized();
            return;
        }

        context.HttpContext.User = authenticateResult.Principal;

        static IActionResult CreateUnauthorized() => new UnauthorizedObjectResult(new ErrorMessage("Unauthorized", 401));
    }
}
```

## Configure Services
Register Authentication Interface in Startup.cs class

```csharp
public void ConfigureServices(IServiceCollection services)
{
   services.AddControllers(o => o.Filters.Add(new BearerAuthorizeFilter()));
}
```

