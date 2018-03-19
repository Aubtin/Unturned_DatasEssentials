

using System;
using System.Collections.Generic;
using Rocket.API;
using SDG.Unturned;
using Rocket.Unturned.Chat;
using UnityEngine;
using Rocket.Unturned.Player;
using System.Linq;
using Rocket.Core.Logging;

namespace datathegenius.DatasEssentials
{
    public class CommandPing : IRocketCommand
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
                return "Checks ping.";
            }
        }

        public string Name
        {
            get
            {
                return "ping";
            }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "datasessentials.ping" };
            }
        }

        public string Syntax
        {
            get
            {
                return "<ping>";
            }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Count() == 1)
            {
                string playerName = command[0];

                foreach (SteamPlayer plr in Provider.Players)
                {
                    //So let's convert each SteamPlayer into an UnturnedPlayer
                    UnturnedPlayer unturnedPlayer = UnturnedPlayer.FromSteamPlayer(plr);

                    if (unturnedPlayer.DisplayName.ToLower().IndexOf(playerName.ToLower()) != -1 || unturnedPlayer.CharacterName.ToLower().IndexOf(playerName.ToLower()) != -1 || unturnedPlayer.SteamName.ToLower().IndexOf(playerName.ToLower()) != -1 || unturnedPlayer.CSteamID.ToString().Equals(playerName))
                    {
                        UnturnedChat.Say(caller, "The ping of " + unturnedPlayer.DisplayName + " is " + (unturnedPlayer.Ping * 1000) + " ms.", Color.green);
                        return;
                    }
                }
            }
            else if (command.Count() == 0)
            {
                UnturnedPlayer tempPlayer = (UnturnedPlayer)caller;

                UnturnedChat.Say(caller, "Your ping is " + (tempPlayer.Ping * 1000) + " ms.", Color.green);
                return;
            }
            else
            {
                UnturnedChat.Say(caller, "Error, used this wrong. Syntax: /ping (player)");
            }
        }
    }
}
