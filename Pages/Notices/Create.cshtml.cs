﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ArgoCMS.Data;
using ArgoCMS.Models;

namespace ArgoCMS.Pages.Notices
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
            return Page();
        }

        [BindProperty]
        public Notice Notice { get; set; }
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Notices.Add(Notice);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
