namespace src.DTO.RoleDto
{
    public class RoleRequestCreateDto
    {
        public string RoleName { get; set; } 
        public string Description { get; set; }
        public List<string> Permission { get; set; }
    }

    public class RoleRequestUpdateDto
    {
        public string Id { get; set; }
        public string RoleName { get; set; }
        public string Description { get; set; }
        public List<string> Permission { get; set; }
    }
}
