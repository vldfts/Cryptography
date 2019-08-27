using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
namespace Geffe_generator
{
    class Program
    {
        static List<List<bool>> finish_L1 = new List<List<bool>>();
        static List<List<bool>> finish_L2 = new List<List<bool>>();

        static List<List<bool>> FINISH = new List<List<bool>>();
        static List<Thread> threads3 = new List<Thread>();
        static List<long> l_1 = new List<long> { 1 };
        static List<long> l_2 = new List<long> { 1 };
        static List<long> l_3 = new List<long> { 1 };
        static List<int> s_list_int_t = new List<int>();//місце де в L3 мають стояти відомі значення
        static List<int> s_list_int_f = new List<int>();//місце де в L3 мають стояти невідомі значення
        static int[] L2 = new int[] { 30, 0, 1, 4, 6 };//перше - максимасимальне n, далі записуються номери степенів у поліномі, спочатку 0 а потім можна хаотично
        static int[] L1 = new int[] { 31, 0, 3 };
        static int[] L3 = new int[] { 32, 0, 1, 2, 3, 5, 7 };
        static string data = File.ReadAllText("data141.txt");

        //static int[] L1 = new int[] { 25, 0, 3 };//перше - максимасимальне n, далі записуються номери степенів у поліномі, спочатку 0 а потім можна хаотично
        //static int[] L2 = new int[] { 26, 0, 1, 2, 6 };
        //static int[] L3 = new int[] { 27, 0, 1, 2, 5 };
        //static string data = File.ReadAllText("data14.txt");

        static bool flag0 = false;
        static List<int> koef_L1 = Koef(L1);
        static List<int> koef_L2 = Koef(L2);

        static List<bool> z = ReadText(data, data.Length);

        static void Pererozpodil(int n, int k, List<long> L)
        {
            long s = 0;
            if (n < 31)
            {
                s = (2 << (n - 1)) / k;
            }
            else { s = Convert.ToInt64(Math.Pow(2, n)); }
            long m = s;
            for (int i = 0; i < k; i++)
            {
                L.Add(s);
                s += m;
            }
        }
        static List<int> Koef(int[] L)
        {
            List<int> koef_list = new List<int>();
            double p1 = 0.25;
            double p2 = 0.5;
            double t1 = 2.326;
            double beta = 1 / Math.Pow(2, L[0]);
            Console.WriteLine("Введiть значення квантиля норм розподiлу для = " + (1 - beta));
            double t2 = Convert.ToDouble(Console.ReadLine());
            int N = Convert.ToInt16((Math.Pow((t2 * Math.Sqrt(p2 * (1 - p2)) + t1 * Math.Sqrt(p1 * (1 - p1))) / (p1 - p2), 2)));
            int C = Convert.ToInt16((N * p1 + t1 * Math.Sqrt(N * p1 * (1 - p1))));
            koef_list.Add(N);
            koef_list.Add(C);
            Console.WriteLine("N = " + N);
            Console.WriteLine("C = " + C + "\n" + new string('_', 20));
            return koef_list;
        }
        static bool Statistika(List<bool> gen_list_L, List<int> koef_L)
        {
            int N = koef_L[0];
            int C = koef_L[1];
            int R = 0;
            List<bool> data_list = ReadText(data, Convert.ToInt32(N));
            for (int r = 0; r < Convert.ToInt32(N); r++)
            {
                if (gen_list_L[r] != data_list[r]) { R++; }
            }
            if (R > C) { return false; }
            else { return true; }
        }
        static List<bool> ReadText(string text, int n)
        {
            List<bool> data_bool_list = new List<bool>();
            for (int v = 0; v < n; v++)
            {
                if (text[v] == '1') { data_bool_list.Add(true); }
                else { data_bool_list.Add(false); }
            }
            return data_bool_list;
        }
        public static void Generator_L1(object N)
        {
            long first = l_1[Convert.ToInt32(N)];
            long second = l_1[Convert.ToInt32(N) + 1];

            while (first < second)
            {
                string str = Convert.ToString(first, 2).PadLeft(L1[0], '0');
                List<bool> b = str.Select((x) => x == '1').ToList();
                for (int j = 0; j < koef_L1[0] - L1[0]; j++)
                {
                    bool z = b[j];
                    for (int u = 2; u < L1.Length; u++)//чого u = 2 а не 1?
                    {
                        z = z ^ b[L1[u] + j];
                    }
                    b.Add(z);
                }
                if (Statistika(b, koef_L1))
                {
                    b.RemoveRange(L1[0], b.Count - L1[0]);
                    finish_L1.Add(b);
                    //Console.WriteLine(str);
                }


                //finish.Add(b);

                //Console.WriteLine(str);
                first++;
            }
        }
        public static void Generator_L2(object N)
        {
            long first = l_2[Convert.ToInt32(N)];
            long second = l_2[Convert.ToInt32(N) + 1];

            while (first < second)
            {
                string str = Convert.ToString(first, 2).PadLeft(L2[0], '0');
                List<bool> b = str.Select((x) => x == '1').ToList();
                for (int j = 0; j < koef_L2[0] - L2[0]; j++)
                {
                    bool z = b[j];
                    for (int u = 2; u < L2.Length; u++)
                    {
                        z = z ^ b[L2[u] + j];
                    }
                    b.Add(z);
                }
                if (Statistika(b, koef_L2))
                {
                    b.RemoveRange(L2[0], b.Count - L2[0]);
                    finish_L2.Add(b);
                    //Console.WriteLine(str);
                }


                //finish.Add(b);

                //Console.WriteLine(str);
                first++;
            }
        }
        public static void Generator_L3(object N)
        {
            while (!flag0)
            {
                if (flag0) { break; }
                long first = l_3[Convert.ToInt32(N)];
                long second = l_3[Convert.ToInt32(N) + 1];
                //int first = 1;
                //int second = 2 << (s_list_int_f.Count-1);
                List<bool> s_not_full = new List<bool>();
                for (int t = 0; t < L3[0]; t++) { s_not_full.Add(false); }
                foreach (int h in s_list_int_t)
                {
                    if (FINISH[0][h] == z[h]) { s_not_full[h] = true; }
                }
                while (first < second)
                {
                    if (flag0) { break; }
                    string str = Convert.ToString(first, 2).PadLeft(s_list_int_f.Count, '0');
                    List<bool> s_perebor = str.Select((x) => x == '1').ToList();
                    for (int zi = 0; zi < s_list_int_f.Count; zi++)
                    {
                        s_not_full[s_list_int_f[zi]] = s_perebor[zi];
                    }
                    List<bool> s = Generator(s_not_full, L3, data.Length);
                    bool flag2 = false;
                    for (int count = 0; count < data.Length; count++)
                    {
                        if (s[count])
                        {
                            if (FINISH[0][count] == z[count]) { flag2 = true; }
                            else { flag2 = false; break; }
                        }
                        else
                        {
                            if (FINISH[1][count] == z[count]) { flag2 = true; }
                            else { flag2 = false; break; }
                        }
                    }
                    if (flag2)
                    {
                        for (int e = 0; e < L1[0]; e++) { char a = (FINISH[0][e]) ? '1' : '0'; Console.Write(a); }
                        Console.Write("\n");
                        for (int c = 0; c < L2[0]; c++) { char a = (FINISH[1][c]) ? '1' : '0'; Console.Write(a); }
                        Console.Write("\n");
                        foreach (var v in s_not_full) { char a = (v) ? '1' : '0'; Console.Write(a); }
                        Console.Write("\n");
                        flag0 = true;
                        break;
                    }
                    first++;
                }
            }
        }


