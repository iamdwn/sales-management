using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using PRN221.SalesManagement.Repo.Interfaces;
using PRN221.SalesManagement.Repo.Models;
using PRN221.SalesManagement.Repo.Persistences;

namespace PRN221.SalesManagement.Web.Pages.CategoryPage
{
    public class CreateModel : PageModel
    {
        private readonly IUnitOfWork _unitOfWork;

        public CreateModel(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Category Category { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _unitOfWork.CategoryRepository.Insert(Category);

            _unitOfWork.Save();

            return RedirectToPage("./Index");
        }
    }
}
