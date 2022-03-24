using System.Net;

namespace ALMS.Model
{
    public class OperationResult<TDataModel>
    {
        public OperationResult() { }
        public OperationResult(string message, bool noContent = false)
        {
            Message = message;
            NoContent = noContent;
        }
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public TDataModel DataModel { get; set; }
        public bool NoContent { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }
        public ResultType ResultType { get; set; }
    }
}