using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;


namespace BankingSystem
{
    class UserAccount
    {

        private User User { get;}
   
        private Account Account {get;set;}

        private readonly Account[] Accounts;
        public Account getAccount()
        {           
            return Account;
        }

        public bool UserExists(string username)
        {
            var account = Accounts.SingleOrDefault(ac => ac.IsUser(username));
            if (account == null)
                return false;
            return true;

        }

        private Account GetAdmnAccount()
        {
            return Accounts.SingleOrDefault(ac => ac.IsAdmin());
        }

        public bool ToCoOp(decimal amountTaken)
        {
            return DepositTo(amountTaken, GetAdmnAccount());

        }

        public bool DepositToUser(string username, decimal amountTaken)
        {
            var account = Accounts.SingleOrDefault(ac => ac.IsUser(username));
            if (account == null)
            {
                Console.WriteLine("username not found");
                return false;
            }
            return DepositTo(amountTaken, account);

        }
        public bool DepositTo(decimal amountGiven, Account otheraccount)
        {
            if (Account.Amount >= amountGiven)
            {
                Console.WriteLine($"{Account.User.Username} before : " + Account.ReturnBalance());
                Account.SubAmount (amountGiven);
                Console.WriteLine($"{Account.User.Username} after : " + Account.ReturnBalance());

                Console.WriteLine($"{otheraccount.User.Username} before : " + otheraccount.ReturnBalance());
                otheraccount.AddAmount(amountGiven);
                Console.WriteLine($"{otheraccount.User.Username} after : " + otheraccount.ReturnBalance());
                return true;
            }
            else
                return false;
        }
        public bool WithdrawFromUser(string username, decimal amountTaken)
        {

            if (this.Account.IsAdmin())
            {
                var account = Accounts.SingleOrDefault(ac => ac.IsUser(username));
                if (account == null)
                {
                    Console.WriteLine("username not found");
                    return false;
                }
                return WithdrawFrom(amountTaken, account);
            }
            else
            {
                Console.WriteLine("You don't have the required privilege to complete this action.");
                return false;
            }

        }

        public bool WithdrawFrom(decimal amountTaken, Account otheraccount)
        {
            if (otheraccount.Amount >= amountTaken)
            {
                Console.WriteLine($"{otheraccount.User.Username} before : " + otheraccount.ReturnBalance());
                otheraccount.SubAmount(amountTaken);
                Console.WriteLine($"{otheraccount.User.Username} after : " + otheraccount.ReturnBalance());

                Console.WriteLine($"{Account.User.Username} before : " + Account.ReturnBalance());
                Account.AddAmount(amountTaken);
                Console.WriteLine($"{Account.User.Username} after : " + Account.ReturnBalance());

                return true;
            }
            else
                return false;
        }

        public List<string> ReturnMembers()
        {
            List<string> list = new List<string>();
            foreach (var ac in Accounts)
            {
                if (Account.Id != ac.Id && !ac.IsAdmin())
                    list.Add(ac.User.Username);
            }
            return list;
        }

        public void DisplayMembersAccounts()
        {
            Console.WriteLine("------------------------------------------------------------------------");
            if (this.Account.IsAdmin())
            {
                Console.WriteLine("Members’ bank accounts:");
                foreach (var ac in Accounts)
                {
                    Console.WriteLine($"User: {ac.User.Username} balance: {ac.ReturnBalance()}");
                }
            }
            else
            {
                Console.WriteLine("You don't have the required privilege to complete this action.");

            }
            Console.WriteLine("------------------------------------------------------------------------");
        }

    

        public UserAccount(User user, Account[] accounts)
        {
            User = user;
            Accounts = accounts;
            Account = Accounts.FirstOrDefault(ac => ac.Id == User.Id);
            //Account.User = User;
            Console.WriteLine($"User.Username: {User.Username}");
        }

        public override string ToString()
        {
            return $"Username: {User.Username}\nTransaction date:{Account.TransDate}\nAccount's balance::{Account.ReturnBalance()}\n";
        }
    }
}
