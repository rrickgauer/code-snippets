
public class HttpResponseException : Exception
{
    public HttpResponseException(HttpStatusCode statusCode, object? value = null) => (StatusCode, Value) = ((int)statusCode, value);

    public int StatusCode { get; }

    public object? Value { get; }
}



public class NotFoundHttpResponseException : HttpResponseException
{
    public NotFoundHttpResponseException() : base(HttpStatusCode.NotFound, null)
    {

    }
}


public class ForbiddenHttpResponseException : HttpResponseException
{
    public ForbiddenHttpResponseException() : base(HttpStatusCode.Forbidden, null)
    {

    }
}
