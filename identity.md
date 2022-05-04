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

