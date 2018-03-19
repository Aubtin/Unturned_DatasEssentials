using Rocket.API;
using Rocket.Unturned.Chat;
using SDG.Unturned;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace datathegenius.DatasEssentials
{
    class CommandRespawnVehicles : IRocketCommand
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
                return "Respawns all vehicles.";
            }
        }

        public string Name
        {
            get
            {
                return "respawnvehicles";
            }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "datasessentials.respawnvehicles" };
            }
        }

        public string Syntax
        {
            get
            {
                return "<respawnvehicles>";
            }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            var spawns = LevelVehicles.spawns;
            int spawnedCount = 0;

            for (var j = 0; j < spawns.Count; j++)
            {
                var vehicleSpawnpoint = spawns[j];
                var vehicle = LevelVehicles.getVehicle(vehicleSpawnpoint);

                if (vehicle == 0) continue;

                var point = vehicleSpawnpoint.point;
                point.y += 1f;
                VehicleManager.spawnVehicle(vehicle, point, Quaternion.Euler(0f, vehicleSpawnpoint.angle, 0f));

                spawnedCount++;
            }
            UnturnedChat.Say(caller, "Respawned " + spawnedCount + " vehicles.", Color.green);
        }
    }
}
