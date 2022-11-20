using System.Reflection;
using Microsoft.Extensions.Logging;

namespace DiscordAutomod;

public class DiscordLogger 
{
    /// <summary>
    /// Create a new logger to log discord events
    /// </summary>
    /// <returns></returns>
    public static ILogger CreateLogger(string? name = null)
      => LoggerFactory.Create( builder => 
          builder.SetMinimumLevel(LogLevel.Trace)
                 .AddConsole())
        .CreateLogger(name ?? AppDomain.CurrentDomain.FriendlyName);
}