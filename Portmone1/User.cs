using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Portmone1
{
    public abstract class User
    {
        public string Name, Famely, Town, Email, Passw;
        public int Age { get; set; }  //  public string Passw { get { return Passw; } set { Passw = value; } }
        public double UserBalance { get; set; }
        public List<string> Languages { get; internal set; }
    }
    internal class CreateUser : User
    {
        bool Cheking;
        bool Cheking1 = true;
        public string[] ComparyName { get; set; }
        public string[] ComparyEmail { get; set; }
        public int TotalUsers;

        public CreateUser(bool cheking, int totalusers, string[] comparyname, string[] comparyemail) 
        { Cheking = cheking; TotalUsers = totalusers; ComparyName = comparyname; ComparyEmail = comparyemail; }

        internal void UserData()
        {
            while (Cheking1)
            {
                if (Cheking)
                {
                    Console.WriteLine("Enter your Username (all symbols after 10 will be removed): ");
                    Name = Console.ReadLine().Trim(); if (Name.Length > 10) {Name = Name.Remove(10); Console.WriteLine($"Your Username: {Name}"); };
                    ChekUser Chek = new ChekUser(TotalUsers, Name, ComparyName);
                    Cheking1 = Chek.Cheking();                   
                }
                else
                {
                    Console.WriteLine("Enter your Username (all symbols after 10 will be removed): ");
                    Name = Console.ReadLine().Trim(); if (Name.Length > 9) { Name = Name.Remove(10); Console.WriteLine($"Your Username: {Name}"); };
                    Cheking1 = false;
                }
            }  

            Console.WriteLine("Enter your Famely name: ");
            Famely = UpperLetter(Console.ReadLine().Trim());
            Console.WriteLine("Enter your Town: ");
            Town = UpperLetter(Console.ReadLine().Trim());

            static string UpperLetter(string temp)
            {
                bool bool1 = true;
                while (bool1)
                {
                    if (Regex.IsMatch(temp, (@"^[a-z]"), RegexOptions.IgnoreCase))
                    {
                        bool1 = false;
                        return Convert.ToString(temp[0]).ToUpper() + temp.Substring(1); 
                    }
                    else
                    {
                        Console.WriteLine($"The {temp} not accepted. Only alphabetical symbol at the start, please. Try again.");
                        temp = UpperLetter(Console.ReadLine().Trim());
                    }
                }
                return "Empty slot."; //not executed code
            }

            bool bool1 = true;
            while (bool1)
            {
                Console.WriteLine("Enter your Age (10-100 years): ");
                try
                {
                    Age = Convert.ToInt32(Console.ReadLine());
                    if (Age < 10 || Age > 100) { Console.WriteLine("Incorrect Age."); } else { bool1 = false; };
                }
                catch { Console.WriteLine("Only numbers accepted for Age."); }
            }

            Cheking1 = true;
            while (Cheking1)
            {
                if (Cheking)
                {
                    Email = EmailFormat ();
                    ChekUser Chek = new ChekUser(TotalUsers, Email, ComparyEmail);
                    Cheking1 = Chek.Cheking();
                }
                else
                {
                    Email = EmailFormat();
                    Cheking1 = false;
                }
            }     

            Console.WriteLine("Enter your Password: ");
            Passw = Console.ReadLine();
        }

        private string EmailFormat()
        {
            string pattern = @"^(?("")(""[^""]+?""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
              @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9]{2,17}))$";   //MSDN code
            while (true)
            {
                Console.WriteLine("Enter your Email: ");
                Email = Console.ReadLine();
                if (Regex.IsMatch(Email, pattern, RegexOptions.IgnoreCase))
                {
                    Console.WriteLine("Email entered correctly.");
                    return Email;
                }
                else
                {
                    Console.WriteLine("Uncorrect email");
                }
            }
        }
    }

    class U
    {
        public DateTime UDate { get; set; }
        public string UCurr { get; set; }
        public double URate { get; set; }
        public string UOper { get; set; }
        public double UAmount { get; set; }
        public double UBalance { get; set; }
    }

}
