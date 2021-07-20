using BoxesLibrary;
using System;

namespace testing
{
    class Program
    {
        static void Main(string[] args)
        {
             User userDialogue = new User();
             Manager manager = new Manager(userDialogue);

            manager.Add(20, 2, 30);
            manager.Add(13, 2, 4);
            manager.Add(13, 3, 3);
            manager.Add(14, 6, 5);
            manager.Add(14.5, 8.8, 50);
            manager.Add(17.003, 78.7, 900);
            manager.Add(12, 1.3, 3);
            manager.Add(20.6, 5, 90);
            manager.Add(20.6, 23.9, 800);
            manager.Add(14, 9, 4);
            manager.Add(12, 6, 3);
            manager.Add(12, 7, 2);
            manager.Add(13.5, 7.5, 15);

            DisplayMenu(manager);
        }

        static public bool UserInfoForAddAndBuy(out double width, out double height, out int quantity,string str)
        {
            Console.WriteLine($"Please enter the width, height and quantity of the {str}");

            do
            {
                if (double.TryParse(Console.ReadLine(), out width) && double.TryParse(Console.ReadLine(), out height)
               && int.TryParse(Console.ReadLine(), out quantity))
                {
                    return true;
                }

            } while (true);
        }

        static public bool UserInfoForGetData(out double width, out double height, string str)
        {
            Console.WriteLine($"Please enter the width and height of the {str}");

            do
            {
                if (double.TryParse(Console.ReadLine(), out width) && double.TryParse(Console.ReadLine(), out height))
                {
                    return true;
                }

            } while (true);
        }

        static public void DisplayMenu(Manager manager)
        {
            string result;
            do
            {
                Console.ForegroundColor = ConsoleColor.Magenta;
                Console.WriteLine();
                Console.WriteLine("BOXES STORE\n", Console.Title);
                Console.ResetColor();
                Console.ForegroundColor = ConsoleColor.Cyan;
                Console.WriteLine("1. Add box");
                Console.WriteLine("*********************");
                Console.WriteLine("2. Get box data");
                Console.WriteLine("*********************");
                Console.WriteLine("3. Buy box for gift");
                Console.WriteLine("*********************");
                Console.WriteLine("4. Exit");
                Console.WriteLine("*********************");
                Console.ResetColor();
                Console.Write("\nSelect your option:\n");

                double width, height;
                int quantity;
                result = Console.ReadLine();

                switch (result)
                {
                    case "1":
                        {   //add
                            UserInfoForAddAndBuy(out width, out height, out quantity, "box to add");
                            try
                            {
                                manager.Add(width, height, quantity);
                            }
                            catch (ArgumentException e)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.WriteLine(e.Message);
                                Console.ResetColor();
                                break;
                            }
                            Console.WriteLine("Box added successfully!");
                        }
                        break;

                    case "2":
                        {   //get data
                            UserInfoForGetData(out width, out height, "box to search");
                            string data=manager.GetData(width, height);

                            if (data == null)
                            {
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.WriteLine($"The box was not found\n");
                                Console.ResetColor();
                            }
                            else
                            Console.WriteLine($"{data}\n");
                        }
                        break;

                    case "3":
                        {   //buy
                            UserInfoForAddAndBuy(out width, out height, out quantity, "gift");
                            if (manager.FindBestMatch(width, height, quantity))
                                Console.WriteLine("The purchase was made successfully!\n");
                            else
                            {
                                Console.ForegroundColor = ConsoleColor.DarkRed;
                                Console.WriteLine("The purchase was NOT made,no matching boxes were found for your request.");
                                Console.ResetColor();
                            }  
                        }
                        break;

                    case "4":
                        break;//exit
                }

            } while (result!="4");
           
        }
    }
}
