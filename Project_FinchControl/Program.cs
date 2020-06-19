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
    public enum Move
    {
        NONE,
        MOVEFORWARD,
        MOVEBACKWARD,
        STOPMOTORS,
        WAIT,
        TURNRIGHT,
        TURNLEFT,
        LEDON,
        LEDOFF,
        GETTEMPERATURE,
        DONE
    }

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
                        DataRecorderMenuScreen(finchRobot);
                        break;

                    case "d":
                        LightAlarmMenuScreen(finchRobot);
                        break;

                    case "e":
                        UserProgramMenuSreen(finchRobot);
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

        #region User Programming
        private static void UserProgramMenuSreen(Finch finchRobot)
        {

            bool quitwMenu = false;
            string menuChoice;

            (int motorSpeed, int ledBrightness, double waitSeconds) commandParamaters;
            commandParamaters.motorSpeed = 0;
            commandParamaters.ledBrightness = 0;
            commandParamaters.waitSeconds = 0;

            List<Move> commands = new List<Move>();
            do
            {
                DisplayScreenHeader("User Programming Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Set Command Parameters");
                Console.WriteLine("\tb) Add Commands");
                Console.WriteLine("\tc) View Commands");
                Console.WriteLine("\td) Execute Commands");
                Console.WriteLine("\tq) Quit");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        commandParamaters = UserProgramingSetCommandParameters();
                        break;

                    case "b":
                        UserProgramingGetFinchCommands(commands);
                        break;

                    case "c":
                        UserProgramingDisplayFinchCommands(commands);
                        break;

                    case "d":
                        UserPragramingDisplayExecuteFinchCommands(finchRobot, commands, commandParamaters);
                        break;

                    case "q":
                        quitwMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitwMenu);
        }

        private static void UserPragramingDisplayExecuteFinchCommands(Finch finchRobot,
            List<Move> commands,
            (int motorSpeed, int ledBrightness, double waitSeconds) commandParamaters)
        {
            int motorSpeed = commandParamaters.motorSpeed;
            int ledBrightness = commandParamaters.ledBrightness;
            int waitMillaseconds = (int)(commandParamaters.waitSeconds * 1000);
            string commandFeedback = "";
            const int TURNING_SPEED = 100;

            DisplayScreenHeader("Execute Finch Commands");

            Console.WriteLine("\tMr. Finch is ready to execute the commands");
            DisplayContinuePrompt();

            foreach (Move command in commands)
            {
                switch (command)
                {
                    case Move.DONE:
                        commandFeedback = Move.DONE.ToString();
                        break;

                    case Move.GETTEMPERATURE:
                        commandFeedback = $"Tempature: {finchRobot.getTemperature().ToString("n2")}";
                        break;

                    case Move.WAIT:
                        finchRobot.wait(waitMillaseconds);
                        commandFeedback = Move.WAIT.ToString();
                        break;

                    case Move.LEDOFF:
                        finchRobot.setLED(0, 0, 0);
                        commandFeedback = Move.LEDOFF.ToString();
                        break;


                    case Move.LEDON:
                        finchRobot.setLED(ledBrightness, ledBrightness, ledBrightness);
                        commandFeedback = Move.LEDON.ToString();
                        break;

                    case Move.NONE:
                        break;

                    case Move.STOPMOTORS:
                        finchRobot.setMotors(0, 0);
                        commandFeedback = Move.STOPMOTORS.ToString();
                        break;

                    case Move.TURNLEFT:
                        finchRobot.setMotors(-TURNING_SPEED, TURNING_SPEED);
                        commandFeedback = Move.TURNLEFT.ToString();
                        break;

                    case Move.TURNRIGHT:
                        finchRobot.setMotors(TURNING_SPEED, -TURNING_SPEED);
                        commandFeedback = Move.TURNRIGHT.ToString();
                        break;

                    case Move.MOVEBACKWARD:
                        finchRobot.setMotors(-motorSpeed, -motorSpeed);
                        commandFeedback = Move.MOVEBACKWARD.ToString();
                        break;

                    case Move.MOVEFORWARD:
                        finchRobot.setMotors(motorSpeed, motorSpeed);
                        commandFeedback = Move.MOVEFORWARD.ToString();
                        break;

                    default:
                        break;
                }

                Console.WriteLine($"\t{commandFeedback}");

            }
            DisplayContinuePrompt();
        }



        private static void UserProgramingDisplayFinchCommands(List<Move> commands)
        {
            DisplayScreenHeader("Finch Robot Commands");

            foreach (Move command in commands)
            {
                Console.WriteLine($"\t{command}");
            }

            DisplayMenuPrompt("User Programming");
        }

        private static void UserProgramingGetFinchCommands(List<Move> commands)
        {
            Move command = Move.DONE;

            DisplayScreenHeader("Finch Robot Comamnds");

            Console.WriteLine("Here are all of the available commands");

            DisplayContinuePrompt();

            foreach (string movement in Enum.GetNames(typeof(Move)))
            {
                if (movement != "NONE")
                {
                    Console.Write("   ");
                    Console.WriteLine(movement.ToLower().ToString());

                }
            }
            Console.WriteLine();
            Console.WriteLine("\tEnter the commands you would like the robot to take part in");
            do
            {
                Console.Write("\tEnter Commnd: ");
                if (Enum.TryParse(Console.ReadLine().ToUpper(), out command))
                {
                    commands.Add(command);
                }
                else
                {
                    Console.WriteLine("****************************************");
                    Console.WriteLine("Please enter one of the correct commands");
                    Console.WriteLine("****************************************");
                }
            } while (command != Move.DONE);

            DisplayMenuPrompt("User Programming");
        }

        private static (int motorSpeed, int ledBrightness, double waitSeconds) UserProgramingSetCommandParameters()
        {
            (int motorSpeed, int ledBrightness, double waitSeconds) commandParamaters;
            commandParamaters.motorSpeed = 0;
            commandParamaters.ledBrightness = 0;
            commandParamaters.waitSeconds = 0;

            bool goNogo = false;

            DisplayScreenHeader("Command Parameter");

            do
            {
                Console.Write("\tEnter Motor Speed [0 - 255]: ");
                goNogo = int.TryParse(Console.ReadLine(), out commandParamaters.motorSpeed);
                Console.WriteLine();

            } while (!goNogo || commandParamaters.motorSpeed < 0 || commandParamaters.motorSpeed > 255);

            do
            {
                Console.Write("\tEnter LED Brightness [0 - 255]: ");
                goNogo = int.TryParse(Console.ReadLine(), out commandParamaters.ledBrightness);
                Console.WriteLine();

            } while (!goNogo || commandParamaters.ledBrightness < 0 || commandParamaters.ledBrightness > 255);


            do
            {
                Console.Write("\tEnter Wait in seconds [0 - 10]: ");
                goNogo = double.TryParse(Console.ReadLine(), out commandParamaters.waitSeconds);
                Console.WriteLine();

            }
            while (!goNogo || commandParamaters.waitSeconds < 0 || commandParamaters.waitSeconds > 10);
            Console.WriteLine();
            Console.WriteLine($"\tMotor Speed: {commandParamaters.motorSpeed}");
            Console.WriteLine($"\tLED Brightness: {commandParamaters.ledBrightness}");
            Console.WriteLine($"\tWait Command Duration: {commandParamaters.waitSeconds}");

            DisplayContinuePrompt();
            return commandParamaters;
        }
        #endregion

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

        #region Alarm System
        /// <summary>
        /// *****************************************************************
        /// *                     Light Alarm Menu                          *
        /// *****************************************************************
        /// </summary>
        /// <param name="finchRobot"></param>
        private static void LightAlarmMenuScreen(Finch finchRobot)
        {
            Console.CursorVisible = true;
            bool quitwMenu = false;
            string sensorToMonitor = "";
            string rangeType = "";
            string sensorAmp = "";
            int minMaxThresholdValue = 0;
            int timeToMonitor = 0;

            do
            {
                DisplayScreenHeader("Light Alarm Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Set Sensors to Monitor");
                Console.WriteLine("\tb) Set Range Type");
                Console.WriteLine("\tc) Set Min/Max Threshold");
                Console.WriteLine("\td) Set Time to Monitor");
                Console.WriteLine("\te) Set alarm");
                Console.WriteLine("\tq) Quit");
                Console.Write("\t\tEnter Choice:");
                string menuChoice = Console.ReadLine().ToLower();


                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        sensorToMonitor = LightAlarmDisplaySetSensorsToMonitor();
                        sensorAmp = AmountOfSensors(sensorToMonitor);

                        break;

                    case "b":
                        rangeType = LightAlarmDisplaySetRangeType();
                        break;

                    case "c":
                        minMaxThresholdValue = LightAlarmDisplaySetMinMaxThreshold(rangeType, finchRobot);
                        break;

                    case "d":
                        timeToMonitor = LightAlarmDisplaySetTimeToMonitor();
                        break;

                    case "e":
                        LightAlarmSetAlarm(finchRobot, sensorToMonitor, rangeType, minMaxThresholdValue, timeToMonitor, sensorAmp);
                        break;

                    case "q":
                        quitwMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }
            } while (!quitwMenu);


        }

        private static string AmountOfSensors(string sensorToMonitor)
        {
            string sensorAmp = "";

            switch (sensorToMonitor)
            {
                case "left":
                    sensorAmp = "sensor";
                    break;

                case "right":
                    sensorAmp = "sensor";
                    break;

                case "both":
                    sensorAmp = "sensors";
                    break;
            }
            return sensorAmp;
        }

        static void LightAlarmSetAlarm(Finch finchRobot, string sensorToMonitor,
                                       string rangeType, int minMaxThresholdValue,
                                       int timeToMonitor, string sensorAmp)
        {
            int secondsElapsed = 0;
            bool thresholdExceded = false;
            int currentLightValue = 0;

            DisplayScreenHeader("Set Alarm");

            Console.WriteLine("Monitoring: {0} {1}", sensorToMonitor, sensorAmp);
            Console.WriteLine("Range type: {0}", rangeType);
            Console.WriteLine("{0} threshold vlaue: {1}", UppercaseFirst(rangeType), minMaxThresholdValue);
            Console.WriteLine("Monitoring for: {0} seconds", timeToMonitor);
            Console.WriteLine();
            Console.WriteLine("Press any key to begin monitoring");
            Console.ReadKey();
            Console.WriteLine();

            while (secondsElapsed < timeToMonitor && !thresholdExceded)
            {
                switch (sensorToMonitor)
                {
                    case "left":
                        currentLightValue = finchRobot.getLeftLightSensor();


                        break;

                    case "right":
                        currentLightValue = finchRobot.getRightLightSensor();

                        break;

                    case "both":
                        currentLightValue = (finchRobot.getRightLightSensor() + finchRobot.getRightLightSensor()) / 2;

                        break;
                }
                switch (rangeType)
                {
                    case "maximum":
                        if (currentLightValue > minMaxThresholdValue)
                        {
                            thresholdExceded = true;
                        }
                        break;

                    case "minimum":
                        if (currentLightValue < minMaxThresholdValue)
                        {
                            thresholdExceded = true;
                        }
                        break;
                }
                finchRobot.wait(1000);
                secondsElapsed++;

            }
            if (thresholdExceded)
            {
                Console.WriteLine($"\tThe current light value of {currentLightValue} has " +
                    $"exceded the {rangeType} value of {minMaxThresholdValue}");
                finchRobot.setLED(255, 0, 0);
            }
            else
            {
                Console.WriteLine($"\tThe current light value of {currentLightValue} has " +
                    $"not exceded the {rangeType} value of {minMaxThresholdValue}");
                finchRobot.setLED(0, 255, 0);
            }

            DisplayMenuPrompt("Light Alarm");

        }

        static int LightAlarmDisplaySetTimeToMonitor()
        {
            int timeToMonitor;
            bool usp;

            DisplayScreenHeader("Time to Monitor");

            do
            {
                Console.Write("\tHow long would you like to monitor the light levels (seconds): ");
                usp = int.TryParse(Console.ReadLine(), out timeToMonitor);
            } while (!usp);
            Console.WriteLine();
            Console.WriteLine($"\tMr. Finch will monitor the light level for {timeToMonitor} seconds");

            DisplayMenuPrompt("Light Alarm");

            return timeToMonitor;
        }

        static int LightAlarmDisplaySetMinMaxThreshold(string rangeType, Finch finchRobot)
        {
            int minMaxThresholdVlaue;
            bool usp;

            DisplayScreenHeader("Minimum/Maximum Threshold Vlaue");

            Console.WriteLine("\tThe left light sensors ambient reading is: {0}", finchRobot.getLeftLightSensor());
            Console.WriteLine("\tThe right light sensors ambient reading is: {0}", finchRobot.getRightLightSensor());
            Console.WriteLine();
            do
            {
                Console.Write("\tEnter the {0} light sensor value: ", rangeType);
                usp = int.TryParse(Console.ReadLine(), out minMaxThresholdVlaue);
            } while (!usp);
            Console.WriteLine();
            Console.WriteLine($"\tMr. finch's threshold has been set at {minMaxThresholdVlaue}");

            DisplayMenuPrompt("Light Alarm");

            return minMaxThresholdVlaue;
        }

        static string LightAlarmDisplaySetSensorsToMonitor()
        {
            string sensorToMonitor = "";
            bool usp = false;
            string sensorAmp = "";
            DisplayScreenHeader("Sensors to Monitor");

            do
            {
                Console.Write("\tWhich sensor would you like to monitor? [left, right, both]: ");
                sensorToMonitor = Console.ReadLine();
                usp = sensorToMonitor == "left" || sensorToMonitor == "right" || sensorToMonitor == "both";
                Console.WriteLine();
            } while (!usp);

            switch (sensorToMonitor)
            {
                case "left":
                    sensorAmp = "sensor";
                    break;

                case "right":
                    sensorAmp = "sensor";
                    break;

                case "both":
                    sensorAmp = "sensors";
                    break;
            }


            Console.WriteLine($"\tMr. Finch will monitor: {sensorToMonitor} {sensorAmp}");
            DisplayMenuPrompt("Light Alarm");



            return sensorToMonitor;
        }

        static string LightAlarmDisplaySetRangeType()
        {
            string rangeType;
            bool usp = false;
            DisplayScreenHeader("Range Type");
            Console.WriteLine("\tHere we will set the maximum or minumim bounds for the testing range");
            Console.WriteLine();
            do
            {
                Console.Write("\tWhould you like the sensor to alarm you when it has reached the \"minimum\" or the \"maximum\": ");
                rangeType = Console.ReadLine();
                usp = rangeType == "minimum" || rangeType == "maximum";
            } while (!usp);
            Console.WriteLine();

            Console.WriteLine($"\tMr. Finch's will monitor the {rangeType} bound");
            DisplayMenuPrompt("Light Alarm");


            return rangeType;
        }
        #endregion

        #region DATA RECORDER

        static void DataRecorderMenuScreen(Finch finchRobot)
        {
            int numberOfDataPoints = 0;
            double dataPointFrequency = 0;
            double[] tempatures = null;

            bool quitwMenu = false;
            string menuChoice;

            do
            {
                DisplayScreenHeader("Data Recorder Menu");

                //
                // get user menu choice
                //
                Console.WriteLine("\ta) Number of Data Points");
                Console.WriteLine("\tb) Frequency of Data Points ");
                Console.WriteLine("\tc) Get Data");
                Console.WriteLine("\td) Show Data");
                Console.WriteLine("\te) Data Analysis");
                Console.WriteLine("\tq) Main Menu");
                Console.Write("\t\tEnter Choice:");
                menuChoice = Console.ReadLine().ToLower();

                //
                // process user menu choice
                //
                switch (menuChoice)
                {
                    case "a":
                        numberOfDataPoints = DataRecorderGetNumberOfDataPoints();
                        break;

                    case "b":
                        dataPointFrequency = DataRecorderDisplayDataPointFrequency();
                        break;

                    case "c":
                        tempatures = DataRecorderDisplayGetData(numberOfDataPoints, dataPointFrequency, finchRobot);
                        break;

                    case "d":
                        DataRecorderDisplayGetData(tempatures);
                        break;

                    case "e":
                        DataRecorderDisplayDataAnalysis(tempatures, numberOfDataPoints);
                        break;

                    case "q":
                        quitwMenu = true;
                        break;

                    default:
                        Console.WriteLine();
                        Console.WriteLine("\tPlease enter a letter for the menu choice.");
                        DisplayContinuePrompt();
                        break;
                }

            } while (!quitwMenu);
        }


        private static void DataRecorderDisplayGetData(double[] tempatures)
        {
            DisplayScreenHeader("Show Data");

            //
            // Table Headers
            //
            Console.WriteLine(
                "Recording #".PadLeft(15) +
                "Tempature".PadLeft(15));
            Console.WriteLine(
                "-----------".PadLeft(15) +
                "---------".PadLeft(15));

            //
            // Table of values
            //
            for (int cnt = 0; cnt < tempatures.Length; cnt++)
            {
                Console.WriteLine(
                (cnt + 1).ToString().PadLeft(15) +
                tempatures[cnt].ToString("n2").PadLeft(15));
            }
            DisplayContinuePrompt();
        }
        /// <summary>
        /// Finds 
        /// </summary>
        /// <param name="tempatures"></param>
        /// <param name="numberOfDataPoints"></param>
        private static void DataRecorderDisplayDataAnalysis(double[] tempatures, int numberOfDataPoints)
        {
            DisplayScreenHeader("Data Analysis");

            double sum = 0;
            for (int index = 0; index < tempatures.Length; index++)
            {
                sum += tempatures[index];
            }
            double avg = sum / numberOfDataPoints;
            double avgF = ((avg * 9 / 5) + 32);

            Console.WriteLine();
            Console.WriteLine("The sum of the tempatures is: {0}", sum.ToString("n2"));
            Console.WriteLine("The average tempature in degrees Celsius is: {0}", avg.ToString("n2"));
            Console.WriteLine("The average tempature in degrees Fahrenheit is: {0}", avgF.ToString("n2"));
            DisplayContinuePrompt();
        }

        private static double[] DataRecorderDisplayGetData(int numberOfDataPoints, double dataPointFrequency, Finch finchRobot)
        {
            double[] tempatures = new double[numberOfDataPoints];

            DisplayScreenHeader(" Data Collection");
            DisplayContinuePrompt();
            Console.WriteLine();
            Console.WriteLine("\tNumber of Data Points: {0}", numberOfDataPoints);
            Console.WriteLine("\tFrequency of Data Points: {0}", dataPointFrequency);
            Console.WriteLine();
            Console.WriteLine("\tMr. Finch is ready to collect your data!!");
            DisplayContinuePrompt();

            for (int index = 0; index < numberOfDataPoints; index++)
            {

                tempatures[index] = finchRobot.getTemperature();
                Console.WriteLine(" Reading {0}: {1}", index + 1, tempatures[index].ToString("n2"));
                finchRobot.wait((int)(dataPointFrequency * 1000));
            }

            DisplayContinuePrompt();

            return tempatures;
        }


        /// <summary>
        /// Receive the frequency of data points the user wants to collect
        /// </summary>
        /// <returns> Frequency of data points </returns>
        private static double DataRecorderDisplayDataPointFrequency()
        {
            double dataPointFrequency;
            bool usp;
            DisplayScreenHeader("Frequency of Data Points Menu");

            do
            {
                Console.Write("\tHow frequent would you like the data points to be collected: ");
                usp = double.TryParse(Console.ReadLine(), out dataPointFrequency);
                if ((!usp || dataPointFrequency < 0))
                {
                    Console.WriteLine();
                    Console.WriteLine("Error. Please enter a NUMBER greater than zero");
                    Console.WriteLine();
                }
            } while (!usp || dataPointFrequency < 0);

            DisplayContinuePrompt();

            return dataPointFrequency;
        }

        /// <summary>
        /// Receive the amout of data points the user wants to collect
        /// </summary>
        /// <returns> number of data points </returns>
        private static int DataRecorderGetNumberOfDataPoints()
        {
            int numberOfDataPoints;
            bool usp;
            DisplayScreenHeader("Data Recorder"); // Valdiate user

            do
            {
                Console.Write("\tHow many data points would you like: ");
                usp = int.TryParse(Console.ReadLine(), out numberOfDataPoints);
                //Console.WriteLine(usp);
                if ((!usp || numberOfDataPoints < 0))
                {
                    Console.WriteLine();
                    Console.WriteLine("Error. Please enter a NUMBER greater than zero");
                    Console.WriteLine();
                }
            } while (!usp || numberOfDataPoints < 0);

            DisplayContinuePrompt();

            return numberOfDataPoints;
        }

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


        /// Upcases the first letter of the word
        /// Thank you Sam Allen from www.dotnetperls.com
        static string UppercaseFirst(string s)
        {
            // Check for empty string.
            if (string.IsNullOrEmpty(s))
            {
                return string.Empty;
            }
            // Return char and concat substring.
            return char.ToUpper(s[0]) + s.Substring(1);
        }
    }
}
