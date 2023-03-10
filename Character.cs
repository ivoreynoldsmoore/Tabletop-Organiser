using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using System.Xml.Linq;
using Utils;

namespace Tabletop_Organiser
{
    public class Character
    {
        public string name { get; set; } = "";

        public Races.RaceIndex race { get; set; }

        public AbilityScores scores { get; set; } = new AbilityScores(10, 10, 10, 10, 10, 10);
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

        public bool[] proficiencies { get; set; } = new bool[18];

        public int movementSpeed { get; set; }

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

        public ArmourType armourType { get; set; }

        public int hitpoints { get; private set; }

        public bool inspiration { get; set; }

        public bool autoAC { get; set; }

        private int customAC;
        public int AC
        {
            get { return autoAC ? calcAC() : customAC; }
            set {
                autoAC= false;
                customAC = value;
            }
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

    static public class Races
    {
        public enum RaceIndex
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

        public class Race
        {
            public RaceIndex index { get; set; }
            public string name { get; set; }
            public AbilityScores score { get; set; }
            public Dictionary<string, AbilityScores[]> subraces { get; set; } = new Dictionary<string, AbilityScores[]>();
            public Race(RaceIndex index, string name, AbilityScores score)
            {
                this.index= index;
                this.name = name;
                this.score= score;
            }

            public Race(RaceIndex index, string name, AbilityScores score, Dictionary<string, AbilityScores[]> subraces)
            {
                this.index = index;
                this.name = name;
                this.score = score;
                this.subraces = subraces;
            }
        }

        static readonly public Race[] races = new Race[] {
            new Race(RaceIndex.human, "Human", new AbilityScores(1, 1, 1, 1, 1, 1)),
            new Race(RaceIndex.elf, "Elf", new AbilityScores(0, 2, 0, 0, 0, 0)),
            new Race(RaceIndex.halfElf, "Half Elf", new AbilityScores(0, 0, 0, 0, 0, 2)),
            new Race(RaceIndex.dwarf, "Dwarf", new AbilityScores(0, 0, 2, 0, 0, 0)),
            new Race(RaceIndex.halfOrc, "Half Orc", new AbilityScores(2, 0, 1, 0, 0, 0)),
            new Race(RaceIndex.dragonborn, "Dragonborn", new AbilityScores(0, 0, 0, 0, 2, 0)),
            new Race(RaceIndex.halfling, "Halfling", new AbilityScores(0, 2, 0, 0, 0, 0)),
            new Race(RaceIndex.gnome, "Gnome", new AbilityScores(0, 0, 0, 2, 0, 0)),
            new Race(RaceIndex.tiefling, "Tiefling", new AbilityScores(0, 0, 0, 0, 0, 2))
        };

        static public AbilityScores GetRacialBonus(RaceIndex raceIndex)
        {
            return races.Single(race => race.index == raceIndex).score;
        }

        public static string GetRaceName(RaceIndex raceIndex)
        {
            return races.Single(race => race.index == raceIndex).name;
        }

        public static RaceIndex GetRaceIndex(string name)
        {
            return races.Single(race => race.name == name).index;
        }

        public static Dictionary<string, AbilityScores[]> GetSubraces(RaceIndex raceIndex)
        {
            IEnumerable<Race> race = races.Where(race => race.index == raceIndex);
            if (race.Count() > 0)
            {
                return race.First().subraces;
            }
            return new Dictionary<string, AbilityScores[]> { };
        }

        public static string[] GetRaces()
        {
            string[] names = { };
            foreach (Race race in races)
            {
                names = names.Append(race.name).ToArray();
            }
            return names;
        }
    }

    
}
