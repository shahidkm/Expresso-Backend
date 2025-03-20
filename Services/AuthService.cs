using AuthenticationService.DTOs;
using AuthenticationService.Repositories;
namespace AuthenticationService.Services
{
    public class AutheService : IAuthService
    {
        public IAuthRepository _repository;
        public AutheService(IAuthRepository repository)
        {

            _repository = repository;
        }
        public async Task<string> CreateAccount(RegisterDTO registerDTO)
        {
            try
            {
                if (registerDTO == null)
                {

                    return "Register datas required";
                }
                bool user = await _repository.CheckUser(registerDTO.Email);

                if (user)
                {
                    return "User already exists";
                }
                else
                {

                    await _repository.RegisterUser(registerDTO);


                    return "User Registered Sucessfully";

                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        public async Task<LoginResponseDTO> UserLogin(LoginDTO loginDTO)
        {
            var result = await _repository.LoginUser(loginDTO);
            return result;
        }


    }
}
