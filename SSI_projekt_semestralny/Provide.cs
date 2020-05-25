using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.CompilerServices;

namespace SSI_projekt_semestralny
{
    class Provide
    {
        List<Day> DaysList { get; set; }
        public Provide()
        {
            DaysList = new List<Day>();
        }
        public IDictionary<string, int> Bayes(Day dzien) 
        {
            var ResultPropositions = new Dictionary<string, int>();
            //przydzielanie dnia do opowiedniej grupy
            var TempLims = SetLims(dzien.WeatherConditions["Temp"], 5);
            var WindSpeedLims = SetLims(dzien.WeatherConditions["WindSpeed"], 3);
            var CloudyLims = SetLims(dzien.WeatherConditions["Cloudy"], 25);
            var RainFallLims = SetLims(dzien.WeatherConditions["RainFall"], 15);
            var SunnyHLims = SetLims(dzien.WeatherConditions["SunnyH"], 3);
            var UvLims = SetLims(dzien.WeatherConditions["Uv"], 3);
            foreach (var key in dzien.Proposition.Keys)
            {
                int[] DecCount = {0 ,0};
                int[] TempCount = { 0, 0 };
                int[] StormCount = { 0, 0 };
                int[] WindCount = { 0, 0 };
                int[] CloudyCount = { 0, 0 };
                int[] RainFallCount = { 0, 0 };
                int[] SunnyHCount = { 0, 0 };
                int[] UvCount = { 0, 0 };
                double[] result = { 0, 0 };
                foreach (var day in DaysList)
                {
                    if (day.Proposition[key] == 1) 
                    {
                        DecCount[1]++;
                        if (day.WeatherConditions["Temp"] >= TempLims[0] && day.WeatherConditions["Temp"] < TempLims[1])TempCount[1]++;
                        if (day.WeatherConditions["Storm"] == dzien.WeatherConditions["Storm"]) StormCount[1]++;
                        if (day.WeatherConditions["WindSpeed"] >= WindSpeedLims[0] && day.WeatherConditions["WindSpeed"] < WindSpeedLims[1]) WindCount[1]++;
                        if (day.WeatherConditions["Cloudy"] >= CloudyLims[0] && day.WeatherConditions["Cloudy"] < CloudyLims[1]) CloudyCount[1]++;
                        if (day.WeatherConditions["RainFall"] >= RainFallLims[0] && day.WeatherConditions["RainFall"] < RainFallLims[1]) RainFallCount[1]++;
                        if (day.WeatherConditions["SunnyH"] >= SunnyHLims[0] && day.WeatherConditions["SunnyH"] < SunnyHLims[1]) SunnyHCount[1]++;
                        if (day.WeatherConditions["Uv"] >= UvLims[0] && day.WeatherConditions["Uv"] < UvLims[1]) UvCount[1]++;
                    }
                    else //czyli jezeli Day.Proposition[key]==0
                    {
                        DecCount[0]++;
                        if (day.WeatherConditions["Temp"] >= TempLims[0] && day.WeatherConditions["Temp"] < TempLims[1]) TempCount[0]++;
                        if (day.WeatherConditions["Storm"] == dzien.WeatherConditions["Storm"]) StormCount[1]++;
                        if (day.WeatherConditions["WindSpeed"] >= WindSpeedLims[0] && day.WeatherConditions["WindSpeed"] < WindSpeedLims[1]) WindCount[0]++;
                        if (day.WeatherConditions["Cloudy"] >= CloudyLims[0] && day.WeatherConditions["Cloudy"] < CloudyLims[1]) CloudyCount[0]++;
                        if (day.WeatherConditions["RainFall"] >= RainFallLims[0] && day.WeatherConditions["RainFall"] < RainFallLims[1]) RainFallCount[0]++;
                        if (day.WeatherConditions["SunnyH"] >= SunnyHLims[0] && day.WeatherConditions["SunnyH"] < SunnyHLims[1]) SunnyHCount[0]++;
                        if (day.WeatherConditions["Uv"] >= UvLims[0] && day.WeatherConditions["Uv"] < UvLims[1]) UvCount[0]++;
                    }
                    
                }
                result[1] = DecCount[1] * TempCount[1] * StormCount[1] * WindCount[1] * CloudyCount[1] * RainFallCount[1] * SunnyHCount[1] * UvCount[1] / (DaysList.Count * Math.Pow(DecCount[1], 7));
                result[0] = DecCount[0] * TempCount[0] * StormCount[0] * WindCount[0] * CloudyCount[0] * RainFallCount[0] * SunnyHCount[0] * UvCount[0] / (DaysList.Count * Math.Pow(DecCount[0], 7));
                if (result[1] >= result[0]) ResultPropositions.Add(key, 1);
                else ResultPropositions.Add(key, 0);

            }
            return ResultPropositions;
        }
        //przydzielanie grupy dla Bayesa
        int[] SetLims(double value, int bar) 
        {
            int UpLim = bar;
            while (value >= UpLim) UpLim += bar;
            return new int[] {UpLim-bar,UpLim};
        }

