

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
    public class CommandAscend : IRocketCommand
    {
        public List<string> Aliases
        {
            get
            {
                return new List<string>() { "asc" };
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
                return "Ascends player x amount.";
            }
        }

        public string Name
        {
            get
            {
                return "ascend";
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
                return "<ascend>";
            }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            int ascendAmount = 0;
            float rotation = 0;

            if (command.Count() == 1)
            {
                command[0] = command[0].ToLower();
                ascendAmount = Convert.ToInt32(command[0]);
            }
            else
            {
                UnturnedChat.Say(caller, "Didn't use this correctly.", Color.red);
                return;
            }

            if(ascendAmount < 0)
            {
                UnturnedChat.Say(caller, "Can't use negative numbers to ascend!", Color.red);
                return;
            }

            Vector3 currentPosition = new Vector3();
            UnturnedPlayer pCaller = (UnturnedPlayer)caller;

            rotation = pCaller.Rotation;
            currentPosition = pCaller.Position;

            float playerX = currentPosition.x;
            float playerY = currentPosition.y;
            float playerZ = currentPosition.z;

            Vector3 newPosition = new Vector3(playerX, playerY + ascendAmount, playerZ);
            pCaller.Teleport(newPosition, rotation);

            UnturnedChat.Say(caller, "Ascended " + ascendAmount + " meters.", Color.green);
        }
    }
}
