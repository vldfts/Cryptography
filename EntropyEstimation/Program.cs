using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace EntropyEstimation
{
    class Program
    {
        static Dictionary<string, int> Frq_dict(string data, bool mono, bool peret, bool probil)
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();
            using (StreamReader read = File.OpenText(data))
            {
                string text = null;
                while ((text = read.ReadLine()) != null)
                {
                    if (mono)//виводимо монограми
                    {
                        foreach (var v in text)
                        {
                            if (dict.ContainsKey(v.ToString())) { dict[v.ToString()]++; }
                            else { dict.Add(v.ToString(), 1); }
                        }
                    }
                    else if (probil)//виводимо біграми з пробілом
                    {
                        Bigrms(peret, text, dict);
                    }
                    else //виводимо біграми без пробіла
                    {
                        text = text.Replace(" ", "");
                        Bigrms(peret, text, dict);
                    }
                }
            }
            return dict;
        }
        static void Bigrms(bool peret, string text, Dictionary<string, int> dict)
        {
            int a;
            if (peret) { a = 1; }
            else { a = 2; }//без перетину
            for (int i = 0; i < text.Length - a; i += a)
            {
                if (dict.ContainsKey(text[i].ToString() + text[i + 1].ToString())) { dict[text[i].ToString() + text[i + 1].ToString()]++; }
                else { dict.Add(text[i].ToString() + text[i + 1].ToString(), 1); }
            }

        }
        static void ShwColumn(Dictionary<string, int> li)
        {
            double entropy = 0;
            foreach (var v in li)
            {
                double a = v.Value / Suma(li);
                if (v.Key.ToString() == " ") { Console.WriteLine("[Space]" + " -- " + a); }
                else { Console.WriteLine(v.Key + " -- " + a); }
                entropy += -(a * Math.Log(a, 2));
            }
            Console.WriteLine("Ентропiя монограми = " + entropy);
            Console.WriteLine("Надлишковiсть = " + (1 - entropy / 5));
        }
        static void ShwMatrix(Dictionary<string, int> li, string text)
        {
            var lz1 = Frq_dict(text, true, true, true).Keys.OrderBy(x => x).ToArray();

            Console.WriteLine(Suma(li));
            Console.Write("\t");
            foreach (var v in lz1)
            {
                if (v.ToString() == " ") { Console.Write("[Space]" + "\t"); continue; }
                Console.Write(v + "\t");
            }
            Console.Write("\n");
            double entropy = 0;
            for (int i = 0; i < lz1.Length; i++)
            {
                if (lz1[i] == " ") { Console.Write("[Space]" + "\t"); }
                else
                {
                    Console.Write(lz1[i] + "\t");
                }
                for (int j = 0; j < lz1.Length; j++)
                {
                    try
                    {
                        double a = li[lz1[i].ToString() + lz1[j].ToString()] / Suma(li);
                        Console.Write(Math.Round(a, 5) + "\t");
                        entropy += -(a * Math.Log(a, 2)) / 2;
                    }
                    catch (KeyNotFoundException) { Console.Write("-----\t"); }

                }
                Console.Write("\n");
            }
            Console.WriteLine("Ентропiя бiграми = " + entropy);
            Console.WriteLine("Надлишковiсть = " + (1 - entropy / 5));
        }
        static double Suma(Dictionary<string, int> li)
        {
            double sum = 0;
            foreach (var v in li)
            {
                sum += v.Value;
            }
            return sum;
        }
        static void Main(string[] args)
        {
            string text = "text.txt";//написати фю обробки тексту з проб і без

            ShwColumn(Frq_dict(text, true, true, true));//монограма
            Console.WriteLine(new string('_', 300));
            Console.WriteLine("Бiграма з перетином з пробiлом");
            ShwMatrix(Frq_dict(text, false, true, true), text);
            Console.WriteLine(new string('_', 300));
            Console.WriteLine("Бiграма з перетином без пробiла");
            ShwMatrix(Frq_dict(text, false, true, false), text);
            Console.WriteLine(new string('_', 300));
            Console.WriteLine("Бiграма без перетину з пробiлом");
            ShwMatrix(Frq_dict(text, false, false, true), text);
            Console.WriteLine(new string('_', 300));
            Console.WriteLine("Бiграма без перетину без пробiла");
            ShwMatrix(Frq_dict(text, false, false, false), text);
            Console.ReadKey();
        }
    }
}
