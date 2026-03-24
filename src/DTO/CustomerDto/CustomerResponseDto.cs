namespace src.DTO.CustomerDto;

public class CustomerResponseDto
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Contact { get; set; }
    public string Email { get; set; }
    public DateTime JoinDate { get; set; }
    public int Orders { get; set; }
    public decimal TotalSpent { get; set; }
    public bool Status { get; set; }
}