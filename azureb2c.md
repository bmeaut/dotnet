# Linkek

- [OAuth flow-k](https://medium.com/@darutk/diagrams-and-movies-of-all-the-oauth-2-0-flows-194f3c3ade85)
- [OpenID specifikációk](https://openid.net/developers/specs/)
- [Duende IdentityServer](https://docs.duendesoftware.com/identityserver/v6)
- [Authorization code flow - MIP](https://docs.microsoft.com/en-us/azure/active-directory/develop/v2-oauth2-auth-code-flow)
- [Azure előfizetések BME hallgatóknak](https://www.aut.bme.hu/Course/felho) - **Azure erőforrások** alcím

# WebAssembly w Azure B2C

https://docs.microsoft.com/en-us/aspnet/core/blazor/security/webassembly/hosted-with-azure-active-directory-b2c?view=aspnetcore-6.0

# User debug Razor component

https://github.com/dotnet/aspnetcore/blob/v6.0.4/src/Components/WebAssembly/testassets/Wasm.Authentication.Client/Pages/User.razor

# Authorization policy

```csharp
builder.Services.AddAuthorization(options=>
    options.AddPolicy("Admin", policy =>
        policy.RequireClaim(
            "http://schemas.microsoft.com/identity/claims/objectidentifier"
            //Vegyünk fel egy-két Object ID-t a regisztrált felhasználók közül
            , "00000000-0000-0000-0000-000000000000"
            , "00000000-0000-0000-0000-000000000000" ))
    
);
```

# Admin policy

```csharp
/*kóddarabka*/
options.AddPolicy("Admin", policy =>
    policy.RequireClaim(
        "http://schemas.microsoft.com/identity/claims/objectidentifier"
    //Vegyünk fel egy-két Object ID-t a regisztrált felhasználók közül
        , "00000000-0000-0000-0000-000000000000"
        , "00000000-0000-0000-0000-000000000000"));
```
