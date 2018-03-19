using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using SDG.Unturned;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace datathegenius.DatasEssentials
{
    class CommandBan : IRocketCommand
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
                return "Bans player.";
            }
        }

        public string Name
        {
            get
            {
                return "ban";
            }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "datasessentials.ban" };
            }
        }

        public string Syntax
        {
            get
            {
                return "<ban>";
            }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if(caller.DisplayName != "Console")
            {
                UnturnedPlayer pCaller = (UnturnedPlayer)caller;
                if (command.Count() == 3)
                {
                    string playerName = command[0];
                    string reason = command[1];
                    uint duration = (uint)Convert.ToInt32(command[2]);
                    //Find player
                    foreach (SteamPlayer plr in Provider.Players)
                    {
                        //So let's convert each SteamPlayer into an UnturnedPlayer
                        UnturnedPlayer unturnedPlayer = UnturnedPlayer.FromSteamPlayer(plr);


                        if (unturnedPlayer.DisplayName.ToLower().IndexOf(playerName.ToLower()) != -1 || unturnedPlayer.CharacterName.ToLower().IndexOf(playerName.ToLower()) != -1 || unturnedPlayer.SteamName.ToLower().IndexOf(playerName.ToLower()) != -1 || unturnedPlayer.CSteamID.ToString().Equals(playerName))
                        {
                            if (CommandModerator.isMod(pCaller))
                            {
                                if (duration > DatasEssentialsManager.Instance.Configuration.Instance.maxBanLength)
                                {
                                    UnturnedChat.Say(caller, "Can't ban longer than " + DatasEssentialsManager.Instance.Configuration.Instance.maxBanLength + " seconds as a moderator.");
                                    return;
                                }

                                DateTime time = DateTime.Now;

                                DatasEssentialsManager.Instance.Configuration.Instance.modKickBanList.Add("[" + time.ToString("M/d/yyyy h:mm:ss tt") + "] Banned " + unturnedPlayer.CharacterName + "(" + unturnedPlayer.CSteamID.ToString() + ") for " + duration + " seconds because " + reason + " by " + pCaller.CharacterName + ".");
                            }

                            SteamBlacklist.ban(unturnedPlayer.CSteamID, pCaller.CSteamID, reason, duration);
                            //         Provider.ban(unturnedPlayer.CSteamID, reason, duration);
                            UnturnedChat.Say(unturnedPlayer.DisplayName + " has been banned for " + reason + " for " + duration + " seconds.", Color.cyan);
                            return;
                        }
                    }
                    UnturnedChat.Say(caller, "Did not find anyone with the name \"" + playerName + "\".", Color.red);
                }
                else
                {
                    UnturnedChat.Say(caller, "Used that wrong, syntax is /ban (player) (reason) (duration seconds)", Color.red);
                }
            }
            else
            {
                if (command.Count() == 3)
                {
                    string playerName = command[0];
                    string reason = command[1];
                    uint duration = (uint)Convert.ToInt32(command[2]);
                    //Find player
                    foreach (SteamPlayer plr in Provider.Players)
                    {
                        //So let's convert each SteamPlayer into an UnturnedPlayer
                        UnturnedPlayer unturnedPlayer = UnturnedPlayer.FromSteamPlayer(plr);


                        if (unturnedPlayer.DisplayName.ToLower().IndexOf(playerName.ToLower()) != -1 || unturnedPlayer.CharacterName.ToLower().IndexOf(playerName.ToLower()) != -1 || unturnedPlayer.SteamName.ToLower().IndexOf(playerName.ToLower()) != -1 || unturnedPlayer.CSteamID.ToString().Equals(playerName))
                        {
                            CSteamID consoleID = new CSteamID(Convert.ToUInt64(0));
                            SteamBlacklist.ban(unturnedPlayer.CSteamID, consoleID, reason, duration);
                            //         Provider.ban(unturnedPlayer.CSteamID, reason, duration);
                            UnturnedChat.Say(unturnedPlayer.DisplayName + " has been banned for " + reason + " for " + duration + " seconds.", Color.cyan);
                            Logger.Log(unturnedPlayer.DisplayName + " (" + unturnedPlayer.CSteamID + ") has been banned for " + reason + " for " + duration + " seconds.");
                            return;
                        }
                    }
                    UnturnedChat.Say(caller, "Did not find anyone with the name \"" + playerName + "\".", Color.red);
                }
                else
                {
                    UnturnedChat.Say(caller, "Used that wrong, syntax is /ban (player) (reason) (duration seconds)", Color.red);
                }
            }
           
        }
    }
}
