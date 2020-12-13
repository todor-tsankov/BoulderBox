﻿using System.Threading.Tasks;

using BoulderBox.Services.Data.Forum;
using BoulderBox.Web.Areas.Administration.Controllers.Common;
using BoulderBox.Web.ViewModels.Forum.Categories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoulderBox.Web.Areas.Administration.Controllers
{
    public class CategoriesController : AdministrationController
    {
        private readonly ICategoriesService categoriesService;

        public CategoriesController(ICategoriesService categoriesService)
        {
            this.categoriesService = categoriesService;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryInputModel categoryInput, IFormFile formFile)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(categoryInput);
            }

            var image = await this.SaveImageFileAsync(formFile);
            await this.categoriesService.AddAsync(categoryInput, image);

            return this.RedirectToAction("Index", "Categories", new { area = "Forum" });
        }

        public IActionResult Edit(string id)
        {
            var category = new CategoryEditViewModel()
            {
                Id = id,
                CategoryInput = this.categoriesService
                    .GetSingle<CategoryInputModel>(x => x.Id == id),
            };

            return this.View(category);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, CategoryInputModel categoryInput)
        {
            var existsCategory = this.categoriesService
                .Exists(x => x.Id != id && x.Name == categoryInput.Name);

            if (!this.ModelState.IsValid || existsCategory)
            {
                var category = new CategoryEditViewModel()
                {
                    Id = id,
                    CategoryInput = this.categoriesService
                    .GetSingle<CategoryInputModel>(x => x.Id == id),
                };

                return this.View(category);
            }

            await this.categoriesService.EditAsync(id, categoryInput);

            return this.RedirectToAction("Index", "Categories", new { area = "Forum" });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            await this.categoriesService.DeleteAsync(x => x.Id == id);

            return this.RedirectToAction("Index", "Categories", new { area = "Forum" });
        }
    }
}