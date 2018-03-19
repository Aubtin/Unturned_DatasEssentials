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
    class CommandUnmute : IRocketCommand
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
                return "Unmutes player.";
            }
        }

        public string Name
        {
            get
            {
                return "unmute";
            }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "datasessentials.mute" };
            }
        }

        public string Syntax
        {
            get
            {
                return "<unmute>";
            }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (caller.DisplayName != "Console")
            {
                UnturnedPlayer pCaller = (UnturnedPlayer)caller;

                if (command.Count() == 1)
                {
                    string playerName = command[0];

                    //Find player
                    foreach (SteamPlayer plr in Provider.Players)
                    {
                        //So let's convert each SteamPlayer into an UnturnedPlayer
                        UnturnedPlayer unturnedPlayer = UnturnedPlayer.FromSteamPlayer(plr);

                        if (unturnedPlayer.DisplayName.ToLower().IndexOf(playerName.ToLower()) != -1 || unturnedPlayer.CharacterName.ToLower().IndexOf(playerName.ToLower()) != -1 || unturnedPlayer.SteamName.ToLower().IndexOf(playerName.ToLower()) != -1 || unturnedPlayer.CSteamID.ToString().Equals(playerName))
                        {
                            if (((PlayerComponent)unturnedPlayer.GetComponent<PlayerComponent>()).isMuted == true)
                            {
                                if (CommandModerator.isMod(pCaller))
                                {
                                    DateTime time = DateTime.Now;

                                    DatasEssentialsManager.Instance.Configuration.Instance.modKickBanList.Add("[" + time.ToString("M/d/yyyy h:mm:ss tt") + "] Unmuted " + unturnedPlayer.CharacterName + "(" + unturnedPlayer.CSteamID.ToString() + ") by " + pCaller.CharacterName + ".");
                                }

                                ((PlayerComponent)unturnedPlayer.GetComponent<PlayerComponent>()).isMuted = false;
                                UnturnedChat.Say(unturnedPlayer.DisplayName + " has been unmuted.", Color.cyan);
                                return;
                            }
                            else
                            {
                                UnturnedChat.Say(caller, "That player is not muted.", Color.red);
                                return;
                            }
                        }
                    }
                    UnturnedChat.Say(caller, "Did not find anyone with the name \"" + playerName + "\".", Color.red);
                }
                else
                {
                    UnturnedChat.Say(caller, "Used that wrong, syntax is /unmute (player)", Color.red);
                }
            }
            else
            {
                if (command.Count() == 1)
                {
                    string playerName = command[0];

                    //Find player
                    foreach (SteamPlayer plr in Provider.Players)
                    {
                        //So let's convert each SteamPlayer into an UnturnedPlayer
                        UnturnedPlayer unturnedPlayer = UnturnedPlayer.FromSteamPlayer(plr);

                        if (unturnedPlayer.DisplayName.ToLower().IndexOf(playerName.ToLower()) != -1 || unturnedPlayer.CharacterName.ToLower().IndexOf(playerName.ToLower()) != -1 || unturnedPlayer.SteamName.ToLower().IndexOf(playerName.ToLower()) != -1 || unturnedPlayer.CSteamID.ToString().Equals(playerName))
                        {
                            if (((PlayerComponent)unturnedPlayer.GetComponent<PlayerComponent>()).isMuted == true)
                            {
                                ((PlayerComponent)unturnedPlayer.GetComponent<PlayerComponent>()).isMuted = false;
                                UnturnedChat.Say(unturnedPlayer.DisplayName + " has been unmuted.", Color.cyan);
                                Logger.Log(unturnedPlayer.DisplayName + " (" + unturnedPlayer.CSteamID + ") has been unmuted.");
                                return;
                            }
                            else
                            {
                                UnturnedChat.Say(caller, "That player is not muted.", Color.red);
                                return;
                            }
                        }
                    }
                    UnturnedChat.Say(caller, "Did not find anyone with the name \"" + playerName + "\".", Color.red);
                }
                else
                {
                    UnturnedChat.Say(caller, "Used that wrong, syntax is /unmute (player)", Color.red);
                }
            }
        }
    }
}
