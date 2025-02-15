using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace test1;

public class RoleModel
{
    [Required(),DataType(DataType.EmailAddress)]
    public string Email {get; set;}
    [Required(),DataType(DataType.Password)]
    public string Password {get;set;}
    public string  role{get;set;}

}
