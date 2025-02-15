using System;
namespace CMPNatural.Core.Base
{
    public class PagesQueryResponse<T>
    {
        public List<T> elements { get; set; } = new List<T>();
        public int pageNumber { get; set; }
        public int totalPages { get; set; }
        public int totalElements { get; set; }

        public PagesQueryResponse(List<T> elements, int pageNumber, int totalPages, int totalElements)
        {
            this.elements = elements;
            this.pageNumber = pageNumber;
            this.totalPages = totalPages;
            this.totalElements = totalElements;
        }
    }

}
