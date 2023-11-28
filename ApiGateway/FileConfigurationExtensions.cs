using Ocelot.Configuration.File;

namespace ApiGateway
{
    public static class FileConfigurationExtensions
    {
        public static IServiceCollection ConfigureDownstreamHostAndPortsPlaceholders(
        this IServiceCollection services,
        IConfiguration configuration)
        {
            services.PostConfigure<FileConfiguration>(fileConfiguration =>
            {
                var globalHosts = configuration
                    .GetSection($"{nameof(FileConfiguration.GlobalConfiguration)}:Hosts")
                    .Get<Dictionary<string, Uri>>();

                var globalNames = configuration
                    .GetSection($"{nameof(FileConfiguration.GlobalConfiguration)}:Names")
                    .Get<Dictionary<string, string>>();

                foreach (var route in fileConfiguration.Routes)
                {
                    ConfigureRoute(route, globalHosts, globalNames);
                }
            });

            return services;
        }

        private static void ConfigureRoute(
            FileRoute route,
            Dictionary<string, Uri> globalHosts,
            Dictionary<string, string> globalNames)
        {
            foreach (var hostAndPort in route.DownstreamHostAndPorts)
            {
                var host = hostAndPort.Host;
                if (host.StartsWith("[") && host.EndsWith("]"))
                {
                    var placeHolder = host.TrimStart('[').TrimEnd(']');
                    if (globalHosts.TryGetValue(placeHolder, out var uri))
                    {
                        route.DownstreamScheme = uri.Scheme;
                        hostAndPort.Host = uri.Host;
                        hostAndPort.Port = uri.Port;
                    }
                }

                while (route.DownstreamPathTemplate.Any(c => c == '['))
                {
                    var firstIndex = route.DownstreamPathTemplate.IndexOf('[');
                    var length = route.DownstreamPathTemplate.IndexOf(']') - firstIndex;
                    var nameVariable = route.DownstreamPathTemplate.Substring(firstIndex, length + 1);
                    if (globalNames.TryGetValue(nameVariable.TrimStart('[').TrimEnd(']'), out var name))
                    {
                        route.DownstreamPathTemplate = route.DownstreamPathTemplate.Replace(nameVariable, name);
                    }
                }

                while (route.UpstreamPathTemplate.Any(c => c == '['))
                {
                    var firstIndex = route.UpstreamPathTemplate.IndexOf('[');
                    var length = route.UpstreamPathTemplate.IndexOf(']') - firstIndex;
                    var nameVariable = route.UpstreamPathTemplate.Substring(firstIndex, length + 1);
                    if (globalNames.TryGetValue(nameVariable.TrimStart('[').TrimEnd(']'), out var name))
                    {
                        route.UpstreamPathTemplate = route.UpstreamPathTemplate.Replace(nameVariable, name);
                    }
                }

            }
        }
    }
}
