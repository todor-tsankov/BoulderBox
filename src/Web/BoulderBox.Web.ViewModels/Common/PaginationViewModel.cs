using System;

namespace BoulderBox.Web.ViewModels.Common
{
    public class PaginationViewModel
    {
        private const int DefaultPageStep = 1;

        public PaginationViewModel(int firstPage, int lastPage, int currentPage)
        {
            this.FirstPage = firstPage;
            this.LastPage = lastPage;
            this.CurrentPage = currentPage;
        }

        public int FirstPage { get; set; }

        public int LastPage { get; set; }

        public int CurrentPage { get; set; }

        public int NextPage => this.CurrentPage + DefaultPageStep;

        public int PrevoiusPage => this.CurrentPage - DefaultPageStep;

        public bool HasNext => this.CurrentPage + DefaultPageStep <= this.LastPage;

        public bool HasPrevoius => this.CurrentPage - DefaultPageStep >= this.FirstPage;
    }
}
