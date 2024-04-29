using System;
using System.Collections.Generic;
using System.Text;

namespace pmes.core.Common.Models.Response
{
    public class PagedResponse<T> : GenericResponse<T>
    {
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }
        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;
        public string OrderBy { get; private set; }
        public string Query { get; private set; }
        public dynamic ScoreCards { get; private set; }

        public PagedResponse(T data, string message, int statusCode, int pageNumber, int pageSize, int totalCount, string orderBy = null, string query = null, dynamic scoreCards = null) : base(data, message, statusCode)
        {
            CurrentPage = pageNumber;
            PageSize = pageSize;
            TotalCount = totalCount;
            OrderBy = orderBy;
            Query = query;
            TotalPages = (int)Math.Ceiling(TotalCount / (double)pageSize);
            ScoreCards = scoreCards;
        }
    }
}
