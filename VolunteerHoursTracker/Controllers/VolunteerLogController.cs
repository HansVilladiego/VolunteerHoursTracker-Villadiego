using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VolunteerHoursTracker.Infrastructure.Entities;
using VolunteerHoursTracker.Services.Interfaces;
using VolunteerHoursTracker.ViewModels;

namespace VolunteerHoursTracker.Controllers
{
    [Authorize]
    public class VolunteerLogController : Controller
    {
        private readonly IVolunteerLogService _volunteerLogService;
        private readonly UserManager<ApplicationUser> _userManager;

        public VolunteerLogController(
            IVolunteerLogService volunteerLogService,
            UserManager<ApplicationUser> userManager)
        {
            _volunteerLogService = volunteerLogService;
            _userManager = userManager;
        }

        // GET: VolunteerLog
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            var logs = await _volunteerLogService.GetLogsByUserIdAsync(userId!);

            var viewModels = logs.Select(log => new VolunteerLogViewModel
            {
                Id = log.Id,
                Activity = log.Activity,
                HoursWorked = log.HoursWorked,
                Date = log.Date,
                Description = log.Description
            }).ToList();

            return View(viewModels);
        }

        // GET: VolunteerLog/Create
        public IActionResult Create()
        {
            var model = new VolunteerLogViewModel
            {
                Date = DateTime.Today
            };
            return View(model);
        }

        // POST: VolunteerLog/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(VolunteerLogViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = _userManager.GetUserId(User);
                var log = new VolunteerLog
                {
                    UserId = userId!,
                    Activity = model.Activity,
                    HoursWorked = model.HoursWorked,
                    Date = model.Date,
                    Description = model.Description
                };

                var result = await _volunteerLogService.CreateLogAsync(log);
                if (result)
                {
                    TempData["SuccessMessage"] = "Volunteer hours logged successfully!";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Unable to save volunteer log. Please try again.");
            }

            return View(model);
        }

        // GET: VolunteerLog/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var log = await _volunteerLogService.GetLogByIdAsync(id);
            if (log == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            if (log.UserId != userId)
            {
                return Forbid();
            }

            var model = new VolunteerLogViewModel
            {
                Id = log.Id,
                Activity = log.Activity,
                HoursWorked = log.HoursWorked,
                Date = log.Date,
                Description = log.Description
            };

            return View(model);
        }

        // POST: VolunteerLog/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, VolunteerLogViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var log = await _volunteerLogService.GetLogByIdAsync(id);
                if (log == null)
                {
                    return NotFound();
                }

                var userId = _userManager.GetUserId(User);
                if (log.UserId != userId)
                {
                    return Forbid();
                }

                log.Activity = model.Activity;
                log.HoursWorked = model.HoursWorked;
                log.Date = model.Date;
                log.Description = model.Description;

                var result = await _volunteerLogService.UpdateLogAsync(log);
                if (result)
                {
                    TempData["SuccessMessage"] = "Volunteer log updated successfully!";
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError("", "Unable to update volunteer log. Please try again.");
            }

            return View(model);
        }

        // GET: VolunteerLog/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var log = await _volunteerLogService.GetLogByIdAsync(id);
            if (log == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            if (log.UserId != userId)
            {
                return Forbid();
            }

            var model = new VolunteerLogViewModel
            {
                Id = log.Id,
                Activity = log.Activity,
                HoursWorked = log.HoursWorked,
                Date = log.Date,
                Description = log.Description
            };

            return View(model);
        }

        // POST: VolunteerLog/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var result = await _volunteerLogService.DeleteLogAsync(id);
            if (result)
            {
                TempData["SuccessMessage"] = "Volunteer log deleted successfully!";
            }
            else
            {
                TempData["ErrorMessage"] = "Unable to delete volunteer log.";
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: VolunteerLog/Leaderboard
        [AllowAnonymous]
        public async Task<IActionResult> Leaderboard()
        {
            var topVolunteers = await _volunteerLogService.GetTopVolunteersAsync(10);

            var viewModel = new TopVolunteersViewModel
            {
                TopVolunteers = topVolunteers
            };

            if (User.Identity?.IsAuthenticated == true)
            {
                var userId = _userManager.GetUserId(User);
                viewModel.CurrentUserTotalHours = await _volunteerLogService.GetTotalHoursByUserIdAsync(userId!);

                var allVolunteers = topVolunteers.OrderByDescending(v => v.Value).ToList();
                viewModel.CurrentUserRank = allVolunteers.FindIndex(v => v.Value == viewModel.CurrentUserTotalHours) + 1;
            }

            return View(viewModel);
        }
    }
}