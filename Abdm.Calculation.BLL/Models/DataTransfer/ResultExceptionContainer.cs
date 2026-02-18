namespace Abdm.Calculation.BLL.Models.DataTransfer
{
    public class ResultExceptionContainer<T> where T : class
    {
        public T? Result { get; set; }

        public bool IsSuccess => Exception == null;

        public Exception? Exception { get; protected set; }

        public ResultExceptionContainer(T data)
        {
            Result = data;
        }

        public ResultExceptionContainer(Exception exception)
        {
            Exception = exception;
        }

        public ResultExceptionContainer(T data, Exception exception)
        {
            Result = data;
            Exception = exception;
        }

        public void AddException(Exception ex)
        {
            if (Exception == null)
            {
                Exception = ex;
            }
            else if (Exception is AggregateException aggregateEx)
            {
                Exception = new AggregateException(aggregateEx.InnerExceptions.Append(ex));
            }
            else
            {
                Exception = new AggregateException(new List<Exception> { Exception, ex });
            }
        }
    }
}
