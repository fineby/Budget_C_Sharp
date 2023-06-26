using System;


namespace Portmone1
{ 
    internal class ChekUser
    {   
        string CurrentInput;          
        public string[] ComparyArray { get; set; }
        public string[] ComparyPassw { get; set; }
        string CurrentPassw;
        public int TotalUsers;
        bool ChekResult;

        public ChekUser(int totalusers, string currentinput, string[] comparyarray) { TotalUsers = totalusers; CurrentInput = currentinput; ComparyArray = comparyarray;  }
        public ChekUser(int totalusers, string currentinput, string currentpassw, string[] comparyarray, string[] comparypassw) { TotalUsers = totalusers; CurrentInput = currentinput;CurrentPassw=currentpassw; ComparyArray = comparyarray; ComparyPassw = comparypassw; }
        public bool Cheking()
        {
            for (int i = 0; i < TotalUsers; i++)
            {        
             if (Equals(ComparyArray[i], CurrentInput)) { ChekResult = true; Console.WriteLine("Such data already present at the system!"); return ChekResult;}                
            }
            ChekResult = false; return ChekResult;
        }
        
        public (bool,int) Autorization()
        {
            
            for (int i = 0; i < TotalUsers; i++)
            {
                if (Equals(ComparyArray[i], CurrentInput) && Equals(ComparyPassw[i], CurrentPassw)) { ChekResult = false; Console.WriteLine("Autorization succesful.");
                   (bool,int) Tuple1 = (ChekResult,i); return (Tuple1); }
            }
            ChekResult = true; Console.WriteLine("Invalid username or password!"); (bool, int) Tuple2 =(ChekResult,-1); return Tuple2;
        }   
    }
  }

