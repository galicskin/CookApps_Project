using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// https://www.redblobgames.com/grids/hexagons/ Âü°í
namespace GHJ_Lib
{
	public class HexaCoordinateSystem
	{
		List<List<Object>> hexaCollection_q = new List<List<Object>>();
		int length;
		public int Length { get { return length + 1; } }

		public HexaCoordinateSystem(int SideLength)
		{
			length = SideLength-1;
			int ListLength = length * 2 + 1;
			int direction = 1;
			int Listcapacity = length + 1;
			for (int i = 0; i < ListLength; ++i)
			{
				List<Object> hexaVericalElements = new List<Object>();
				hexaVericalElements.Capacity = Listcapacity;
				if (i == length)
					direction = -1;	
				Listcapacity += direction;
				hexaCollection_q.Add(hexaVericalElements);
			}
		}

		public Object GetObjectInHexaTile(int q, int r,int s)
		{
			if (q + r + s != 0)
				return null;
			return hexaCollection_q[length + q ][q < 0 ? length - s  : length + r ];
		}
	}
}