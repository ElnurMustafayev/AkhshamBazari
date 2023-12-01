using AkhshamBazari.Attributes.Base;

namespace AkhshamBazari.Attributes
{
    public class HttpPostAttribute : HttpAttribute
    {
        public HttpPostAttribute(string routing) : base(HttpMethod.Post, routing) {}

        public HttpPostAttribute() : base(HttpMethod.Post, null) {}
    }
}