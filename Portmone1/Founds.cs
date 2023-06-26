using System;
using System.Collections.Generic;
using System.Linq;


namespace Portmone1
{
    delegate void ch1();
    public interface IFounds
    { 
      public string Name { get; set; }
      public double Balance { get; set; } 
    }

    public class SetRates
    { 
        public double EURrate { get; set; }
        public double GBPrate { get; set; }
        internal SetRates(double eurrate, double gbprate) { EURrate = eurrate; GBPrate = gbprate; }       
    }

    public class Founds : SetRates,IFounds
    {
        double rate = 1;
        bool Amnt = false;
        double Amount;
        string curr;
        public string Name { get; set; }
        public double Balance { get; set; }       
        public List<object> Operations { get; set; }        
        internal Founds(string name, double balance, List<object> operations,double eurrate, double gbprate):base(eurrate, gbprate) { Name = name; Balance = balance; Operations = operations; }
        internal Founds(string name, List<object> operations, double eurrate, double gbprate):base(eurrate,gbprate) { Name = name; Operations = operations; }
       
        string selection;
        string oper;
        public List<object> Operations1 = new List<object> { };

       public object[] FoundsData()
        {          
            do
            {
                while (!Amnt)
                {
                    Console.WriteLine($"Choose currency for operation (USD, EUR, GBP): ");
                    curr = Console.ReadLine();
                    switch (curr)
                    { case "USD": rate = 1; break; case "EUR": rate = EURrate; break; case "GBP": rate = GBPrate; break;
                        default: Console.WriteLine("Invalid input. Just USD, EUR, GBP accepted. \t"); continue; }
                    Console.WriteLine($"You choose currency {curr}.");  
                    rate = Math.Round(rate, 4);

                    Console.WriteLine($"{Name}, enter amount for operation (balance will be recalculated to USD): ");
                    Amnt = Double.TryParse(Console.ReadLine(), out Amount);
                    Amount = Math.Round(Amount, 2);
                    if (Amnt && Amount > 0 && Amount < 100000000.01)
                    {
                        Console.WriteLine("{0}, enter /-/ for WITHDRAWAL or any input for ADD: ", Name);
                        selection = Console.ReadLine();
                        Balance = selection == "-" ? (Balance - Amount * rate) : (Balance + Amount * rate);
                        Balance = Math.Round(Balance, 2);

                        if (Balance < 0)
                        { Amnt = false; Console.WriteLine("Incorrect input - final balanse must be more then null."); Balance = Balance + Amount * rate; }

                    } else { Console.WriteLine("Incorrect input - only numbers more then null and less then 100 millions acceptable. Try again."); if (Amount <= 0) { Amnt = false; } }

                }

                if (selection == "-") { oper = "-"; } else { oper = "+"; }
                Operations.AddRange(new object[] { Name, DateTime.Now, curr, rate, oper, Amount, Balance });
                Console.WriteLine("Please, press /1/ if you would like create another input.");
                if (Console.ReadLine() == "1") { Amnt = false; }

            } while (!Amnt);
            
            object[] arr = Operations.ToArray();
            return arr; 
         }

        bool sw = false;
        public void ListFounds()
        {
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------\t");
            Console.WriteLine("|        Timeframe      |  Currency |   Rate   | Operation |      Amount                   Balance(USD)       \t");
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------\t");
            
            for (int i = 0; i < Operations.Count; i++)

            { try { sw = Equals(Name, (string)Operations[i]); } catch { continue; }

                if (sw) {

                    Console.WriteLine("|  {0}  |    {1}    |  {2:0.0000}  |     {3}     |       {4:0.00}                    {5:0.00}             \t", Operations[i + 1], Operations[i + 2], Operations[i + 3], Operations[i + 4], Operations[i + 5], Operations[i + 6]);
                  
                }
            }
            Console.WriteLine("-------------------------------------------------------------------------------------------------------------\t");
            
            Console.WriteLine("Press any key to continue.");
            Console.ReadLine();
        }

        public void Report()
        {
            Console.Clear();
            bool ent1 = true;
            bool sw1;        
            var start = DateTime.Now;
            var finish = DateTime.Now;

            List<U> u = new List<U>();
            for (int i = 0; i < Operations.Count; i++)
            {
                try { sw1 = Equals(Name, Operations[i]); } catch { continue; }

                if (sw1)
                {               
                    u.Add(new U { UDate = (DateTime)Operations[i + 1], UCurr = (string)Operations[i + 2], URate = (double)Operations[i + 3], UOper = (string)Operations[i + 4], UAmount = (double)Operations[i + 5], UBalance = (double)Operations[i + 6] });
                }
            }

            while (ent1)
            {
                Console.WriteLine("For report for the period enter 1, for report with operations on particaly currency enter 2.");
                string selection1 = Console.ReadLine();
                if (Equals(selection1, "1")) { ch1 rep = OperReport; rep(); ent1 = false; }
                else if (Equals(selection1, "2")) { ch1 rep = CurrReport; rep(); ent1 = false; }
                else { Console.WriteLine("Invalid input."); }
            }

            void OperReport() 
            { bool In = false, In1 = false;
                while (!In || !In1)
                {
                    Console.WriteLine($"{Name} Enter first date for the report.");
                    In = DateTime.TryParse(Console.ReadLine(), out  start);
                    Console.WriteLine("Enter second date for the report."); 
                    In1 = DateTime.TryParse(Console.ReadLine(), out  finish);                    
                    if (!In || !In1) Console.WriteLine($"Unacceptable date. Try again.");
                    if (DateTime.Compare(start, finish) != -1 || finish > DateTime.Now ) {Console.WriteLine(" Re-input dates."); In = false; }
                    
                }
                var selectedI = u.Where(j => j.UDate >= start && j.UDate <= finish);
                Console.WriteLine("For the period {0} - {1}: ", start, finish);

                Console.WriteLine("-------------------------------------------------------------------------------------------------------------\t");
                Console.WriteLine("|        Timeframe      |  Currency |   Rate   | Operation |      Amount                   Balance(USD)       \t");
                Console.WriteLine("-------------------------------------------------------------------------------------------------------------\t");
                foreach (U u in selectedI)
                {
                    Console.WriteLine("|  {0}  |    {1}    |  {2:0.0000}  |     {3}     |       {4:0.00}                    {5:0.00}             \t", u.UDate, u.UCurr, u.URate, u.UOper, u.UAmount, u.UBalance);
                }
                Console.WriteLine("-------------------------------------------------------------------------------------------------------------\t");
           
            }
            Console.WriteLine("Press any key to continue.");
            Console.ReadLine();
            
            void CurrReport() 
            {       
                var CurrGroups = u.GroupBy(p => p.UCurr);
                foreach (IGrouping<string, U> g in CurrGroups)
                {
                    Console.WriteLine($"Currency {g.Key}");
                    foreach (var t in g)
                    Console.WriteLine($"Date: {t.UDate} and Amount: { t.UAmount}");
                    Console.WriteLine();
                }
             
            }

           }            
         }
       }