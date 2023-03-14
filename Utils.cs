using System;
using System.Linq;

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
        public int strength;
        public int dexterity;
        public int constitution;
        public int intelligence;
        public int wisdom;
        public int charisma;

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

        public AbilityScores(int strength = 0, int dexterity = 0, int constitution = 0, int intelligence = 0, int wisdom = 0, int charisma = 0)
        {
            this.strength = strength;
            this.dexterity = dexterity;
            this.constitution = constitution;
            this.intelligence = intelligence;
            this.wisdom = wisdom;
            this.charisma = charisma;
        }

        public static AbilityScores Add(AbilityScores statsA, AbilityScores statsB)
        {
            int strength = Math.Clamp(statsA.strength + statsB.strength, 1, 20);
            int dexterity = Math.Clamp(statsA.dexterity + statsB.dexterity, 1, 20);
            int constitution = Math.Clamp(statsA.constitution + statsB.constitution, 1, 20);
            int intelligence = Math.Clamp(statsA.intelligence + statsB.intelligence, 1, 20);
            int wisdom = Math.Clamp(statsA.wisdom + statsB.wisdom, 1, 20);
            int charisma = Math.Clamp(statsA.charisma + statsB.charisma, 1, 20);
            AbilityScores newStats = new(strength, dexterity, constitution, intelligence, wisdom, charisma);
            return newStats;
        }
    }
}
