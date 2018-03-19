

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
    public class CommandJump : IRocketCommand
    {
        UnturnedPlayer player;

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
                return "Jumps player to what they're looking at.";
            }
        }

        public string Name
        {
            get
            {
                return "jump";
            }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "datasessentials.jump" };
            }
        }

        public string Syntax
        {
            get
            {
                return "<jump>";
            }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            player = (UnturnedPlayer)caller;      

            var dist = 1000f;
            
            var eyePos = GetEyePosition(dist, player);

            if (!eyePos.HasValue)
            {
                UnturnedChat.Say(caller, "Don't see where you want to jump to.", Color.red);
                return;
            }

            var point = eyePos.Value;
            point.y += 6;

            player.Teleport(point, player.Rotation);
            UnturnedChat.Say(caller, "You have successfully jumped!", Color.green);
        }
        public Vector3? GetEyePosition(float distance, UnturnedPlayer tempPlayer)
        {
            RaycastHit raycast;
            int masks = RayMasks.BLOCK_COLLISION & ~(1 << 0x15);
            PlayerLook Look = tempPlayer.Player.look;

            Physics.Raycast(Look.aim.position, Look.aim.forward, out raycast, distance, masks);

            if (raycast.transform == null)
                return null;

            return raycast.point;
        }
    }
}
