

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
    public class CommandModeratorReport : IRocketCommand
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
                return "Lists all kicks/bans moderators executed.";
            }
        }

        public string Name
        {
            get
            {
                return "modreport";
            }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "datasessentials.moderatorreport" };
            }
        }

        public string Syntax
        {
            get
            {
                return "<modreport>";
            }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Count() == 1)
            {
                if(command[0] == "clear")
                {
                    DatasEssentialsManager.Instance.Configuration.Instance.modKickBanList = new List<string>();
                    UnturnedChat.Say(caller, "Cleared mod report list.", Color.cyan);
                    return;
                }
            }
            else if (command.Count() == 0)
            {
                for(int x = 0; x < DatasEssentialsManager.Instance.Configuration.Instance.modKickBanList.Count; x++)
                {
                    UnturnedChat.Say(caller, (x + 1) + ". " + DatasEssentialsManager.Instance.Configuration.Instance.modKickBanList[x], Color.green);
                }
                return;
            }
            else
            {
                UnturnedChat.Say(caller, "Error #1245");
            }
        }
    }
}
