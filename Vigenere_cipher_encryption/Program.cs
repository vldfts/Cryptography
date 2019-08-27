using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Vigenere_cipher_encryption
{
    class Program
    {
        static List<string> Abetka = new List<string>() { "а", "б", "в", "г", "д", "е", "ж", "з", "и", "й", "к", "л", "м", "н", "о", "п", "р", "с", "т", "у", "ф", "х", "ц", "ч", "ш", "щ", "ъ", "ы", "ь", "э", "ю", "я" };
        static List<string> KeyList = new List<string>() { "то", "вау", "трио", "витер", "здаров", "розшифроване" };
        static DirectoryInfo d = new DirectoryInfo(Directory.GetCurrentDirectory());

        static void Encrypt(string file)
        {
            d.CreateSubdirectory("data");
            string data0 = File.ReadAllText(file).ToLower();
            Regex reg = new Regex(@"\W");
            string data = reg.Replace(data0, "");
            using (StreamWriter writer0 = File.CreateText("data\\data.txt")) { writer0.Write(data); writer0.Close(); }
            for (int i = 0; i < KeyList.Count; i++)
            {
                using (StreamWriter writer = File.CreateText("data\\Encr_Lenth_" + KeyList[i].Length.ToString() + ".txt"))
                {
                    int j = 0;
                    var li = KeyList[i].ToArray();
                    foreach (var v in data)
                    {
                        try
                        {

                            string bukv = Abetka[(Abetka.IndexOf(v.ToString()) + Abetka.IndexOf(li[j % li.Length].ToString())) % Abetka.Count];
                            writer.Write(bukv);
                        }
                        catch { continue; }
                        j++;

                    }
                    writer.Close();

                }
            }

        }

        static List<Dictionary<string, int>> Count(string file, int r)
        {
            string data0 = File.ReadAllText(file);
            Regex reg = new Regex(@"\W");
            string data = reg.Replace(data0, "");
            //Console.Write(data);
            List<Dictionary<string, int>> genList = new List<Dictionary<string, int>>();
            int k = 0;

            while (k != r)
            {
                Dictionary<string, int> dict = new Dictionary<string, int>();//якщо 30 то треба дод 30 значень ш поділити на 30
                for (int i = k; i < data.Length; i += r)
                {

                    if (dict.ContainsKey(data[i].ToString()))
                    {
                        dict[data[i].ToString()]++;
                    }
                    else { dict.Add(data[i].ToString(), 1); }

                }
                foreach (var v in dict)
                {
                    // Console.WriteLine(v.Key+" -- "+ v.Value);
                }
                genList.Add(dict);
                //Console.WriteLine("--------------" );
                k++;
            }
            return genList;
        }//розбиваєм текст на r блоків
        static double Index(string file, int r)
        {
            List<Dictionary<string, int>> genList = Count(file, r);
            List<double> indexList = new List<double>();
            string data = File.ReadAllText(file);
            double index = 0;

            for (int i = 0; i < genList.Count; i++)
            {
                double index0 = 0;
                double index1 = 0;
                foreach (var v in genList[i])
                {
                    index0 += (v.Value) * (v.Value - 1);
                    index1 += v.Value;
                    //Console.WriteLine(index0 + "  ---------");
                }
                //Console.WriteLine(data.Length);
                //index += index0/(data.Length*(data.Length-1));//індекс для блока r
                index += index0 / (index1 * (index1 - 1));
                //Console.WriteLine(index);
            }
            //Console.WriteLine(index / r);
            return index / r;//середнє ар для всіх блоків r

        }//сер індекс для r блоків
        static int KeyLenth(string file)
        {
            int r = 30;
            int t = 0;
            double h = 0;
            while (r != 1)
            {
                double g = Index(file, r);
                Console.WriteLine("{0} -- {1}", r, Math.Round(g, 6));//показати кожне значення 
                if (h < g)
                {
                    h = g;
                    t = r;
                }
                r--;
            }
            //Console.WriteLine(new string('=', 30));
            //Console.WriteLine("Max при " + t + " -- " + h); переписати не під максимум а під найблище до вт
            return t;
        }
        static void ShwIndexForFiles()
        {
            foreach (var v in d.GetFiles("*.txt", SearchOption.AllDirectories))
            {
                try
                {
                    double a = Index("data//" + v.ToString(), 1);
                    Console.Write(v.ToString() + " =\t");
                    Console.WriteLine(a);
                }
                catch { continue; }
            }
        }// виводить індекс для файлів

        static void Decrypt(string crypt_text, int r)
        {
            List<Dictionary<string, int>> FrqEncrList = Count(crypt_text, r);//r словників з кількостями кожної букви
            string[] list1 = new string[FrqEncrList.Count];//для букви о
            string[] list2 = new string[FrqEncrList.Count];//для букви е
            string[] list3 = new string[FrqEncrList.Count];//для букви а
            for (int i = 0; i < FrqEncrList.Count; i++)
            {
                int h = 0;
                string m = "";
                foreach (var v in FrqEncrList[i])
                {
                    //Console.WriteLine(v.Key + " -- " + v.Value);
                    if (h < v.Value)
                    {
                        h = v.Value;
                        m = v.Key;

                    }

                }

                //Console.WriteLine("==================");
                //Console.WriteLine(m+ " -- " + h);
                //Console.WriteLine(n + " -- " + g);
                //Console.WriteLine(Abetka.IndexOf("е"));
                //Console.WriteLine(Abetka.IndexOf("ы"));
                //Console.WriteLine(Abetka[8]);
                //Console.WriteLine(Abetka[20]);


                list1[i] = Abetka[((Abetka.IndexOf(m) - Abetka.IndexOf("о")) + Abetka.Count) % Abetka.Count];

                list2[i] = Abetka[((Abetka.IndexOf(m) - Abetka.IndexOf("e")) + Abetka.Count) % Abetka.Count];

                list3[i] = Abetka[((Abetka.IndexOf(m) - Abetka.IndexOf("а")) + Abetka.Count) % Abetka.Count];

            }

            for (int s = 0; s < 3; s++)
            {

                for (int d = 0; d < list1.Length; d += 2)
                {
                    try
                    {
                        if (s == 0) { Console.Write(list1[d]); Console.Write(list1[d + 1]); }
                        else if (s == 1) { Console.Write(list2[d]); Console.Write(list2[d + 1]); }
                        else { Console.Write(list3[d]); Console.Write(list3[d + 1]); }

                    }
                    catch { continue; }
                }
                Console.WriteLine();
                //Console.WriteLine("\n===========");
            }



        }
        static void Main(string[] args)
        {
            //Encrypt("data0.txt");

            string file = "Encr_lab.txt";

            ShwIndexForFiles();

            //Console.WriteLine(KeyLenth(file));



            //for (int i = 2; i <= 30; i++)
            //{
            //    Console.WriteLine(i);
            //    Decrypt(file, i);
            //    Console.WriteLine("-----------------");
            //}

            //
            Decrypt(file, KeyLenth(file));

            Console.ReadKey();
        }
    }
}