        static List<bool> Generator(List<bool> finish, int[] L, int n)
        {

            List<bool> finish2 = new List<bool>();
            foreach (bool v in finish) { finish2.Add(v); }
            for (int k = 0; k < n - L[0]; k++)
            {
                bool z = finish2[k];
                for (int u = 2; u < L.Length; u++)
                {
                    z = z ^ finish2[L[u] + k];
                }
                finish2.Add(z);
            }
            //Console.WriteLine(finish.Count);
            return finish2;
        }
        static void Check()
        {
            foreach (var v in finish_L2)
            {
                List<bool> fin_l2 = Generator(v, L2, data.Length);
                foreach (var g in finish_L1)
                {
                    List<int> s_list_intt = new List<int>();
                    List<int> s_list_intf = new List<int>();
                    List<bool> fin_l1 = Generator(g, L1, data.Length);
                    bool flag = false;

                    for (int i = 0; i < fin_l1.Count; i++)
                    {
                        if (fin_l1[i] == fin_l2[i])
                        {
                            if (fin_l1[i] == z[i]) { flag = true; if (i < L3[0]) { s_list_intf.Add(i); } }
                            else { flag = false; break; }
                        }
                        else { if (i < L3[0]) { s_list_intt.Add(i); } }

                    }
                    if (flag)
                    {
                        FINISH.Add(fin_l1);
                        FINISH.Add(fin_l2);
                        s_list_int_t = s_list_intt;
                        s_list_int_f = s_list_intf;
                    }
                }
            }
        }

        static void Main(string[] args)
        {
            Stopwatch sw1 = new Stopwatch();
            sw1.Start();
            int thread_count = 10;

            int i = 0;
            int j = 0;
            int q = 0;
            //int e = 0;

            Pererozpodil(L1[0], thread_count, l_1);
            Pererozpodil(L2[0], thread_count, l_2);
            List<Thread> threads1 = new List<Thread>();
            List<Thread> threads2 = new List<Thread>();
            List<Thread> threads3 = new List<Thread>();
            while (i < thread_count)
            {
                threads1.Add(new Thread(new ParameterizedThreadStart(Generator_L1)));
                threads1[i].Start(i);

                ++i;
            }
            while (i > 1)
            {
                threads1[i - 1].Join();
                i--;
            }
            Console.Beep();
            Console.WriteLine((sw1.ElapsedMilliseconds / 60000.0).ToString());
            while (j < thread_count)
            {
                threads2.Add(new Thread(new ParameterizedThreadStart(Generator_L2)));
                threads2[j].Start(j);

                ++j;
            }
            while (j > 1)
            {
                threads2[j - 1].Join();
                j--;
            }
            Console.Beep();
            Console.WriteLine((sw1.ElapsedMilliseconds / 60000.0).ToString());

            Check();
            //Generator_L3();
            Pererozpodil(s_list_int_f.Count, thread_count, l_3);
            while (q < thread_count)
            {
                threads3.Add(new Thread(new ParameterizedThreadStart(Generator_L3)));
                threads3[q].Start(q);
                ++q;
            }
            while (q > 1)
            {
                threads3[q - 1].Join();
                q--;
            }

            Console.Beep();
            Console.WriteLine((sw1.ElapsedMilliseconds / 60000.0).ToString());
            sw1.Stop();

            Console.ReadLine();
        }
    }
}
