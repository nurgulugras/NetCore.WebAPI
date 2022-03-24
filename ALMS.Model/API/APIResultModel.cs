namespace ALMS.Model
{
    public class APIResultModel<TData>
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
        public TData Result { get; set; }
    }
}