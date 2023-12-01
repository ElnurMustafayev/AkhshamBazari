namespace AkhshamBazari.Attributes.Base;

public abstract class HttpAttribute : Attribute
{
    public readonly HttpMethod MethodType;
    private readonly string? routing;
    public string? NormalizedRouting => routing?.Trim('/').ToLower();
    
    public HttpAttribute(HttpMethod methodType, string? routing)
    {
        this.routing = routing;
        this.MethodType = methodType;
    }
}