/*
Title:
  Rumor Spread Simulation

Created By:
  Michael Bramer

Created On:
  04/09/2021

Prompt:
  N students are at a party (N is an even integer 2 ≤ N ≤ 10,000)
  At some point, all students pair off at random and talk for exactly one minute.
  At the end of the minute, all students again pair off with another person at random.
  One student wants to start a rumor. He spreads the rumor to his conversation partner at noon.
  Every person who has knowledge of the rumor obeys these rules:
    1. The likelihood of spreading a rumor to another person is 0.5
    2. After a person has heard the rumor 2 times, he/she will assume everyone has heard the rumor and will no longer try to spread it further. 

Questions:
    For N = {100, 1,000, 10,000} run your simulation several times to determine:
        1. On average, what % of the attendees will have heard the rumor after 10 minutes?
        2. On average, what % of the attendees will have heard the rumor after 20 minutes?
        3. On average, what % of the attendees will have heard the rumor after 40 minutes?
        4. At what time, t, will 10% of the party have heard the rumor? (N = 10,000).
        5. At what time, t, will 50% of the party have heard the rumor? (N = 10,000). 
*/


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Globalization;
using System.ComponentModel;

namespace RumorSpreadSimulation
{
    class Program
    {
        private static int SuspendSpreadLimit = 2;
        private static int NumberOfGuest = 100;
        private static float ProbabilityOfSpreadingTheRumor = 0.5f;

        struct Person
        {
            public Boolean HasHeardRumor;
            public int InformedOfRumorCount;
            public float ProbabilityOfSpreadingTheRumor;
        };


        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }

        private static Person InitializePerson( Boolean HasHeardRumor, int InformedOfRumorCount, float ProbabilityOfSpread)
        {
            Person Guest;
            Guest.InformedOfRumorCount           = InformedOfRumorCount;
            Guest.HasHeardRumor                  = HasHeardRumor;
            Guest.ProbabilityOfSpreadingTheRumor = ProbabilityOfSpread;
            return Guest;
        }

        //Refrenced From: https://www.geeksforgeeks.org/shuffle-a-given-array-using-fisher-yates-shuffle-algorithm/
        private static int[] FisherYatesShuffle(int[] arr, int size)
        {
            // Creating a object for Random class
            Random r = new Random();

            // Start from the last element and swap one by one. 
            // We don't need to run for the first element that's why i > 0
            for (int i = size - 1; i > 0; i--)
            {

                // Pick a random index from 0 to i
                int j = r.Next(0, i + 1);

                // Swap arr[i] with the element at random index
                int temp = arr[i];
                arr[i] = arr[j];
                arr[j] = temp;
            }

            //Returned the Schuffled Int Array 
            return arr;
        }






    }
}
