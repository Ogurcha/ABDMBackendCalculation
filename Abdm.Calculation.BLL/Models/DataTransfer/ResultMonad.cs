namespace Abdm.Calculation.BLL.Models.DataTransfer
{
    public class ResultMonad<T> where T : class
    {
        public T? Result { get; set; }

        public bool IsSuccess => Exception == null;

        public Exception? Exception { get; protected set; }

        public ResultMonad(T data)
        {
            Result = data;
        }

        public ResultMonad(Exception exception)
        {
            Exception = exception;
        }

        public ResultMonad(T data, Exception exception)
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
