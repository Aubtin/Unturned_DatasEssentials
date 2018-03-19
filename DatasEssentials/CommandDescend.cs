

using System;
using System.Collections.Generic;
using Rocket.API;
using SDG.Unturned;
using Rocket.Unturned.Chat;
using UnityEngine;
using Rocket.Unturned.Player;
using System.Linq;

namespace datathegenius.DatasEssentials
{
    public class CommandDescend : IRocketCommand
    {
        public List<string> Aliases
        {
            get
            {
                return new List<string>() { "desc" };
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
                return "descends player x amount.";
            }
        }

        public string Name
        {
            get
            {
                return "descend";
            }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "datasessentials.descend" };
            }
        }

        public string Syntax
        {
            get
            {
                return "<descend>";
            }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            int descendAmount = 0;
            float rotation = 0;

            if (command.Count() == 1)
            {
                command[0] = command[0].ToLower();
                descendAmount = Convert.ToInt32(command[0]);
            }
            else
            {
                UnturnedChat.Say(caller, "Didn't use this correctly.", Color.red);
                return;
            }

            if(descendAmount < 0)
            {
                UnturnedChat.Say(caller, "Can't use negative numbers to descend!", Color.red);
                return;
            }

            Vector3 currentPosition = new Vector3();
            UnturnedPlayer pCaller = (UnturnedPlayer)caller;

            rotation = pCaller.Rotation;
            currentPosition = pCaller.Position;

            float playerX = currentPosition.x;
            float playerY = currentPosition.y;
            float playerZ = currentPosition.z;

            Vector3 newPosition = new Vector3(playerX, playerY - descendAmount, playerZ);
            pCaller.Teleport(newPosition, rotation);

            UnturnedChat.Say(caller, "Descended " + descendAmount + " meters.", Color.green);
        }
    }
}
