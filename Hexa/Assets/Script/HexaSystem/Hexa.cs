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

        public static bool operator ==(Hexa a, Hexa b)
        {
            return a.q == b.q && a.r == b.r;
        }

        public static bool operator !=(Hexa a, Hexa b)
        {
            return a.q != b.q || a.r != b.r || a.q != b.q;
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

