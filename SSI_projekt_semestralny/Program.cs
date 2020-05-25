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
            przewidywanie.PrintList();
            Day dzien0 = new Day(20, 0, 3, 53, 17, 5, 5);
            Day dzien1 = new Day(20, 0, 3, 53, 17, 5, 5);
            Day dzien2 = new Day(20, 0, 3, 53, 17, 5, 5);
            dzien0.SetProposition(przewidywanie.KNN(dzien0, 10));
            dzien1.SetProposition(przewidywanie.WKNN(dzien1, 10));
            dzien2.SetProposition(przewidywanie.Bayes(dzien2));
            Console.WriteLine(dzien0.toString());
            Console.WriteLine(dzien1.toString());
            Console.WriteLine(dzien2.toString());
            Console.ReadKey();
        }
    }
}
