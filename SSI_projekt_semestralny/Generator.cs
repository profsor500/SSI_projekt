using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSI_projekt_semestralny
{
    class Generator
    {
        Generator() 
        { 
        
        }
        public static void DayToTxtFile(int number, string path) 
        {
            var rand = new Random();
            using (System.IO.StreamWriter file =
            new System.IO.StreamWriter(@path))
            {
                for (int i = 0; i < number; i++)
                {
                    int Temp = rand.Next(-30,40);
                    if (Temp > 5) file.WriteLine("{0};{1};{2};{3};{4};{5};{6};{7}", 
                    Temp, rand.Next(100), rand.Next(15), rand.Next(71), rand.Next(101), 0, rand.Next(101) , rand.Next(101));
                    else file.WriteLine("{0};{1};{2};{3};{4};{5};{6};{7}",
                    Temp, rand.Next(100), rand.Next(15), rand.Next(71), rand.Next(101), rand.Next(101), rand.Next(101) , rand.Next(101));
                }
            }
        }
        public static Day RandDay() 
        {
            var rand = new Random();
            
                int Temp = rand.Next(-30, 40);
                if (Temp > 5) return new Day(Temp, rand.Next(101), rand.Next(20), rand.Next(71), rand.Next(101), 0, rand.Next(101));
                else return new Day(Temp, rand.Next(101), rand.Next(20), rand.Next(71), rand.Next(101), rand.Next(101), rand.Next(101));
        }
    }
}
