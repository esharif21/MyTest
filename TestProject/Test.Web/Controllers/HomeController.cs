using Test.Model.Entities;
//using Test.Model.ViewModels;
using Test.Web.Classes;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace Test.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly TestProjectContext _context;

        public HomeController(TestProjectContext context, ILogger<HomeController> logger)
        {
            _context = context;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        [CustomAuthorize("User,Admin")]
        public IActionResult Dashboard()
        {
            //var payments = _context.PaymentInfos.Where(x => x.PaymentStatusId == (int)PaymentStatusEnum.Verified && x.CreatedAt.Value.Month == DateTime.Now.Month);
            //return View(new
            //{
            //    GeneralMember = _context.Members.Where(x => x.MemberTypeId == 1 && x.IsActive == true).Count(),
            //    AssociateMember = _context.Members.Where(x => x.MemberTypeId == 2 && x.IsActive == true).Count(),
            //    NoOfCurrentMonthCollection = payments.Count(),
            //    SumOfCurrentMonthCollection = payments.Sum(x => x.Amount),
            //});
            return RedirectToAction("Index");
        }
        public IActionResult Privacy()
        {
            return View();
        }
    }
}
