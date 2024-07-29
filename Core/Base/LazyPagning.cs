using System.Collections;

namespace ScoutDirect.Core.Base
{
    public class LazyPagning<T> : IPagedList<T>, IEnumerable<T>, IEnumerable
    {
        public const int DefaultPageSize = 10;
        private IList<T> _results;
        private int _totalItems;

        public int PageSize { get; private set; }
        public IEnumerable<T> Source { get; private set; }
        public int PageNumber { get; private set; }

        public LazyPagning(IEnumerable<T> source, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            Source = source;
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            TryExecuteQuery();

            foreach (var item in _results)
            {
                yield return item;
            }
        }
        protected void TryExecuteQuery()
        {
            if (_results != null)
                return;

            _totalItems = Source.Count();
            _results = ExecuteQuery();
        }
        protected virtual IList<T> ExecuteQuery()
        {
            if (PageSize == int.MaxValue) //There is no paging
                return Source.ToList<T>();

            int numberToSkip = (PageNumber - 1) * PageSize;
            return Source.Skip<T>(numberToSkip).Take<T>(PageSize).ToList<T>();
        }
        public IEnumerator GetEnumerator()
        {
            return ((IEnumerable<T>)this).GetEnumerator();
        }
        public int TotalItems
        {
            get
            {
                TryExecuteQuery();
                return _totalItems;
            }
        }
        public int TotalPages
        {
            get { return (int)Math.Ceiling(((double)TotalItems) / PageSize); }
        }
        public int FirstItem
        {
            get
            {
                TryExecuteQuery();
                return ((PageNumber - 1) * PageSize) + 1;
            }
        }
        public int LastItem
        {
            get
            {
                return (FirstItem + _results.Count) - 1;
            }
        }
        public bool HasPreviousPage
        {
            get { return PageNumber > 1; }
        }
        public bool HasNextPage
        {
            get { return PageNumber < TotalPages; }
        }
    }
}
