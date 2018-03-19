

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
    public class CommandBroadcast : IRocketCommand
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
                return "Broadcasts message to server.";
            }
        }

        public string Name
        {
            get
            {
                return "broadcast";
            }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "datasessentials.broadcast" };
            }
        }

        public string Syntax
        {
            get
            {
                return "<broadcast>";
            }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            string broadcastMessage = "";

            for (int x = 0; x < command.Count(); x++)
            {
                broadcastMessage += command[x] + " ";
            }

            UnturnedChat.Say(broadcastMessage, Color.cyan);
        }
    }
}
