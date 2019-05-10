# Linkek

- [OAuth flow-k](https://medium.com/@darutk/diagrams-and-movies-of-all-the-oauth-2-0-flows-194f3c3ade85)
- [OpenID specifikációk](https://openid.net/developers/specs/)
- [IdentityServer4](https://identityserver4.readthedocs.io/en/latest/)

# Azure B2C tutorial-ok

1. [Azure előfizetések BME hallgatóknak](https://www.aut.bme.hu/Course/felho) - **Azure erőforrások** alcím
2. [Tenant létrehozás](https://docs.microsoft.com/en-us/azure/active-directory-b2c/tutorial-create-tenant)
3. [Webalkalmazás regisztráció](https://docs.microsoft.com/en-us/azure/active-directory-b2c/tutorial-register-applications)
4. [Folyamat létrehozás](https://docs.microsoft.com/en-us/azure/active-directory-b2c/tutorial-create-user-flows#create-a-sign-up-and-sign-in-user-flow)
5. [Kliensalkalmazás regisztráció](https://docs.microsoft.com/hu-hu/azure/active-directory-b2c/add-native-application)


# Vastagkliens (WPF) mintaprojekt
- [Repo](https://github.com/bmeaut/active-directory-b2c-dotnet-desktop/tree/msalv3)

# Ha nem működik a HTTPS
```powershell
dotnet dev-certs https --clean
dotnet dev-certs https --trust
```

# TLS beállítás
```csharp
System.Net.ServicePointManager.SecurityProtocol =
                SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
```

# Scope policy

```csharp
services.AddAuthorization(options =>
{
    options.AddPolicy("DemoRead", policy =>
        policy.RequireClaim(
            "http://schemas.microsoft.com/identity/claims/scope",
            "demo.read"
        )
    );
});
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
