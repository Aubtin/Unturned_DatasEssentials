

using System;
using System.Collections.Generic;
using Rocket.API;
using SDG.Unturned;
using Rocket.Unturned.Chat;
using System.Linq;
using UnityEngine;

namespace datathegenius.DatasEssentials
{
    public class CommandClearVehicles : IRocketCommand
    {
        public List<string> Aliases
        {
            get
            {
                return new List<string>() { "cv" };
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
                return "Clears all vehicles on the map.";
            }
        }

        public string Name
        {
            get
            {
                return "clearvehicles";
            }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "datasessentials.clearvehicles" };
            }
        }

        public string Syntax
        {
            get
            {
                return "<clearvehicles>";
            }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if(command.Count() == 1)
            {
                var vehicleVar = VehicleUtil.GetVehicle(command[0].ToString());

                if (vehicleVar.IsAbsent)
                {
                    UnturnedChat.Say(caller, "That vehicle does not exist", Color.red);
                    return;
                }

                var vehicleID = vehicleVar.Value.id;

                int destroyedCount = 0;
                
                foreach (InteractableVehicle vehicle in VehicleManager.Vehicles.ToList())
                {
                    if (vehicle.isEmpty && vehicle.id == vehicleID)
                    {
                        VehicleManager.Instance.SteamChannel.send("tellVehicleDestroy", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, vehicle.instanceID);

                        destroyedCount++;
                    }
                }
                UnturnedChat.Say(caller, "Destroyed " + destroyedCount + " of vehicle " + vehicleID);
                return;
            }

            int vehicleCount = 0;
            foreach (InteractableVehicle vehicle in VehicleManager.Vehicles.ToList())
            {
                if(vehicle.isEmpty)
                {
                    VehicleManager.Instance.SteamChannel.send("tellVehicleDestroy", ESteamCall.ALL, ESteamPacket.UPDATE_RELIABLE_BUFFER, vehicle.instanceID);
                    vehicleCount++;
                }
            }
            UnturnedChat.Say(caller, "Cleared " + vehicleCount + " vehicles.");
        }
    }
}
