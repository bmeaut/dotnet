# HTTPS tanusítvány gondok

## A Kestrel nem talál jó tanúsítványt

### Tünet

A `dotnet new`-val generált ASP.NET Core 5-ös projektek el sem indulnak:
> Unable to configure HTTPS endpoint. No server certificate was specified, and the default developer certificate could not be found or is out of date.

### Ok

Van ugyan egy localhost-os tanúsítvány a _CurrentUser\My store_-ban, ami nem jó (valamiért?), de a .NET nem tud róla, így nem is törli pl. a `dotnet dev-certs https --clean`. Hiába hozunk létre újabbat, a rossz eredeti belerondít.

### Megoldás

Törölni *minden* localhost-os tanusítványt a _CurrentUser\My_ store-ból, pl. powershell-el (GUI sokszor le van tiltva vagy, ha el is indul, nem enged törölni).

```powershell
ls Cert:\CurrentUser\My
Remove-Item <localhost-os cert thumbprint>
dotnet dev-certs https -t
```
