namespace BoulderBox.Web.ViewModels.Common
{
    public class CommonViewModel
    {
        public CommonViewModel()
        {
            this.Sorting = new SortingInputModel();
            this.Pagination = new PaginationViewModel(0, 0, 0);
        }

        public SortingInputModel Sorting { get; set; }

        public PaginationViewModel Pagination { get; set; }
    }
}
