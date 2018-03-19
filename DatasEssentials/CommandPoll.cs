

using System;
using System.Collections.Generic;
using Rocket.API;
using SDG.Unturned;
using Rocket.Unturned.Chat;
using UnityEngine;
using Rocket.Unturned.Player;
using System.Linq;
using Rocket.Core.Logging;
using System.IO;
using System.Reflection;

namespace datathegenius.DatasEssentials
{
    public class CommandPoll : IRocketCommand
    {
        public static Boolean pollRunning = false;
        public static string pollReward;
        public static string pollMessage;
        string path = DatasTools.AssemblyDirectory + "/Rocket/Plugins/DatasEssentials/polls.txt";

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
                return "Controls polls and lets users vote.";
            }
        }

        public string Name
        {
            get
            {
                return "poll";
            }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "datasessentials.poll" };
            }
        }

        public string Syntax
        {
            get
            {
                return "<poll>";
            }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (caller.IsAdmin)
            {
                if (command[0] == "on")
                {
                    pollRunning = true;
                    UnturnedChat.Say(caller, "Poll turned on", Color.green);
                    return;
                }
                else if (command[0] == "off")
                {
                    pollRunning = false;
                    UnturnedChat.Say(caller, "Poll turned off", Color.green);
                    return;
                }
                else if (command[0] == "clear")
                {
                    File.WriteAllText(path, CommandPoll.pollReward + Environment.NewLine + CommandPoll.pollMessage + Environment.NewLine + "--start--" + Environment.NewLine);
                    File.OpenWrite(path).Close();
                    UnturnedChat.Say(caller, "Cleared poll.", Color.green);
                    return;
                }
                else if(command[0] == "message")
                {
                    CommandPoll.pollMessage = "";
                    for (int x = 1; x < command.Count(); x++)
                    {
                        CommandPoll.pollMessage += command[x] + " ";
                    }

                    lineChanger(CommandPoll.pollMessage, path, 2);
                    UnturnedChat.Say(caller, "Poll message changed to: " + CommandPoll.pollMessage, Color.green);
                    return;
                }
                else if(command[0] == "reward")
                {
                    CommandPoll.pollReward = command[1];
                    lineChanger(CommandPoll.pollReward, path, 1);
                    UnturnedChat.Say(caller, "Poll reward changed to: " + CommandPoll.pollReward, Color.green);
                    return;
                }
                else if(command[0] == "reload")
                {
                    CommandPoll.pollReward = File.ReadAllLines(path)[0];
                    File.OpenRead(path).Close();
                    CommandPoll.pollMessage = File.ReadAllLines(path)[1];
                    File.OpenRead(path).Close();

                    UnturnedChat.Say(caller, "Poll was reloaded.", Color.green);
                    return;
                }
                else if(command[0] == "results")
                {
                    string results = File.ReadAllText(path);

                    int countYes = 0;
                    int countNo = 0;
                    int posYes = results.IndexOf("--start--") + 10;
                    int posNo = results.IndexOf("--start--") + 10;

                    while ((posYes = results.IndexOf("yes", posYes)) > -1)
                    {
                        countYes++;
                        posYes += "yes".Length;
                    }

                    while ((posNo = results.IndexOf("no", posNo)) > -1)
                    {
                        countNo++;
                        posNo += "no".Length;
                    }

                    UnturnedChat.Say(caller, "Yes: " + countYes + " No: " + countNo);
                    return;
                }
            }

            if(pollRunning)
            {
                if (command[0] == "yes")
                {
                    UnturnedPlayer pCaller = (UnturnedPlayer)caller;
                    Boolean alreadyVoted ;
                    string voteAdd;
                    //If they have not voted, give them their 
                    voteAdd = pCaller.CSteamID + " yes" + Environment.NewLine;

                    alreadyVoted = File.ReadAllText(path).Contains(pCaller.CSteamID.ToString());
                    File.OpenRead(path).Close();

                    if (!alreadyVoted)
                    {

                        using (StreamWriter file = File.AppendText(path))
                        {
                            file.WriteLine(voteAdd);
                        }

                        if(!pCaller.GiveItem((ushort)Convert.ToInt32(pollReward), 1))
                        {
                            Item rewardItem = new Item((ushort)Convert.ToInt32(pollReward), true);
                            ItemManager.dropItem(rewardItem, pCaller.Position, true, true, true);
                            UnturnedChat.Say(caller, "You do not have room in your inventory, your reward was dropped below you.", Color.green);
                        }
                       

                        UnturnedChat.Say(caller, "Thanks for voting, you voted yes! You received your reward.", Color.green);
                        return;
                    }
                    else
                    {
                        List<string> tempPollHold = new List<string>();
                        tempPollHold = File.ReadAllLines(path).ToList<string>();

                        File.OpenRead(path).Close();

                        int index = tempPollHold.IndexOf(pCaller.CSteamID.ToString() + " no");

                        if(index == -1)
                        {
                            UnturnedChat.Say(caller, "You already voted yes.", Color.red);
                            return;
                        }
                        tempPollHold[index] = null;
                        File.WriteAllLines(path, tempPollHold.ToArray());

                        using (StreamWriter file = File.AppendText(path))
                        {
                            file.Write(voteAdd);
                        }
                        File.OpenWrite(path).Close();

                        UnturnedChat.Say(caller, "Thanks for voting, you voted yes! You only receive a reward once though.", Color.green);
                    }
                    return;
                }

                if (command[0] == "no")
                {
                    UnturnedPlayer pCaller = (UnturnedPlayer)caller;
                    Boolean alreadyVoted;
                    string voteAdd;
                    //If they have not voted, give them their 
                    voteAdd = pCaller.CSteamID + " no" + Environment.NewLine;

                    alreadyVoted = File.ReadAllText(path).Contains(pCaller.CSteamID.ToString());
                    File.OpenRead(path).Close();

                    if (!alreadyVoted)
                    {
                        using (StreamWriter file = File.AppendText(path))
                        {
                            file.WriteLine(voteAdd);
                        }

                        if (!pCaller.GiveItem((ushort)Convert.ToInt32(pollReward), 1))
                        {
                            Item rewardItem = new Item((ushort)Convert.ToInt32(pollReward), true);
                            ItemManager.dropItem(rewardItem, pCaller.Position, true, true, true);
                            UnturnedChat.Say(caller, "You do not have room in your inventory, your reward was dropped below you.", Color.green);
                        }


                        UnturnedChat.Say(caller, "Thanks for voting, you voted no! You received your reward.", Color.green);
                        return;
                    }
                    else
                    {
                        List<string> tempPollHold = new List<string>();
                        tempPollHold = File.ReadAllLines(path).ToList<string>();

                        File.OpenRead(path).Close();

                        int index = tempPollHold.IndexOf(pCaller.CSteamID.ToString() + " yes");

                        if(index == -1)
                        {
                            UnturnedChat.Say(caller, "You already voted no.", Color.red);
                            return;
                        }

                        tempPollHold[index] = null;
                        File.WriteAllLines(path, tempPollHold.ToArray());

                        using (StreamWriter file = File.AppendText(path))
                        {
                            file.Write(voteAdd);
                        }

                        File.OpenWrite(path).Close();

                        UnturnedChat.Say(caller, "Thanks for voting, you voted no! You only receive a reward once though.", Color.green);
                    }
                    return;
                }
            }
            else
            {
                UnturnedChat.Say(caller, "There is no running poll.", Color.red);
                return;
            }

            UnturnedChat.Say(caller, "Error, use it like /poll (yes or no)", Color.red);
        }

        void lineChanger(string newText, string fileName, int line_to_edit)
        {
            string[] arrLine = File.ReadAllLines(fileName);
            arrLine[line_to_edit - 1] = newText;
            File.WriteAllLines(fileName, arrLine);
            File.OpenWrite(path).Close();
        }

        static DateTime messageTimeKeeper = DateTime.Now;
        public static void pollMessageAnnouncer()
        {
            if (CommandPoll.pollRunning && (DateTime.Now - CommandPoll.messageTimeKeeper).TotalSeconds > DatasEssentialsManager.Instance.Configuration.Instance.pollDelayTime)
            {
                messageTimeKeeper = DateTime.Now;

                UnturnedChat.Say(CommandPoll.pollMessage, Color.cyan);
            }
        }
    }
}
