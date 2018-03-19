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
    class CommandRespawnItems : IRocketCommand
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
                return "Respawns all items.";
            }
        }

        public string Name
        {
            get
            {
                return "respawnitems";
            }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "datasessentials.respawnitems" };
            }
        }

        public string Syntax
        {
            get
            {
                return "<respawnitems>";
            }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            for (byte b = 0; b < Regions.WORLD_SIZE; b += 1)
            {
                for (byte b2 = 0; b2 < Regions.WORLD_SIZE; b2 += 1)
                {
                    if (LevelItems.spawns[b, b2].Count <= 0) continue;

                    for (var i = 0; i < LevelItems.spawns[b, b2].Count; i++)
                    {
                        var itemSpawnpoint = LevelItems.spawns[b, b2][i];
                        var item = LevelItems.getItem(itemSpawnpoint);

                        if (item == 0) continue;

                        var item2 = new Item(item, true);
                        ItemManager.dropItem(item2, itemSpawnpoint.point, false, false, false);
                    }
                }
            }
            UnturnedChat.Say(caller, "Respawned all items.", Color.green);
        }
    }
}
