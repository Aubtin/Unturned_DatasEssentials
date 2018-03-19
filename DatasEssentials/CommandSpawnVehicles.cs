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
    class CommandSpawnVehicles : IRocketCommand
    {
        public List<string> Aliases
        {
            get
            {
                return new List<string>() { "givevehicle", "gv", "vehicle", "v" };
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
                return "Spawns vehicles /spawnvehicles <player> <id> <amount>";
            }
        }

        public string Name
        {
            get
            {
                return "spawnvehicles";
            }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "datasessentials.spawnvehicles" };
            }
        }

        public string Syntax
        {
            get
            {
                return "<spawnvehicles>";
            }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            try
            {
                if(command.Count() == 1)
                {
                    UnturnedPlayer player = (UnturnedPlayer)caller;
                    var vehicle = VehicleUtil.GetVehicle(command[0].ToString());

                    if (vehicle.IsAbsent)
                    {
                        UnturnedChat.Say(caller, "That vehicle does not exist", Color.red);
                        return;
                    }

                    var vehicleID = vehicle.Value.id;

                    player.GiveVehicle(vehicleID);
                    UnturnedChat.Say(player, "You have received a " + vehicle.Value.vehicleName + ".", Color.cyan);
                    return;
                }

                if(command.Count() == 2)
                {
                    byte vehicleAmount;

                    //Multiple items
                    if (byte.TryParse(command[1], out vehicleAmount))
                    {
                        var vehicle = VehicleUtil.GetVehicle(command[0].ToString());

                        if (vehicle.IsAbsent)
                        {
                            UnturnedChat.Say(caller, "That vehicle does not exist", Color.red);
                            return;
                        }

                        var vehicleID = vehicle.Value.id;

                        UnturnedPlayer player = (UnturnedPlayer)caller;
                        for (int x = 0; x < vehicleAmount; x++)
                            player.GiveVehicle(vehicleID);

                        UnturnedChat.Say(player, "You have received " + vehicleAmount + " of " + vehicle.Value.vehicleName + ".", Color.cyan);
                    }
                    else
                    {
                        string playerName = command[0];
                        var vehicle = VehicleUtil.GetVehicle(command[1].ToString());

                        if (vehicle.IsAbsent)
                        {
                            UnturnedChat.Say(caller, "That vehicle does not exist", Color.red);
                            return;
                        }

                        var vehicleID = vehicle.Value.id;

                        if (playerName.Equals("*"))
                        {
                            UnturnedChat.Say(caller, "Given all players 1 of vehicle ID " + vehicle.Value.vehicleName + ".", Color.cyan);

                            foreach (SteamPlayer plr in Provider.Players)
                            {
                                //So let's convert each SteamPlayer into an UnturnedPlayer
                                UnturnedPlayer unturnedPlayer = UnturnedPlayer.FromSteamPlayer(plr);
                                unturnedPlayer.GiveVehicle(vehicleID);

                                UnturnedChat.Say(unturnedPlayer, "You have received a " + vehicle.Value.vehicleName + ".", Color.cyan);
                            }
                        }
                        else
                        {
                            //Find player
                            foreach (SteamPlayer plr in Provider.Players)
                            {
                                //So let's convert each SteamPlayer into an UnturnedPlayer
                                UnturnedPlayer unturnedPlayer = UnturnedPlayer.FromSteamPlayer(plr);

                                if (unturnedPlayer.DisplayName.ToLower().IndexOf(playerName.ToLower()) != -1 || unturnedPlayer.CharacterName.ToLower().IndexOf(playerName.ToLower()) != -1 || unturnedPlayer.SteamName.ToLower().IndexOf(playerName.ToLower()) != -1 || unturnedPlayer.CSteamID.ToString().Equals(playerName))
                                {

                                    UnturnedChat.Say(caller, "Given " + unturnedPlayer.DisplayName + " a " + vehicle.Value.vehicleName + ".", Color.cyan);
                                    unturnedPlayer.GiveVehicle(vehicleID);
                                    UnturnedChat.Say(unturnedPlayer, "You have received a " + vehicle.Value.vehicleName + ".", Color.cyan);
                                    return;
                                }
                            }
                            UnturnedChat.Say(caller, "Did not find anyone with the name \"" + playerName + "\".", Color.red);
                        }

                    }
                    return;
                }

                if(command.Count() == 3)
                {
                    var vehicle = VehicleUtil.GetVehicle(command[1].ToString());

                    if (vehicle.IsAbsent)
                    {
                        UnturnedChat.Say(caller, "That vehicle does not exist", Color.red);
                        return;
                    }

                    var vehicleID = vehicle.Value.id;
                    byte vehicleAmount = Convert.ToByte(command[2]);

                    if (command[0] == "*")
                    {
                        UnturnedChat.Say(caller, "Given all players " + vehicleAmount + " of vehicle " + vehicle.Value.vehicleName + ".", Color.cyan);

                        foreach (SteamPlayer plr in Provider.Players)
                        {
                            //So let's convert each SteamPlayer into an UnturnedPlayer
                            UnturnedPlayer unturnedPlayer = UnturnedPlayer.FromSteamPlayer(plr);
                            for (int x = 0; x < vehicleAmount; x++)
                                unturnedPlayer.GiveVehicle(vehicleID);

                            UnturnedChat.Say(unturnedPlayer, "You have received " + vehicleAmount + " of vehicle " + vehicle.Value.vehicleName + ".", Color.cyan);
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

                                UnturnedChat.Say(caller, "Given " + unturnedPlayer.DisplayName + " " + vehicleAmount + " of vehicle " + vehicle.Value.vehicleName + ".", Color.cyan);
                                for (int x = 0; x < vehicleAmount; x++)
                                    unturnedPlayer.GiveVehicle(vehicleID);
                                UnturnedChat.Say(unturnedPlayer, "You have received " + vehicleAmount + " of vehicle " + vehicle.Value.vehicleName + ".", Color.cyan);
                                return;
                            }
                        }
                        UnturnedChat.Say(caller, "Did not find anyone with the name \"" + playerName + "\".", Color.red);
                    }
                }
                
            }
            catch
            {
                UnturnedChat.Say(caller, "You did not use that correctly. Syntax: /spawnvehicles <player> <vehicle> <amount>", Color.red);
            }
            

        }
    }
}
