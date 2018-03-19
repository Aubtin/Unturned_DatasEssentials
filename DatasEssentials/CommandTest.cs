

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
    public class CommandTest : IRocketCommand
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
                return "test";
            }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "datasessentials.test" };
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
            UnturnedPlayer player = (UnturnedPlayer)caller;
            List<ItemJar> playerItemsPage2 = new List<ItemJar>();

            var playerInventory = player.Inventory;

            var count = playerInventory.getItemCount(2);

            for (byte index = 0; index < count; index++)
            {
                Logger.Log("Index: " + index);
                playerItemsPage2.Add(playerInventory.getItem(2, index));
                //             playerInventory.removeItem(page, 0);
            }

            

            for(int index = 0; index < playerItemsPage2.Count(); index++)
            {
                Boolean worked = player.Inventory.tryAddItem(playerItemsPage2[index].item, playerItemsPage2[index].PositionX, playerItemsPage2[index].PositionY, 3, playerItemsPage2[index].Rotation);
                Logger.Log("Worked: " + worked);
            }


            /*
            List<ItemJar> playerItems = new List<ItemJar>();

            for (byte page = 0; page < 8; page++)
            {
                var count = playerInventory.getItemCount(page);

                for (byte index = 0; index < count; index++)
                {
                    playerItems.Add(playerInventory.getItem(page, index));
                    //             playerInventory.removeItem(page, 0);
                }
            }

            for (int x = 0; x < playerItems.Count(); x++)
            {
                player.GiveItem(playerItems[x].item);
            }
            */
        }
    }
}
