using System.Threading.Tasks;
using System.Web;

using BoulderBox.Common;
using BoulderBox.Services;
using BoulderBox.Services.Data.Forum;
using BoulderBox.Web.Areas.Administration.Controllers.Common;
using BoulderBox.Web.ViewModels.Forum.Categories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BoulderBox.Web.Areas.Administration.Controllers
{
    public class CategoriesController : AdministrationController
    {
        private const string MatchingCategoryKey = "MatchingCategory";
        private const string MatchingCategoryErrorMessage = "Category with that name already exists.";

        private readonly ICategoriesService categoriesService;
        private readonly ICloudinaryService cloudinaryService;

        public CategoriesController(ICategoriesService categoriesService, ICloudinaryService cloudinaryService)
        {
            this.categoriesService = categoriesService;
            this.cloudinaryService = cloudinaryService;
        }

        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CategoryInputModel categoryInput)
        {
            var existsMatchingCategory = this.categoriesService
                .Exists(x => x.Name == categoryInput.Name);

            if (!this.ModelState.IsValid || existsMatchingCategory)
            {
                if (existsMatchingCategory)
                {
                    this.ModelState.AddModelError(MatchingCategoryKey, MatchingCategoryErrorMessage);
                }

                return this.View(categoryInput);
            }

            var image = await this.cloudinaryService.SaveImageAsync(categoryInput.FormFile);
            await this.categoriesService.AddAsync(categoryInput, image);

            var categoryNameEncoded = HttpUtility.HtmlEncode(categoryInput.Name);
            this.TempData[GlobalConstants.MessageKey] = $"Successfully created category <strong>{categoryNameEncoded}</strong>";

            return this.RedirectToAction("Index", "Categories", new { area = "Forum" });
        }

        public IActionResult Edit(string id)
        {
            var existsCategory = this.categoriesService
                .Exists(x => x.Id == id);

            if (!existsCategory)
            {
                return this.NotFound();
            }

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
                .Exists(x => x.Id == id);

            if (!existsCategory)
            {
                return this.NotFound();
            }

            var existsMatchingCategory = this.categoriesService
                .Exists(x => x.Id != id && x.Name == categoryInput.Name);

            if (!this.ModelState.IsValid || existsMatchingCategory)
            {
                if (existsMatchingCategory)
                {
                    this.ModelState.AddModelError(MatchingCategoryKey, MatchingCategoryErrorMessage);
                }

                var category = new CategoryEditViewModel()
                {
                    Id = id,
                    CategoryInput = categoryInput,
                };

                return this.View(category);
            }

            var image = await this.cloudinaryService.SaveImageAsync(categoryInput.FormFile);
            await this.categoriesService.EditAsync(id, categoryInput, image);

            this.TempData[GlobalConstants.MessageKey] = $"Successfully edited category!";

            return this.RedirectToAction("Details", "Categories", new { area = "Forum", id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var existsCategory = this.categoriesService
                .Exists(x => x.Id == id);

            if (!existsCategory)
            {
                return this.NotFound();
            }

            await this.categoriesService.DeleteAsync(x => x.Id == id);
            this.TempData[GlobalConstants.MessageKey] = $"Successfully deleted category!";

            return this.RedirectToAction("Index", "Categories", new { area = "Forum" });
        }
    }
}
