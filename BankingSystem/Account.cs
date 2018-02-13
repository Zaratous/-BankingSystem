using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Text;

namespace BankingSystem
{
    public  class Account
    {
        protected Account[] Accounts;
        [Key]
        [Column("id")]
        public int Id { get; private set; }

        public  User User { get; private set; }

        [Column("user_id")]
        public int UserId{get; private set;}

        [Column("transaction_date")]
        public DateTime TransDate { get; private set; }

        [Column("amount")]
        public decimal Amount { get; private set; }

        public bool IsAdmin()
        {
            if (User.Id == 1)
                return true;
            return false;
        }

        public bool IsUser(string username)
        {
            if (User.Username == username)
                return true;
            return false;
        }

        public Account(int id, int userId, DateTime TransDate, decimal amount)
        {
          
        }
        public  Account(User user)
        {            
            User = user;            
            UserId = User.Id;
        }

        public Account(Account account)
        { 
            this.Id = account.Id;
            this.TransDate = account.TransDate;
            this.Amount = Amount;
            this.Accounts = account.Accounts;
        }
        public void SetUser(User user)
        {
            User = user;
        }
        public void SetTransDate(DateTime dt)
        {
            TransDate = dt;
        }

        public void AddAmount(decimal amount)
        {
            Amount += amount;
        }

        public void SubAmount(decimal amount)
        {
            Amount -= amount;
        }

        public string ReturnBalance()
        {
            return Amount.ToString("C2", CultureInfo.CreateSpecificCulture("el-GR"));
        }

        private Account()
        {
                
        }



       

    }
}
