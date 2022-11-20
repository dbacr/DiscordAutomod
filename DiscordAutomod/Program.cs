
using DiscordAutomod;
using DSharpPlus;
using DSharpPlus.Entities;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

var logger = DiscordLogger.CreateLogger("Discord");

if (!File.Exists("config.json"))
{
    File.WriteAllText("config.json", JsonConvert.SerializeObject(new Configuration(), Formatting.Indented));
    logger.LogWarning("Configuration file was not found, a new one has been created. Please fill it out and restart the bot.");
    logger.LogInformation($"Path: {Path.GetFullPath("config.json")}");
    Console.ReadKey();
    return;
}

var config = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText("config.json"));
if (config is null)
{ 
    File.WriteAllText("config.json", JsonConvert.SerializeObject(new Configuration(), Formatting.Indented));
    logger.LogWarning("Configuration file is invalid, a new one has been created. Please fill it out and restart the bot.");
    logger.LogInformation($"Path: {Path.GetFullPath("config.json")}");
    Console.ReadKey();
    return;
}

if (string.IsNullOrEmpty(config.Token))
{
    logger.LogError("Discord bot token is not set in the configuration file.");
    Console.ReadKey();
    return;
}

if (config.RoleId == 0)
{
    logger.LogError("Please set a correct role id in the configuration file.");
    Console.ReadKey();
    return;
}

logger.LogInformation("Starting Discord bot...");
var discord = new DiscordClient(new DiscordConfiguration
{
    Token = config.Token,
    TokenType = TokenType.Bot,
    Intents = DiscordIntents.AllUnprivileged
});

discord.GuildMemberAdded += async (_, e) =>
{
    if (!e.Guild.Roles.ContainsKey(config.RoleId))
    {
        if(config.IsDebug)
            logger.LogWarning($"Role with Id: {config.RoleId} does not exist in the guild {e.Guild.Name}.");
        return;
    }

    await e.Member.GrantRoleAsync(e.Guild.Roles[config.RoleId]);
    
    if(config.IsDebug)
        logger.LogInformation($"Role: {e.Guild.Roles[config.RoleId].Name} was granted to {e.Member.Username}#{e.Member.Discriminator}.");
};

await discord.ConnectAsync(new DiscordActivity("New Users", ActivityType.Watching), UserStatus.Online);