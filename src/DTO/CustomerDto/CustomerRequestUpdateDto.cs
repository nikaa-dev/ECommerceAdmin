namespace src.DTO.CustomerDto;

public class CustomerRequestUpdateDto
{
    public string Id { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Status { get; set; }
}

public class CutomerRequestExportDto
{
    public int PageNumber { get; set; }
    public int Count { get; set; }
}