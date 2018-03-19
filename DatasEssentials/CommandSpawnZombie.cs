

using System;
using System.Collections.Generic;
using Rocket.API;
using SDG.Unturned;
using Rocket.Unturned.Chat;
using UnityEngine;
using Rocket.Unturned.Player;
using System.Linq;
using Rocket.Core.Logging;

namespace datathegenius.DatasEssentials
{
    public class CommandSpawnZombie : IRocketCommand
    {
        public List<string> Aliases
        {
            get
            {
                return new List<string>() { "sz" };
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
                return "Spawns zombie.";
            }
        }

        public string Name
        {
            get
            {
                return "spawnzombie";
            }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "datasessentials.spawnzombie" };
            }
        }

        public string Syntax
        {
            get
            {
                return "<descend>";
            }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            UnturnedChat.Say(caller, "This command is not working properly, request denied.", Color.red);
            return;

            Zombie myZombie = new Zombie();
            UnturnedPlayer pCaller = (UnturnedPlayer)caller;

            //            Vector3 test = new Vector3(0, 0, 0);
//            myZombie.isHunting = true;
//            Logger.Log("Flag1" + myZombie.isActiveAndEnabled);
//            myZombie.move = 100;
            myZombie.askAttack(100);
            //            myZombie.enabled = true;
            myZombie.askThrow();
            myZombie.tellSpeciality(EZombieSpeciality.ACID);
//            myZombie.idle = 0;
            
            ZombieManager.sendZombieAlive(myZombie, myZombie.type, (byte)myZombie.speciality, myZombie.shirt, myZombie.pants, myZombie.hat, myZombie.gear, pCaller.Position, 0);
      
            ZombieManager.Instance.addZombie(0, myZombie.type, (byte)myZombie.speciality, myZombie.shirt, myZombie.pants, myZombie.hat, myZombie.gear, myZombie.move, myZombie.idle, pCaller.Position, pCaller.Rotation, false);

        }
    }
}
