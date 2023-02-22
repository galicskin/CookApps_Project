using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// https://www.redblobgames.com/grids/hexagons/ Âü°í
namespace GHJ_Lib
{
	public class HexaCoordinateSystem<T>
	{
		List<List<T>> hexaCollection_q = new List<List<T>>();
		List<T> collections;
		public List<T> Collections
		{
			get 
			{
				if (collections == null)
				{
					collections = new List<T>();
					for (int i = 0; i < hexaCollection_q.Count; ++i)
					{
						collections.AddRange(hexaCollection_q[i]);
					}
				}

				return collections;
			}
		}

		int length;
		public int Length { get { return length + 1; } }

		public HexaCoordinateSystem(int SideLength)
		{
			length = SideLength-1;
			int ListLength = length * 2 + 1;
			int direction = 1;
			int VerticalLength = length + 1;
			for (int i = 0; i < ListLength; ++i)
			{
				List<T> hexaVericalElements = new List<T>();
				hexaVericalElements.Capacity = VerticalLength;
				if (i == length)
					direction = -1;
				VerticalLength += direction;
				hexaCollection_q.Add(hexaVericalElements);
			}
		}

		public bool GetObjectInHexaTile(int q, int r,int s,out T Obj)
		{
			if (q + r + s != 0)
			{
				Debug.LogError("this Position is outofHexaRange");
				Obj = hexaCollection_q[0][0];
				return false;
			}

			Obj = hexaCollection_q[length + q ][q < 0 ? length - s  : length + r ];
			return true;
		}


	}
}