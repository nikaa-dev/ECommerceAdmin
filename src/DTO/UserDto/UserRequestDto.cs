using src.Enums;
namespace src.DTO.UserDto
{
    public class UserRequestDto
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
        public UserStatus Status { get; set; }
    }
}