        //wczytywanie dni/danych.
        public void GetDays(string path) 
        {
            int WrongLine = 0;
            var lines = File.ReadAllLines(@path);
            foreach (var line in lines.Skip(1).ToArray()) {
                string l = line.Replace("\t", "");
                double[] double_line = Array.ConvertAll(l.Split(';'), Double.Parse);
                //trochę sztywne, ale działa
                if (double_line.Length == 7)
                {
                    DaysList.Add(new Day(double_line[0], double_line[1], double_line[2], double_line[3], double_line[4], double_line[5], double_line[6]));
                    DaysList[DaysList.Count - 1].AdjustProposition();
                }
                else { WrongLine++; }
            }
            if(WrongLine>0) Console.WriteLine("nie udało się przekonwertować {0} na {1} wszystkich wyrazów", WrongLine, lines.Length-1);
        }
        
        public void GetDays(string path, int number)
        {
            int WrongLine = 0;
            var lines = File.ReadAllLines(@path);
            for (int i=1;i<number;i++)
            {
                double[] double_line = Array.ConvertAll(lines[i].Split(';'), Double.Parse);
                //trochę sztywne, ale działa
                if (double_line.Length >= 7)
                {
                    DaysList.Add(new Day(double_line[0], double_line[1], double_line[2], double_line[3], double_line[4], double_line[5], double_line[6]));
                    DaysList[DaysList.Count - 1].AdjustProposition();
                }
                else WrongLine++;
            }
            if (WrongLine > 0) Console.WriteLine("nie udało się przekonwertować {0} na {1} wszystkich wyrazów", WrongLine, lines.Length - 1);
        }

        //Wyswietlanie listy
        public void PrintList() 
        {
            foreach (Day d in DaysList) Console.WriteLine(d.toString()+"\n");
        }
        //wyświetla x początkowych dni
        public void PrintList(int x)
        {
            if (DaysList.Count<x)
                for (int i=0;i< DaysList.Count; i++) Console.WriteLine(DaysList[i].toString() + "\n");
            else
                for (int i = 0; i < x; i++) Console.WriteLine(DaysList[i].toString() + "\n");
        }

