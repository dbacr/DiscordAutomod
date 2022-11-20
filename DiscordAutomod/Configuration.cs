namespace DiscordAutomod;

public class Configuration
{
    /// <summary>
    /// Bot token used to start the bot.
    /// </summary>
    public string? Token { get; set; }
    /// <summary>
    /// Role Id to assign to a member
    /// </summary>
    public ulong RoleId { get; set; }
    /// <summary>
    /// Use this to show informations in the console
    /// </summary>
    public bool IsDebug { get; set; }

}