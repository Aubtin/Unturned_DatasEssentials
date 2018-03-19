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
    class CommandModerator : IRocketCommand
    {
        public static List<string> activeMods = new List<string>();

        public List<string> Aliases
        {
            get
            {
                return new List<string>() { "mod" };
            }
        }

        public AllowedCaller AllowedCaller
        {
            get
            {
                return AllowedCaller.Player;
            }
        }

        public string Help
        {
            get
            {
                return "Activates moderator commands.";
            }
        }

        public string Name
        {
            get
            {
                return "moderator";
            }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "datasessentials.moderator" };
            }
        }

        public string Syntax
        {
            get
            {
                return "<moderator>";
            }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            //Check if id is in mod list
            UnturnedPlayer pCaller = (UnturnedPlayer)caller;
            var modExists = DatasEssentialsManager.Instance.Configuration.Instance.modList.FirstOrDefault(item => item == pCaller.CSteamID.ToString());

            if(modExists != null)
            {
                bool isModActive = activeMods.Any(item => item.Equals(pCaller.CSteamID.ToString()));

                if (!isModActive)
                {
                    activeMods.Add(pCaller.CSteamID.ToString());
                    Rocket.Core.R.Permissions.AddPlayerToGroup("Moderator", pCaller);

                    if (DatasEssentialsManager.Instance.Configuration.Instance.announceModActive)
                        UnturnedChat.Say(pCaller.CharacterName + " has gone into moderator mode.", Color.cyan);

                    UnturnedChat.Say(caller, DatasEssentialsManager.Instance.Configuration.Instance.activeModMessage, Color.green);
                }
                else
                {
                    activeMods.Remove(pCaller.CSteamID.ToString());
                    Rocket.Core.R.Permissions.RemovePlayerFromGroup("Moderator", pCaller);

                    if (DatasEssentialsManager.Instance.Configuration.Instance.announceModActive)
                        UnturnedChat.Say(pCaller.CharacterName + " has left moderator mode.", Color.cyan);
                }
            }
            else
            {
                UnturnedChat.Say(caller, "You are not a moderator. You can apply on the forums.", Color.red);
            }
        }

        public static Boolean isMod(UnturnedPlayer player)
        {
            var modExists = activeMods.FirstOrDefault(item => item.Equals(player.CSteamID.ToString()));

            if(modExists != null)
            {
                return true;
            }
            return false;
        }
    }
}
