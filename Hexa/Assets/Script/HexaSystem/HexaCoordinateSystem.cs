using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// https://www.redblobgames.com/grids/hexagons/ 참고할것.
namespace GHJ_Lib
{
	public class HexaCoordinateSystem
	{
		public List<List<Hexa>> hexaCollectionCycle { get; private set; } = new List<List<Hexa>>();
		public int Length { get; private set; }
		public HexaCoordinateSystem(int sideLength)
		{
			hexaCollectionCycle = Hexa.CreateHexaCollection(sideLength);

			Length = 1 + (int)((sideLength - 2) * (sideLength - 1) * 0.5);
		}

		public int GetIndex(Hexa hexa)
		{
			int cycle;
			int element;
			GetCycleAndElement(hexa.q, hexa.r, hexa.s, out cycle, out element);
			return cycle==0 ? 0 : 1 + (cycle*(cycle-1))*3 + element ;
		}

		public Hexa? GetHexa(int q, int r, int s)
		{
			int cycle;
			int element ;
			GetCycleAndElement(q, r, s,out cycle, out element);
			return hexaCollectionCycle[cycle][element];
		}

		void GetCycleAndElement(int q, int r, int s,out int cycle, out int element)
		{
			if (q + r + s != 0)
			{
				cycle = 0;
				element = 0;
				Debug.LogError("q + r + s != 0");
				return;
			}

			cycle = maxAbs(q, r, s);
			element = 0;
			if (q * r <= 0 && r != 0)
			{
				element = -s + cycle;
			}
			else if (r * s < 0 && s != 0)
			{
				element = -q + 3 * cycle;
			}
			else if (s * q < 0 && q != 0)
			{
				element = -r + 5 * cycle;
			}
			else
			{
				Debug.LogError("this hexaIndex not exist");
			}
			
		}


		int maxAbs(int q, int r, int s)
		{
			q = q < 0 ? -q : q;
			r = r < 0 ? -r : r;
			s = s < 0 ? -s : s;

			return Mathf.Max(q, r, s);

		}
	}
}