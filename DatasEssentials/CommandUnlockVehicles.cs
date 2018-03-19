

using System;
using System.Collections.Generic;
using Rocket.API;
using SDG.Unturned;
using Rocket.Unturned.Chat;

namespace datathegenius.DatasEssentials
{
    public class CommandUnlockVehicles : IRocketCommand
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
                return "Unlocks all locked vehicles on the map";
            }
        }

        public string Name
        {
            get
            {
                return "unlockvehicles";
            }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "datasessentials.unlockvehicles" };
            }
        }

        public string Syntax
        {
            get
            {
                return "<unlockvehicles>";
            }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            int lockedCount = 0;
            foreach (InteractableVehicle vehicle in VehicleManager.Vehicles)
            {
                if (vehicle.isLocked)
                {
                    vehicle.tellLocked(vehicle.lockedOwner, vehicle.lockedGroup, false);
                    lockedCount++;
                }
            }      
            UnturnedChat.Say(caller, "Unlocked " + lockedCount + " vehicles.");
        }
    }
}
