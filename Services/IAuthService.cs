using AuthenticationService.DTOs;

namespace AuthenticationService.Services
{
    public interface IAuthService
    {

        Task<string> CreateAccount(RegisterDTO registerDTO);
        Task<LoginResponseDTO> UserLogin(LoginDTO loginDTO);
    }
}
