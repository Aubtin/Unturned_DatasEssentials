using System.Collections.Generic;
using Rocket.API;
using SDG.Unturned;
using Rocket.Unturned.Chat;
using UnityEngine;

namespace datathegenius.DatasEssentials
{
    public class CommandRepairBarricades : IRocketCommand
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
                return AllowedCaller.Player;
            }
        }

        public string Help
        {
            get
            {
                return "Repairs barricades.";
            }
        }

        public string Name
        {
            get
            {
                return "repairbarricades";
            }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "datasessentials.repairbarricades" };
            }
        }

        public string Syntax
        {
            get
            {
                return "<repairbarricades>";
            }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            Transform transform;
            int transformCount = 0;

            BarricadeRegion barricadeRegion;

            for (int k = 0; k < BarricadeManager.BarricadeRegions.GetLength(0); k++)
            {
                for (int l = 0; l < BarricadeManager.BarricadeRegions.GetLength(1); l++)
                {
                    barricadeRegion = BarricadeManager.BarricadeRegions[k, l];
                    transformCount = barricadeRegion.drops.Count;
                    for (int i = 0; i < transformCount; i++)
                    {
                        transform = barricadeRegion.drops[i].model;
                        BarricadeManager.repair(transform, 100, 1);
                    }

                }
            }

            UnturnedChat.Say(caller, "Done repairing barricades.", Color.cyan);
        }
    }
}
