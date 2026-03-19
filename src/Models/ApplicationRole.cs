using Microsoft.AspNetCore.Identity;
using System.Collections.Generic; // កុំភ្លេចបន្ថែម Namespace នេះ

namespace src.Models;

public class ApplicationRole : IdentityRole
{
    public string Description { get; set; } = string.Empty;

    // ១. EF Core ត្រូវការ Empty Constructor នេះជាដាច់ខាត
    public ApplicationRole() : base()
    {
        UserRoles = new HashSet<IdentityUserRole<string>>();
    }

    // ២. ប្រសិនបើចង់ប្រើ Constructor ជាមួយ roleName អ្នកត្រូវដាក់ឈ្មោះ Parameter ឱ្យដូច Property (គឺ "name")
    // ប៉ុន្តែសម្រាប់ EF Core ការប្រើ Empty Constructor គឺមានសុវត្ថិភាពបំផុត
    public ApplicationRole(string name) : base(name)
    {
        UserRoles = new HashSet<IdentityUserRole<string>>();
    }

    public virtual ICollection<IdentityUserRole<string>> UserRoles { get; set; }
}