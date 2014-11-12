using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Goblins
{


    public class Goblin
    {
        public static int Comparations = 0;
        public Random seed;
        public bool TruthTeller;

        public Goblin(bool truthteller,Random seed)
        {
            this.seed = seed;
            TruthTeller = truthteller;
        }

        //worse scenario deceiver always say that b is true
        public bool GoblinCheck(Goblin B)
        {
            return TruthTeller ? B.TruthTeller : true; // seed.Next(0, 2) == 1;
        }

        //true is the True-True case
        //false else
        public static bool Compare(Goblin A, Goblin B)
        {
            Goblin.Comparations++;
            return A.GoblinCheck(B) && B.GoblinCheck(A);
        }
        public override string ToString()
        {
            return TruthTeller ? "Truth Teller" : "Deceiver";
        }
    }


    public class Test
    {
        public static int SeedInt = -1;
        public FindTrueTeller _method;
        public int NumT;
        public int NumD;
        public Random seed;
        public List<Goblin> goblins;

        public Test(int n)
        {
            int s = SeedInt != -1 ? SeedInt : (int)DateTime.Now.Ticks;
            seed = new Random(s);
            int nd = seed.Next(0, (n/2));
            int nt = n - nd;
            GenerateGoblins(nt, nd);
        }

        public Test(int numt, int numd)
        {
            int s = SeedInt != -1 ? SeedInt : (int)DateTime.Now.Ticks;
            seed = new Random(s);
            GenerateGoblins(numt,numd);
        }

        void GenerateGoblins(int numt, int numd)
        {
            NumT = numt;
            NumD = numd;

            Goblin.Comparations = 0;
            goblins = new List<Goblin>();
            int n = numt + numd;
            while (goblins.Count < n)
            {
                Goblin g = new Goblin(seed.Next(0, 2) == 1, seed);
                if (g.TruthTeller && numt > 0)
                {
                    g.TruthTeller = true;
                    goblins.Add(g);
                    numt--;
                }
                else if (numd > 0)
                {
                    goblins.Add(g);
                    g.TruthTeller = false;
                    numd--;
                }
            }
        }

        public delegate Goblin FindTrueTeller(List<Goblin> goblins);

        public Goblin FindGoblin()
        {
            if (_method != null)
            {
                return _method(goblins);
            }
            return null;
        }
    }

    class Program
    {
        static int Deep = 0;

        private class GoblinPair
        {
            public Goblin A;
            public Goblin B;
            public GoblinPair(Goblin a, Goblin b)
            {
                A = a;
                B = b;
            }
            public bool Compare()
            {
                return Goblin.Compare(A, B);
            }
           
        }

        static void Main(string[] args)
        {
           
            for (int i = 0; i < 1000; i++)
            {
                Console.Out.WriteLine("Test #:" + i);
                Test test = new Test(100000);
                test._method = Method;
                Program.Deep = 0;
                Goblin goblin = test.FindGoblin();
                Console.Out.WriteLine("T / D: "+test.NumT +" / " + test.NumD); 
                Console.Out.WriteLine("Goblins Comparations: "+ Goblin.Comparations);
                Console.Out.WriteLine("Recursive Calls: "+ Program.Deep);
                Console.Out.WriteLine("Goblin: " + goblin);
                if (!goblin.TruthTeller)
                {
                    string d = Console.In.ReadLine();
                }
            }
             
            string s = Console.In.ReadLine();
        }

        public static Goblin Method(List<Goblin> goblins)
        {
            if (goblins.Count == 1)
                return goblins[0];
            int id = 0;
            int it = 0;
            Deep++;
            List<Goblin> _new = new List<Goblin>();
            List<GoblinPair> pairs = new List<GoblinPair>();
            
            Goblin extra = null;

            int i = 0;
            int n = goblins.Count;
            while (i < n)
            {
                if (i + 1 < n)
                {
                    pairs.Add(new GoblinPair(goblins[i], goblins[i + 1]));
                    i += 2;
                }
                else
                {
                    extra = goblins[i];
                    i++;
                }
            }

            for(i = 0; i < pairs.Count; i++)
            {
                if (pairs[i].Compare())
                {
                    _new.Add(pairs[i].A);
                    if (pairs[i].A.TruthTeller)
                        it++;
                    else
                        id++;
                }
            }

            if (extra != null && IsEven(_new.Count))
            {
                _new.Add(extra);
            }
            return Method(_new);
        }

        public static bool IsEven(int value)
        {
            return value % 2 == 0;
        }
    }
}
