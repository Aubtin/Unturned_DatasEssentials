

using System;
using System.Collections.Generic;
using Rocket.API;
using SDG.Unturned;
using Rocket.Unturned.Chat;
using UnityEngine;
using Rocket.Unturned.Player;
using System.Linq;

namespace datathegenius.DatasEssentials
{
    public class CommandExplode : IRocketCommand
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
                return "Explodes player.";
            }
        }

        public string Name
        {
            get
            {
                return "explode";
            }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "datasessentials.explode" };
            }
        }

        public string Syntax
        {
            get
            {
                return "<explode>";
            }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if(command.Count() == 0)
            {
                UnturnedPlayer pCaller = (UnturnedPlayer)caller;

                Explode(pCaller.Position);

                UnturnedChat.Say(caller, "You were exploded.", Color.green);
                return;
            }

            if(command.Count() == 1)
            {
                UnturnedPlayer player = DatasTools.findPlayer(caller, command[0]);

                if(player != null)
                {
                    Explode(player.Position);
                    UnturnedChat.Say(caller, "You exploded " + player.CharacterName + ".");
                    UnturnedChat.Say(player, "You were exploded.");
                    return;
                }
            }
        }

        private static void Explode(Vector3 pos)
        {
            EffectManager.sendEffect(20, EffectManager.INSANE, pos);
            DamageTool.explode(pos, 10f, EDeathCause.GRENADE, 200, 200, 200, 200, 200, 200, 200, 200);
        }
    }
}
