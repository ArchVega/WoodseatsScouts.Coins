namespace WoodseatsScouts.Coins.Tests.Integration.Api.Core;

public class ApiResult<T>(T data, HttpResponseMessage response)
{
    public T Data { get; } = data;
    
    public HttpResponseMessage Response { get; } = response;
}