using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSI_projekt_semestralny
{
    class Day
    {
        public IDictionary<string, double> WeatherConditions { get; set; }
        public IDictionary<string, int> Proposition { get; set; }
        public Day(double temp, double storm, double windspeed, double cloudy, double rainfall, double h, double uv)
        {
            //slownik z warunkami pogodowymi
            WeatherConditions = new Dictionary<string, double>()
            {
            {"Temp", temp },//temperatura [-30;40] (*C)
            {"Storm", storm},//wilgotność powietrza [0;100] (%)
            {"WindSpeed", windspeed},//prędkość wiatru [0;] (m/s)
            {"Cloudy", cloudy},//zachmurznie [0;100](%)
            {"RainFall", rainfall},//Opady deszczu [0;100]
            {"SunnyH", h},//Opady śniegu [0;100], dla uproszczenia są jednoznaczne z ilością śniego w sportach zimowych
            {"Uv", uv},//Opady gradu [0;100]
            };
            //słownik sugerujący, co najlepiej robić danego dnia, z automatu przypisane po użyciu metody obiekt.AdjustProposition()
            Proposition = new Dictionary<string, int>()
            {
                {"zostac w domu",0 },//jezeli burza, duże opady lub zachmurzenie
                {"spacer",0 },//cos jak aktywnosc fizyczna, ale z łagodniejszymi obostrzeniami 
                {"aktywność fizyczna",0 },// wymaga odpowiedniej temperatury i warunków pogodowych, aby odzież ochronna nie utrudniała
                {"plazowanie",0} //najbardziej "wybredna" aktywność, wymaga dużego stopnia uv, wysokiej temperatury i niskich opadów
            };
        }
        public void SetProposition(IDictionary<string, int> prop) 
        {
            this.Proposition = prop;
        }
        //funkcje oceniające, czy pogoda nadaje się do jakiejś czynności {0;1} {nie;tak} 
        
        //funckja wyliczająca "odległość" między dwoma obiektami za pomocą pierwiastka kwadratów różnicy czynników pogodowych.
        public double DistanceToOther(Day other)
        {
            double Result = 0;
            double[] modifiers = new double[] {5,20,7,1,1,4,8};//dodatkowy modyfikator wagi brany pod uwagę przy liczeniu, aby wyrównać wartość róznic (np max wartość wiatru -13, opadów - 160)
            int i = 0;
            foreach (var key in this.WeatherConditions.Keys)
            {
                
                Result += Math.Sqrt(Math.Pow((this.WeatherConditions[key] - other.WeatherConditions[key])*modifiers[i], 2));
                i++;
            }
            return Result;
        }
        public string toString() 
        {
            string result = "";
            foreach(var item in WeatherConditions) result+=item.Key+": "+ item.Value.ToString()+" ";
            result += "\n";
            foreach (var item in Proposition) result += item.Key + ": " + item.Value.ToString() + " ";
            return result;

        }
        
        public void AdjustProposition()
        {
            Proposition["zostac w domu"] = StayHome();
            Proposition["spacer"] = Walk();
            Proposition["aktywność fizyczna"] = Workout();
            Proposition["plazowanie"] = Beaching();
        }
        int StayHome()
        {
            if (this.WeatherConditions["Storm"] == 1) return 1;
            else if (this.WeatherConditions["Uv"] >= 8) return 1;
            else if (this.WeatherConditions["RainFall"] > 70) return 1;
            return 0;
        }
        int Walk() 
        {
            if ((this.WeatherConditions["Storm"] == 1 || this.WeatherConditions["RainFall"] > 70) && this.WeatherConditions["SunnyH"] <= 3 ) return 0;
            else if (this.WeatherConditions["Uv"] >= 8 && this.WeatherConditions["Cloudy"] <70) return 0;
            else if (this.WeatherConditions["RainFall"] > 70 && this.WeatherConditions["SunnyH"] <= 3) return 0;
            return 1;
        }
        int Workout() 
        {
            if (this.WeatherConditions["Storm"] == 1 && this.WeatherConditions["SunnyH"] <= 3) return 0;
            else if (this.WeatherConditions["Uv"] >= 8 && this.WeatherConditions["SunnyH"] > 8) return 0;
            else if (this.WeatherConditions["RainFall"] > 70 && this.WeatherConditions["SunnyH"] <= 3) return 0;
            else if (this.WeatherConditions["Temp"] > 25) return 0;
            return 1;
        }
        int Beaching()
        {
            if (this.WeatherConditions["Storm"] == 1 || this.WeatherConditions["SunnyH"] <= 3) return 0;
            else if (this.WeatherConditions["Uv"] >= 8 || this.WeatherConditions["Uv"] <= 4) return 0;
            else if (this.WeatherConditions["RainFall"] > 70 && this.WeatherConditions["SunnyH"] <= 3) return 0;
            else if (this.WeatherConditions["Temp"] < 20) return 0;
            return 1;
        }
    }

}