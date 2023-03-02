using System;
using System.Linq;

namespace Utils
{
    public static class Roll
    {
        static Random rand = new Random();

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
            int[] results = new int[] { };
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
            int[] results = new int[] { };
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
            int[] results = new int[] { };
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
            int[] results = new int[] { };
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
            int[] results = new int[] { };
            for (int i = 0; i < n; i++)
            {
                results = results.Append(RollDice(12)).ToArray();
            }
            return results;
        }

        public static int[] Rolld20(int n)
        {
            int[] results = new int[] { };
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

        public static int[] RollStatArray(int[] bonuses)
        {
            int[] scores = new int[6];
            for (int i = 0; i < 6; i++)
            {
                scores[i] = RollStat() + bonuses[i];
            }
            return scores;
        }
    }
}
