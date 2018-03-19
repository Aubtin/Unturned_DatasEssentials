

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
    public class CommandExp : IRocketCommand
    {
        public List<string> Aliases
        {
            get
            {
                return new List<string>() { "exp" };
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
                return "Gives experience points to player.";
            }
        }

        public string Name
        {
            get
            {
                return "experience";
            }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "datasessentials.experience" };
            }
        }

        public string Syntax
        {
            get
            {
                return "<experience>";
            }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Count() == 2)
            {
                string playerName = command[0];
                uint additionalExperience = (uint)Convert.ToInt32(command[1]);

                if (playerName == "*")
                {
                    //Find player
                    foreach (SteamPlayer plr in Provider.Players)
                    {
                        //So let's convert each SteamPlayer into an UnturnedPlayer
                        UnturnedPlayer unturnedPlayer = UnturnedPlayer.FromSteamPlayer(plr);

                        unturnedPlayer.Experience += additionalExperience;
                        UnturnedChat.Say(unturnedPlayer, "You received " + additionalExperience + " experience.", Color.cyan);
                    }
                    UnturnedChat.Say(caller, "Gave all players " + additionalExperience + " experience");
                    return;
                }
                else
                {
                    //Find player
                    foreach (SteamPlayer plr in Provider.Players)
                    {
                        //So let's convert each SteamPlayer into an UnturnedPlayer
                        UnturnedPlayer unturnedPlayer = UnturnedPlayer.FromSteamPlayer(plr);

                        if (unturnedPlayer.DisplayName.ToLower().IndexOf(playerName.ToLower()) != -1 || unturnedPlayer.CharacterName.ToLower().IndexOf(playerName.ToLower()) != -1 || unturnedPlayer.SteamName.ToLower().IndexOf(playerName.ToLower()) != -1 || unturnedPlayer.CSteamID.ToString().Equals(playerName))
                        {
                            UnturnedChat.Say(caller, "Gave " + unturnedPlayer.DisplayName + " " + additionalExperience + " experience");

                            unturnedPlayer.Experience += additionalExperience;
                            UnturnedChat.Say(unturnedPlayer, "You received " + additionalExperience + " experience.", Color.cyan);
                            return;
                        }
                    }
                }
            }
            else if(command.Count() == 1)
            {
                uint additionalExperience = (uint)Convert.ToInt32(command[0]);

                UnturnedPlayer tempPlayer = (UnturnedPlayer)caller;

                tempPlayer.Experience += additionalExperience;

                UnturnedChat.Say(caller, "You received " + additionalExperience + " experience.", Color.cyan);
                return;
            }
            else
            {
                UnturnedChat.Say(caller, "Error, used this wrong. Syntax: /exp (player) (amount) or /exp (amount)");
            }
        }
    }
}
