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
```json
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
## IdentityServer4 client - full ROPG flow
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
