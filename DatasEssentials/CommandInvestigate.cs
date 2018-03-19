

using System;
using System.Collections.Generic;
using Rocket.API;
using SDG.Unturned;
using Rocket.Unturned.Chat;
using UnityEngine;
using Rocket.Unturned.Player;
using System.Linq;
using Rocket.Core.Logging;
using Steamworks;

namespace datathegenius.DatasEssentials
{
    public class CommandInvestigate : IRocketCommand
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
                return "Gives player's IP and Steam ID.";
            }
        }

        public string Name
        {
            get
            {
                return "investigate";
            }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "datasessentials.investigate" };
            }
        }

        public string Syntax
        {
            get
            {
                return "<investigate>";
            }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if(command.Count() == 1)
            {
                UnturnedPlayer player = DatasTools.findPlayer(caller, command[0]);

                P2PSessionState_t sessionState;
                SteamGameServerNetworking.GetP2PSessionState(player.CSteamID, out sessionState);

                string playerSteamID = player.CSteamID.ToString();
                string playerIP = Parser.getIPFromUInt32(sessionState.m_nRemoteIP);

                UnturnedChat.Say(caller, "Player: " + player.CharacterName + " Steam ID: " + playerSteamID + " IP: " + playerIP);
                return;
            }
            else
            {
                UnturnedChat.Say(caller, "Ugh... Used that wrong... Syntax: /investigate (player)", Color.red);
                return;
            }
        }
    }
}
