using AuthenticationService.DTOs;

namespace AuthenticationService.Repositories
{
    public interface IAuthRepository
    {

        Task<string> RegisterUser(RegisterDTO registerDTO);
        Task<bool> CheckUser(string Email);

        Task<LoginResponseDTO> LoginUser(LoginDTO loginDTO);

    }
}
