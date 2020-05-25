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
            Day dzien = new Day(20, 0, 3, 53, 17, 5, 5);
            dzien.SetProposition(przewidywanie.KNN(dzien, 10));
            Console.WriteLine(dzien.toString());
            //var wknnwynik = przewidywanie.WKNN(dzien, 10);
            //foreach (var k in knnwynik.Keys) { Console.Write("{0}: {1}, ", k, knnwynik[k]); }
            Console.WriteLine();
            
            //foreach (var k in wknnwynik.Keys) { Console.Write("{0}: {1}, ",k,knnwynik[k]); }



            //IDictionary<string, int> dict0 = new Dictionary<string, int>() { { "radek", 0 }, { "radekm", 1 }, { "rade", 0 } };
            //IDictionary<string, int> dict1 = new Dictionary<string, int>() { { "radek", 0 }, { "radekm", 1 }, { "rade", 0 } };
            //if(dict0.Count == dict1.Count && !dict0.Except(dict1).Any()) Console.WriteLine("takie same");
            //else Console.WriteLine("rozne");



           Console.ReadKey();
        }
    }
}
