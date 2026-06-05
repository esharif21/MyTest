using Test.Model.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Isp.Model.ViewModels
{
    public class MemberCreateVm
    {
        public MemberCreateVm()
        {
        }

        [Display(Name = "Full Name")]
        [StringLength(100, ErrorMessage = "Full Name cannot exceed {0} characters")]
        [Required(ErrorMessage = "'{0}' is required")]
        public string FullName { get; set; }


        [Display(Name = "Company Name")]
        [StringLength(100, ErrorMessage = "Company Name cannot exceed {0} characters")]
        public string? MotherCompanyName { get; set; }


        [Display(Name = "Mobile Number")]
        [StringLength(11, ErrorMessage = "Mobile Number cannot exceed {0} digits")]
        [Required(ErrorMessage = "'{0}' is required")]
        [Phone(ErrorMessage = "Invalid Mobile Number")]
        [RegularExpression(@"^(015|016|013|017|018|019)\d{8}$", ErrorMessage = "Not a valid mobile number")]
        public string MobileNo { get; set; }


        [Display(Name = "Address")]
        [StringLength(250, ErrorMessage = "Address cannot exceed 250 characters")]
        public string? Address { get; set; }


        [Display(Name = "Role")]
        [Required(ErrorMessage = "'{0}' is required")]
        public int RoleId { get; set; }


        [Display(Name = "Member Type")]
        [Required(ErrorMessage = "'{0}' is required")]
        public int MemberTypeId { get; set; }


        [Display(Name = "Licence Type")]
        [Required(ErrorMessage = "'{0}' is required")]
        public int LicenceTypeId { get; set; }


        //[Display(Name = "Active")]
        //public bool IsActive { get; set; } = true;

        [Display(Name = "Email")]
        [StringLength(250, ErrorMessage = "Email cannot exceed 250 characters")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        [Required(ErrorMessage = "'{0}' is required")]
        public string Email { get; set; }


        [Display(Name = "Password")]
        [StringLength(250, ErrorMessage = "Password cannot exceed {0} characters")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "'{0}' is required")]
        public string Password { get; set; }
    }
}
