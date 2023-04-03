using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Utils;

namespace Tabletop_Organiser.CharacterBuilder
{
    public struct Race
    {
        public Races.RaceIndex raceIndex = Races.RaceIndex.human;

        public Races.SubraceIndex subraceIndex = 0;

        public Race(Races.RaceIndex raceIndex, Races.SubraceIndex subraceIndex)
        {
            this.raceIndex = raceIndex;
            this.subraceIndex = subraceIndex;
        }
    }

    static public class Races
    {
        public enum RaceIndex
        {
            human,
            elf,
            halfElf,
            dwarf,
            halfOrc,
            dragonborn,
            halfling,
            gnome,
            tiefling,
        }

        public enum SubraceIndex
        {
            none,
            drow,
            highElf,
            woodElf,
            hillDwarf,
            mountainDwarf,
            lightfootHalfling,
            stoutHalfling,
            forestGnome,
            rockGnome,
            svirfneblin,
        }

        public class Race
        {

            public RaceIndex index { get; private set;  }
            public string name { get; private set; } = "";
            public AbilityScores score { get; private set; }
            public Feature[] features { get; private set; }
            public int speed { get; private set; }

            [JsonConstructor]
            public Race(RaceIndex index, string name, AbilityScores score, Feature[] features, int speed)
            {
                this.index = index;
                this.name = name;
                this.score = score;
                this.features = features;
                this.speed = speed;
            }
        }

        static public Race[] races;

        static public Subrace[] subraces;

        public class Subrace
        {
            public SubraceIndex index { get; private set; }

            public string name { get; private set; }

            public RaceIndex parent { get; private set; }

            public AbilityScores score { get; private set; }

            public Feature[] features { get; private set; }

            public Subrace(SubraceIndex index, string name, RaceIndex parent, AbilityScores score, Feature[] features)
            {
                this.index = index;
                this.name = name;
                this.parent = parent;
                this.score = score;
                this.features = features;
            }
        }

        static Races()
        {
            races = Array.Empty<Race>();
        }

        static public AbilityScores GetRacialBonus(CharacterBuilder.Race characterRace)
        {
            Race race = races.Single(race => race.index == characterRace.raceIndex);
            AbilityScores score = race.score;
            if (characterRace.subraceIndex > 0)
            {
                score = AbilityScores.Add(score, subraces.Single(subrace => subrace.index == characterRace.subraceIndex).score);
            }
            return score;
        }

        static public Feature[] GetFeatures(CharacterBuilder.Race characterRace)
        {
            Race race = races.Single(race => race.index == characterRace.raceIndex);
            Feature[] features = race.features;
            if (characterRace.subraceIndex > 0)
            {
                features = features.Concat(subraces.Single(subrace => subrace.index == characterRace.subraceIndex).features).ToArray();
            }
            return features;
        }

        public static Race GetRace(RaceIndex raceIndex)
        {
            return races.Single(race => race.index == raceIndex);
        }
        public static string[] GetRaces()
        {
            string[] names = Array.Empty<string>(); ;
            foreach (Race race in races)
            {
                names = names.Append(race.name).ToArray();
            }
            return names;
        }
        public static Subrace GetSubrace(SubraceIndex index)
        {
            return subraces.Single(subrace => subrace.index == index);
        }

        public static Subrace[] GetSubraces(RaceIndex raceIndex)
        {
            return subraces.Where(subrace => subrace.parent == raceIndex).ToArray();
        }
    }
}
