using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace TripPoint.WindowsPhone.Utils
{
    public class CollectionPaginator<T>
    {
        private IEnumerable<T> _dataset;
        private ReadOnlyCollection<T> _readOnlyDataset;
        
        public CollectionPaginator(IEnumerable<T> dataset)
        {
            if (dataset == null) throw new ArgumentException("dataset");

            _dataset = dataset;
            _readOnlyDataset = new ReadOnlyCollection<T>(new List<T>(dataset));
        }

        public IEnumerable<T> Dataset
        {
            get { return _readOnlyDataset; }
        }

        public int PageSize { get; set; }

        public int CurrentPage { get; private set; }

        public IEnumerable<T> Paginate()
        {
            if (PageSize == 0) throw new InvalidOperationException("PageSize must be greater than zero");
            if (!CanPaginate) return new List<T>();

            var items = GetItemsFromCurrentPage();
            
            CurrentPage++;

            return items;
        }

        public bool CanPaginate
        {
            get
            {
                return _dataset.Count() > GetPaginatedItemsCount();
            }
        }

        private int GetPaginatedItemsCount()
        {
            return CurrentPage * PageSize;
        }

        private IEnumerable<T> GetItemsFromCurrentPage()
        {
            return _dataset.Skip(GetPaginatedItemsCount()).Take(PageSize);
        }
    }
}
