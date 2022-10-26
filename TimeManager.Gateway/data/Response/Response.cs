namespace TimeManager.Gateway.Data.Response
{
    public class Response<T> : IResponse<T>
    {
        public T Data { get; set; }
        public ApiException Exception { get; set; }

        public Response() {}
        public Response(T data) => Data = data;
        public Response(Exception ex) => Exception = new ApiException(ex);
    }
}
