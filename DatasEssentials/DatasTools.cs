using Rocket.API;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace datathegenius.DatasEssentials
{
    class DatasTools
    {
        public static UnturnedPlayer findPlayer(IRocketPlayer caller, String userInput)
        {
            foreach (SteamPlayer plr in Provider.Players)
            {
                //So let's convert each SteamPlayer into an UnturnedPlayer
                UnturnedPlayer unturnedPlayer = UnturnedPlayer.FromSteamPlayer(plr);

                if (unturnedPlayer.DisplayName.ToLower().IndexOf(userInput.ToLower()) != -1 || unturnedPlayer.CharacterName.ToLower().IndexOf(userInput.ToLower()) != -1 || unturnedPlayer.SteamName.ToLower().IndexOf(userInput.ToLower()) != -1 || unturnedPlayer.CSteamID.ToString().Equals(userInput))
                {
                    return unturnedPlayer;
                }
            }
            UnturnedChat.Say(caller, "Did not find anyone with the name \"" + userInput + "\".", Color.red);
            return null;
        }

        public static void maxSkills(UnturnedPlayer tempPlayer, Boolean overpower)
        {
            var pSkills = tempPlayer.Player.skills;

            foreach (var skill in pSkills.skills.SelectMany(skArr => skArr))
            {
                skill.level = overpower ? byte.MaxValue : skill.max;
            }
            pSkills.askSkills(tempPlayer.CSteamID);
        }

        public static string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }
    }
}
