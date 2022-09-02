using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BloggingApis.Models.DTO
{
    public abstract class QueryStringParameter
    {
        const int maxPazeSize = 100;
        private int _pageSize = 10;

        public int PageNo { get; set; } = 1;
        public string Term { get; set; }

        public int PageSize
        {
            get
            {
                return _pageSize;
            }
            set
            {
                _pageSize = (value > maxPazeSize) ? maxPazeSize : value;
            }
        }
            public string OrderBy { get; set; }
    }
}
