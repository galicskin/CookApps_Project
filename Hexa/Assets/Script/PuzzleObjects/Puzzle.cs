using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
	public class Puzzle
	{
		public static float Interval { get; private set; }
		public enum Type {
			Red,
			Blue,
			Green,
			Orange,
			Yellow,
			Purple,
			Top,
			Boundary,
			Empty
		}

		public enum CheckState {
			None,
			Check,
			Erase
		}
		
		public Puzzle(Hexa hexa)
		{
			this.hexa = hexa;
			SetPuzzle();
		}
		public GameObject PuzzleObj;
		public Hexa hexa { get; private set; }
		public Type type { get; private set; }
		public CheckState checkState { get; private set; } = CheckState.None;
		Vector3 moveDirection;
		void Start()
        {
			
		}
		public static void SetInterval(float TileSideLength)
		{
			Interval = TileSideLength;
		}
		public void None()
		{
			checkState = CheckState.None;
		}
		public void Check()
		{
			if (checkState != CheckState.Erase)
				checkState = CheckState.Check;
		}

		public void Erase()
		{
			checkState = CheckState.Erase;
		}

        public void SetPuzzle(Type initType)
		{
			type = initType;
		}
		public void SetPuzzle()
		{
			type = (Type)Random.Range((int)Type.Red, (int)Type.Purple); // enumÀº int32¿¡¼­´Â ¹Ú½Ì ¾ð¹Ú½ÌÀÌ ¾ÈÀÏ¾î³ª¼­ ±¦Ãá
		}

		public void DownMove(Hexa DirectionHexa)
		{
			moveDirection = new Vector3(DirectionHexa.Position.x,DirectionHexa.Position.y);
		}
	}
}