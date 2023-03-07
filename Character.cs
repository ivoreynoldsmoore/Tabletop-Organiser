using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Tabletop_Organiser
{
    public class Character
    {
        static Dictionary<int, AbilityScores> races = new Dictionary<int, AbilityScores>()
        {
            { 0, new AbilityScores(1, 1, 1, 1, 1, 1 ) },
            { 1, new AbilityScores(0, 2, 0, 0, 0, 0) },
            { 2, new AbilityScores(0, 0, 0, 0, 0, 2) },
            { 3, new AbilityScores(0, 0, 2, 0, 0, 0) },
            { 4, new AbilityScores(2, 0, 1, 0, 0, 0) },
            { 5, new AbilityScores(0, 0, 0, 0, 2, 0) },
            { 6, new AbilityScores(0, 2, 0, 0, 0, 0) },
            { 7, new AbilityScores(0, 0, 0, 0, 0, 2) }
        };

        public string name = "";

        public Race race;

        public AbilityScores scores = new AbilityScores(10,10,10,10,10,10);
        public int strengthModifier => (int)Math.Floor((double)scores.strength / 2) - 5;

        public int dexterityModifier => (int)Math.Floor((double)scores.dexterity / 2) - 5;

        public int constitutionModifier => (int)Math.Floor((double)scores.constitution / 2) - 5;

        public int intelligenceModifier => (int)Math.Floor((double)scores.intelligence / 2) - 5;

        public int wisdomModifier => (int)Math.Floor((double)scores.wisdom / 2) - 5;

        public int charismaModifier => (int)Math.Floor((double)scores.charisma / 2) - 5;

        private int _level;
        public int level {
            get { return _level; }
            set { _level = Math.Clamp(value, 1, 20); }
        }

        public int proficiencyBonus => 2 + (int)Math.Floor((double)level / 5);

        public bool[] proficiencies = new bool[18];

        public int movementSpeed;

        public int atheleticsBonus => proficiencies[0] ? strengthModifier + proficiencyBonus : strengthModifier;

        public int acrobaticsBonus => proficiencies[1] ? dexterityModifier + proficiencyBonus : dexterityModifier;

        public int sleightBonus => proficiencies[2] ? dexterityModifier + proficiencyBonus : dexterityModifier;

        public int stealthBonus => proficiencies[3] ? dexterityModifier + proficiencyBonus : dexterityModifier;

        public int arcanaBonus => proficiencies[4] ? intelligenceModifier + proficiencyBonus : intelligenceModifier;

        public int historyBonus => proficiencies[5] ? intelligenceModifier + proficiencyBonus : intelligenceModifier;

        public int investigationBonus => proficiencies[6] ? intelligenceModifier + proficiencyBonus : intelligenceModifier;

        public int natureBonus => proficiencies[7] ? intelligenceModifier + proficiencyBonus : intelligenceModifier;

        public int religionBonus => proficiencies[8] ? intelligenceModifier + proficiencyBonus : intelligenceModifier;

        public int animalBonus => proficiencies[9] ? wisdomModifier + proficiencyBonus : wisdomModifier;

        public int insightBonus => proficiencies[10] ? wisdomModifier + proficiencyBonus : wisdomModifier;

        public int medicineBonus => proficiencies[11] ? wisdomModifier + proficiencyBonus : wisdomModifier;

        public int perceptionBonus => proficiencies[12] ? wisdomModifier + proficiencyBonus : wisdomModifier;

        public int survivalBonus => proficiencies[13] ? wisdomModifier + proficiencyBonus : wisdomModifier;

        public int deceptionBonus => proficiencies[14] ? charismaModifier + proficiencyBonus : charismaModifier;

        public int persuasionBonus => proficiencies[15] ? charismaModifier + proficiencyBonus : charismaModifier;

        public int performanceBonus => proficiencies[16] ? charismaModifier + proficiencyBonus : charismaModifier;

        public int intimidationBonus => proficiencies[17] ? charismaModifier + proficiencyBonus : charismaModifier;

        public ArmourType armourType;

        public int hitpoints { get; private set; }

        public bool inspiration;

        public bool autoAC;

        private int customAC;
        public int AC
        {
            get { return autoAC ? calcAC() : customAC; }
            set {
                autoAC= false;
                customAC = value;
            }
        }

        private void RollStats()
        {
            AbilityScores.GenerateScores(races[(int)race]);
        }

        private int calcAC()
        {
            int AC = 10;
            switch (armourType)
            {
                case ArmourType.none:
                    AC = 10 + dexterityModifier;
                    break;
                case ArmourType.padded:
                    AC = 11 + dexterityModifier;
                    break;
                case ArmourType.leather:
                    AC = 11 + dexterityModifier;
                    break;
                case ArmourType.studdedLeather:
                    AC = 12 + dexterityModifier;
                    break;
                case ArmourType.hide:
                    AC = 12 + Math.Min(2, dexterityModifier);
                    break;
                case ArmourType.chainShirt:
                    AC = 13 + Math.Min(2, dexterityModifier);
                    break;
                case ArmourType.scale:
                    AC = 14 + Math.Min(2, dexterityModifier);
                    break;
                case ArmourType.breast:
                    AC = 14 + Math.Min(2, dexterityModifier);
                    break;
                case ArmourType.halfPlate:
                    AC = 15 + Math.Min(2, dexterityModifier);
                    break;
                case ArmourType.ring:
                    AC = 14;
                    break;
                case ArmourType.chainMail:
                    AC = 16;
                    break;
                case ArmourType.splint:
                    AC = 17;
                    break;
                case ArmourType.fullPlate:
                    AC = 18;
                    break;
                case ArmourType.monkDefense:
                    AC = 10 + dexterityModifier + wisdomModifier;
                    break;
                case ArmourType.barbDefense:
                    AC = 10 + dexterityModifier + constitutionModifier;
                    break;
            }
            return AC;
        }

        public Character()
        {
            armourType = ArmourType.none;
            level = 1;
            customAC = 0;
            autoAC = true;
        }

        public enum ArmourType
        {
            none = 0,
            padded = 1,
            leather = 2,
            studdedLeather = 3,
            hide = 4,
            chainShirt = 5,
            scale = 6,
            breast = 7,
            halfPlate = 8,
            ring = 9,
            chainMail = 10,
            splint = 11,
            fullPlate = 12,
            monkDefense = 13,
            barbDefense = 14
        }
    }

    public enum Race
    {
        human = 0,
        elf = 1,
        halfElf = 2,
        dwarf = 3,
        halfOrc = 4,
        dragonborn = 5,
        halfling = 6,
        gnome = 7,
        tiefling = 8,
    }
}
