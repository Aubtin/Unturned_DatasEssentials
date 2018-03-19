using System.Collections.Generic;
using Rocket.API;
using SDG.Unturned;
using Rocket.Unturned.Chat;
using UnityEngine;

namespace datathegenius.DatasEssentials
{
    public class CommandRepairStructures : IRocketCommand
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
                return "Repairs structures ";
            }
        }

        public string Name
        {
            get
            {
                return "repairstructures";
            }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "datasessentials.repairstructures" };
            }
        }

        public string Syntax
        {
            get
            {
                return "<repairstructures>";
            }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            Transform transform;
            int transformCount = 0;

            StructureRegion structureRegion;

            for (int k = 0; k < StructureManager.StructureRegions.GetLength(0); k++)
            {
                for (int l = 0; l < StructureManager.StructureRegions.GetLength(1); l++)
                {
                    structureRegion = StructureManager.StructureRegions[k, l];
                    transformCount = structureRegion.Structures.Count;
                    for (int i = 0; i < transformCount; i++)
                    {
                        transform = structureRegion.Structures[i];
                        StructureManager.repair(transform, 100, 1);
                    }
                }
            }
            UnturnedChat.Say(caller, "Done repairing structures.", Color.cyan);

        }
    }
}
