using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Goblins
{


    public class Goblin
    {
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
            return A.GoblinCheck(B) && B.GoblinCheck(A);
        }
    }


    public class Test
    {
        public FindTrueTeller _method;

        public Random seed;
        public List<Goblin> goblins;

        public Test(int numt, int numd)
        {
            seed = new Random((int)DateTime.Now.Ticks);
            GenerateGoblins(numt,numd);
        }

        void GenerateGoblins(int numt, int numd)
        {
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
            Test test =new Test(9999, 1);
            test._method = Method;
            Console.Out.WriteLine(test.FindGoblin().TruthTeller);
            string s =Console.In.ReadLine();
        }

        public static Goblin Method(List<Goblin> goblins)
        {
            Console.Out.WriteLine("Method");
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
                }
            }
            if(extra != null && IsEven(_new.Count))
                return extra;
            return Method(_new);
        }

        public static bool IsEven(int value)
        {
            return value % 2 == 0;
        }
    }
}
