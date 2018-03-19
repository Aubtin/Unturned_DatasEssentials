using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Events;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace datathegenius.DatasEssentials
{

    public class DatasEssentialsManager : RocketPlugin<DatasEssentialsConfiguration>
    {
        public static DatasEssentialsManager Instance;

        protected override void Load()
        {
            Instance = this;
            U.Events.OnPlayerConnected += OnPlayerConnected;
            U.Events.OnPlayerDisconnected += OnPlayerDisconnected;
            UnturnedPlayerEvents.OnPlayerChatted += OnPlayerChatted;

            Boolean fileExists;
            string path = DatasTools.AssemblyDirectory + "/Rocket/Plugins/DatasEssentials/polls.txt";

            Logger.Log(DatasTools.AssemblyDirectory + "/Rocket/Plugins/DatasEssentials/polls.txt");
            fileExists = File.Exists(path) ? true : false;

            if(!fileExists)
            {
                File.WriteAllText(path, "363" + System.Environment.NewLine + "This is the default message for polls." + System.Environment.NewLine + "--start--" + System.Environment.NewLine);
                File.OpenWrite(path).Close();

                Logger.Log("polls.txt does not exist, creation of file successful.");
            }

            CommandPoll.pollReward = File.ReadAllLines(path)[0];
            File.OpenRead(path).Close();
            CommandPoll.pollMessage = File.ReadAllLines(path)[1];
            File.OpenRead(path).Close();

        }

        protected override void Unload()
        {
            U.Events.OnPlayerConnected -= OnPlayerConnected;
            U.Events.OnPlayerDisconnected -= OnPlayerDisconnected;
            UnturnedPlayerEvents.OnPlayerChatted += OnPlayerChatted;
        }

        private void OnPlayerConnected(UnturnedPlayer player)
        {
            //Add to group
            for (int x = 0; x < DatasEssentialsManager.Instance.Configuration.Instance.permissionsAddOnConnect.Count(); x++)
            {
                Rocket.Core.R.Permissions.AddPlayerToGroup(DatasEssentialsManager.Instance.Configuration.Instance.permissionsAddOnConnect[x], player);
            }

            //Remove to group
            for (int x = 0; x < DatasEssentialsManager.Instance.Configuration.Instance.permissionsRemoveOnConnect.Count(); x++)
            {
                Rocket.Core.R.Permissions.RemovePlayerFromGroup(DatasEssentialsManager.Instance.Configuration.Instance.permissionsRemoveOnConnect[x], player);
            }

            //Admin section
            if(player.IsAdmin)
            {
                player.GodMode = true;
                player.VanishMode = true;
                DatasTools.maxSkills(player, true);

                if(DatasEssentialsManager.Instance.Configuration.Instance.modKickBanList.Count != 0)
                {
                    UnturnedChat.Say(player, "A mod had muted/kicked/banned a player while away, check with /modreport.", Color.cyan);
                }
            }

            #region Joined Notifcation
            if (DatasEssentialsManager.Instance.Configuration.Instance.announceAdmin && player.IsAdmin)
            {
                UnturnedChat.Say(player.DisplayName + " has joined the server!", Color.cyan);
                return;
            }

            if(!player.IsAdmin)
                UnturnedChat.Say(player.DisplayName + " has joined the server!", Color.green);
            #endregion
        }

        private void OnPlayerDisconnected(UnturnedPlayer player)
        {
            //Check if moderator is active
            if(CommandModerator.activeMods.Count > 0)
            {
                bool isModActive = CommandModerator.activeMods.Any(item => item.Equals(player.CSteamID.ToString()));

                if(isModActive)
                {
                    CommandModerator.activeMods.Remove(player.CSteamID.ToString());
                    Rocket.Core.R.Permissions.RemovePlayerFromGroup("Moderator", player);

                    if (DatasEssentialsManager.Instance.Configuration.Instance.announceModActive)
                        UnturnedChat.Say(player.CharacterName + " has left moderator mode.", Color.cyan);
                }
            }

            #region Left Notifcation
            if (DatasEssentialsManager.Instance.Configuration.Instance.announceAdmin && player.IsAdmin)
            {
                UnturnedChat.Say(player.DisplayName + " has left the server!", Color.cyan);
                return;
            }

            if(!player.IsAdmin)
                UnturnedChat.Say(player.DisplayName + " has left the server!", Color.green);
            #endregion
        }

        private void OnPlayerChatted(Rocket.Unturned.Player.UnturnedPlayer player, ref UnityEngine.Color color, string message, SDG.Unturned.EChatMode chatMode, ref bool cancel)
        {
            PlayerComponent component = player.GetComponent<PlayerComponent>();
            
            if (component.isMuted)
            {
                cancel = true;
                UnturnedChat.Say(player, "Sorry, you are muted.", Color.red);
                return;
            }
        }

        //Clean vehicles on map at set time
        DateTime clearVehicles = DateTime.Now;
        public void vehicleCleaner()
        {
            if ((DateTime.Now - clearVehicles).TotalSeconds > DatasEssentialsManager.Instance.Configuration.Instance.clearVehicleFrequency)
            {
                clearVehicles = DateTime.Now;

                int destroyedCount = 0;

                foreach (InteractableVehicle vehicle in VehicleManager.Vehicles.ToList())
                {
                    if (vehicle.isEmpty && vehicle.id == DatasEssentialsManager.Instance.Configuration.Instance.clearVehicleID)
                    {
                        VehicleManager.Instance.SteamChannel.send("tellVehicleDestroy", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, vehicle.instanceID);

                        destroyedCount++;
                    }
                }
                UnturnedChat.Say("Cleared " + destroyedCount + " kit vehicles from map automatically.", Color.cyan);
            }
        }

        void FixedUpdate()
        {
            //Poll Announcer
            CommandPoll.pollMessageAnnouncer();

            //Clean vehicles
            vehicleCleaner();

            //Home Command
            CommandHome home;
            if (CommandHome.someoneToTransport && DatasEssentialsManager.Instance.Configuration.Instance.bedWaitTime > 0)
            {
                if ((DateTime.Now - CommandHome.getLastCalledHome()).TotalSeconds < DatasEssentialsManager.Instance.Configuration.Instance.bedWaitTime)
                {
                    return;
                }
                CommandHome.goHome();
                CommandHome.someoneToTransport = false;
            }
        }
    }
}
