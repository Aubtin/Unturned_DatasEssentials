using Rocket.API;
using Rocket.Core.Logging;
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
    class CommandClearInventory : IRocketCommand
    {
        public List<string> Aliases
        {
            get
            {
                return new List<string>() { "ci" };
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
                return "Clears inventory /ci <player> ";
            }
        }

        public string Name
        {
            get
            {
                return "clearinventory";
            }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "datasessentials.clearinventory" };
            }
        }

        public string Syntax
        {
            get
            {
                return "<clearinventory>";
            }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            try
            {
                if (command[0] == "*")
                {
                    foreach (SteamPlayer plr in Provider.Players)
                    {
                        //So let's convert each SteamPlayer into an UnturnedPlayer
                        UnturnedPlayer tempPlayer = UnturnedPlayer.FromSteamPlayer(plr);

                        var playerInventory = tempPlayer.Inventory;

                        // "Remove "models" of items from player "body""
                        tempPlayer.Player.channel.send("tellSlot", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, (byte)0, (byte)0, new byte[0]);
                        tempPlayer.Player.channel.send("tellSlot", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, (byte)1, (byte)0, new byte[0]);

                        // Remove items
                        for (byte page = 0; page < 8; page++)
                        {
                            var count = playerInventory.getItemCount(page);

                            for (byte index = 0; index < count; index++)
                            {
                                playerInventory.removeItem(page, 0);
                            }
                        }

                        // Remove clothes

                        // Remove unequipped cloths
                        System.Action removeUnequipped = () =>
                        {
                            for (byte i = 0; i < playerInventory.getItemCount(2); i++)
                            {
                                playerInventory.removeItem(2, 0);
                            }
                        };

                        // Unequip & remove from inventory
                        tempPlayer.Player.clothing.askWearBackpack(0, 0, new byte[0], true);
                        removeUnequipped();

                        tempPlayer.Player.clothing.askWearGlasses(0, 0, new byte[0], true);
                        removeUnequipped();

                        tempPlayer.Player.clothing.askWearHat(0, 0, new byte[0], true);
                        removeUnequipped();

                        tempPlayer.Player.clothing.askWearPants(0, 0, new byte[0], true);
                        removeUnequipped();

                        tempPlayer.Player.clothing.askWearMask(0, 0, new byte[0], true);
                        removeUnequipped();

                        tempPlayer.Player.clothing.askWearShirt(0, 0, new byte[0], true);
                        removeUnequipped();

                        tempPlayer.Player.clothing.askWearVest(0, 0, new byte[0], true);
                        removeUnequipped();

                        UnturnedChat.Say(tempPlayer, "Your inventory was cleared.", Color.cyan);
                    }
                    UnturnedChat.Say(caller, "Cleared all player's inventory.", Color.cyan);
                }
                else
                {
                    string playerName = command[0];

                    //Find player
                    foreach (SteamPlayer plr in Provider.Players)
                    {
                        //So let's convert each SteamPlayer into an UnturnedPlayer
                        UnturnedPlayer tempPlayer = UnturnedPlayer.FromSteamPlayer(plr);

                        if(tempPlayer.DisplayName.ToLower().IndexOf(playerName.ToLower()) != -1 || tempPlayer.CharacterName.ToLower().IndexOf(playerName.ToLower()) != -1 || tempPlayer.SteamName.ToLower().IndexOf(playerName.ToLower()) != -1 || tempPlayer.CSteamID.ToString().Equals(playerName))
                        {

                            var playerInventory = tempPlayer.Inventory;

                            // "Remove "models" of items from player "body""
                            tempPlayer.Player.channel.send("tellSlot", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, (byte)0, (byte)0, new byte[0]);
                            tempPlayer.Player.channel.send("tellSlot", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, (byte)1, (byte)0, new byte[0]);

                            // Remove items
                            for (byte page = 0; page < 8; page++)
                            {
                                var count = playerInventory.getItemCount(page);

                                for (byte index = 0; index < count; index++)
                                {
                                    playerInventory.removeItem(page, 0);
                                }
                            }

                            // Remove clothes

                            // Remove unequipped cloths
                            System.Action removeUnequipped = () =>
                            {
                                for (byte i = 0; i < playerInventory.getItemCount(2); i++)
                                {
                                    playerInventory.removeItem(2, 0);
                                }
                            };

                            // Unequip & remove from inventory
                            tempPlayer.Player.clothing.askWearBackpack(0, 0, new byte[0], true);
                            removeUnequipped();

                            tempPlayer.Player.clothing.askWearGlasses(0, 0, new byte[0], true);
                            removeUnequipped();

                            tempPlayer.Player.clothing.askWearHat(0, 0, new byte[0], true);
                            removeUnequipped();

                            tempPlayer.Player.clothing.askWearPants(0, 0, new byte[0], true);
                            removeUnequipped();

                            tempPlayer.Player.clothing.askWearMask(0, 0, new byte[0], true);
                            removeUnequipped();

                            tempPlayer.Player.clothing.askWearShirt(0, 0, new byte[0], true);
                            removeUnequipped();

                            tempPlayer.Player.clothing.askWearVest(0, 0, new byte[0], true);
                            removeUnequipped();

                            UnturnedChat.Say(caller, "Cleared " + tempPlayer.CharacterName + "'s inventory.", Color.cyan);
                            UnturnedChat.Say(tempPlayer, "Your inventory was cleared.", Color.cyan);
                            return;
                        }
                    }
                    UnturnedChat.Say(caller, "Did not find anyone with the name \"" + playerName + "\".", Color.red);
                }
            }
            catch
            {
                UnturnedChat.Say(caller, "You did not use that correctly. Syntax: /ci <player>", Color.red);
            }
            

        }
    }
}
