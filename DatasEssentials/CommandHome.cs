

using System;
using System.Collections.Generic;
using Rocket.API;
using SDG.Unturned;
using Rocket.Unturned.Chat;
using Rocket.Unturned.Player;
using UnityEngine;
using Rocket.Core.Logging;

namespace datathegenius.DatasEssentials
{
    public class CommandHome : IRocketCommand
    {
        private static DateTime lastCalledHome;
        static UnturnedPlayer player;
        static Vector3 bedPos;
        static byte bedRot;
        public static Boolean someoneToTransport = false;

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
                return "Teleports player to home.";
            }
        }

        public string Name
        {
            get
            {
                return "home";
            }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "datasessentials.home" };
            }
        }

        public string Syntax
        {
            get
            {
                return "<home>";
            }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            player = (UnturnedPlayer)caller;

            if (player.Stance == EPlayerStance.DRIVING || player.Stance == EPlayerStance.SITTING)
            {
                // They are in a vehicle.
                UnturnedChat.Say(player, "You're in a vehicle, aborting teleport to bed.", Color.yellow);
                return;
            }

            if(!BarricadeManager.tryGetBed(player.CSteamID, out bedPos, out bedRot))
            {
                UnturnedChat.Say(player, "You do not have a bed to teleport to.", Color.red);
                return;
            }

            if(player.IsAdmin)
            {
                player.Teleport(bedPos, bedRot);
                UnturnedChat.Say(player, "You were teleported to your bed.", Color.yellow);
            }
            else
            {   
                if (DatasEssentialsManager.Instance.Configuration.Instance.bedWaitTime > 0)
                {
                    someoneToTransport = true;

                    UnturnedChat.Say(player, "You will be transported to your bed in " + DatasEssentialsManager.Instance.Configuration.Instance.bedWaitTime + " seconds.", Color.yellow);

                    lastCalledHome = DateTime.Now;

                    return;
                }

                goHome();

            }
        }

        public static void goHome()
        {
            player.Teleport(bedPos, bedRot);
            UnturnedChat.Say(player, "You were teleported to your bed.", Color.yellow);
            return;
        }

        public static DateTime getLastCalledHome()
        {
            return lastCalledHome;
        }
    }
}
