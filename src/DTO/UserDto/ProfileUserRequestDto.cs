using src.Enums;

namespace src.DTO.UserDto
{
    public class ProfileUserRequestDto
    {
        public string Id { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string PhotoProfile { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public UserStatus Status { get; set; }
        public string Address { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
    }
}
