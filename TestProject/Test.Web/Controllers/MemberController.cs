using Isp.Model.Entities;
using Isp.Model.ViewModels;
using Isp.Web.Classes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Isp.Web.Controllers
{
    public class MemberController : BaseController
    {
        private readonly IspAppContext _context;
        private readonly AppSettings _appSettings;
        public MemberController(IspAppContext context, AppSettings appSettings)
        {
            _context = context;
            _appSettings = appSettings;
        }

        public async Task<IActionResult> Details()
        {
            int? userid = null;
            if (_currentRole.HasValue /*&& _currentRole.Value == (int)RoleEnum.User*/)
            {
                userid = _currentUserId;
            }
            if (userid == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .Include(m => m.AccountVerify)
                .Include(m => m.MemberType)
                .Include(m => m.Role)
                .FirstOrDefaultAsync(m => m.Id == userid);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members.FindAsync(id);
            if (member == null)
            {
                return NotFound();
            }
            var memberVm = new MemberEditVm
            {
                Id = member.Id,
                FullName = member.FullName,
                Email = member.Email,
                Address = member.Address,
                IsActive = member.IsActive ?? false,
                MemberTypeId = member.MemberTypeId ?? 1,
                //RoleId = member.RoleId ?? 1,
                MobileNo = member.MobileNo,
                LicenceTypeId = member.LicenceTypeId ?? 3,
                UpdateAt = member.UpdateAt,
                //UpdateBy = member.UpdateBy,
                MotherCompanyName = member.MotherCompanyName,
                LastLoginAt = member.LastLoginAt,
                EntryAt = member.EntryAt,

            };
            LoadDropdownData(memberVm.MemberTypeId, null, memberVm.LicenceTypeId);
            return View(memberVm);
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(MemberEditVm memberVm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var member = _context.Members.Find(memberVm.Id);
                    if (member == null)
                    {
                        return NotFound();
                    }

                    member.FullName = memberVm.FullName;
                    //Email = memberVm.Email,
                    member.Address = memberVm.Address;
                    member.IsActive = memberVm.IsActive;
                    //MemberTypeId = memberVm.MemberTypeId,
                    //RoleId = userId == null ? (int)RoleEnum.User : memberVm.RoleId,
                    member.MobileNo = memberVm.MobileNo;
                    //Password = memberVm.Password,
                    //EntryAt = DateTime.Now,
                    member.LicenceTypeId = memberVm.LicenceTypeId;
                    member.MotherCompanyName = memberVm.MotherCompanyName;
                    //EntryBy = HttpContext.Session.GetInt32("UserId")

                    _context.Update(member);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MemberExists(memberVm.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details");
            }

            ViewData["LicenceTypeId"] = new SelectList(_context.LicenceTypes, "Id", "Id", memberVm.LicenceTypeId);
            //ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Id", member.RoleId);
            return View(memberVm);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members
                .Include(m => m.AccountVerify)
                .Include(m => m.MemberType)
                .Include(m => m.Role)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (member == null)
            {
                return NotFound();
            }

            return View(member);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var member = await _context.Members.FindAsync(id);
            if (member != null)
            {
                _context.Members.Remove(member);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MemberExists(int id)
        {
            return _context.Members.Any(e => e.Id == id);
        }

        private void LoadDropdownData(int? memberTypeId, int? roleId, int? licenceTypeId)
        {
            ViewData["MemberTypeId"] = new SelectList(_context.MemberTypes, "Id", "Name", memberTypeId);
            ViewData["RoleId"] = new SelectList(_context.Roles, "Id", "Name", roleId);
            ViewData["LicenceTypeId"] = new SelectList(_context.LicenceTypes, "Id", "Name", licenceTypeId);
        }

        [CustomAuthorize("Admin")]
        public async Task<IActionResult> Index()
        {
            return View();
        }

        [CustomAuthorize("Admin,User")]
        public async Task<IActionResult> GetPagedData(int pageNumber, int pageSize)
        {
            var query = _context.Members
                .Select(m => new
                {
                    m.Id,
                    m.MembershipId,
                    m.FullName,
                    m.MobileNo,
                    m.Address,
                    m.MotherCompanyName,
                    LicenceType = _context.LicenceTypes
                                    .Where(l => l.Id == m.LicenceTypeId)
                                    .Select(l => l.Name)
                                    .FirstOrDefault(),
                    Role = _context.Roles
                                .Where(r => r.Id == m.RoleId)
                                .Select(r => r.Name)
                                .FirstOrDefault(),
                    MemberType = _context.MemberTypes
                                    .Where(mt => mt.Id == m.MemberTypeId)
                                    .Select(mt => mt.Name)
                                    .FirstOrDefault(),
                    IsActive = m.IsActive == true ? "Yes" : "No",
                    m.Email
                });

            var totalRecords = await query.CountAsync();
            var data = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
            return Json(new { data, totalRecords });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfilePicture(MemberProfileVm model)
        {
            if (model.CompanyLogoFile == null || model.CompanyLogoFile.Length <= 0)
                return View(model);
            var member = await _context.Members.FindAsync(model.Id);
            if (member == null)
                return View(model);
            // Generate unique file name
            var fileName = Guid.NewGuid() + Path.GetExtension(model.CompanyLogoFile.FileName);
            var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/member_logo/", fileName);

            // Save file
            using (var stream = new FileStream(uploadPath, FileMode.Create))
            {
                await model.CompanyLogoFile.CopyToAsync(stream);
            }

            member.CompanyLogoFileName = fileName;
            await _context.SaveChangesAsync();

            // Redirect back to profile page (refreshes sidebar)
            return RedirectToAction("details", new { id = model.Id });
        }

        public IActionResult GetProfilePicture(int id)
        {
            var member = _context.Members.FirstOrDefault(x => x.Id == id);
            if (member == null) return NotFound();

            var vm = new MemberProfileVm
            {
                Id = member.Id,
                MembershipId = member.MembershipId,
                CompanyLogoFileName = "wwwroot/uploads/member_logo/" + member.CompanyLogoFileName ?? "~/images/default-user.png"
            };

            return PartialView("_ProfilePicture", vm);
        }
    }
}
