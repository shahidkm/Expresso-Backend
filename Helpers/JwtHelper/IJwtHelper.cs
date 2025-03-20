using AuthenticationService.Models;

namespace AuthenticationService.Helpers.JwtHelper
{
    public interface IJwtHelper
    {
        String GenerateToken(User user);


    }
}
