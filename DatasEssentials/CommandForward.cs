

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
    public class CommandForward : IRocketCommand
    {
        public List<string> Aliases
        {
            get
            {
                return new List<string>() { "forw" };
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
                return "Moves player forward x amount.";
            }
        }

        public string Name
        {
            get
            {
                return "forward";
            }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "datasessentials.forward" };
            }
        }

        public string Syntax
        {
            get
            {
                return "<forward>";
            }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            int forwardAmount = 0;
            float rotation = 0;

            if (command.Count() == 1)
            {
                command[0] = command[0].ToLower();
                forwardAmount = Convert.ToInt32(command[0]);
            }
            else
            {
                UnturnedChat.Say(caller, "Didn't use this correctly.", Color.red);
                return;
            }

            if(forwardAmount < 0)
            {
                UnturnedChat.Say(caller, "Can't use negative numbers to move forward!", Color.red);
                return;
            }

            Vector3 currentPosition = new Vector3();
            UnturnedPlayer pCaller = (UnturnedPlayer)caller;

            rotation = pCaller.Rotation;
            currentPosition = pCaller.Position;

            float playerX = currentPosition.x;
            float playerY = currentPosition.y;
            float playerZ = currentPosition.z;

            Vector3 newPosition = new Vector3(playerX + forwardAmount, playerY, playerZ);
            pCaller.Teleport(newPosition, rotation);

            UnturnedChat.Say(caller, "Moved forward " + forwardAmount + " meters.", Color.green);
        }
    }
}
