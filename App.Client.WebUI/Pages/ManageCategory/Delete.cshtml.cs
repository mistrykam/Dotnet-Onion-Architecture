﻿using App.Domain.Entities;
using App.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

namespace App.Client.WebUI.Pages.ManageCategory
{
    public class DeleteModel : PageModel
    {
        private readonly ICatalogueUnitOfWork _catalogueUnitOfWork;

        public DeleteModel(ICatalogueUnitOfWork catalogueUnitOfWork)
        {
            _catalogueUnitOfWork = catalogueUnitOfWork;
        }

        [BindProperty]
        public Category Category { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Category = await _catalogueUnitOfWork.CategoryRepository.FindAsync((int)id);

            if (Category == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Category = await _catalogueUnitOfWork.CategoryRepository.FindAsync((int)id);

            if (Category != null)
            {
                await _catalogueUnitOfWork.CategoryRepository.RemoveAsync(Category);
                await _catalogueUnitOfWork.CommitAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
