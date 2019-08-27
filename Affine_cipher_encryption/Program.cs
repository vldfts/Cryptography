using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Affine_cipher_encryption
{
    class Program
    {
        static List<string> Abetka = new List<string>() { "а", "б", "в", "г", "д", "е", "ж", "з", "и", "й", "к", "л", "м", "н", "о", "п", "р", "с", "т", "у", "ф", "х", "ц", "ч", "ш", "щ", "ь", "ы", "э", "ю", "я" };
        static List<string> Five_Bigr_OT = new List<string>() { "ст", "но", "то", "на", "ен" };
        static string text = File.ReadAllText("14.txt");
        static int mod = 31 * 31;
        static void Read()
        {
            Console.WriteLine(text);
        }
        static List<string> Bigrms_Top_Five()
        {
            Dictionary<string, int> dict = new Dictionary<string, int>();
            for (int i = 0; i < text.Length - 2; i += 2)
            {
                if (dict.ContainsKey(text[i].ToString() + text[i + 1].ToString())) { dict[text[i].ToString() + text[i + 1].ToString()]++; }
                else { dict.Add(text[i].ToString() + text[i + 1].ToString(), 1); }
            }
            dict = dict.OrderByDescending(value => value.Value).ToDictionary(p => p.Key, p => p.Value);
            List<string> li = new List<string>();
            int k = 1;
            foreach (var v in dict)
            {
                //Console.WriteLine(v.Value+" ..:.. "+v.Key);
                li.Add(v.Key);
                k++;
                if (k > 5) { break; }
            }
            return li;
        }
        static int Number_Bigr(string bigr)
        {
            var li = bigr.ToArray();
            int numb = Abetka.IndexOf(li[0].ToString()) * 31 + Abetka.IndexOf(li[1].ToString());
            return numb;
        }//видає номер біграми //перевірено!
        static string Str_Bigr(int bigr)
        {
            int a = bigr / 31;
            int b = bigr % 31;
            if (a < 0) { a += 31; }
            if (b < 0) { b += 31; }
            string persha = Abetka[a];
            string dryga = Abetka[b];
            return persha + dryga;
        }//перевірено!
        static List<int> Func_input(int x1, int x2, int y1, int y2)//x1-розш1 x2-розш2 y1-заштфр1 y2-зашифр2
        {
            List<int> li = new List<int>();
            int x = (x1 - x2) % mod;
            if (x < 0) { x += mod; }
            int y = (y1 - y2) % mod;
            if (y < 0) { y += mod; }
            int d = NSD(x);
            if (d == 1)
            {
                //Console.WriteLine("d==1");
                int par_a = Obern_Func(x, y, mod);
                int par_b = (y1 - (par_a * x1)) % mod;
                if (par_b < 0) { par_b += mod; }
                li.Add(par_a);
                li.Add(par_b);
                return li;
                //визиває фю з стандарт знач
            }
            else
            {
                if ((y % d) != 0)
                {
                    //Console.WriteLine("y%d!=0");
                    return li;
                }
                else
                {
                    //Console.WriteLine("mod/d");
                    int par_a = Obern_Func((x) / d, (y) / d, (mod) / d);
                    int k = 0;
                    while (k < d)
                    {
                        int par_a1 = par_a + k * (mod / d);
                        li.Add(par_a1);
                        int par_b = (y1 - par_a1 * x1) % (mod);
                        if (par_b < 0) { par_b += mod; }
                        li.Add(par_b);

                        k++;
                    }
                    return li;
                }
            }
        }
        static int Obern_Func(int x, int y, int mod1)//ax=b(modm)//видає х
        {
            int b = mod1;
            x = x % mod1;
            List<int> q = new List<int>();
            int zile = 0;
            int ostacha = 0;
            while (x != 0)
            {
                zile = mod1 / x;
                q.Add(zile);
                ostacha = mod1 % x;
                mod1 = x;
                x = ostacha;
            }
            int p0 = 0;
            int p1 = 0;
            int p = 1;
            for (int i = 0; i < q.Count - 1; i++)
            {
                p1 = p0;
                p0 = p;
                p = p * q[i] + p1;
            }
            int a = (Convert.ToInt32(Math.Pow(-1, q.Count - 1)) * p * y) % b;
            if (a < 0) { return a + b; }
            else { return a; }

        }
        static int NSD(int a)
        {
            int b = mod;
            while (b != 0)
            {
                b = a % (a = b);
            }
            return a;
        }

        static bool Check(string text1)
        {
            //if(text1.)
            if (text1.Contains("аы") || text1.Contains("аь") || text1.Contains("оь") || text1.Contains("ыь") || text1.Contains("еь"))
            {
                return false;
            }
            else
                return true;
        }

        static string Decrypt(int a, int b)
        {
            string zag_text = "";
            for (int i = 0; i < text.Length; i += 2)
            {
                string ab = text[i].ToString() + text[i + 1].ToString();
                //Console.Write(ab);
                int int_bigr = (Obern_Func(a, 1, mod) * (Number_Bigr(ab) - b)) % mod;
                if (int_bigr < 0) { int_bigr += mod; }
                zag_text += Str_Bigr(int_bigr);
            }
            return zag_text;

        }
        static void Enter()
        {
            int c = 0;
            List<string> b = Bigrms_Top_Five();
            string x1, x2, y1, y2;
            for (int i = 0; i < 5; i++)
            {
                y1 = b[i];
                for (int ix = 0; ix < 5; ix++)
                {
                    x1 = Five_Bigr_OT[ix];
                    for (int j = 0; j < 5; j++)
                    {
                        if (j == i) { continue; }
                        else
                        {
                            y2 = b[j];

                        }
                        for (int jx = 0; jx < 5; jx++)
                        {
                            if (jx == ix) { continue; }
                            else
                            {
                                x2 = Five_Bigr_OT[jx];
                                List<int> ji = Func_input(Number_Bigr(x1), Number_Bigr(x2), Number_Bigr(y1), Number_Bigr(y2));
                                //Console.WriteLine(y1+" "+x1+" "+y2+" "+x2);
                                //try
                                for (int t = 0; t < ji.Count; t += 2)
                                {
                                    //Console.WriteLine("a = " + ji[t] + "  b = " + ji[t + 1]);
                                    string zag_text = Decrypt(ji[t], ji[t + 1]);

                                    if (Check(zag_text))
                                    {
                                        Console.WriteLine(y1 + " " + x1 + " " + y2 + " " + x2);
                                        Console.WriteLine("a = " + ji[t] + "  b = " + ji[t + 1]);
                                        Console.WriteLine(zag_text);
                                        Console.WriteLine(c + "\n" + new string('-', 50));


                                    }
                                    c++;


                                    //break;
                                    //Console.WriteLine(ji[t]+" - "+ji[t+1]);
                                }


                                //catch { continue; }

                                // Console.WriteLine("\n"+new string('-',50));
                            }
                        }
                    }
                }
            }
            //Console.WriteLine(c);

        }
        static void Main(string[] args)
        {
            //Read();
            //Bigrms_Top_Five();

            //Console.WriteLine(Abetka.IndexOf("я"));
            //Console.WriteLine(Obern_Func(111,75,31));
            //List<int> ji = Func_input(9,2,9,1);
            //foreach(var v in ji)
            //{
            //    Console.WriteLine(v);
            //}
            Enter();
            //відповідь 74
            //Decrypt(12,13);
            //Console.WriteLine(Str_Bigr(63));
            Console.ReadKey();
        }
    }
}
