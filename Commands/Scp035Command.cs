using CommandSystem;
using LabApi.Features.Wrappers;
using PlayerRoles;
using System;

namespace Scp035.Commands
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    internal class Scp035Command : ICommand
    {
        public string Command => "scp035";

        public string[] Aliases => new string[] { "035" };

        public string Description => "Forces a player into SCP-035.";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission(PlayerPermissions.ForceclassWithoutRestrictions))
            {
                response = "You don't have permission to use this command.";
                return false;
            }
            if (arguments.Count < 1)
            {
                response = "Less arguments than expected! Correct usage: scp035 <player id>";
                return false;
            }
            if (!int.TryParse(arguments.At(0), out int id))
            {
                response = "Cannot parse number! Correct usage: scp035 <player id>";
                return false;
            }

            Player player = Player.Get(id);

            if (player == null)
            {
                response = "Player not found.";
                return false;
            }

            player.SetRole(RoleTypeId.Tutorial, flags: RoleSpawnFlags.None);
            player.GameObject.AddComponent<Scp035Component>();

            response = "Successfully executed.";
            return true;
        }
    }
}
