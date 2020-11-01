using System;
using System.Collections.Generic;
using System.Linq;

namespace confirma_pay_technical_test
{
    class Game
    {
        private int target;
        private int oldTarget;
        private Random r;

        private Dictionary<char, int> guessDict = new Dictionary<char, int> {{'h', 1}, {'l', -1}};

        public Game()
        {
            r = new Random();
            target = r.Next(1, 101);
        }

        public bool Play()
        {
            Console.WriteLine($"The last number generated was {target}.");
            Console.WriteLine("Please guess if the next number will be (H)igher or (L)ower:");
            oldTarget = target;
            target = GetNextTarget();
            char response = Utils.GetInput(guessDict.Keys.ToArray());

            if (target.CompareTo(oldTarget) == guessDict[response])
            {
                Console.WriteLine("Congratulations!");
                return true;
            }

            Console.WriteLine($"Uh-oh! The number was actually {target} - you lose!");
            return false;
        }

        private int GetNextTarget()
        {
            int retval;
            do
            {
                retval = r.Next(1, 101);
            } while (retval == target);

            return retval;
        }
    }
}