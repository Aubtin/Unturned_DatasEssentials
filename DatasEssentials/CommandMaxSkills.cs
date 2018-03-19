

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
    public class CommandMaxSkills : IRocketCommand
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
                return AllowedCaller.Both;
            }
        }

        public string Help
        {
            get
            {
                return "Increases skills to max.";
            }
        }

        public string Name
        {
            get
            {
                return "maxskills";
            }
        }

        public List<string> Permissions
        {
            get
            {
                return new List<string>() { "datasessentials.maxskills" };
            }
        }

        public string Syntax
        {
            get
            {
                return "<maxskills>";
            }
        }

        public void Execute(IRocketPlayer caller, string[] command)
        {
            if (command.Count() == 2)
            {
                player = DatasTools.findPlayer(caller, command[0]);

                var pSkills = player.Player.skills;

                Boolean overpower = false;
                if (command[1].Equals("true", StringComparison.InvariantCultureIgnoreCase))
                    overpower = true;
                else if (command[1].Equals("false", StringComparison.InvariantCultureIgnoreCase))
                    overpower = false;
                else
                {
                    UnturnedChat.Say(caller, "You used that wrong. Syntax: /maxskils (player name) (Persistent/OP)", Color.red);
                    return;
                }

                foreach (var skill in pSkills.skills.SelectMany(skArr => skArr))
                {
                    skill.level = overpower ? byte.MaxValue : skill.max;
                }
                pSkills.askSkills(player.CSteamID);

                UnturnedChat.Say(caller, "Max skills given to " + player.CharacterName + ".", Color.cyan);
                UnturnedChat.Say(player, "You've received max skills.", Color.cyan);
                return;
            }
            else
            {
                UnturnedChat.Say(caller, "You used that wrong. Syntax: /maxskils (player name) (Persistent/OP)", Color.red);
                return;
            }
        }

        //Not needed.
        public Skill GetSkill(USkill uSkill)
        {

            var skills = player.Player.skills; 
            return skills.skills[uSkill.SpecialityIndex][uSkill.SkillIndex];
        }

        public void SetSkillLevel(USkill uSkill, byte value)
        {
            GetSkill(uSkill).level = value;

            var skills = player.Player.skills;

            skills.askSkills(player.CSteamID);
        }

        public byte GetSkillLevel(USkill uSkill)
        {
            return GetSkill(uSkill).level;
        }
    }
}
