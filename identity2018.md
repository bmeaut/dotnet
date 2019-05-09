## RegisterUserDTO
```csharp
 public class RegisterUserDTO
 {
     [Required]
     public string UserName { get; set; }

     [Required]
     public string Email { get; set; }

     [Required]
     public string Password { get; set; }
 }
```
## Register user test
### Header
```
Content-Type: application/json
```
### Body
```javascript
{
    "UserName" : "dotnetrox",
    "Email" : "foo@bar.com",
    "Password" : "Admin123."
}
```
### IdentityServer4 config
```csharp
 public class Config
 {
     public static IEnumerable<ApiResource> GetApiResources()
     {
         return new List<ApiResource>
         {
             new ApiResource("api1", "My API")
         };
     }

     public static IEnumerable<IdentityResource> GetIdentityResources()
     {
         return new List<IdentityResource>
         {
             new IdentityResources.OpenId(),
             new IdentityResources.Profile(),
             new IdentityResources.Email(),
         };
     }

     public static IEnumerable<Client> GetClients()
     {
         return new List<Client>
         {
             new Client
             {
                 ClientId = "ro.client",
                 AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                 ClientSecrets =
                 {
                     new Secret("secret".Sha256())
                 },
                 AllowedScopes = { "api1" }
             }
         };
     }
 }

```
## ROPG flow
http://www.bubblecode.net/en/2016/01/22/understanding-oauth2/#Resource_Owner_Password_Credentials_Grant
## IdentityServer4 client - get access token in ROPG flow
```csharp
static async Task Main(string[] args)
{
     // discover endpoints from metadata
     var disco = await DiscoveryClient.GetAsync("https://localhost:PORT/");
     if (disco.IsError)
     {
         Console.WriteLine(disco.Error);
     }
     else
     {
         // request token
         var tokenClient = new TokenClient(disco.TokenEndpoint, "ro.client", "secret");
         var tokenResponse = await tokenClient
             .RequestResourceOwnerPasswordAsync("dotnetrox", "Admin123.", "api1");
         if (tokenResponse.IsError)
         {
             Console.WriteLine(tokenResponse.Error);
         }
         else
         {
             Console.WriteLine(tokenResponse.Json);           
     	  }
     Console.ReadLine();
}
```
## IdentityServer4 client - almost full ROPG flow (no expiration handling!)
```csharp
 static async Task Main(string[] args)
 {
     // discover endpoints from metadata
     var disco = await DiscoveryClient.GetAsync("https://localhost:PORT/");
     if (disco.IsError)
     {
         Console.WriteLine(disco.Error);
     }
     else
     {
         // request token
         var tokenClient = new TokenClient(disco.TokenEndpoint, "ro.client", "secret");
         var tokenResponse = await tokenClient.RequestResourceOwnerPasswordAsync("dotnetrox", "Admin123.", "api1");

         if (tokenResponse.IsError)
         {
             Console.WriteLine(tokenResponse.Error);
         }
         else
         {
             Console.WriteLine(tokenResponse.Json);
             using (var client = new HttpClient())
             {
                 client.SetBearerToken(tokenResponse.AccessToken);

                 var response = await client.GetAsync("https://localhost:PORT/api/values");
                 if (!response.IsSuccessStatusCode)
                 {
                     Console.WriteLine(response.StatusCode);
                 }
                 else
                 {
                     var content = await response.Content.ReadAsStringAsync();
                     Console.WriteLine(content);
                 }
             }
         }
     }
     Console.ReadLine();
 }
```
## Register user with claim
```javascript
{
    "UserName" : "dotnetGURU",
    "Email" : "foobar@bar.com",
    "Password" : "Admin123.",
    "Level":"Guru"
}
```
## ProfileService
```csharp
 public class ProfileService : IProfileService
 {
     protected UserManager<ApplicationUser> _userManager;

     public ProfileService(UserManager<ApplicationUser> userManager)
     {
         _userManager = userManager;
     }

     public async Task GetProfileDataAsync(ProfileDataRequestContext context)
     {
         var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));
        
         var user = await _userManager.FindByIdAsync(subject.GetSubjectId());
         if (user == null)
             throw new ArgumentException("Invalid subject identifier");

         var claims = new List<Claim>
         {
             new Claim(JwtClaimTypes.Subject, user.Id.ToString()),
             new Claim(JwtClaimTypes.PreferredUserName, user.UserName)
         };

         claims.AddRange(await _userManager.GetClaimsAsync(user));
         context.IssuedClaims = claims.ToList();
     }

     public async Task IsActiveAsync(IsActiveContext context)
     {
         var subject = context.Subject ?? throw new ArgumentNullException(nameof(context.Subject));

         var user = await _userManager.FindByIdAsync(subject.GetSubjectId());

         context.IsActive = false;

         if (user != null)
         {
             if (_userManager.SupportsUserSecurityStamp)
             {
                 var security_stamp = subject.Claims
                     .Where(c => c.Type == "security_stamp")
                     .Select(c => c.Value).SingleOrDefault();
                 if (security_stamp != null)
                 {
                     var db_security_stamp = await _userManager
                         .GetSecurityStampAsync(user);
                     if (db_security_stamp != security_stamp)
                         return;
                 }
             }

             context.IsActive =
                 !user.LockoutEnabled ||
                 !user.LockoutEnd.HasValue ||
                 user.LockoutEnd <= DateTime.Now;
         }
     }
 }
```
