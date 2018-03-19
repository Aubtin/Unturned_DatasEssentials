

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
    public class CommandKill : IRocketCommand
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
                return "Kills player.";
            }
        }

        public string Name
        {
            get
            {
                return "kill";
            }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "datasessentials.ascend" };
            }
        }

        public string Syntax
        {
            get
            {
                return "<player>";
            }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Count() == 1)
            {
                string playerName = command[0];

                //Find player
                foreach (SteamPlayer plr in Provider.Players)
                {
                    //So let's convert each SteamPlayer into an UnturnedPlayer
                    UnturnedPlayer unturnedPlayer = UnturnedPlayer.FromSteamPlayer(plr);

                    if (unturnedPlayer.DisplayName.ToLower().IndexOf(playerName.ToLower()) != -1 || unturnedPlayer.CharacterName.ToLower().IndexOf(playerName.ToLower()) != -1 || unturnedPlayer.SteamName.ToLower().IndexOf(playerName.ToLower()) != -1 || unturnedPlayer.CSteamID.ToString().Equals(playerName))
                    {
                        unturnedPlayer.Damage(200, unturnedPlayer.Position, EDeathCause.KILL, ELimb.LEFT_BACK, unturnedPlayer.CSteamID);
                        UnturnedChat.Say(caller, "Killed " + unturnedPlayer.DisplayName + ".", Color.green);
                        return;
                    }
                }
                UnturnedChat.Say(caller, "Player not found.", Color.red);
            }
            else
            {
                UnturnedChat.Say(caller, "Error.", Color.red);
                return;
            }
        }
    }
}
