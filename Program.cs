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
        private static float ProbabilityOfSpreadingTheRumor = 0.5f;

        struct Person
        {
            public int Id;
            public Boolean HasHeardRumor;
            public int InformedByCount;
            public int FirstInformedBy;
            public int SecondInformedBy;
            public float ProbabilityOfSpreadingTheRumor;
        };

        struct CommunitySpreadStatistics
        {
            public int MinutesElapsed;
            public int CountAwareOfRumor;
            public float PercentAwareOfRumor;
        }

        
        static void Main(string[] args)
        {
            bool exit_program = false;

            //Program Control Flow 
            while (!exit_program)
            {
                switch (DisplayMainMenu())
                {
                    case 1: //Display Prompt
                        DisplayPrompt();
                        break;
                    case 2: //Run Simulation for 100 Attendees
                        SimulateAverageRumorSpread(10, 40, 100);
                        break;
                    case 3: //Run Simulation for 1000 Attendees
                        SimulateAverageRumorSpread(10, 40, 1000);
                        break;
                    case 4: //Run Simulation for 10000 Attendees
                        SimulateAverageRumorSpread(10, 40, 10000);
                        break;
                    case 6: //Run Simulation for Averages of  N = {100, 1,000, 10,000} Attendees
                        //SimulateMultipleAverageRumorSpread(1, 100);
                        break;
                    case 5: //Exit Simulation
                        exit_program = true;
                        break;
                    default:
                        PressEnterToReturn("ERROR: Please Provide a Valid Numerical Input and Try Again!");
                        break;
                }

                Console.Clear();
            }
        }

        #region User Interface Functions
         public static int DisplayMainMenu()
        {
            int selection_code = 0;
            Console.WriteLine("|=====================================================|");
            Console.WriteLine("|       Welcome to the Rumor Spread Simulation!       |");
            Console.WriteLine("|=====================================================|\n\n");
            Console.WriteLine("Please Select an Action to Perform:");
            Console.WriteLine("\n\t1.] Display Prompt");
            Console.WriteLine("\t2.] Run Simulation for 100 Attendees");
            Console.WriteLine("\t3.] Run Simulation for 1000 Attendees");
            Console.WriteLine("\t4.] Run Simulation for 10000 Attendees");
            //Console.WriteLine("\t5.] Run Simulation for Averages of  N = {100, 1,000, 10,000} Attendees");
            Console.WriteLine("\t5.] Exit Simulation");

            Console.Write("\nInput: ");
            string Input = Console.ReadLine();

            try
            {
                selection_code = Int32.Parse(Input);
            }
            catch (FormatException)
            {
                //Console.WriteLine("Please Provide a Valid Numerical Input and Try Again!\n");
                selection_code = -1;
            }
            return selection_code;
        }

        public static void DisplayPrompt()
        {
            Console.WriteLine("\n\nN students are at a party (N is an even integer 2 ≤ N ≤ 10,000)." +
                              "\nAt some point, all students pair off at random and talk for exactly one minute." +
                              "\nAt the end of the minute, all students again pair off with another person at random." +
                              "\nOne student wants to start a rumor.He spreads the rumor to his conversation partner at noon." +
                              "\nEvery person who has knowledge of the rumor obeys these rules:" +
                              "\n\t1.The likelihood of spreading a rumor to another person is 0.5" +
                              "\n\t2.After a person has heard the rumor 2 times, he/she will assume everyone has heard the rumor and will no longer try to spread it further.\n" );
            Console.WriteLine("\n\nAssumptions Made for this Implementation:" +
                              "\n\t1.] All party sizes will consist of equal number of attendees (i.e. no groups of 3 or individuals left out)." +
                              "\n\t2.] When 2 people converse only one person can tell the rumor." +
                              "\n\t3.] When 2 people converse only one person can hear the rumor." +
                              "\n\t4.] When the simulation starts the initial person spreading the rumor is counted as having heard the rumor." +
                              "\n\t5.] The person hearing the rumor must hear it it from 2 unique people before he/she stops spreading the rumor.");
            PressEnterToReturn("Please press ENTER to return to the main menu...");

        }

        public static void PressEnterToReturn(string DisplayText)
        {
            Console.WriteLine("\n" + DisplayText + "\n");
            string DummyInput = Console.ReadLine();
        }

        #endregion


        #region Utility Functions
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

        private static void SimulateAverageRumorSpread(int NumberOfRuns, int DurationInMinutes, int NumberOfParticipants)
        {
            CommunitySpreadStatistics[,] TraceFileData = new CommunitySpreadStatistics[NumberOfRuns + 1, DurationInMinutes];
            CommunitySpreadStatistics[] PartyTrace     = new CommunitySpreadStatistics[DurationInMinutes];
            for (int RunCount = 0; RunCount < NumberOfRuns; RunCount++)
            {
                PartyTrace = SimulateParty(DurationInMinutes, NumberOfParticipants, true);
                for(int i =0; i < DurationInMinutes; i++)
                {
                    TraceFileData[RunCount, i] = PartyTrace[i];

                    //Populate The Average Structure
                    TraceFileData[NumberOfRuns, i].MinutesElapsed = i;
                    TraceFileData[NumberOfRuns, i].PercentAwareOfRumor += PartyTrace[i].PercentAwareOfRumor;
                    TraceFileData[NumberOfRuns, i].CountAwareOfRumor   += PartyTrace[i].CountAwareOfRumor;
                }
            }

            //Calculate the Averages in the Index of the Structure
            for(int i = 0; i < DurationInMinutes; i++)
            {
                TraceFileData[NumberOfRuns, i].PercentAwareOfRumor = TraceFileData[NumberOfRuns, i].PercentAwareOfRumor / NumberOfRuns;
                TraceFileData[NumberOfRuns, i].CountAwareOfRumor   = TraceFileData[NumberOfRuns, i].CountAwareOfRumor   / NumberOfRuns;
            }

            GenerateFile(TraceFileData, NumberOfRuns, NumberOfParticipants, DurationInMinutes);
        }

        private static void GenerateFile(CommunitySpreadStatistics[,] TraceFile, int NumberOfRuns, int NumberOfParticipants, int DurationInMinutes)
        {
            string path = @"Rumor_Spread_Simulation_for_N_Equals_" + NumberOfParticipants.ToString() + @".txt";
            try
            {
                // Create the file, or overwrite if the file exists.
                using (FileStream fs = File.Create(path))
                {
                    byte[] infoh = new UTF8Encoding(true).GetBytes("Minutes_Elapsed");
                    fs.Write(infoh, 0, infoh.Length);

                    for(int i = 0; i < NumberOfRuns + 1; i++)
                    {
                        if(i == NumberOfRuns)
                        {
                            infoh = new UTF8Encoding(true).GetBytes("," + "Average_Percent_Aware" + "\n");
                        }
                        else
                        {
                            infoh = new UTF8Encoding(true).GetBytes("," + "Run_#_" + (i+1).ToString() + "_Percent_Aware" );
                        }
                        fs.Write(infoh, 0, infoh.Length);
                    }

                   for(int i = 0; i < DurationInMinutes; i++)
                   {
                        byte[] info = new UTF8Encoding(true).GetBytes(TraceFile[0, i].MinutesElapsed.ToString());
                        fs.Write(info, 0, info.Length);
                        for (int j = 0; j < NumberOfRuns + 1; j++)
                        {
                            info = new UTF8Encoding(true).GetBytes("," + TraceFile[j, i].PercentAwareOfRumor.ToString());
                            fs.Write(info, 0, info.Length);
                        }
                        info = new UTF8Encoding(true).GetBytes("\n");
                        fs.Write(info, 0, info.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }

        private static CommunitySpreadStatistics[] SimulateParty(int DurationInMinutes, int NumberOfGuests, Boolean OnlyCountUniqueSpread)
        {
            //Initialize The Trace Array
            CommunitySpreadStatistics[] PartyTrace = new CommunitySpreadStatistics[DurationInMinutes];

            //Initialize Guests and Pairing List
            Person[] GuestList = new Person[NumberOfGuests];
            int[] PairingList = new int[NumberOfGuests];

            for (int i = 0; i < NumberOfGuests; i++)
            {
                //Generate Guest
                if (i == 0)
                {
                    GuestList[i].HasHeardRumor = true;
                }
                else
                {
                    GuestList[i].HasHeardRumor = false;
                }

                GuestList[i].FirstInformedBy  = -1;
                GuestList[i].SecondInformedBy = -1;
                GuestList[i].InformedByCount  = 0;
                GuestList[i].ProbabilityOfSpreadingTheRumor = 0.5f;
                GuestList[i].Id = i;

                //Add Guest ID to Pairing List
                PairingList[i] = i;
            }

            //Simulate Party
            for (int TimeElapsed = 0; TimeElapsed < DurationInMinutes; TimeElapsed++)
            {
                PairingList = FisherYatesShuffle(PairingList, PairingList.Length);
                PartyTrace[TimeElapsed].MinutesElapsed = TimeElapsed + 1;

                //Assume that only an Even Number of Guests are Present
                for (int i = 1; i < NumberOfGuests; i += 2 )
                {
                    int PersonAIndex = i - 1;
                    int PersonBIndex = i;
                    Person PersonA = GuestList[PairingList[PersonAIndex]];
                    Person PersonB = GuestList[PairingList[PersonBIndex]];
                    Boolean PersonAWantsToTellRumor = WillSpreadRumor(GuestList[i - 1]);
                    Boolean PersonBWantsToTellRumor = WillSpreadRumor(GuestList[i]);

                    //Assume once one person brings the rumor it discourages their conversation partner from bringing it up
                    if(PersonAWantsToTellRumor && PersonBWantsToTellRumor)
                    {
                        Random r = new Random();
                        if (r.NextDouble() < 0.5)
                        {
                            PersonAWantsToTellRumor = false;
                        }
                        else
                        {
                            PersonBWantsToTellRumor = false;
                        }
                    }

                    //Person A Tells Person B
                    if (PersonAWantsToTellRumor)
                    {
                        if (OnlyCountUniqueSpread && PersonB.FirstInformedBy != PersonA.Id)
                        {
                            PersonB.InformedByCount += 1;
                            PersonB.HasHeardRumor   = true;
                            PersonB.FirstInformedBy = PersonA.Id;

                        }
                        else
                        {
                            PersonB.InformedByCount += 1;
                            PersonB.HasHeardRumor = true;
                            PersonB.FirstInformedBy = PersonA.Id;
                        }
                    }

                    //Person B Tells Person A
                    if (PersonBWantsToTellRumor)
                    {
                        if (OnlyCountUniqueSpread && PersonA.FirstInformedBy != PersonB.Id)
                        {
                            PersonA.InformedByCount += 1;
                            PersonA.HasHeardRumor = true;
                            PersonA.FirstInformedBy = PersonB.Id;

                        }
                        else
                        {
                            PersonB.InformedByCount += 1;
                            PersonB.HasHeardRumor = true;
                            PersonB.FirstInformedBy = PersonB.Id;
                        }

                    } 
                    
                    //Update Trace and Record Changes
                    if(PersonA.HasHeardRumor == true)
                    {
                        PartyTrace[TimeElapsed].CountAwareOfRumor += 1;

                    }
                    GuestList[PairingList[PersonAIndex]] = PersonA;

                    if (PersonB.HasHeardRumor == true)
                    {
                        PartyTrace[TimeElapsed].CountAwareOfRumor += 1;
                    }
                    GuestList[PairingList[PersonBIndex]] = PersonB;

                }

                PartyTrace[TimeElapsed].PercentAwareOfRumor = (float)PartyTrace[TimeElapsed].CountAwareOfRumor / NumberOfGuests;
            }

            return PartyTrace;
        }

        private static Boolean WillSpreadRumor(Person person)
        {
            Random r = new Random();
            if (person.HasHeardRumor == true && r.NextDouble() < person.ProbabilityOfSpreadingTheRumor && person.InformedByCount < 2)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion        

    }
}
