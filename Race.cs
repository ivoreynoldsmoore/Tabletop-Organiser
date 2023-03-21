using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils;

namespace Tabletop_Organiser.CharacterBuilder
{
    public struct Race
    {
        public Races.RaceIndex raceIndex;

        public int subraceIndex;

        public Race(Races.RaceIndex raceIndex, int subraceIndex)
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

        public class Race
        {
            public class Subrace
            {
                public string name { get; private set; }
                
                public AbilityScores score { get; private set; }

                public Feature[] features { get; private set; }

                public Subrace(string name, AbilityScores score, Feature[] features)
                {
                    this.name = name;
                    this.score = score;
                    this.features = features;
                }
            }

            public RaceIndex index { get; private set;  }
            public string name { get; private set; }
            public AbilityScores score { get; private set; }
            public Feature[] features { get; private set; } = Array.Empty<Feature>();
            public Subrace[] subraces { get; private set; } = Array.Empty<Subrace>();
            public Race(RaceIndex index, string name, AbilityScores score, Feature[] features)
            {
                this.index = index;
                this.name = name;
                this.score = score;
                this.features = features;
            }

            public Race(RaceIndex index, string name, AbilityScores score, Feature[] features, Subrace[] subraces)
            {
                this.index = index;
                this.name = name;
                this.score = score;
                this.features = features;
                this.subraces = subraces;
            }
        }

        static public Race[] races;

        static Races()
        {
            races = new Race[] {
                new Race(RaceIndex.human, "Human", new AbilityScores(1, 1, 1, 1, 1, 1), Array.Empty<Feature>()),
                new Race(RaceIndex.elf, "Elf", new AbilityScores(dexterity:2), new Feature[]{ feature }, new Race.Subrace[] { new Race.Subrace("Drow",new AbilityScores(charisma:1), Array.Empty<Feature>()), new Race.Subrace("High",new AbilityScores(intelligence:1), Array.Empty<Feature>()), new Race.Subrace("Wood", new AbilityScores(wisdom: 1), Array.Empty<Feature>()) }),
                new Race(RaceIndex.halfElf, "Half Elf", new AbilityScores(charisma:2), Array.Empty<Feature>()),
                new Race(RaceIndex.dwarf, "Dwarf", new AbilityScores(constitution:2), Array.Empty<Feature>(), new Race.Subrace[] { new Race.Subrace("Hill", new AbilityScores(wisdom: 1), Array.Empty<Feature>()), new Race.Subrace("Mountain", new AbilityScores(strength: 2), Array.Empty<Feature>()) }),
                new Race(RaceIndex.halfOrc, "Half Orc", new AbilityScores(strength:2, constitution:1), Array.Empty < Feature >()),
                new Race(RaceIndex.dragonborn, "Dragonborn", new AbilityScores(wisdom:2), Array.Empty < Feature >()),
                new Race(RaceIndex.halfling, "Halfling", new AbilityScores(dexterity:2), Array.Empty<Feature>(), new Race.Subrace[] { new Race.Subrace("Lightfoot", new AbilityScores(charisma: 1), Array.Empty<Feature>()), new Race.Subrace("Stout", new AbilityScores(constitution: 1), Array.Empty<Feature>()) }),
                new Race(RaceIndex.gnome, "Gnome", new AbilityScores(intelligence: 2), Array.Empty<Feature>(), new Race.Subrace[] { new Race.Subrace("Forest",new AbilityScores(dexterity:1), Array.Empty<Feature>()), new Race.Subrace ("Rock",new AbilityScores(constitution:1), Array.Empty<Feature>()), new Race.Subrace ("Dark",new AbilityScores(dexterity:1), Array.Empty<Feature>()) }),
                new Race(RaceIndex.tiefling, "Tiefling", new AbilityScores(charisma:2, intelligence:1), Array.Empty < Feature >())
            };
        }


        static public AbilityScores GetRacialBonus(CharacterBuilder.Race characterRace)
        {
            Race race = races.Single(race => race.index == characterRace.raceIndex);
            AbilityScores score = race.score;
            if (characterRace.subraceIndex >= 0)
            {
                score = AbilityScores.Add(score, race.subraces[characterRace.subraceIndex].score);
            }
            return score;
        }

        static public Feature[] GetFeatures(CharacterBuilder.Race characterRace)
        {
            Race race = races.Single(race => race.index == characterRace.raceIndex);
            Feature[] features = race.features;
            if (characterRace.subraceIndex >= 0)
            {
                features = features.Concat(race.subraces[characterRace.subraceIndex].features).ToArray();
            }
            return features;
        }

        public static string GetRaceName(RaceIndex raceIndex)
        {
            return races.Single(race => race.index == raceIndex).name;
        }

        public static RaceIndex GetRaceIndex(string name)
        {
            return races.Single(race => race.name == name).index;
        }

        public static Race.Subrace[] GetSubraces(RaceIndex raceIndex)
        {
            return races.Single(race => race.index == raceIndex).subraces;
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
    }
}
