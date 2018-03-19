using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace datathegenius.DatasEssentials
{
    class CommandResetPermissions : IRocketCommand
    {
        public List<string> Aliases
        {
            get
            {
                return new List<string>() {};
            }
        }

        public AllowedCaller AllowedCaller
        {
            get
            {
                return AllowedCaller.Both;
            }
        }

        public string Help
        {
            get
            {
                return "Reset permissions";
            }
        }

        public string Name
        {
            get
            {
                return "resetpermissions";
            }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "datasessentials.resetpermissions" };
            }
        }

        public string Syntax
        {
            get
            {
                return "<resetpermissions>";
            }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            try
            {
                if (command[0] == "*")
                {
                    foreach (SteamPlayer plr in Provider.Players)
                    {
                        //So let's convert each SteamPlayer into an UnturnedPlayer
                        UnturnedPlayer unturnedPlayer = UnturnedPlayer.FromSteamPlayer(plr);

                        //Add to group
                        for (int x = 0; x < DatasEssentialsManager.Instance.Configuration.Instance.permissionsAddOnConnect.Count(); x++)
                        {
                            Rocket.Core.R.Permissions.AddPlayerToGroup(DatasEssentialsManager.Instance.Configuration.Instance.permissionsAddOnConnect[x], unturnedPlayer);
                        }

                        //Remove to group
                        for (int x = 0; x < DatasEssentialsManager.Instance.Configuration.Instance.permissionsRemoveOnConnect.Count(); x++)
                        {
                            Rocket.Core.R.Permissions.RemovePlayerFromGroup(DatasEssentialsManager.Instance.Configuration.Instance.permissionsRemoveOnConnect[x], unturnedPlayer);
                        }

                        UnturnedChat.Say(caller, "All player permissions reset.", Color.green);
                    }
                }
                else
                {
                    string playerName = command[0];

                    //Find player
                    foreach (SteamPlayer plr in Provider.Players)
                    {
                        //So let's convert each SteamPlayer into an UnturnedPlayer
                        UnturnedPlayer unturnedPlayer = UnturnedPlayer.FromSteamPlayer(plr);

                        if(unturnedPlayer.DisplayName.ToLower().IndexOf(playerName.ToLower()) != -1 || unturnedPlayer.CharacterName.ToLower().IndexOf(playerName.ToLower()) != -1 || unturnedPlayer.SteamName.ToLower().IndexOf(playerName.ToLower()) != -1 || unturnedPlayer.CSteamID.ToString().Equals(playerName))
                        {
                            //Add to group
                            for (int x = 0; x < DatasEssentialsManager.Instance.Configuration.Instance.permissionsAddOnConnect.Count(); x++)
                            {
                                Rocket.Core.R.Permissions.AddPlayerToGroup(DatasEssentialsManager.Instance.Configuration.Instance.permissionsAddOnConnect[x], unturnedPlayer);
                            }

                            //Remove to group
                            for (int x = 0; x < DatasEssentialsManager.Instance.Configuration.Instance.permissionsRemoveOnConnect.Count(); x++)
                            {
                                Rocket.Core.R.Permissions.RemovePlayerFromGroup(DatasEssentialsManager.Instance.Configuration.Instance.permissionsRemoveOnConnect[x], unturnedPlayer);
                            }

                            UnturnedChat.Say(caller, unturnedPlayer.DisplayName + "'s permissions have been reset.", Color.green);
                            UnturnedChat.Say(unturnedPlayer, "Your permissions have been reset!", Color.green);
                            return;
                        }
                    }
                    UnturnedChat.Say(caller, "Did not find anyone with the name \"" + playerName + "\".", Color.red);
                }
            }
            catch
            {
                UnturnedChat.Say(caller, "You did not use that correctly. Syntax: /resetpermissions (name) or /resetpermissions *", Color.red);
            }
        }
    }
}
