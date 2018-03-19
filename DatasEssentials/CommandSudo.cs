using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace datathegenius.DatasEssentials
{
    class CommandSudo : IRocketCommand
    {
        public List<string> Aliases
        {
            get
            {
                return new List<string>();
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
                return "Makes player perform a command.";
            }
        }

        public string Name
        {
            get
            {
                return "sudo";
            }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "datasessentials.sudo" };
            }
        }

        public string Syntax
        {
            get
            {
                return "<sudo>";
            }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Count() == 2)
            {
                string playerName = command[0];
                string action = command[1];

                if (playerName.Equals("*"))
                {
                    foreach (SteamPlayer plr in Provider.Players)
                    {
                        //So let's convert each SteamPlayer into an UnturnedPlayer
                        UnturnedPlayer unturnedPlayer = UnturnedPlayer.FromSteamPlayer(plr);

                        ChatManager.Instance.askChat(unturnedPlayer.CSteamID, (byte)EChatMode.GLOBAL, action);
                        UnturnedChat.Say(unturnedPlayer, "You performed the action: " + action);
                    }
                    UnturnedChat.Say(caller, "Performed the action: " + action + " on all players.");
                    return;
                }

                //Find player
                foreach (SteamPlayer plr in Provider.Players)
                {
                    //So let's convert each SteamPlayer into an UnturnedPlayer
                    UnturnedPlayer unturnedPlayer = UnturnedPlayer.FromSteamPlayer(plr);

                    if (unturnedPlayer.DisplayName.ToLower().IndexOf(playerName.ToLower()) != -1 || unturnedPlayer.CharacterName.ToLower().IndexOf(playerName.ToLower()) != -1 || unturnedPlayer.SteamName.ToLower().IndexOf(playerName.ToLower()) != -1 || unturnedPlayer.CSteamID.ToString().Equals(playerName))
                    {
                        ChatManager.Instance.askChat(unturnedPlayer.CSteamID, (byte)EChatMode.GLOBAL, action);
                        UnturnedChat.Say(unturnedPlayer, "You performed the action: " + action);
                        UnturnedChat.Say(caller, unturnedPlayer.DisplayName + " performed the action: " + action);
                        return;
                    }
                }
                UnturnedChat.Say(caller, "Did not find anyone with the name \"" + playerName + "\".", Color.red);
            }
            else
            {
                UnturnedChat.Say(caller, "Used that wrong, syntax is /sudo (player) (action)", Color.red);
                return;
            }
        }
    }
}
