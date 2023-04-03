using System;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Utils
{
    public static class Roll
    {
        static Random rand = new();

        static public void SetSeed(int seed)
        {
            rand = new Random(seed);
        }

        private static int RollDice(int size)
        {
            return rand.Next(1, size+1);
        }

        public static int Rolld4()
        {
            return RollDice(4);
        }

        public static int[] Rolld4(int n)
        {
            int[] results = Array.Empty<int>();
            for (int i = 0; i < n; i++)
            {
                results = results.Append(RollDice(4)).ToArray();
            }
            return results;
        }

        public static int Rolld6()
        {
            return RollDice(6);
        }

        public static int[] Rolld6(int n)
        {
            int[] results = Array.Empty<int>();
            for (int i = 0; i < n; i++)
            {
                results = results.Append(RollDice(6)).ToArray();
            }
            return results;
        }

        public static int Rolld8()
        {
            return RollDice(8);
        }

        public static int[] Rolld8(int n)
        {
            int[] results = Array.Empty<int>();
            for (int i = 0; i < n; i++)
            {
                results = results.Append(RollDice(8)).ToArray();
            }
            return results;
        }

        public static int Rolld10()
        {
            return RollDice(10);
        }

        public static int[] Rolld10(int n)
        {
            int[] results = Array.Empty<int>();
            for (int i = 0; i < n; i++)
            {
                results = results.Append(RollDice(10)).ToArray();
            }
            return results;
        }

        public static int Rolld12()
        {
            return RollDice(12);
        }

        public static int[] Rolld12(int n)
        {
            int[] results = Array.Empty<int>();
            for (int i = 0; i < n; i++)
            {
                results = results.Append(RollDice(12)).ToArray();
            }
            return results;
        }

        public static int[] Rolld20(int n)
        {
            int[] results = Array.Empty<int>();
            for (int i = 0; i < n; i++)
            {
                results = results.Append(RollDice(20)).ToArray();
            }
            return results;
        }

        public static int RollStat()
        {
            int[] rolls = Rolld6(4);
            int score = rolls.Sum() - rolls.Min();
            return score;
        }
    }

    public class AbilityScores
    {
        private static readonly int max = 30;
        public int strength { get; set; }
        public int dexterity { get; set; }
        public int constitution { get; set; }
        public int intelligence { get; set; }
        public int wisdom { get; set; }
        public int charisma { get; set; }

        public static AbilityScores RollScores()
        {
            int strength = Roll.RollStat();
            int dexterity = Roll.RollStat();
            int constitution = Roll.RollStat();
            int intelligence = Roll.RollStat();
            int wisdom = Roll.RollStat();
            int charisma = Roll.RollStat();
            AbilityScores scores = new(strength, dexterity, constitution, intelligence, wisdom, charisma);
            return scores;
        }
        public static AbilityScores GenerateScores(AbilityScores bonuses)
        {
            AbilityScores scores = RollScores();
            scores = Add(scores,bonuses);
            return scores;
        }
        public AbilityScores() { }

        public AbilityScores(int strength = 0, int dexterity = 0, int constitution = 0, int intelligence = 0, int wisdom = 0, int charisma = 0)
        {
            this.strength = strength;
            this.dexterity = dexterity;
            this.constitution = constitution;
            this.intelligence = intelligence;
            this.wisdom = wisdom;
            this.charisma = charisma;
        }

        public override int GetHashCode() => (strength, dexterity, constitution, intelligence, wisdom, charisma).GetHashCode();

        public override bool Equals(object? obj) => this.Equals(obj as AbilityScores);

        public bool Equals(AbilityScores? scores)
        {
            if (scores == null) return false;

            if (ReferenceEquals(this,scores)) return true;

            if (this.GetType() != scores.GetType()) return false;

            return (scores.strength == strength) && (scores.dexterity == dexterity) && (scores.constitution == constitution) && (scores.intelligence == intelligence) && (scores.wisdom == wisdom) && (scores.charisma == charisma);
        }

        public static bool operator ==(AbilityScores? left, AbilityScores? right)
        {
            if (left is null)
            {
                if (right is null)
                {
                    return true;
                }

                // Only the left side is null.
                return false;
            }
            // Equals handles case of null on right side.
            return left.Equals(right);
        }

        public static bool operator !=(AbilityScores left, AbilityScores right) => !(left == right);

        public static AbilityScores Add(AbilityScores statsA, AbilityScores statsB)
        {
            int strength = Math.Clamp(statsA.strength + statsB.strength, 0, max);
            int dexterity = Math.Clamp(statsA.dexterity + statsB.dexterity, 0, max);
            int constitution = Math.Clamp(statsA.constitution + statsB.constitution, 0, max);
            int intelligence = Math.Clamp(statsA.intelligence + statsB.intelligence, 0, max);
            int wisdom = Math.Clamp(statsA.wisdom + statsB.wisdom, 0, max);
            int charisma = Math.Clamp(statsA.charisma + statsB.charisma, 0, max);
            AbilityScores newStats = new(strength, dexterity, constitution, intelligence, wisdom, charisma);
            return newStats;
        }

        public override string ToString()
        {
            return
$@"Strength: {strength}
Dexterity:{dexterity}
Constitution: {constitution}
Intelligence: {intelligence}
Wisdom: {wisdom}
Charisma: {charisma}";
        }
    }
}
