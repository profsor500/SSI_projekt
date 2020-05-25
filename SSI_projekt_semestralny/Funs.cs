using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSI_projekt_semestralny
{
    class Funs
    {
        static public double Trojkatna(double a, double b, double c, double x)
        {
            if (x < a || x > c) return 0;
            else if (x < b) return (x - a) / (b - a);
            else return (x - b) / (b - c);
        }
        static public double Trapezowa(double a, double b, double c, double d, double x)
        {
            if (x < a || x > d) return 0;
            else if (x < b) return (x - a) / (b - a);
            else if (x < c) return (x - c) / (b - c);
            else return (x - d) / (c - d);
        }

        static public double Zadeh(double pp, double x) 
        { 
            return 1 / (1 - (x - pp)); 
        }
        static public double Dzwon(double x, double a, double b, double c)
        {
            var z = Math.Pow(Math.Abs((x - c) / a), 2*b);
            return 1 / (1 +z);
        }

        static public double Gauss(double x, double b, double a) 
        {
            if(a!=0)
            return Math.Exp(-1 * Math.Pow((x - b) / a, 2));
            else return Math.Exp(-1 * Math.Pow((x - b) / 0.00000000000001, 2));
        }
    }
}
