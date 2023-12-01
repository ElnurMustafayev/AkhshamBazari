using AkhshamBazari.Attributes.Base;

namespace AkhshamBazari.Attributes
{
    public class HttpDeleteAttribute : HttpAttribute
    {
        public HttpDeleteAttribute(string routing) : base(HttpMethod.Delete, routing) {}

        public HttpDeleteAttribute() : base(HttpMethod.Delete, null) {}
    }
}