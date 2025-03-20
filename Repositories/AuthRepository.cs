using AuthenticationService.Data;
using AuthenticationService.DTOs;
using AuthenticationService.Models;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Serilog;
using AuthenticationService.Helpers.JwtHelper;
namespace AuthenticationService.Repositories
{
    public class AuthRepository : IAuthRepository
    {


        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IJwtHelper _jwtHelper;
     
        public AuthRepository(AppDbContext context, IMapper mapper, IJwtHelper jwtHelper)
        {
            _context = context;
            _mapper = mapper;
            _jwtHelper = jwtHelper;
            
        }


        public async Task<bool> CheckUser(string Email)
        {
            try
            {
                if (Email == null)
                {
                    Log.Information("Email is empty");
                }





                return await _context.Users.AnyAsync(u => u.Email == Email);
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                return false;
                
            }

        }


        public async Task<string> RegisterUser(RegisterDTO register)
        {
            try
            {
                if (register.Username == null)
                {
                    Log.Information("Username is empty");
                    return "Username is empty";
                }
                if (register.Email == null)
                {
                    Log.Information("Email is empty");
                    return "Email is empty";
                }
                if (register.PasswordHash == null)
                {
                    Log.Information("Password is empty");
                    return "Password is empty";
                }
                register.PasswordHash = BCrypt.Net.BCrypt.HashPassword(register.PasswordHash);
                var user = _mapper.Map<User>(register);
                await _context.Users.AddAsync(user);
                await _context.SaveChangesAsync();
                return "User registered successfully";
                
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                return ex.ToString();
            }

        }




        public async Task<LoginResponseDTO> LoginUser(LoginDTO loginDTO)
        {

            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == loginDTO.Email);

                if (user == null)
                {
                    Log.Information("User not found");
                    return new LoginResponseDTO { Message = "User not found" };
                }
                bool password = BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.PasswordHash);



                if (!password)
                {
                    Log.Information("Password is empty");
                    return new LoginResponseDTO { Message = "Password dont matches" };

                }

                if (user.IsActive == false)
                {
                    Log.Information("User id unactive, user blocked by admin");
                    return new LoginResponseDTO { Message = "User blocked by admin" };

                }

                var token = _jwtHelper.GenerateToken(user);


                var result = new LoginResponseDTO
                {
                    Username = user.Username,
                    Email = user.Email,
                    Token = token,
                    Message = "Successfully loged in"
                };
                return result;
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message);
                return new LoginResponseDTO { Message = ex.Message };
            }

        }
    }
}
