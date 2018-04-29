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
