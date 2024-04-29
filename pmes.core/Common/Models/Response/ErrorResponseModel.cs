namespace pmes.core.Common.Models.Response
{
    /// <summary>
    /// Error response
    /// </summary>
    public class ErrorResponseModel
    {
        public List<string> Messages { get; set; } = [];
        public List<ErrorDetailModel> Details { get; set; } = [];
        public string? Source { get; set; }
        public string? Exception { get; set; }
        public string? ErrorId { get; set; }
        public string? SupportMessage { get; set; }
        public int StatusCode { get; set; }
    }
    /// <summary>
    /// Details of invalid request property
    /// </summary>
    public class ErrorDetailModel
    {
        public string PropertyName { get; set; }
        public string Message { get; set; }
    }
}
