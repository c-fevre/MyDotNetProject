using System.Net.Http.Headers;
using System.Web.Http;
using MyContractsGenerator.Common.WebAPI;

namespace MyContractsGenerator.WebAPI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // TODO : Si la WebAPI ne doit être disponible que sur HTTPS :
            //config.Filters.Add(new RequireHttpsAttribute());
            config.MessageHandlers.Add(new ApiAuthServerHandler());

            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
        }
    }
}