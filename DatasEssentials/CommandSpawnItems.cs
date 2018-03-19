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
    class CommandSpawnItems : IRocketCommand
    {
        public List<string> Aliases
        {
            get
            {
                return new List<string>() { "giveitem", "gi", "item", "i" };
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
                return "Spawns items /spawnitems <player> <item> <amount>";
            }
        }

        public string Name
        {
            get
            {
                return "spawnitems";
            }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "datasessentials.spawnitems" };
            }
        }

        public string Syntax
        {
            get
            {
                return "<spawnitems>";
            }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            try
            {
                //Spawns item for caller.
                if(command.Count() == 1)
                {
                    string itemID = command[0];

                    var itemAsset = ItemUtil.GetItem(itemID);

                    if (itemAsset.IsAbsent)
                    {
                        UnturnedChat.Say(caller, "That item does not exist", Color.red);
                        return;
                    }

                    var item = new Item(itemAsset.Value.id, true);

                    if (itemAsset.IsAbsent)
                    {
                        UnturnedChat.Say(caller, "That item does not exist", Color.red);
                        return;
                    }

                    UnturnedPlayer unturnedPlayer = (UnturnedPlayer)caller;

                    unturnedPlayer.GiveItem(item.id, 1);
                    UnturnedChat.Say(unturnedPlayer, "You have received a " + itemAsset.Value.itemName + ".", Color.cyan);
                    return;
                }

                //Spawns multiple items for caller or one item for another player.
                if(command.Count() == 2)
                {
                    byte itemAmount = 1;

                    //Multiple items
                    if (byte.TryParse(command[1], out itemAmount))
                    {
                        string itemID = command[0];
                        var itemAsset = ItemUtil.GetItem(itemID);

                        if (itemAsset.IsAbsent)
                        {
                            UnturnedChat.Say(caller, "That item does not exist", Color.red);
                            return;
                        }

                        var item = new Item(itemAsset.Value.id, true);

                        UnturnedPlayer unturnedPlayer = (UnturnedPlayer)caller;

                        unturnedPlayer.GiveItem(item.id, itemAmount);
                        UnturnedChat.Say(unturnedPlayer, "You have received " + itemAmount + " of " + itemAsset.Value.itemName + ".", Color.cyan);
                    }
                    else
                    {
                        string playerName = command[0];
                        string itemID = command[1];

                        if (playerName.Equals("*"))
                        {
                            var itemAsset = ItemUtil.GetItem(itemID);

                            if (itemAsset.IsAbsent)
                            {
                                UnturnedChat.Say(caller, "That item does not exist", Color.red);
                                return;
                            }

                            var item = new Item(itemAsset.Value.id, true);

                            UnturnedChat.Say(caller, "Given all players a " + itemAsset.Value.itemName + ".", Color.cyan);

                            foreach (SteamPlayer plr in Provider.Players)
                            {
                                //So let's convert each SteamPlayer into an UnturnedPlayer
                                UnturnedPlayer unturnedPlayer = UnturnedPlayer.FromSteamPlayer(plr);

                                unturnedPlayer.GiveItem(item.id, 1);
                                UnturnedChat.Say(unturnedPlayer, "You have received a " + itemAsset.Value.itemName + ".", Color.cyan);
                            }
                        }
                        else
                        {
                            foreach (SteamPlayer plr in Provider.Players)
                            {
                                //So let's convert each SteamPlayer into an UnturnedPlayer
                                UnturnedPlayer unturnedPlayer = UnturnedPlayer.FromSteamPlayer(plr);

                                if (unturnedPlayer.DisplayName.ToLower().IndexOf(playerName.ToLower()) != -1 || unturnedPlayer.CharacterName.ToLower().IndexOf(playerName.ToLower()) != -1 || unturnedPlayer.SteamName.ToLower().IndexOf(playerName.ToLower()) != -1 || unturnedPlayer.CSteamID.ToString().Equals(playerName))
                                {
                                    var itemAsset = ItemUtil.GetItem(itemID);

                                    if (itemAsset.IsAbsent)
                                    {
                                        UnturnedChat.Say(caller, "That item does not exist", Color.red);
                                        return;
                                    }

                                    var item = new Item(itemAsset.Value.id, true);

                                    UnturnedChat.Say(caller, "Given " + unturnedPlayer.DisplayName + " item " + itemAsset.Value.itemName + ".", Color.cyan);
                                    unturnedPlayer.GiveItem(item.id, 1);
                                    UnturnedChat.Say(unturnedPlayer, "You have received a " + itemAsset.Value.itemName + ".", Color.cyan);
                                    return;
                                }
                            }
                            UnturnedChat.Say(caller, "Did not find anyone with the name \"" + playerName + "\".", Color.red);
                        }
                        
                    }
                    return;
                }
                //Choosee who to give what and how many.
                if(command.Count() == 3)
                {
                    string itemID = command[1];
                    byte itemAmount;

                    if (command[2] == "")
                        itemAmount = 1;
                    else
                        itemAmount = Convert.ToByte(command[2]);

                    if (command[0] == "*")
                    {
                        var itemAsset = ItemUtil.GetItem(itemID);

                        if (itemAsset.IsAbsent)
                        {
                            UnturnedChat.Say(caller, "That item does not exist", Color.red);
                            return;
                        }

                        var item = new Item(itemAsset.Value.id, true);

                        UnturnedChat.Say(caller, "Given all players " + itemAmount + " of item " + itemAsset.Value.itemName + ".", Color.cyan);

                        foreach (SteamPlayer plr in Provider.Players)
                        {
                            //So let's convert each SteamPlayer into an UnturnedPlayer
                            UnturnedPlayer unturnedPlayer = UnturnedPlayer.FromSteamPlayer(plr);

                            unturnedPlayer.GiveItem(item.id, itemAmount);
                            UnturnedChat.Say(unturnedPlayer, "You have received " + itemAmount + " of item " + itemAsset.Value.itemName + ".", Color.cyan);
                        }
                    }
                    else
                    {
                        string playerName = command[0];

                        //Find player
                        foreach (SteamPlayer plr in Provider.Players)
                        {
                            //So let's convert each SteamPlayer into an UnturnedPlayer
                            UnturnedPlayer unturnedPlayer = UnturnedPlayer.FromSteamPlayer(plr);

                            if (unturnedPlayer.DisplayName.ToLower().IndexOf(playerName.ToLower()) != -1 || unturnedPlayer.CharacterName.ToLower().IndexOf(playerName.ToLower()) != -1 || unturnedPlayer.SteamName.ToLower().IndexOf(playerName.ToLower()) != -1 || unturnedPlayer.CSteamID.ToString().Equals(playerName))
                            {
                                var itemAsset = ItemUtil.GetItem(itemID);

                                if (itemAsset.IsAbsent)
                                {
                                    UnturnedChat.Say(caller, "That item does not exist", Color.red);
                                    return;
                                }

                                var item = new Item(itemAsset.Value.id, true);

                                UnturnedChat.Say(caller, "Given " + unturnedPlayer.DisplayName + " " + itemAmount + " of item " + itemAsset.Value.itemName + ".", Color.cyan);
                                unturnedPlayer.GiveItem(item.id, itemAmount);
                                UnturnedChat.Say(unturnedPlayer, "You have received " + itemAmount + " of item " + itemAsset.Value.itemName + ".", Color.cyan);
                                return;
                            }
                        }
                        UnturnedChat.Say(caller, "Did not find anyone with the name \"" + playerName + "\".", Color.red);
                    }
                }
               
            }
            catch
            {
                UnturnedChat.Say(caller, "You did not use that correctly. Syntax: /spawnitems <player> <item> <amount>", Color.red);
            }
            

        }
    }
}
