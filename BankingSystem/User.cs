using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace BankingSystem
{
    public class User
    {
        [Key]
        [Column("id")]
        public int Id { get; private set; }

        [Column("username")]
        public string Username { get; private set; }

        [Column("password")]
        public string Password { get; private set; }

        private User()
        {

        }

        public  User(string username, string password)
        {
           
            Username = username;
            Password = password;
        }

        public User(int id,string username, string password)
        {
            Id = id;
            Username = username;
            Password = password;
        }
    }
}
