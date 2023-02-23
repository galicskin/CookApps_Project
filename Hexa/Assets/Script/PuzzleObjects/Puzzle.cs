using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
	public class Puzzle: MonoBehaviour
	{
		public enum Type {
			Red,
			Blue,
			Green,
			Orange,
			Yellow,
			Purple,
			Top 
		}
		public Puzzle(Hexa hexa)
		{
			this.hexa = hexa;
			SetPuzzle();
		}
		public Hexa hexa { get; private set; }
		public Type type { get; private set; }
        void Start()
        {
			
		}
        public void SetPuzzle(Type initType)
		{
			type = initType;
		}
		public void SetPuzzle()
		{
			type = (Type)Random.Range((int)Type.Red, (int)Type.Purple); // enumÀº int32¿¡¼­´Â ¹Ú½Ì ¾ð¹Ú½ÌÀÌ ¾ÈÀÏ¾î³ª¼­ ±¦Ãá
		}
	}
}