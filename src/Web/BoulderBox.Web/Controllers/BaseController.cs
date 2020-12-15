using BoulderBox.Services;
using BoulderBox.Web.ViewModels.Common;
using Microsoft.AspNetCore.Mvc;

namespace BoulderBox.Web.Controllers
{
    public class BaseController : Controller
    {
        protected const int DefaultFirstPage = 1;
        protected const int DefaultItemsPerPage = 12;

        protected PaginationViewModel GetPaginationModel(int currentPage, int count, int itemsPerPage = DefaultItemsPerPage)
        {
            var lastPage = count / itemsPerPage;

            if (count % itemsPerPage != 0)
            {
                lastPage++;
            }

            if (lastPage < DefaultFirstPage)
            {
                lastPage = DefaultFirstPage;
            }

            return new PaginationViewModel(DefaultFirstPage, lastPage, currentPage);
        }
    }
}
