﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ArgoCMS.Data;
using ArgoCMS.Models.JointEntities;

namespace ArgoCMS.Pages.Admin.EmployeeNotificationGroup
{
    public class CreateModel : PageModel
    {
        private readonly ArgoCMS.Data.ApplicationDbContext _context;

        public CreateModel(ArgoCMS.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id");
        ViewData["NotificationGroupId"] = new SelectList(_context.NotificationGroups, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public ArgoCMS.Models.JointEntities.EmployeeNotificationGroup EmployeeNotificationGroup { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || _context.EmployeeNotificationGroups == null || EmployeeNotificationGroup == null)
            {
                return Page();
            }

            _context.EmployeeNotificationGroups.Add(EmployeeNotificationGroup);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}