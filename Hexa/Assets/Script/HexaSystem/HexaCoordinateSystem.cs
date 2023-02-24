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

		public Hexa[] GetHexaVectors()
		{
			Hexa[] hexaArray = new Hexa[3];

			hexaArray[0] = new Hexa(0,- 1);
			hexaArray[1] = new Hexa(1,  -1);
			hexaArray[2] = new Hexa(1, 0);

			return hexaArray;
		}

		public List<T> SetHexaCollect<T>(int sideLength) where T : Puzzle
		{
			List<T> T_List = new List<T>();
			T ds = (T)new Puzzle(hexaCollectionCycle[0][0]);
			T_List.Add((T)new Puzzle(hexaCollectionCycle[0][0]));


			for (int cycle = 1; cycle < sideLength; ++cycle)
			{
				for (int element = 0; element < cycle * 6; ++element)
				{
					T_List.Add((T)new Puzzle(hexaCollectionCycle[cycle][element]));
				}
			}

			return T_List;
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
			if (q >= 0 && r < 0)
			{
				element = -s + cycle;
			}
			else if (r  >= 0 && s < 0)
			{
				element = -q + 3 * cycle;
			}
			else if (s >= 0 && q < 0)
			{
				element = -r + 5 * cycle;
			}
			else if (cycle == 0)
			{
				cycle = 0;
				element = 0;
			}
			else
			{

				Debug.LogError($"this hexaIndex not exist, q : {q}, r : {r}, s: {s} cycle : {cycle} , element {element}");
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