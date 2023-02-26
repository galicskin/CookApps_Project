using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
    public struct Hexa 
    {
        public Hexa(int q, int r)
        {
            this.q = q;
            this.r = r;
            s = -q-r;
        }
        public static Hexa operator +(Hexa a, Hexa b)
        {
            return new Hexa(a.q + b.q, a.r + b.r);
        }
        public static Hexa operator -(Hexa a)
        {
            return new Hexa(-a.q, -a.r );
        }
        public static Hexa operator -(Hexa a, Hexa b)
        {
            return new Hexa(a.q - b.q, a.r - b.r);
        }
        public static Hexa operator *(Hexa a ,int b)
        {
            return new Hexa(a.q * b, a.r * b);
        }
        public static Hexa operator *(int a, Hexa b)
        {
            return new Hexa(a * b.q, a * b.r);
        }
        public static bool operator ==(Hexa a, Hexa b)
        {
            return a.q == b.q && a.r == b.r;
        }

        public static bool operator !=(Hexa a, Hexa b)
        {
            return a.q != b.q || a.r != b.r || a.q != b.q;
        }

        public static int Distance(Hexa a ,Hexa b)
        {
            Hexa hexa = a - b;
            return GetCycle(hexa);
        }

        public static int GetCycle(Hexa hexa)
        {

            int q = hexa.q < 0 ? -hexa.q : hexa.q;
            int r = hexa.r < 0 ? -hexa.r : hexa.r;
            int s = hexa.s < 0 ? -hexa.s : hexa.s;

            return Mathf.Max(q, r, s);
        }

        public static Hexa[] GetAround(Hexa hexa)
        {
            Hexa[] Around = new Hexa[6];
            Around[0] = hexa + Up;
            Around[1] = hexa + RightUp;
            Around[2] = hexa + RightDown;
            Around[3] = hexa + Down;
            Around[4] = hexa + LeftDown;
            Around[5] = hexa + LeftUp;
            return Around;
        }
        public static Hexa Up
        {
            get 
            {
                return new Hexa(0, -1);
            }
        }

        public static Hexa RightUp
        {
            get
            {
                return new Hexa(1, -1);
            }
        }

        public static Hexa RightDown
        {
            get
            {
                return new Hexa(1, 0);
            }
        }

        public static Hexa Down
        {
            get
            {
                return new Hexa(0, 1);
            }
        }

        public static Hexa LeftDown
        {
            get
            {
                return new Hexa(-1, 1);
            }
        }

        public static Hexa LeftUp
        {
            get
            {
                return new Hexa(-1, 0);
            }
        }



        public int q { get; private set; }
        public int r { get; private set; }
        public int s { get; private set; }

        public Vector2 Position
        {
            get 
            {
                return new Vector2(q * 1.5f, 1.732f * (s - r)*0.5f);
            }
        }

        public static List<List<Hexa>> CreateHexaCollection(int sideLength)
        {
            List<List<Hexa>> hexalist = null;
            if (sideLength > 0)
            {

                List<Hexa> center = new List<Hexa>();
                center.Add(new Hexa(0, 0));
                hexalist = new List<List<Hexa>>();
                hexalist.Add(center);

                for (int cycle = 1; cycle < sideLength; ++cycle)
                {
                    List<Hexa> cycleList = new List<Hexa>();
                    int q = 0;
                    int r = -cycle;

                    for (int element = 0; element < cycle * 6; ++element)
                    {
                        cycleList.Add(new Hexa(q, r));
                        if (element < cycle)
                        {
                            ++q;
                        }
                        else if (element < cycle * 2)
                        {
                            ++r;
                        }
                        else if (element < cycle * 3)
                        {
                            --q;
                            ++r;
                        }
                        else if (element < cycle * 4)
                        {
                            --q;
                        }
                        else if (element < cycle * 5)
                        {
                            --r;
                        }
                        else if (element < cycle * 6)
                        {
                            ++q;
                            --r;
                        }
                    }
                    hexalist.Add(cycleList);
                }

            }

            return hexalist;
        }

        public static Vector2 ToVector2(int q, int r, int s)
        {
            return new Vector2(q * 1.5f, 1.732f * (s - r)*0.5f);
        }


    }

}

