namespace AkhshamBazari.Extensions;

using System.Text;

public static class EnumerableExtensions
{
    public static string GetHtml<T>(this IEnumerable<T> products)
    {
        Type type = typeof(T);

        var props = type.GetProperties();

        StringBuilder sb = new StringBuilder(100);
        sb.Append("<ul>");

        foreach (var product in products)
        {
            foreach (var prop in props)
            {
                sb.Append($"<li><span>{prop.Name}: </span>{prop.GetValue(product)}</li>");
            }
            sb.Append("<br/>");
        }
        sb.Append("</ul>");

        return sb.ToString();
    }
}