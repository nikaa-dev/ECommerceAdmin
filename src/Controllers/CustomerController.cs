using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using src.DTO.CustomerDto;
using src.Extensions.Pagenations;
using src.Models;
using src.Services.CustomerServices;
using src.Services.UserServices;
using System.Diagnostics;
using System.Threading.Tasks;

namespace src.Controllers;

public class CustomerController(ILogger<HomeController> logger,ICustomerService customerService) : Controller
{
    private readonly ILogger<HomeController> _logger = logger;
    [Authorize]
    public async Task<IActionResult> Index(string? filterByStatus,string? shortBy,string? searchItem,int pageNumber = 1)
    {
        var customers = await customerService.GetCustomerIncludedAsync();
        var now = DateTime.Now;
        
        ViewBag.activeCustomer = customers.Count(c => c.Status == true);
        ViewBag.inActiveCustomer = customers.Count(c => c.Status == false);
        ViewBag.Avg = customers.Any()
            ? customers.Average(c => c.TotalSpent)
            : 0;
        ViewBag.newMonth = customers
            .Count(c => c.JoinDate.Month == now.Month && c.JoinDate.Year == now.Year);
        
        if (filterByStatus != null) 
            if(filterByStatus == "Active")
                customers = customers.Where(c => c.Status == true).ToList();
            else
                customers = customers.Where(c => c.Status == false).ToList();
        if (searchItem != null) customers = customers.Where(c => c.Name == searchItem).ToList();
        if (!string.IsNullOrEmpty(shortBy))
        {
            customers = shortBy switch
            {
                "Name" => customers.OrderBy(c => c.Name).ToList(),
                "Order" => customers.OrderByDescending(c => c.Orders).ToList(),
                "Spending" => customers.OrderByDescending(c => c.TotalSpent).ToList(),
                _ => customers
            };
        }
        
        var queriyable = customers.AsQueryable();
        var paginations = queriyable.ToPagedResultAsync(pageNumber, 8);
        return View(paginations);
    }

    [HttpPost]
    public async Task<IActionResult> Update(CustomerRequestUpdateDto customerRequestUpdateDto) 
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

       var editCustomer = await customerService.UpdateCustomerAsync(customerRequestUpdateDto);

        if (!editCustomer)
        {
            return BadRequest(new { success = false, message = "Update failed" });
        }

        return Json(new { success = true, message = "Customer updated successfully" });
    }

    [HttpPost]
    public async Task<IActionResult> Delete(string Id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var deleteCustomer = await customerService.DeleteCustomerAsync(Id);

        if (!deleteCustomer)
        {
            return BadRequest(new { success = false, message = "Delete failed" });
        }

        return Json(new { success = true, message = "Customer deleted successfully" });
    }

    public async Task Export(CutomerRequestExportDto exportCustomer) {

        await customerService.ExportCustomerData(exportCustomer);

    }

}