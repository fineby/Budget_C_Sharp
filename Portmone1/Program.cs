using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace Portmone1
{    
    delegate void Delete(int DelIndx);
    class Program
    {
        static void Main(string[] args)
        {
            LoadSave Load = new LoadSave();
            Load.Load();
            int UserNumber = Load.y; int TotalUsers = UserNumber + 1;
            bool NoSave = Load.NoSave; bool FirstStart = false;
            bool delkey = false; int CurrentUserIndex = 0;
            bool temp = true;
            int count1 = 0;
            bool MenuCh = false;
            double Balance;
            object[] UserFounds1;
            List<object> Operations1 = new List<object>(); 

            string[] UserDateTime = new string[TotalUsers];
            string[] Name = new string[TotalUsers];
            string[] Famely = new string[TotalUsers];
            string[] Town = new string[TotalUsers];
            int[] Age = new int[TotalUsers];
            string[] Email = new string[TotalUsers];
            string[] Passw = new string[TotalUsers];
            object[] UserBalance = new object[TotalUsers];

            double EURrate; double GBPrate;
            bool ShowCurrAdd = false;

            if (NoSave)
                { Console.WriteLine("That is a first start of the program."); }
                else
                {
                    Console.WriteLine($"Users loaded. Currently it is {TotalUsers} users in the system.");

                    object[][] Loaded = (object[][])Load.Load();

                    if (Loaded.Length == 1) { FirstStart = true; }

                    for (int i = 0; i < Loaded.Length; i++)
                    {
                        UserDateTime[i] = Convert.ToString(Loaded[i][0]);
                        Name[i] = Convert.ToString(Loaded[i][1]);
                        Famely[i] = Convert.ToString(Loaded[i][2]);
                        Town[i] = Convert.ToString(Loaded[i][3]);
                        Age[i] = Convert.ToInt32(Loaded[i][4]);
                        Email[i] = Convert.ToString(Loaded[i][5]);
                        Passw[i] = Convert.ToString(Loaded[i][6]);
                        UserBalance[i] = Loaded[i][7];
                    }
                }

            Currency();          
            Menu(); 

            void Menu3()
            {
                Console.Clear();
                SetRates PassCurr = new(EURrate, GBPrate);
                try
                {
                    UserFounds1 = (object[])UserBalance[CurrentUserIndex];
                    Operations1 = UserFounds1.ToList();
                    Balance = (double)UserFounds1.Last();
                }
                catch { Balance = 0; }
              //  bool sw5 = true;
                while (true)
                {
                    Console.WriteLine("YOUR CURRENT BALANCE IS {0} USD. PLEASE, {1} MAKE YOUR CHOISE.", Balance, Name[CurrentUserIndex]);
                    Console.WriteLine("1. List all operations.");
                    Console.WriteLine("2. Add or withdraw founds.");
                    Console.WriteLine("3. Report from the period.");
                    Console.WriteLine("4. Download new rates.");
                    Console.WriteLine("5. Back to previous menu.");
                    switch (Convert.ToInt32(Console.ReadLine()))
                    {
                        case 1: Founds Init1 = new Founds(Name[CurrentUserIndex], Operations1, PassCurr.EURrate, PassCurr.GBPrate); Init1.ListFounds(); Menu3(); break;
                        case 2:
                            Founds Init = new Founds(Name[CurrentUserIndex], Balance, Operations1, PassCurr.EURrate, PassCurr.GBPrate);
                            object[] UserFounds = Init.FoundsData();
                            UserBalance[CurrentUserIndex] = UserFounds;
                            Founds(); Menu3(); break;
                        case 3: Founds Init2 = new Founds(Name[CurrentUserIndex], Operations1, PassCurr.EURrate, PassCurr.GBPrate); Init2.Report(); Menu3(); break;
                        case 4: ShowCurrAdd = true; Currency(); Menu3(); break;
                        case 5: Menu2(); break;
                        default: Console.WriteLine("Invalid input. Just number from menu accepted. \t"); continue;
                    }
                }
            }

            void Currency()
            {
                bool temp1 = false;
                bool temp2 = false;
                string EUR = "";
                string GBP = "";
                string BuildDate = "";

                FileInfo fileCurr = new FileInfo(".\\usd.xml");
                if (fileCurr.Exists && !ShowCurrAdd) { LoadCurr(); }
                else if (!fileCurr.Exists && !ShowCurrAdd) 
                {
                    Console.WriteLine("It is not any currency rates at the system." +
                        "You need to load rates. Before first currency load EUR/USD and GBP/USD rates will be 1.");
                    EURrate = 1; GBPrate = 1; Task.Delay(8000);
                }
                else 
                {
                using (var c = new WebClient())
                c.DownloadFile("http://www.floatrates.com/daily/usd.xml", Path.Combine(@".\", "usd.xml"));
                LoadCurr();
                }
                
                if (ShowCurrAdd)
                {
                    Console.WriteLine("New currency rates loaded.");
                    Console.WriteLine();
                    Console.WriteLine($"Currency date: {BuildDate}");
                    Console.WriteLine($"EUR/USD: {EUR} and GBP/USD: {GBP}");
                    Thread.Sleep(10000);
                }

                void LoadCurr()
                {
                    XmlDocument CurrencyDoc = new XmlDocument();
                    CurrencyDoc.Load(".\\usd.xml");
                    XmlElement curren = CurrencyDoc.DocumentElement;

                    foreach (XmlNode xnode in curren)
                    {
                        if (xnode.Name == "lastBuildDate") { BuildDate = xnode.InnerText; }

                        foreach (XmlNode childnode in xnode.ChildNodes)
                        {
                            if (childnode.Name == "targetCurrency" && childnode.InnerText == "EUR") { temp1 = true; }
                            if (childnode.Name == "targetCurrency" && childnode.InnerText == "GBP") { temp2 = true; }

                            if (childnode.Name == "inverseRate" && temp1) { EUR = childnode.InnerText; temp1 = false; }
                            if (childnode.Name == "inverseRate" && temp2) { GBP = childnode.InnerText; temp2 = false; }
                        }
                    }                   
                    Double.TryParse(EUR.Replace('.', ','), out EURrate);
                    Double.TryParse(GBP.Replace('.', ','), out GBPrate);                   
                }
            }


            void Menu2() 
            {
                Console.Clear();

                MenuCh = true;
                int Inp2=0;
                bool Inp3 = true;

                Console.WriteLine("PLEASE, {0} MAKE YOUR CHOISE.", Name[CurrentUserIndex]); 
                Console.WriteLine("1. My founds.");
                Console.WriteLine("2. Registration for new users.");
                Console.WriteLine("3. Delete registered user.");
                Console.WriteLine("4. Change current user.");
                Console.WriteLine("5. Log out.");            
                                      
                while (Inp3)
                {
                    try {Inp2 = Convert.ToInt32(Console.ReadLine()); if (Inp2 == 1 || Inp2 == 2 || Inp2 == 3 || Inp2 ==4 || Inp2 ==5) {Inp3 = false; Console.WriteLine($"You choose number {Inp2}.");  } }
                    
                    catch { Console.WriteLine("Invalid input. Just number from menu accepted. \t"); }
                    finally { if (Inp2 == 1) { Menu3(); }; if (Inp2 == 2) { Registration(); }; if (Inp2 == 3) { delete(); }; if (Inp2 == 4) { Autorization(); };
                     if (Inp2 == 5) { MenuCh=false; Console.Clear(); Menu(); }; Inp3 = false;  }                
                }
            }

            void Founds()
            {                         
                object[][] User1 = new object[TotalUsers][];
                for (int i = 0; i < TotalUsers; i++)
                {
                    User1[i] = new object[8]{ UserDateTime[i], Name[i], Famely[i], Town[i],
                            Age[i], Email[i], Passw[i], UserBalance[i] };
                }

                LoadSave Save = new LoadSave(User1, UserNumber, delkey);
                Save.Save(); Console.WriteLine("Information saved. \t");            
            }

            void Menu()
            {                
                if (temp)
                { 
                    bool Inp = false;
                
                ConsoleColor color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkGreen;
                Console.SetCursorPosition(5, 3);
                Console.WriteLine("PLEASE, MAKE YOUR CHOISE.");
                Console.ForegroundColor = ConsoleColor.DarkBlue;
                Console.SetCursorPosition(2, 4);
                Console.WriteLine("1. Registration for new users.");
                Console.SetCursorPosition(2, 5);
                Console.WriteLine("2. Login for registered user.");
                Console.SetCursorPosition(2, 6);
                Console.WriteLine("3. Quit from the program.");
                Console.ForegroundColor = color;
                while (!Inp)
                {   
                    Inp = Int32.TryParse(Console.ReadLine(), out int Inp1);
                    switch (Inp1)
                    {
                        case 1: Registration(); continue;
                        case 2: if (!NoSave) { Autorization(); } else { Console.WriteLine("Impossible login to empty system!"); Inp = false; }; break;
                        case 3: Console.WriteLine("Exit from the program complited."); Environment.Exit(0); break;
                        default: Console.WriteLine("Invalid choise. Just number 1 to 3 is acceptable."); Inp = false; break;
                    };
                }
            } else { Console.WriteLine("Exit from the program complited."); }
            }                                  
            
                void Registration()
            {
                Console.Clear();             
                           
                            CreateUser NewUser = new CreateUser(!NoSave, TotalUsers, Name, Email);
                            NewUser.UserData();

                            if (UserNumber != 0 || FirstStart || count1 != 0) { TotalUsers++; UserNumber = TotalUsers - 1; }

                            Array.Resize(ref UserDateTime, TotalUsers);
                            Array.Resize(ref Name, TotalUsers);
                            Array.Resize(ref Famely, TotalUsers);
                            Array.Resize(ref Town, TotalUsers);
                            Array.Resize(ref Age, TotalUsers);
                            Array.Resize(ref Email, TotalUsers);
                            Array.Resize(ref Passw, TotalUsers);
                            Array.Resize(ref UserBalance, TotalUsers);

                            UserDateTime[UserNumber] = Convert.ToString(DateTime.Now);
                            Name[UserNumber] = NewUser.Name;
                            Famely[UserNumber] = NewUser.Famely;
                            Town[UserNumber] = NewUser.Town;
                            Age[UserNumber] = NewUser.Age;
                            Email[UserNumber] = NewUser.Email;
                            Passw[UserNumber] = NewUser.Passw;
                            UserBalance[UserNumber] = NewUser.UserBalance;

                            count1++;
                   
                            Console.WriteLine("User adding finished.");
                            if (count1 != 0) {Founds(); NoSave = false;}
                     
                Console.WriteLine("Press any key to continue.");
                Console.ReadLine();
                Console.Clear();

                if (MenuCh) { Menu2(); } else { Menu();};
            }
               
            void Autorization() 
            {
                if (!NoSave)
                {
                    bool AutorizeResult;
                    int Count3 = 5;

                    do
                    {
                        Console.WriteLine($"Your have just {Count3} attepmpts for autorization. \t");
                        Console.WriteLine("Please, enter your username: ");
                        string AutorName = Console.ReadLine();
                        Console.WriteLine("Please, enter your password: ");
                        string AutorPassw = Console.ReadLine();

                        ChekUser Autorize = new ChekUser(TotalUsers, AutorName, AutorPassw, Name, Passw);
                        (bool, int) Tuple = Autorize.Autorization();
                        AutorizeResult = Tuple.Item1;
                        if (Tuple.Item2 != -1) { CurrentUserIndex = Tuple.Item2; } else { CurrentUserIndex = 0; }

                        Count3--;
                        if (Count3 == 0) { Console.WriteLine("You are not autorize. Program halted."); break; }
                    }
                    while (AutorizeResult);
                                       
                    if (Count3 == 0) { }
                    else
                    {
                        Console.WriteLine($"User {Name[CurrentUserIndex]} has been autorized.");
                        Console.WriteLine($"For user {CurrentUserIndex + 1}." +
                        $"DateTime: {UserDateTime[CurrentUserIndex]}, Name: {Name[CurrentUserIndex]}, Famely: {Famely[CurrentUserIndex]}," +
                        $" Town: {Town[CurrentUserIndex]}, Age: {Age[CurrentUserIndex]}, Email: {Email[CurrentUserIndex]}, Password: {Passw[CurrentUserIndex]} \t");

                        Console.WriteLine("Press any key to continue.");
                        Console.ReadLine();
                        Menu2();
                    }
                }                
            }
           
               void delete()
            {
                double Balance1;
                if ((!NoSave) && Name[CurrentUserIndex] == "Admin")
                {
                    Console.WriteLine("The full list of users at the system: \t");
                    for (int i = 0; i < TotalUsers; i++)
                    {
                        try
                        {
                        object [] UserFounds2 = (object[])UserBalance[i];                     
                        Balance1 = (double)UserFounds2.Last();
                        }
                        catch { Balance1 = 0; }                       

                        Console.WriteLine($"User {i + 1}." +
                            $"DateTime: {UserDateTime[i]}, Name: {Name[i]}, Famely: {Famely[i]}, Town: {Town[i]}, Age: {Age[i]}, Email: {Email[i]}, Password: {Passw[i]}, Balance: {Balance1} \t");
                    }

                    Console.WriteLine($"If you want to delete User, please, input user number for deleting.");
                    int DelUser;
                    try { DelUser = Convert.ToInt32(Console.ReadLine()); } catch { Console.WriteLine("Invalid input."); DelUser = -1; }
                    if ((DelUser <= TotalUsers) && (TotalUsers > 0) && (DelUser != -1))
                    { Delete del = deleting; del(DelUser); }
                }
                else if (!NoSave)
                {
                    Console.WriteLine($"If you want to delete User {Name[CurrentUserIndex]}, please, confirm it by typing world 'delete'.");
                    string DelUser = Console.ReadLine();
                    if (Equals(DelUser, "delete")) { Delete del = deleting; del(CurrentUserIndex); }
                }
                               
                Console.WriteLine("Press any key to continue.");
                Console.ReadLine();
               
                if (temp) { Menu2(); } else { Menu(); };
             }

              void deleting (int DelIndx) 
            {
                try
                {
                    UserDateTime = UserDateTime.Except(new string[] { UserDateTime[DelIndx-1] }).ToArray();
                    Name = Name.Except(new string[] { Name[DelIndx-1] }).ToArray();
                    Famely = Famely.Except(new string[] { Famely[DelIndx-1] }).ToArray();
                    Town = Town.Except(new string[] { Town[DelIndx-1] }).ToArray();
                    Age = Age.Except(new int[] { Age[DelIndx-1] }).ToArray();
                    Email = Email.Except(new string[] { Email[DelIndx-1] }).ToArray();
                    Passw = Passw.Except(new string[] { Passw[DelIndx-1] }).ToArray();
                    UserBalance = UserBalance.Except(new object[] { UserBalance[DelIndx - 1] }).ToArray();

                    TotalUsers--; UserNumber--;

                    Array.Resize(ref UserDateTime, TotalUsers);
                    Array.Resize(ref Name, TotalUsers);
                    Array.Resize(ref Famely, TotalUsers);
                    Array.Resize(ref Town, TotalUsers);
                    Array.Resize(ref Age, TotalUsers);
                    Array.Resize(ref Email, TotalUsers);
                    Array.Resize(ref Passw, TotalUsers);
                    Array.Resize(ref UserBalance, TotalUsers);

                }
                catch {
                    if (DelIndx != -1 )
                    {
                        object[][] User2 = new object[TotalUsers][];
                        LoadSave Save1 = new LoadSave(User2, UserNumber, delkey = true); ;
                        Save1.Save(); Console.WriteLine("The system is empty."); temp = false;
                    }
                } 
             if (DelIndx != -1 || !delkey)
               {
                    object[][] User1 = new object[TotalUsers][];
                    for (int i = 0; i < TotalUsers; i++)
                    {
                        User1[i] = new object[8]{ UserDateTime[i], Name[i], Famely[i], Town[i],
                            Age[i], Email[i], Passw[i], UserBalance[i]};
                    }
                    if (UserNumber == -1) { delkey = true; temp = false; Console.WriteLine("The system is empty."); }
                    LoadSave Save = new LoadSave(User1, UserNumber, delkey);
                    Save.Save(); Console.WriteLine("User has been deleted. Information saved. \t");
                }
                delkey = false;
            }           
          }        
        }   
      }