using System;
using System.Collections.Generic;
using System.IO;
using FinchAPI;

namespace Project_FinchControl
{

    // **************************************************
    //
    // Title: Finch Control - Menu 
    // Description: Solution with the helper methods,
    //              opening and closing screens, and the menu
    // Application Type: Console
    // Author: Robitshek, Noah
    // Dated Created: 5/31/2020
    // Last Modified: 5/51/2020
    //
    // **************************************************

    class Program
    {
        /// <summary>
        /// first method run when the app starts up
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            SetTheme();

            DisplayWelcomeScreen();
            DisplayMenuScreen();
            DisplayClosingScreen();
        }

        /// <summary>
        /// setup the console theme
        /// </summary>
        static void SetTheme()
        {
            Console.ForegroundColor = ConsoleColor.DarkBlue;
            Console.BackgroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// *****************************************************************
        /// *                     Main Menu                                 *
        /// *****************************************************************
        /// </summary>
        static void DisplayMenuScreen()
        {
            Console.CursorVisible = true;

            bool quitApplication = false;
            string menuChoice;

            Finch finchRobot = new Finch();

            do
            {
                DisplayScreenHeader("Main Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Connect Finch Robot");
                Console.WriteLine("\tb) Talent Show");
                Console.WriteLine("\tc) Data Recorder");
                Console.WriteLine("\td) Alarm System");
                Console.WriteLine("\te) User Programming");
                Console.WriteLine("\tf) Disconnect Finch Robot");
                Console.WriteLine("\tq) Quit");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine();//.ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        DisplayConnectFinchRobot(finchRobot);
                        break;

                    case "b":
                        DisplayTalentShowMenuScreen(finchRobot);
                        break;

                    case "c":

                        break;

                    case "d":

                        break;

                    case "e":

                        break;

                    case "f":
                        DisplayDisconnectFinchRobot(finchRobot);
                        break;

                    case "q":
                        DisplayDisconnectFinchRobot(finchRobot);
                        quitApplication = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitApplication);
        }

        #region TALENT SHOW

        /// <summary>
        /// *****************************************************************
        /// *                     Talent Show Menu                          *
        /// *****************************************************************
        /// </summary>
        static void DisplayTalentShowMenuScreen(Finch myFinch)
        {
            Console.CursorVisible = true;

            bool quitTalentShowMenu = false;
            string menuChoice;

            do
            {
                DisplayScreenHeader("Talent Show Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Light and Sound");
                Console.WriteLine("\tb) Dance, Dance, Baby");
                Console.WriteLine("\tc) Close Encounters with Aliens");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        DisplayLightAndSound(myFinch);
                        break;

                    case "b":
                        DisplayDance(myFinch);
                        break;

                    case "c":
                        DisplayMixingItUp(myFinch);
                        break;


                    case "q":
                        quitTalentShowMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitTalentShowMenu);
        }

        /// <summary>
        /// *****************************************************************
        /// *               Talent Show > Light and Sound                   *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void DisplayLightAndSound(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Light and Sound");

            Console.WriteLine("\tThe Finch robot will not show off its Crazy lights!");

            DisplayContinuePrompt();
            Console.WriteLine();
            Console.WriteLine("Light show...commencing");

            //
            // Strobe light loop
            //
            for (int lightSoundLevel = 0; lightSoundLevel < 255; lightSoundLevel++)
            {
                finchRobot.setLED(lightSoundLevel, 0, 0);
                finchRobot.wait(10);
                finchRobot.setLED(0, lightSoundLevel, 0);
                finchRobot.wait(10);
                finchRobot.setLED(0, 0, lightSoundLevel);
                finchRobot.wait(10);
            }
            //
            // Tone raise loop
            //
            for (int soundLev = 50; soundLev < 900; soundLev += 50)
            {
                finchRobot.noteOn(soundLev);
                finchRobot.wait(100);

            }
            //
            // Tone lower loop
            //
            for (int soundLev = 900; soundLev > 50; soundLev -= 50)
            {
                finchRobot.noteOn(soundLev);
                finchRobot.wait(100);

            }
            //
            // Reset finch
            //
            finchRobot.noteOff();
            finchRobot.setLED(0, 0, 0);

            DisplayMenuPrompt("Talent Show");
        }

        static void DisplayDance(Finch finchRobot)
        {
            
            string answ; // User response variable

            DisplayScreenHeader("Dance, Dance, Baby");

            Console.WriteLine("\tThe Finch robot will now break some crazy moves!!");

            DisplayContinuePrompt();

            //
            // Call dancing method
            //
            FigureEight(finchRobot);

            Console.WriteLine();
            //
            // Ask user if thay want the robot to dance
            //
            Console.Write("Would you like the finch to do\" The Scarn\" \"yes\" or \"no\"? ");
            answ = Console.ReadLine();
            Console.WriteLine();
            //
            // If the user confirms the answer then the robot will dance
            //
            if (answ == "yes" || answ == "Yes" || answ == "sure")
            {
                finchRobot.setMotors(0, -100);
                finchRobot.wait(1000);
                finchRobot.setMotors(-100, 0);
                finchRobot.wait(1000);
                finchRobot.setMotors(0, -100);
                finchRobot.wait(1000);
                finchRobot.setMotors(-100, 0);
                finchRobot.wait(1000);
                finchRobot.setMotors(0, 0);
            }
            else
            {
                Console.WriteLine("ok");
            }

            DisplayMenuPrompt("Talent Show");
        }

