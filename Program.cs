using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Linq;

namespace EAs
{
    class Program
    {
        public static Dictionary<char, int> n_counter;
        static string RandomString(int n, Random r)
        {
            StringBuilder sb = new StringBuilder();
            List<string> chars = new List<string>();
            for (int i = 0; i < n; i++)
            {
                string c = ((char)(65 + Math.Floor((r.NextDouble() * 26)))).ToString();
                /*
                try
                {
                    n_counter.Add(char.Parse(c), 1);
                }
                catch
                {
                    n_counter[char.Parse(c)] += 1;
                }
                */
                chars.Add(c);
            }
            return string.Join("", chars);
        }
        static char Add(char s, int n)
        {
            int num = (int)s + n;
            int res = num;
            if (num > 89)
            {
                res = 65 + ((num - 65) % 25);
            }
            if (num < 65)
            {
                res = 90 - (65 - num);
            }
            return char.Parse(((char)(res)).ToString());
        }
        static string Mutate(string s, Random r)
        {
            StringBuilder sb = new StringBuilder(s);
            for (int i = 0; i < s.Length; i++)
            {
                if (r.NextDouble() < 0.2) {
                    if (r.NextDouble() < 0.5)
                    {
                        char l = s[i];
                        sb[i] = Add(l, 1);
                    }
                    else
                    {
                        char l = s[i];
                        sb[i] = Add(l, -1);
                    }
                }
            }
            return sb.ToString();
        }

        static int distance(char c1, char c2)
        {
            int p1 = (int)c1 - 65;
            int p2 = (int)c2 - 65;
            int diff1 = Math.Abs(p1 - p2);
            int diff2 = 26;
            if (p2 > p1) { diff2 = Math.Abs(p1 - (p2 - 26)); }
            if (p1 > p2) { diff2 = Math.Abs(p2 - (p1 - 26)); }
            return diff1<=diff2 ? diff1: diff2;
        }
        static int HD(string s1, string s2)
        {
            int tot = 0;
            for(int i = 0; i < s1.Length; i++)
            {
                tot += distance(s1[i], s2[i]);
            }
            return tot;
        }
        static string Select(List<string> strings, string ts)
        {
            List<float> fitnesses = new List<float>();
            foreach (string s in strings)
            {
                fitnesses.Add(HD(s, ts));
            }
            return strings[fitnesses.IndexOf(fitnesses.Min())];
        }
        static List<string> MutateAll(string s, int n, Random r)
        {
            List<string> ss = new List<string>();
            for(int i = 0;i<n;i++)
            {
                ss.Add(Mutate(s, r));
            }
            return ss;
        }
        
        static void Main(string[] args)
        {
            Random r = new Random();
            n_counter = new Dictionary<char, int>();
            string targetString = "ASKE";
            string startingString = RandomString(targetString.Length, r);
            string guess = startingString;
            Stopwatch st = new Stopwatch();
            float tottime = 0f;
            int iter_counter;
            for (int oi = 0; oi < 100; oi++)
            {
                st.Start();
                iter_counter = 0;
                do
                {
                    guess = RandomString(targetString.Length, r);//Mutate(guess, r);
                                                                 //Debug.WriteLine(guess);
                    iter_counter += 1;
                } while (guess != targetString);
                st.Stop();
                foreach (KeyValuePair<char, int> kvp in n_counter)
                {
                    Debug.WriteLine(kvp.Key + " = " + kvp.Value);
                }
                Console.WriteLine("this took " + st.Elapsed.TotalSeconds + " " + iter_counter + " string guesses");
                tottime += iter_counter-1;
            }
            Debug.WriteLine("average iters = " + tottime / 100);
            string alpha;
            tottime = 0;
            for (int jk = 0; jk < 100; jk++)
            {
                iter_counter = 0;
                List<string> pop = new List<string>();
                for (int i = 0; i < 10; i++)
                {
                    pop.Add(RandomString(targetString.Length, r));
                }
                do
                {
                    alpha = Select(pop, targetString);
                    pop = MutateAll(alpha, 10, r);
                    iter_counter+=9;
                } while (alpha != targetString);
                Debug.WriteLine("iterations = " + iter_counter);
                tottime += iter_counter;
            }
            Debug.WriteLine("aberage = " +tottime/100);
        }
    }
}