        //KNN i powiązane
        public IDictionary<string, int> KNN(Day dzien, int k) //KNN
        {
            List<knnStruct> distances = new List<knnStruct>(); //lista struktur 
            for (int i = 0; i < DaysList.Count; i++) distances.Add(new knnStruct(DaysList[i], dzien.DistanceToOther(DaysList[i])));// za pomocą metody DistanceToOther() wyliczamy odległości do każdego obiektu w bazie
            distances = distances.OrderBy(x => x.distance).ToList();//sortujemy listę rosnąco po zmiennej dystans
            var kdistances = distances.Take(k).ToList(); //budujemy nową listę, która zawiera k elementów z najmniejszym dystansem (z uprzednio posortowanej listy)
            var propos = new List<IDictionary<string, int>>(); //budujemy listę słowników przetrzymującą propozycje, co robić aktualnego dnia
            var proposCount = new List<int>();//lista, zawierająca licnzik powtórzeń, ile razy wystąpiła dana propozycja; proposCount[i] - ilość powtórzeń propozycji propos[i] 
            foreach (var d in kdistances)
                //Console.WriteLine(d.day.toString());


                for (int i = 0; i < kdistances.Count; i++)//każdy z k najblizszych
                {
                    bool nieznaleziono = true; //zmienna pomocnicza, dzięki któej budujemy "else" do poniższej potętli pętli
                    for (int j = 0; j < propos.Count; j++)//sprawdzamy każdą dorychczas zapisaną propozycję
                    {
                        //jeżeli słowniki są takie same (sprawdzne funkcję ze stackoverflow), to zwiększ odpowiedni licznik o 1, nie wykonuj "else" dla pętli, zakoncz pętle
                        if (propos[j].Count == kdistances[i].day.Proposition.Count && !propos[j].Except(kdistances[i].day.Proposition).Any()) { proposCount[j]++; nieznaleziono = false; break; };
                    }
                    if (nieznaleziono)//"else" powyższej pętli. jeżeli nie znaleziono identycznej propozycji, to:
                    {
                        propos.Add(kdistances[i].day.Proposition); //dodaj nową propozycję do listy
                        proposCount.Add(1); // dodaj nowy licznik z wartością 1 (bo wystąpiła propozycja 1 raz do tej pory)
                    }

                }
            //Console.WriteLine("spośród {0} najbliższych obiektów {1}: ", k, proposCount.Max());
            return propos[proposCount.IndexOf(proposCount.Max())];
        }
        public IDictionary<string, int> WKNN(Day dzien, int k)//KNN ważone
        {
            List<knnStruct> distances = new List<knnStruct>();
            for (int i = 0; i < DaysList.Count; i++) distances.Add(new knnStruct(DaysList[i], dzien.DistanceToOther(DaysList[i])));
            distances = distances.OrderBy(x => x.distance).ToList();
            var kdistances = distances.Take(k).ToList();
            double maxd = kdistances.Max(x => x.distance); //zmienna przetrzymująca najdłuższy z k dystansów, potrzebne ponizej
            var propos = new List<IDictionary<string, int>>();
            var proposCount = new List<double>(); //licznik listy przetrzymuje double'a, a nie inta jak w zwykłym knn

            for (int i = 0; i < kdistances.Count; i++)
            {
                bool nieznaleziono = true;
                for (int j = 0; j < propos.Count; j++)
                {// jeżeli propozycja się powtarze, to zwiększamy liczni o Math.Sqrt(1 - Math.Sqrt(kdistances[i].distance / maxd)), czyli pierwiastek z 1 minus pierwiastek z aktualny dystans podzielone przez max dystans
                    if (propos[j].Count == kdistances[i].day.Proposition.Count && !propos[j].Except(kdistances[i].day.Proposition).Any()) { proposCount[j] += Math.Sqrt(1 - Math.Sqrt(kdistances[i].distance / maxd)); nieznaleziono = false; break; };
                }
                if (nieznaleziono)
                {
                    propos.Add(kdistances[i].day.Proposition);
                    proposCount.Add(Math.Sqrt(1 - Math.Sqrt(kdistances[i].distance / maxd))); //do licznika jako pierwszą wartość dodajemy nie 1, a liczbę wyliczoną ze wzoru co powyzej
                }

            }
            //Console.WriteLine("spośród {0} najbliższych obiektów {1}: ", k, proposCount.Max());
            return propos[proposCount.IndexOf(proposCount.Max())];
        }
        struct knnStruct
        {
            public Day day; //dzień
            public double distance; //w przypadku knn trzyma to odległość między dniem próbowanym do przyporządkowania a obiektem z bazy
            public knnStruct(Day day, double distance) //konstruktor
            {
                this.day = day;
                this.distance = distance;
            }
        }

    }
}
