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
        void PrintArray(double[] array) {
            foreach (var a in array)
            {
                Console.Write(a+" ");
            }
        }
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
            Console.WriteLine("nie udało się przekonwertować {0} na {1} wszystkich wyrazów", WrongLine, lines.Length-1);
        }
        //wczytuje dni
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
            Console.WriteLine("udało się przekonwertować {0} na {1} wszystkich wyrazów", WrongLine, number);
        }
        public IDictionary<string, int> KNN(Day dzien, int k) //KNN
        {
            List<knnStruct> distances = new List<knnStruct>(); //lista struktur 
            for (int i = 0; i < DaysList.Count; i++) distances.Add(new knnStruct(DaysList[i], dzien.DistanceToOther(DaysList[i])));// za pomocą metody DistanceToOther() wyliczamy odległości do każdego obiektu w bazie
            distances = distances.OrderBy(x => x.distance).ToList();//sortujemy listę rosnąco po zmiennej dystans
            var kdistances = distances.Take(k).ToList(); //budujemy nową listę, która zawiera k elementów z najmniejszym dystansem (z uprzednio posortowanej listy)
            var propos = new List<IDictionary<string, int>>(); //budujemy listę słowników przetrzymującą propozycje, co robić aktualnego dnia
            var proposCount = new List<int>();//lista, zawierająca licnzik powtórzeń, ile razy wystąpiła dana propozycja; proposCount[i] - ilość powtórzeń propozycji propos[i] 
            foreach(var d in kdistances)
            Console.WriteLine(d.day.toString());


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
            Console.WriteLine("spośród {0} najbliższych obiektów {1}: ", k, proposCount.Max());
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
                    if (propos[j].Count == kdistances[i].day.Proposition.Count && !propos[j].Except(kdistances[i].day.Proposition).Any()) { proposCount[j]+=Math.Sqrt(1 - Math.Sqrt(kdistances[i].distance / maxd)); nieznaleziono = false;Console.WriteLine(Math.Sqrt(1 - Math.Sqrt(kdistances[i].distance / maxd))) ; break;};
                }
                if (nieznaleziono)
                {
                    propos.Add(kdistances[i].day.Proposition);
                    proposCount.Add(Math.Sqrt(1 - Math.Sqrt(kdistances[i].distance / maxd))); //do licznika jako pierwszą wartość dodajemy nie 1, a liczbę wyliczoną ze wzoru co powyzej
                }

            }
            Console.WriteLine("spośród {0} najbliższych obiektów {1}: ", k, proposCount.Max());
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



        //wyświetla liste wczytanych dni
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



    }
}
