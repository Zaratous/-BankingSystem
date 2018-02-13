using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;


namespace BankingSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            CultureInfo.CurrentUICulture = new CultureInfo("el-GR");
            List<string> transactions = new List<string>();

            using (BankContext bcxt = new BankContext())
            {
                try
                {
                    bcxt.Accounts.Count();

                }
                catch (SqlException )
                {
                    Console.WriteLine("Could not connect to database");
                    Console.ReadKey();
                    Environment.Exit(-1);
                }

            }
            // Console.WriteLine(DbAccounts.GetType());
            User user;
            int j = 0;//for counting times inputing login details
            bool flaglogin;
            Console.WriteLine("Welcome to our prestigious bank");

            do
            {
                flaglogin = false;
                switch (j)
                {
                    case 1:
                        Console.WriteLine("Two attemts remaining");
                        break;
                    case 2:
                        Console.WriteLine("One attemt remaining");
                        break;
                    case 3:
                        Console.WriteLine("You have exceeded the number of allowed login attempts. Please try again later\nPress any key to exit.");
                        Console.ReadKey();
                        return;
                }

                User sysUser = Login.DisplayLogin();
                Console.WriteLine();
                //var DbUsers = bcxt.Users.ToList();
                using (BankContext bcxt = new BankContext())
                {
                    user = bcxt.Users.FirstOrDefault(u => u.Username == sysUser.Username);
                }
                if (user == null)
                {
                    Console.WriteLine("incorrect username");
                    System.Threading.Thread.Sleep(2000);
                    flaglogin = true;
                    Console.Clear();
                    j++;
                    continue;
                }
                string[] password = user.Password.Split(" ");//Sth vash uparxei to password field exei th morfh  hashed_passwordkenosalt
                byte[] hashBytes = Convert.FromBase64String(password[0]);//pairnoume to salt
                byte[] salt = Convert.FromBase64String(password[1]);//pairnoume to hashed_password

                Array.Copy(hashBytes, 0, salt, 0, 16);
                /* Compute the hash on the password the user entered */
                var pbkdf2 = new Rfc2898DeriveBytes(sysUser.Password, salt, 10000);
                byte[] hash = pbkdf2.GetBytes(20);
                /* Compare the results */
                for (int i = 0; i < 20; i++)
                    if (hashBytes[i + 16] != hash[i])
                    {
                        Console.WriteLine("incorrect password.\n");
                        System.Threading.Thread.Sleep(2000);
                        flaglogin = true;
                        Console.Clear();
                        j++;
                        break;
                        //throw new UnauthorizedAccessException();
                    }



            } while (flaglogin);
            //Menu.DisplayMenu(user.Username);
            Console.WriteLine($"userId:{user.Id} user.Username: { user.Username} ");
            using (BankContext bcxt = new BankContext())
            {

                string input = "";
                UserAccount usAc;

                var DbUsers = bcxt.Users.ToList();
                var DbAccounts = bcxt.Accounts.ToArray();
                usAc = new UserAccount(user, DbAccounts);



                Account account = usAc.getAccount();
                //Account adAc = (account);

                do
                {


                    if (account.IsAdmin())
                    {


                        input = Menu.DisplaySuperMenu();
                        switch (input)
                        {
                            case "c":
                                Console.Clear();
                                Console.WriteLine(usAc.ToString());
                                Console.WriteLine("Press any key to return to main menu.");
                                Console.ReadKey();
                                break;
                            case "m":
                                Console.Clear();
                                usAc.DisplayMembersAccounts();
                                Console.WriteLine("Press any key to return to main menu.");
                                Console.ReadKey();
                                break;
                            case "d":
                                string input2 = "";
                                bool flagUs = true;
                                bool flagAm = true;
                                bool flag;
                                int o;
                                do
                                {                                    
                                    List<string> members = usAc.ReturnMembers();
                                    do
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Enter the number that corresponds to the user you wish to deposit to or enter \"0\" to return to main menu");

                                        for (int i = 0; i < members.Count; i++)
                                            Console.WriteLine($"{i + 1}) {members[i]}");
                                        flag = int.TryParse(Console.ReadLine(), out o);
                                        if (flag)
                                        {
                                            if (o == 0) input2 = "r";
                                            else if (o < 0 || o > members.Count)
                                            {
                                                Console.WriteLine("incorrect number! Please try again.");
                                                System.Threading.Thread.Sleep(2000);
                                                flag = false;
                                            }
                                            else
                                            {
                                                input2 = members[o - 1];
                                            }
                                        }


                                    }
                                    while (!flag);


                                    if (input2 == "r") break;
                                    string username = input2;
                                    flagUs = Validate(usAc, username);
                                    if (flagUs)
                                    {
                                        do
                                        {
                                            Console.WriteLine("Please insert the amount  to deposit.");
                                            int amount = Menu.ReadSubmenu();
                                            if (amount == 0)
                                            {
                                                flagAm = false;
                                                break;
                                            }
                                            flagAm = Validate(usAc.DepositToUser(username, amount));
                                            if (!flagAm)
                                                flagAm = ExitSubmenu();
                                            else
                                            {

                                                string dateStr = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.FFF");
                                                account.SetTransDate(DateTime.Parse(dateStr));
                                                bcxt.SaveChanges();
                                                transactions.Add(string.Format("{0,-10} {1,-10} {2,-10} {3,-30} {4,15}\n", "Deposit", user.Username, username, dateStr, amount.ToString("C2", CultureInfo.CreateSpecificCulture("el-GR"))));

                                                flagAm = false;

                                            }
                                        }
                                        while (flagAm);
                                    }
                                    else
                                    {
                                        flagUs = ExitSubmenu();
                                    }
                                }
                                while (flagUs && flagAm);


                                break;
                            case "w":

                                string input3 = "";
                                bool flagU = true;
                                bool flagA = true;
                                bool flagW;
                                int oW;
                                do
                                {
                                    List<string> members = usAc.ReturnMembers();
                                    do
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Enter the number that corresponds to the user you wish to withdraw from or enter \"0\" to return to main menu");

                                        for (int i = 0; i < members.Count; i++)
                                            Console.WriteLine($"{i + 1}) {members[i]}");
                                        flagW = int.TryParse(Console.ReadLine(), out oW);
                                        if (flagW)
                                        {
                                            if (oW == 0) input3 = "r";
                                            else if (oW < 0 || oW > members.Count)
                                            {
                                                Console.WriteLine("incorrect number! Please try again.");
                                                System.Threading.Thread.Sleep(2000);
                                                flagW = false;
                                            }
                                            else
                                            {
                                                input3 = members[oW - 1];
                                            }
                                        }


                                    }
                                    while (!flagW);
                                    if (input3 == "r") break;
                                    string username = input3;
                                    flagU = Validate(usAc, username);
                                    if (flagU)
                                    {
                                        do
                                        {
                                            Console.WriteLine("Please insert the amount  to withdraw.");
                                            int amount = Menu.ReadSubmenu();
                                            if (amount == 0)
                                            {
                                                flagA = false;
                                                break;
                                            }

                                            flagA = Validate(usAc.WithdrawFromUser(username, amount));
                                            if (!flagA)
                                                flagA = ExitSubmenu();
                                            else
                                            {

                                                string dateStr = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.FFF");
                                                account.SetTransDate(DateTime.Parse(dateStr));
                                                bcxt.SaveChanges();
                                                transactions.Add(string.Format("{0,-10} {1,-10} {2,-10} {3,-30} {4,15}\n", "Withdraw", user.Username, username, dateStr, amount.ToString("C2", CultureInfo.CreateSpecificCulture("el-GR"))));

                                                flagA = false;
                                            }
                                        }
                                        while (flagA);
                                    }
                                    else
                                    {
                                        flagU = ExitSubmenu();
                                    }
                                }
                                while (flagU && flagA);

                                break;
                            case "s":

                                DateTime dt = DateTime.Now;
                                string adminFile = string.Format("statement_{0}_{1}.txt", user.Username, dt.ToString("dd/MM/yyyy").Replace(" ", "_")).Replace(" ", "_").Replace("-", "_");

                                FileWriting.writeOrAddToFile(adminFile, transactions);

                                Console.WriteLine($"{adminFile} is complete! press any key to exit");
                                Console.ReadKey();
                                goto case "q";
                            case "q":
                                return;
                            default:
                                break;
                        }


                    }
                    else
                    {

                        input = Menu.DisplaySimpleMenu();
                        switch (input)
                        {
                            case "v":
                                Console.Clear();
                                Console.WriteLine(usAc.ToString());
                                Console.WriteLine("Press any key to return to main menu.");
                                Console.ReadKey();
                                break;
                            case "c":

                                bool flag = true;
                                do
                                {
                                    Console.Clear();
                                    Console.WriteLine("Choose the amount you want to deposit to Cooperative’s internal bank account or press b to return to main menu");

                                    int amount = Menu.ReadSubmenu();
                                    if (amount == 0)
                                    {
                                        break;
                                    }
                                    flag = Validate(usAc.ToCoOp(amount));
                                    if (!flag)
                                        flag = ExitSubmenu();
                                    else
                                    {

                                        string dateStr = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.FFF");
                                        account.SetTransDate(DateTime.Parse(dateStr));
                                        bcxt.SaveChanges();
                                        transactions.Add(string.Format("{0,-10} {1,-10} {2,-10} {3,-30} {4,15}\n", "Deposit", user.Username, "admin", dateStr, amount.ToString("C2", CultureInfo.CreateSpecificCulture("el-GR"))));

                                        flag = false;
                                    }
                                }
                                while (flag);
                                break;


                            case "d":
                                string input3 = "";
                                bool flagU = true;
                                bool flagA = true;
                                bool flagD;
                                int o;
                                do
                                {
                                    List<string> members = usAc.ReturnMembers();
                                    do
                                    {
                                        Console.Clear();
                                        Console.WriteLine("Enter the number that corresponds to the user you wish to deposit to or enter \"0\" to return to main menu");

                                        for (int i = 0; i < members.Count; i++)
                                            Console.WriteLine($"{i + 1}) {members[i]}");
                                        flagD = int.TryParse(Console.ReadLine(), out o);
                                        if (flagD)
                                        {
                                            if (o == 0) input3 = "r";
                                            else if (o < 0 || o > members.Count)
                                            {
                                                Console.WriteLine("incorrect number! Please try again.");
                                                System.Threading.Thread.Sleep(2000);
                                                flagD = false;
                                            }
                                            else
                                            {
                                                input3 = members[o - 1];
                                            }
                                        }


                                    }
                                    while (!flagD);
                                    if (input3 == "r") break;
                                    string username = input3;
                                    flagU = Validate(usAc, username);
                                    if (flagU)
                                    {
                                        do
                                        {
                                            Console.WriteLine("Please insert the amount  to deposit.");
                                            int amount = Menu.ReadSubmenu();
                                            if (amount == 0)
                                            {
                                                flagA = false;
                                                break;
                                            }
                                            flagA = Validate(usAc.DepositToUser(username, amount));
                                            if (!flagA)
                                                flagA = ExitSubmenu();
                                            else
                                            {

                                                string dateStr = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.FFF");
                                                account.SetTransDate(DateTime.Parse(dateStr));
                                                bcxt.SaveChanges();
                                                transactions.Add(string.Format("{0,-10} {1,-10} {2,-10} {3,-30} {4,15}\n", "Deposit", user.Username, username, dateStr, amount.ToString("C2", CultureInfo.CreateSpecificCulture("el-GR"))));

                                                flagA = false;
                                            }
                                        }
                                        while (flagA);
                                    }
                                    else
                                    {
                                        flagU = ExitSubmenu();
                                    }
                                }
                                while (flagU && flagA);
                                break;
                            case "s":

                                DateTime dt = DateTime.Now;
                                string simpleUSerFile = string.Format("statement_{0}_{1}.txt", user.Username, dt.ToString("dd/MM/yyyy").Replace(" ", "_")).Replace(" ", "_").Replace("-", "_");

                                FileWriting.writeOrAddToFile(simpleUSerFile, transactions);

                                Console.WriteLine($"{simpleUSerFile} is complete! Press any key to exit");
                                Console.ReadKey();
                                goto case "q";
                            case "q": return;
                            default:
                                break;
                        }
                    }

                } while (input != "q");
            }


            Console.ReadKey();

        }

        private static bool ExitSubmenu()
        {
            Console.WriteLine(" Do you want to try again? (\"y\" for yes/ any other letter for no)");
            string input = Console.ReadLine();

            if (input == "y") return true;

            return false;
        }

        private static bool Validate(UserAccount usac, string username)
        {
            bool flag = usac.UserExists(username);
            if (!flag)
            {
                Console.WriteLine($"The user:{username} was not found.");

            }

            return flag;

        }


        private static bool Validate(bool flag)
        {

            if (flag)
            {
                Console.WriteLine("The transaction was successful.\nPress any key to return to main menu.");
                Console.ReadKey();

            }
            else
            {
                Console.WriteLine("insufficient funds.");
                return false;

            }
            return true;



        }
    }
}
