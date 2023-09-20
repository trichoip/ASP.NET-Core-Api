using Microsoft.AspNetCore.Mvc.Routing;

namespace AspNetCore.Api.Filters
{
    public class MyApiControllerAttribute : Attribute, IRouteTemplateProvider
    {
        public string Template => "api/[controller]";
        public int? Order => 2;
        public string Name { get; set; } = string.Empty;
    }
}
