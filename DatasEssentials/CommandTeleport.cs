

using System;
using System.Collections.Generic;
using Rocket.API;
using SDG.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using UnityEngine;
using Rocket.Core.Logging;
using System.Linq;

namespace datathegenius.DatasEssentials
{
    public class CommandTeleport : IRocketCommand
    {
        public List<string> Aliases
        {
            get
            {
                return new List<string>() { "tp" };
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
                return "Teleports player.";
            }
        }

        public string Name
        {
            get
            {
                return "teleport";
            }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "datasessentials.teleport" };
            }
        }

        public string Syntax
        {
            get
            {
                return "<teleport>";
            }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            switch (command.Count())
            {
                /*
                    /tp player  -> teleport sender to player
                    /tp place   -> teleport sender to place
                */
                case 1:
                    {
                        string playerName = command[0];

                        foreach (SteamPlayer plr in Provider.Players)
                        {
                            //So let's convert each SteamPlayer into an UnturnedPlayer
                            UnturnedPlayer unturnedPlayer = UnturnedPlayer.FromSteamPlayer(plr);

                            if (unturnedPlayer.DisplayName.ToLower().IndexOf(playerName.ToLower()) != -1 || unturnedPlayer.CharacterName.ToLower().IndexOf(playerName.ToLower()) != -1 || unturnedPlayer.SteamName.ToLower().IndexOf(playerName.ToLower()) != -1 || unturnedPlayer.CSteamID.ToString().Equals(playerName))
                            {
                                UnturnedPlayer pCaller = (UnturnedPlayer)caller;

                                pCaller.Teleport(unturnedPlayer);
                                UnturnedChat.Say(caller, "Teleported you to " + unturnedPlayer.CharacterName + ".", Color.green);
                                return;
                            }
                        }
                        LocationNode location;
                        Boolean found = TryFindPlace(playerName, out location);

                        if (found)
                        {
                            UnturnedPlayer pCaller = (UnturnedPlayer)caller;
                            pCaller.Teleport(location.Position, 0);
                            UnturnedChat.Say(caller, "Teleported you to " + location.Name + ".", Color.green);
                            return;
                        }

                        UnturnedChat.Say(caller, "Did not find anyone/anywhere with the name \"" + playerName + "\".", Color.red);
                        return;
                    }
                case 2:
                    {
                        string playerOne = command[0];
                        string playerTwo = command[1];

                        UnturnedPlayer player1 = null;

                        foreach (SteamPlayer plr in Provider.Players)
                        {
                            //So let's convert each SteamPlayer into an UnturnedPlayer
                            UnturnedPlayer unturnedPlayer = UnturnedPlayer.FromSteamPlayer(plr);

                            if (unturnedPlayer.DisplayName.ToLower().IndexOf(playerOne.ToLower()) != -1 || unturnedPlayer.CharacterName.ToLower().IndexOf(playerOne.ToLower()) != -1 || unturnedPlayer.SteamName.ToLower().IndexOf(playerOne.ToLower()) != -1 || unturnedPlayer.CSteamID.ToString().Equals(playerOne))
                            {
                                player1 = UnturnedPlayer.FromSteamPlayer(plr);

                            }
                        }

                        if (player1 == null)
                        {
                            UnturnedChat.Say(caller, "Couldn't find the first player. Aborting.", Color.red);
                            return;
                        }
                           
                        foreach (SteamPlayer plr in Provider.Players)
                        {
                            //So let's convert each SteamPlayer into an UnturnedPlayer
                            UnturnedPlayer unturnedPlayer = UnturnedPlayer.FromSteamPlayer(plr);

                            if (unturnedPlayer.DisplayName.ToLower().IndexOf(playerTwo.ToLower()) != -1 || unturnedPlayer.CharacterName.ToLower().IndexOf(playerTwo.ToLower()) != -1 || unturnedPlayer.SteamName.ToLower().IndexOf(playerTwo.ToLower()) != -1 || unturnedPlayer.CSteamID.ToString().Equals(playerTwo))
                            {
                                UnturnedPlayer player2 = UnturnedPlayer.FromSteamPlayer(plr);

                                player1.Teleport(player2);
                                UnturnedChat.Say(caller, "Teleported " + player1.CharacterName + " to " + player2.CharacterName + ".", Color.green);
                                UnturnedChat.Say(player1, "Teleported you to " + player2.CharacterName + ".", Color.green);
                                UnturnedChat.Say(player2, "Teleported " + player1.CharacterName + " to you.", Color.green);
                                return;
                            }
                        }

                        LocationNode location;
                        Boolean found = TryFindPlace(playerTwo, out location);

                        if (found)
                        {
                            player1.Teleport(location.Position, 0);
                            UnturnedChat.Say(caller, "Teleported " + player1.CharacterName + " to " + location.Name + ".", Color.green);
                            UnturnedChat.Say(player1, "Teleported you to " + location.Name + ".", Color.green);
                            return;

                        }
                        UnturnedChat.Say(caller, "Couldn't find the second player", Color.red);
                        return;
                    }
            }
        }
        

        private static bool TryFindPlace(string name, out LocationNode outNode)
        {
            outNode = (
                from node in LevelNodes.Nodes
                where node.type == ENodeType.LOCATION
                let locNode = node as LocationNode
                where locNode.Name.ToLower().Contains(name.ToLower())
                select locNode
            ).FirstOrDefault();
            return outNode != null;
        }
    }
}
