using System;
using System.Collections.Generic;
using System.Text;

namespace BankingSystem
{
    public class Menu
    {
        public static string DisplaySuperMenu()
        {
            Console.Clear();
            Console.WriteLine("Please type the letter that corresponds to the action you want:");
            Console.WriteLine("c) View Cooperative’s(super admin) internal bank account");
            Console.WriteLine("m) View Members’ bank accounts");
            Console.WriteLine("d) Deposit to Member’s bank account");
            Console.WriteLine("w) Withdraw from Member’s bank account");
            Console.WriteLine("s) Write statement_admin_dd_mm_yyyy.txt");
            Console.WriteLine("q) Exit the application");

            string input = Console.ReadLine();
            return input;
            
        }

        public static string DisplaySimpleMenu()
        {
            Console.Clear();
            Console.WriteLine("Please type the letter that corresponds to the action you want:");
            Console.WriteLine("v) View your bank account\n" +
                            "c) Deposit to Cooperative’s internal bank account\n" +
                            "d) Deposit to another Member’s bank account\n" +
                            "s) Send to the statement_user_x_dd_mm_yyyy.txt file today’s transactions where dd_mm_yyyy are today’s day, month, year\n" +
                            "q) Exit the application");
            string input = Console.ReadLine();
            /* switch (input)
             {
                 case "v": break;
                 case "c":


                     break;

                 case "s":
                 case "q": break;
                 default:
                     break;
             }*/

            return input;

        }

        public static int ReadSubmenu()
        {
            int num;
            string input;
            bool flag;
            

            do {
                
                input = Console.ReadLine();
                flag = !int.TryParse(input, out num);
              
                if (input == "r")
                {
                    num = 0;
                    break;
                }
                if (flag)
                {
                    Console.WriteLine($"\"{input}\" is not valid, please try again or enter \"r\" to return to main menu.");
                }
                if (num<0)
                {
                    Console.WriteLine("Only positive numbers allowed! Please try again or enter \"r\" to return to main menu.");
                    flag = true;
                }
            }
            while (flag);

            return num;
        }

    }
}
