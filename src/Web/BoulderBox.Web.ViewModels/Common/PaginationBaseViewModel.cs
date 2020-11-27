using System;

namespace BoulderBox.Web.ViewModels.Common
{
    public class PaginationBaseViewModel
    {
        public int CurrentPage { get; set; }

        public int ItemsCount { get; set; }

        public int ItemsPerPage { get; set; }

        public int PagesCount => GetPagesCount();

        public bool HasPrevoius => this.CurrentPage > 1;

        public bool HasNext => this.CurrentPage < this.PagesCount;

        public int PrevoiusPage => this.CurrentPage - 1;

        public int NextPage => this.CurrentPage + 1;

        private int GetPagesCount()
        {
            var pagesCount = (int)Math.Ceiling((double)this.ItemsCount / this.ItemsPerPage);

            if (pagesCount < 1)
            {
                return 1;
            }

            return pagesCount;
        }
    }
}
