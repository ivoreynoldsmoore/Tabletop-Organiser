using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Dynamic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;
using System.Xml.Linq;
using Tabletop_Organiser.CharacterBuilder.Proficiencies;
using Utils;

namespace Tabletop_Organiser.CharacterBuilder
{
    public struct Feature
    {
        public readonly string name { get; }
        public readonly string description { get; }

        public Feature(string name, string description)
        {
            this.name = name;
            this.description = description;
        }
    }

    public class Character
    {
        public string name { get; set; } = "";

        public Race race { get; set; }

        public Roles.CharacterRole role { get; set; }

        private bool autoScores = true;

        private AbilityScores customScores;
        private AbilityScores baseScores = new AbilityScores(10, 10, 10, 10, 10, 10);
        public AbilityScores scores
        {
            get { return autoScores ? calcScores(): customScores; }
            set {
                customScores = value;
                autoScores = false;
            }
        }

        public void setScores(AbilityScores scores)
        {
            baseScores = scores;
        }

        public void resetScores()
        {
            autoScores = true;
        }

        private AbilityScores calcScores()
        {
            AbilityScores scores = AbilityScores.Add(baseScores, Races.GetRacialBonus(race));
            return scores;
        }
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

        public Skills[] proficiencies { get; set; } = Array.Empty<Skills>();

        public int movementSpeed { get; set; }

        public int atheleticsBonus => IsProficient(Skills.Atheletics) ? strengthModifier + proficiencyBonus : strengthModifier;

        public int acrobaticsBonus => IsProficient(Skills.Acrobatics) ? dexterityModifier + proficiencyBonus : dexterityModifier;

        public int sleightBonus => IsProficient(Skills.Sleight) ? dexterityModifier + proficiencyBonus : dexterityModifier;

        public int stealthBonus => IsProficient(Skills.Stealth) ? dexterityModifier + proficiencyBonus : dexterityModifier;

        public int arcanaBonus => IsProficient(Skills.Arcana) ? intelligenceModifier + proficiencyBonus : intelligenceModifier;

        public int historyBonus => IsProficient(Skills.History) ? intelligenceModifier + proficiencyBonus : intelligenceModifier;

        public int investigationBonus => IsProficient(Skills.Investigation) ? intelligenceModifier + proficiencyBonus : intelligenceModifier;

        public int natureBonus => IsProficient(Skills.Nature) ? intelligenceModifier + proficiencyBonus : intelligenceModifier;

        public int religionBonus => IsProficient(Skills.Religion) ? intelligenceModifier + proficiencyBonus : intelligenceModifier;

        public int animalBonus => IsProficient(Skills.AnimalHandling) ? wisdomModifier + proficiencyBonus : wisdomModifier;

        public int insightBonus => IsProficient(Skills.Insight) ? wisdomModifier + proficiencyBonus : wisdomModifier;

        public int medicineBonus => IsProficient(Skills.Medicine) ? wisdomModifier + proficiencyBonus : wisdomModifier;

        public int perceptionBonus => IsProficient(Skills.Perception) ? wisdomModifier + proficiencyBonus : wisdomModifier;

        public int survivalBonus => IsProficient(Skills.Survival) ? wisdomModifier + proficiencyBonus : wisdomModifier;

        public int deceptionBonus => IsProficient(Skills.Deception) ? charismaModifier + proficiencyBonus : charismaModifier;

        public int persuasionBonus => IsProficient(Skills.Persuasion) ? charismaModifier + proficiencyBonus : charismaModifier;

        public int performanceBonus => IsProficient(Skills.Performance) ? charismaModifier + proficiencyBonus : charismaModifier;

        public int intimidationBonus => IsProficient(Skills.Intimidation) ? charismaModifier + proficiencyBonus : charismaModifier;

        public ArmourType armourType { get; set; }

        public int hitpoints { get; private set; }

        public bool inspiration { get; set; }

        public bool autoAC { get; set; }

        private int customAC;
        public int AC
        {
            get { return autoAC ? calcAC() : customAC; }
            set {
                autoAC = false;
                customAC = value;
            }
        }

        public void resetAC()
        {
            autoAC = true;
        }

        public bool IsProficient(Skills targetSkill)
        {
            return proficiencies.Where(skill => skill == targetSkill).Any();
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
            customScores = new AbilityScores();
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
}
