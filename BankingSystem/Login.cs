using System;
using System.Collections.Generic;
using System.Security;
using System.Text;

namespace BankingSystem
{
    static class Login
    {
        public static User DisplayLogin()
        {
            
            Console.Write("Please insert your username: ");
            string usrnm = Console.ReadLine();
            
            string password = getPasswordFromConsole("Please insert your password: ");
            User user = new User(usrnm, password);
            return user;
        }
        public static string getPasswordFromConsole(string displayMessage)
        {
            String pass = "";
            Console.Write(displayMessage);
            ConsoleKeyInfo key;

            do
            {
                key = Console.ReadKey(true);

                // Backspace Should Not Work
                if (key.Key != ConsoleKey.Backspace && key.Key != ConsoleKey.Enter)
                {
                    pass += key.KeyChar;
                    Console.Write("*");
                }
                else
                {
                    if (key.Key == ConsoleKey.Backspace && pass.Length > 0)
                    {
                        pass = pass.Substring(0, (pass.Length - 1));
                        Console.Write("\b \b");
                    }
                }
            }
            // Stops Receving Keys Once Enter is Pressed
            while (key.Key != ConsoleKey.Enter);
            return pass;
        }
    }
}
