using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSI_projekt_semestralny
{
    class Program
    {
        static void Main(string[] args)
        {
            //Kod służący wygenerowaniu Dni do pliku tekstowego
            //Generator.DayToTxtFile(100, @"C:\Users\profsor500\Desktop\Studia\SystemySztucznejInteligencji\SSI_projekt_semestralny\SSI_projekt_semestralny\DaysList.txt");
            var przewidywanie = new Provide();
            przewidywanie.GetDays(@"C:\Users\profsor500\Desktop\Studia\SystemySztucznejInteligencji\SSI_projekt_semestralny\SSI_projekt_semestralny\WeatherDataSet.txt");
            //przewidywanie.PrintList();
            int number;
            string k;
            do
            {
                Console.Clear();
                Console.Write("Podaj liczbę od 0 do {0}, która będzie 'K' dla metody KNN i WKNN: ", przewidywanie.DaysCount()); k = Console.ReadLine();
            } while (!Int32.TryParse(k, out number) || number<=0 || number> przewidywanie.DaysCount());
            Day dzien0 = new Day(20, 0, 3, 53, 17, 5, 5);
            Day dzien1 = new Day(20, 0, 3, 53, 17, 5, 5);
            Day dzien2 = new Day(20, 0, 3, 53, 17, 5, 5);
            Day dzien3 = new Day(20, 0, 3, 53, 17, 5, 5);
            dzien0.SetProposition(przewidywanie.KNN(dzien0, number));
            dzien1.SetProposition(przewidywanie.WKNN(dzien1, number));
            dzien2.SetProposition(przewidywanie.Bayes(dzien2));
            dzien3.AdjustProposition();
            Console.WriteLine("\n Przyporządkowanie 'Właściwe':");
            Console.WriteLine(dzien3.toString());
            Console.WriteLine("\n propozycja KNN:");
            Console.WriteLine(dzien0.toString());
            Console.WriteLine("\n propozycja WKNN:");
            Console.WriteLine(dzien1.toString());
            Console.WriteLine("\n propozycja Bayes:");
            Console.WriteLine(dzien2.toString());
            Console.WriteLine();
            przewidywanie.MultiMethodsComparison(number, @"C:\Users\profsor500\Desktop\Studia\SystemySztucznejInteligencji\SSI_projekt_semestralny\SSI_projekt_semestralny\TestWeDatatSet.txt");
            Console.ReadKey();
        }
    }
}
