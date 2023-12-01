using AkhshamBazari.Attributes.Base;

namespace AkhshamBazari.Attributes
{
    public class HttpGetAttribute : HttpAttribute
    {
        public HttpGetAttribute(string routing) : base(HttpMethod.Get, routing) {}

        public HttpGetAttribute() : base(HttpMethod.Get, null) {}
    }
}