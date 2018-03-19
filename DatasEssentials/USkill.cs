using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace datathegenius.DatasEssentials
{
    //Don't think I'm using this class.
    public class USkill
    {
        public static readonly USkill OVERKILL = new USkill(0, 0, "OVERKILL");
        public static readonly USkill SHARPSHOOTER = new USkill(0, 1, "SHARPSHOOTER");
        public static readonly USkill DEXTERITY = new USkill(0, 2, "DEXTERITY");
        public static readonly USkill CARDIO = new USkill(0, 3, "CARDIO");
        public static readonly USkill EXERCISE = new USkill(0, 4, "EXERCISE");
        public static readonly USkill DIVING = new USkill(0, 5, "DIVING");
        public static readonly USkill PARKOUR = new USkill(0, 6, "PARKOUR");
        public static readonly USkill SNEAKYBEAKY = new USkill(1, 0, "SNEAKYBEAKY");
        public static readonly USkill VITALITY = new USkill(1, 1, "VITALITY");
        public static readonly USkill IMMUNITY = new USkill(1, 2, "IMMUNITY");
        public static readonly USkill TOUGHNESS = new USkill(1, 3, "TOUGHNESS");
        public static readonly USkill STRENGTH = new USkill(1, 4, "STRENGTH");
        public static readonly USkill WARMBLOODED = new USkill(1, 5, "WARMBLOODED");
        public static readonly USkill SURVIVAL = new USkill(1, 6, "SURVIVAL");
        public static readonly USkill HEALING = new USkill(2, 0, "HEALING");
        public static readonly USkill CRAFTING = new USkill(2, 1, "CRAFTING");
        public static readonly USkill OUTDOORS = new USkill(2, 2, "OUTDOORS");
        public static readonly USkill COOKING = new USkill(2, 3, "COOKING");
        public static readonly USkill FISHING = new USkill(2, 4, "FISHING");
        public static readonly USkill AGRICULTURE = new USkill(2, 5, "AGRICULTURE");
        public static readonly USkill MECHANIC = new USkill(2, 6, "MECHANIC");
        public static readonly USkill ENGINEER = new USkill(2, 7, "ENGINEER");

        public static readonly USkill[] Skills = {
            OVERKILL,
            SHARPSHOOTER,
            DEXTERITY,
            CARDIO,
            EXERCISE,
            DIVING,
            PARKOUR,
            SNEAKYBEAKY,
            VITALITY,
            IMMUNITY,
            TOUGHNESS,
            STRENGTH,
            WARMBLOODED,
            SURVIVAL,
            HEALING,
            CRAFTING,
            OUTDOORS,
            COOKING,
            FISHING,
            AGRICULTURE,
            MECHANIC,
            ENGINEER
        };

        internal byte SpecialityIndex;
        internal byte SkillIndex;

        public string Name { get; }

        private USkill(byte specialityIndex, byte skillIndex, string name)
        {
            SpecialityIndex = specialityIndex;
            SkillIndex = skillIndex;
            Name = name;
        }

        /// <summary>
        ///   Get skill from name.
        /// </summary>
        /// <param name="input">Skill name</param>
        /// <returns>
        ///   <see cref="Optional{USkill}.Empty"/> if not found,
        ///   otherwise return an <see cref="Optional{USkill}"/> containing the skill.
        /// </returns>
        public static Optional<USkill> FromName(string input)
        {
            var skill = Skills
                .FirstOrDefault(sk => sk.Name.Equals(input, StringComparison.InvariantCultureIgnoreCase))
                        ?? Skills.FirstOrDefault(sk => sk.Name.IndexOf(input, StringComparison.InvariantCultureIgnoreCase) >= 0);

            return Optional<USkill>.OfNullable(skill);
        }

    };
}
