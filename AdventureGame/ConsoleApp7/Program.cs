using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventureGame
{
    class Program
    {
        static void Main(string[] args)
        {
            // this is actually y,x and not x,y like normally, (arrays) 
            int[,] gameMap = new int[,] { {0, 0, 0, 0, 0 },
                                          {0, 2, 6, 9, 0 },
                                          {4, 1, 5, 3, 0 },
                                          {0, 7, 1, 8, 0 },
                                          {0, 0, 0, 0, 0 }
            };

            //Room Types:
            /*
			 0 = walls
			 5 = start room 
			 1 = corridor room 
			 2 = enemy room
			 3 = trap room
			 4 = exit room */

            int[,] monsterMap = new int[,] { {0, 0, 0, 0, 0 },
                                             {0, 2, 6, 9, 0 },
                                             {4, 1, 5, 3, 0 },
                                             {0, 7, 1, 8, 0 },
                                             {0, 0, 0, 0, 0 }
            };

            // Monster Types
            /*
             7 = spider */


            int[,] itemMap = new int[,] { {0, 0, 0, 0, 0 },
                                          {0, 0, 0, 0, 0 },
                                          {0, 1, 2, 0, 0 },
                                          {0, 0, 0, 0, 0 },
                                          {0, 0, 0, 0, 0 }
            };

            /* Item Types 
			 0 = no item
			 1 = Torch
			 2 = Axe


			 
			 
			 */
            // Initialize player and put at center of map 
            Location playerLocation = new Location();
            playerLocation.X = 2;
            playerLocation.Y = 2;

            // controls when game ends 
            bool gameRunning = true; // game start 

            Console.WriteLine("*****Welcome to Adventure Game.*****");


            string ascii = @"                             -|             |-
         -|                  [-_-_-_-_-_-_-_-]                  |-
         [-_-_-_-_-]          |             |          [-_-_-_-_-]
          | o   o |           [  0   0   0  ]           | o   o |
           |     |    -|       |           |       |-    |     |
           |     |_-___-___-___-|         |-___-___-___-_|     |
           |  o  ]              [    0    ]              [  o  |
           |     ]   o   o   o  [ _______ ]  o   o   o   [     | ----__________
_____----- |     ]              [ ||||||| ]              [     |
           |     ]              [ ||||||| ]              [     |
       _-_-|_____]--------------[_|||||||_]--------------[_____|-_-_
      ( (__________------------_____________-------------_________) )";


            Console.WriteLine(ascii);

            Console.WriteLine("Commands are: WALK LEFT, WALK RIGHT, WALK FORWARD, WALK BACK, LOOK, KILL SELF, TAKE ITEM");





            while (gameRunning)
            {
                // first lets get the room at the current player location 
                int room = gameMap[playerLocation.Y, playerLocation.X];
                // Get the descrition of the room 
                string description = GetRoomDescription(room);



                Console.WriteLine(description);
                // if player reached end room exit
                if (room == 4)
                {
                    Console.WriteLine();
                    Console.WriteLine("You are free!");
                    Console.WriteLine("Game Over");
                    gameRunning = false;
                    continue;
                }


                Console.WriteLine("Enter your command:");


                // Convert whatever the player typed into a command the game undersands
                string command = Console.ReadLine();
                CommandTypes commandType = ParseCommandLine(command);

                playerLocation = UpdatePlayerLocation(commandType, playerLocation);

                playerLocation = CheckBoundaries(playerLocation, gameMap.GetUpperBound(1), gameMap.GetUpperBound(0));




                if (commandType == CommandTypes.KILL_SELF)
                {
                    Console.WriteLine("Finding no way out, you say goodbye to this cruel world.");
                    Console.WriteLine("Game Over");
                    gameRunning = false;

                }
                else if (commandType == CommandTypes.NOT_FOUND)
                {
                    Console.WriteLine("I did not understand your command!");
                    Console.WriteLine("Commands are: WALK LEFT, WALK RIGHT, WALK FORWARD, WALK BACK, LOOK, KILL SELF, TAKE ITEM");
                }
                else if (commandType == CommandTypes.LOOK)
                {
                    // first get item location from our ItemMap
                    int itemId = itemMap[playerLocation.Y, playerLocation.X];
                    // get desc of item
                    string itemDesc = GetItemDescription(itemId);
                    // print out the item desc
                    Console.WriteLine(itemDesc);
                }
                else if (commandType == CommandTypes.TAKE_ITEM)
                {
                    int itemId = itemMap[playerLocation.Y, playerLocation.X];
                    string takeItem = GetItem(itemId);
                    Console.WriteLine(takeItem);
                }





            }
            Console.ReadLine();
        }

        // commands use in the game 
        enum CommandTypes
        {
            WALK_LEFT, WALK_RIGHT, WALK_FORWARD, WALK_BACK, KILL_SELF, NOT_FOUND, LOOK,
            TAKE_ITEM
        }

        // converts whatever the user typed into stuff the program understands 
        static CommandTypes ParseCommandLine(string input)
        {

            if (input == "WALK LEFT")
            {
                return CommandTypes.WALK_LEFT;
            }
            else if (input == "WALK RIGHT")
            {
                return CommandTypes.WALK_RIGHT;
            }
            else if (input == "WALK FORWARD")
            {
                return CommandTypes.WALK_FORWARD;
            }
            else if (input == "WALK BACK")
            {
                return CommandTypes.WALK_BACK;
            }
            else if (input == "KILL SELF")
            {
                return CommandTypes.KILL_SELF;
            }
            else if (input == "LOOK")
            {
                return CommandTypes.LOOK;
            }
            else if (input == "TAKE ITEM")
            {
                return CommandTypes.TAKE_ITEM;
            }
            else
            {
                return CommandTypes.NOT_FOUND;
            }
        }

        // Take in a room number and return a description
        static string GetRoomDescription(int roomNo)
        {
            string msg;
            if (roomNo == 5)
            {
                msg = "You find yourself at the entrance of an ancient castle, you walk inside.";

            }
            else if (roomNo == 4)
            {
                msg = "You spot an opening in the narrow cave wall and manage to squeeze through.";
            }
            else if (roomNo == 3)
            {
                msg = "You walk into a room with a large pit in the center.";
            }
            else if (roomNo == 1)
            {
                msg = "You find yourself in an empty corridor with light flickering in the distance.";
            }
            else if (roomNo == 6)
            {
                msg = "You walk into a dark room with a dim red light glowing in the corner.";
            }
            else if (roomNo == 7)
            {
                msg = "As you walk into the room you look up. Holy Shit! There's a man sized hairy spider on the ceiling!";
            }
            else if (roomNo == 8)
            {
                msg = "Walking into this room you smell old trash";
            }
            else if (roomNo == 9)
            {
                msg = "As you walk into this room you slip on something slimy, falling on your ass.";
            }
            else if (roomNo == 0)
            {
                msg = "You walked into a wall!";
            }
            else
            {
                msg = "You're in an UNKNOWN room! Something went terribly wrong!";
            }
            return msg;
        }


        static string GetItemDescription(int itemId)
        {
            string itemDesc;
            if (itemId == 2)
            {
                itemDesc = "You see a small wooden axe on the ground.";
            }
            else if (itemId == 1)
            {
                itemDesc = "There's a torch on the wall.";
            }
            else
            {
                itemDesc = "There is nothing of interest.";
            }
            return itemDesc;

        }

        static string GetItem(int itemId)
        {
            string takeItem;
            if (itemId == 2)
            {
                takeItem = "You pick up the small axe.";
            }
            else if (itemId == 1)
            {
                takeItem = "You take the torch from the wall.";
            }
            else
            {
                takeItem = "There is nothing to pick up.";
            }
            return takeItem;

        }

        static Location UpdatePlayerLocation(CommandTypes commandType, Location currentLocation)
        {

            // HANDLE THE DIFFERENT COMMANDS RECEIVED0
            //Process any commands the player typed 

            if (commandType == CommandTypes.WALK_LEFT)
            {
                //Console.WriteLine("You requested walk left");
                currentLocation.X--;
            }
            if (commandType == CommandTypes.WALK_RIGHT)
            {
                currentLocation.X++;
            }
            if (commandType == CommandTypes.WALK_FORWARD)
            {
                currentLocation.Y--;
            }
            if (commandType == CommandTypes.WALK_BACK)
            {
                currentLocation.Y++;
            }

            return currentLocation;
        }



        static Location CheckBoundaries(Location currentLocation, int xMax, int yMax)
        {


            // ---- CHECK THE BOUNDARIES SO THE PLAYER DOESNT WALK OFF THE MAP..
            // check if x is o (about to go negative and crash) or check if x is at he upper end of the x part of the array
            if (currentLocation.X < 0)
            {

                Console.WriteLine("You can't walk that way you idiot! The wall is solid stone!");
                currentLocation.X++;


            }

            //compare player location against the upper most allowed X value 
            if (currentLocation.X > xMax)
            {
                Console.WriteLine("You can't walk that way you idiot! The wall is solid stone!");
                currentLocation.X--;
            }
            //CHECK Y BOUNDARIES
            if (currentLocation.Y < 0)
            {

                Console.WriteLine("You can't walk that way you idiot! The wall is solid stone!");
                currentLocation.Y++;


            }
            if (currentLocation.Y > xMax)
            {
                Console.WriteLine("You can't walk that way you idiot! The wall is solid stone!");
                currentLocation.Y--;
            }
            return currentLocation;
        }





        class Monster

        {
            public string monsterType;
            public int monsterHealth = 20;
            public int monsterDamage = 1;

        }


        class Location
        {
            public int X;
            public int Y;

        }

        class GameItem
        {
            public string Name;
            public int Uses;
            public int Damage;

        }
    }


}


// int declaration int i;
// new int defintion int = 56; 