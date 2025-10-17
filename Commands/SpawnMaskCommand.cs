using CommandSystem;
using LabApi.Features.Wrappers;
using Scp035;
using System;

namespace Sco035.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class SpawnMaskCommand : ICommand
    {
        public string Command => "Scp035Item";

        public string[] Aliases => new string[] { "035Item" };

        public string Description => "Spawns a SCP-035 mask item.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (sender == null) // sender is server console
            {
                response = "You must be on the server to use this command!";
                return false;
            }

            if (!sender.CheckPermission(PlayerPermissions.GivingItems))
            {
                response = "You don't have permission to use this command.";
                return false;
            }

            Player player = Player.Get(sender);

            if (player == null)
            {
                response = "This command can be only executed on the server.";
                return false;
            }

            Plugin.CreateScp035Item(player.Position);

            response = "Successfully executed.";
            return true;
        }
    }
}
