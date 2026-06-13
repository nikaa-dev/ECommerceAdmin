using src.Enums;
using src.Models.Ecommerce;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace src.DTO.ProductDto;

public class ProductRequestDto
{
    
}


public class ProductRequestExportDto
{
    public int PageNumber { get;set; }
    public int Count { get; set; }
}

public class ProductRequestCreateDto
{


    public Guid Id { get; set; } = Guid.NewGuid();

    public string Name { get; set; }

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public string? ImageUrl { get; set; }

    public string Status { get; set; }

    public int Stock { get; set; }

    public string Category { get; set; }
}

public class ProductRequestUpdateDto
{
    public Guid Id { get; set; }

    public string Name { get; set; } 

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public string? ImageUrl { get; set; }

    public string Status { get; set; }

    public int Stock { get; set; }

    public string Category { get; set; }
}