        static void DisplayMixingItUp(Finch finchRobot)
        {
            //
            // Display Header
            //
            DisplayScreenHeader("Close Encounters with Aliens");
            //
            //
            //
            Console.WriteLine("\tThe Finch robot enact \"Close Encounters with Aliens\" mode");
            Console.WriteLine();
            Console.WriteLine("Mr. Finch will play tunes and display the lights all at the same time!!!");
            DisplayContinuePrompt();
            //
            // Play tones with lights 
            //
            finchRobot.noteOn(390);
            finchRobot.setLED(255, 0, 0);
            finchRobot.wait(1000);
            finchRobot.noteOn(440);
            finchRobot.setLED(0, 255, 0);
            finchRobot.wait(1000);
            finchRobot.noteOn(350);
            finchRobot.setLED(0, 0, 255);
            finchRobot.wait(1000);
            finchRobot.noteOn(174);
            finchRobot.setLED(255, 0, 0);
            finchRobot.wait(1000);
            finchRobot.noteOn(261);
            finchRobot.setLED(0, 255, 0);
            finchRobot.wait(2000);
            finchRobot.noteOff();

            DisplayMenuPrompt("Talent Show");

        }

        static void FigureEight(Finch finchRobot) // Dance move method
        {
            finchRobot.setMotors(0, 100);
            finchRobot.wait(2500);
            finchRobot.setMotors(100, 0);
            finchRobot.wait(2500);
            finchRobot.setMotors(0, 0);
            finchRobot.setMotors(0, 100);
            finchRobot.wait(2500);
            finchRobot.setMotors(100, 0);
            finchRobot.wait(2500);
            finchRobot.setMotors(0, 0);
        }
        #endregion

        #region DATA RECORDER

        #endregion

        #region FINCH ROBOT MANAGEMENT

        /// <summary>
        /// *****************************************************************
        /// *               Disconnect the Finch Robot                      *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        static void DisplayDisconnectFinchRobot(Finch finchRobot)
        {
            Console.CursorVisible = false;

            DisplayScreenHeader("Disconnect Finch Robot");

            Console.WriteLine("\tAbout to disconnect from the Finch robot.");
            DisplayContinuePrompt();

            finchRobot.disConnect();

            Console.WriteLine("\tThe Finch robot is now disconnect.");

            DisplayMenuPrompt("Main Menu");
        }

        /// <summary>
        /// *****************************************************************
        /// *                  Connect the Finch Robot                      *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot">finch robot object</param>
        /// <returns>notify if the robot is connected</returns>
        static bool DisplayConnectFinchRobot(Finch finchRobot)
        {
            Console.CursorVisible = false;

            bool robotConnected;

            DisplayScreenHeader("Connect Finch Robot");

            Console.WriteLine("\tAbout to connect to Finch robot. Please be sure the USB cable is connected to the robot and computer now.");
            DisplayContinuePrompt();

            robotConnected = finchRobot.connect();

            // TODO test connection and provide user feedback - text, lights, sounds

            DisplayMenuPrompt("Main Menu");

            //
            // reset finch robot
            //
            finchRobot.setLED(0, 0, 0);
            finchRobot.noteOff();

            return robotConnected;
        }

        #endregion

        #region USER INTERFACE

        /// <summary>
        /// *****************************************************************
        /// *                     Welcome Screen                            *
        /// *****************************************************************
        /// </summary>
        static void DisplayWelcomeScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tFinch Control");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// *****************************************************************
        /// *                     Closing Screen                            *
        /// *****************************************************************
        /// </summary>
        static void DisplayClosingScreen()
        {
            Console.CursorVisible = false;

            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\tThank you for using Finch Control!");
            Console.WriteLine();

            DisplayContinuePrompt();
        }

        /// <summary>
        /// display continue prompt
        /// </summary>
        static void DisplayContinuePrompt()
        {
            Console.WriteLine();
            Console.WriteLine("\tPress any key to continue.");
            Console.ReadKey();
        }

        /// <summary>
        /// display menu prompt
        /// </summary>
        static void DisplayMenuPrompt(string menuName)
        {
            Console.WriteLine();
            Console.WriteLine($"\tPress any key to return to the {menuName} Menu.");
            Console.ReadKey();
        }

        /// <summary>
        /// display screen header
        /// </summary>
        static void DisplayScreenHeader(string headerText)
        {
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("\t\t" + headerText);
            Console.WriteLine();
        }

        #endregion
    }
}
