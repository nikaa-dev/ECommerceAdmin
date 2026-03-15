using src.DTO;
using src.DTO.AuthDto;
using src.Common;

namespace src.Services.AuthServices;


public interface IAuthService
{
    //Task<ApiResponse<TokenResponseDto>> RegisterAsync(RegisterDto dto);
    Task<ApiResponse<LoginResponseDto>> LoginAsync(LoginDto dto);
    //Task<ApiResponse<UserDetailDto>> GetCurrentUserAsync(string userId);
}