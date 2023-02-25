using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
	public class Puzzle 
	{
		public static float Interval { get; private set; }
		public enum Type {
			Red = 0,
			Blue,
			Green,
			Orange,
			Yellow,
			Purple,
			Top,
			Boundary,
			Empty
		}

		public enum State {
			None,
			Check,
			Erase,
			Move,
		}
		
		public Puzzle(Hexa hexa)
		{
			this.hexa = hexa;
			SetPuzzle();
		}
		public GameObject PuzzleObj = null;
		public Hexa hexa { get; private set; }
		public Type type { get; private set; }
		public State CurState { get; private set; } = State.None;
        public static void SetInterval(float TileSideLength)
		{
			Interval = TileSideLength;
		}
		Hexa moveHexaDirection=new Hexa(0,0);
		//Puzzle destPuzzle;
		float MoveVelocity = 10.0f;

		public void None()
		{
			CurState = State.None;
		}
		public void Check()
		{
			if (CurState != State.Erase)
				CurState = State.Check;
		}

		public void Erase()
		{
			CurState = State.Erase;
		}

        public void SetPuzzle(Type initType)
		{
			type = initType;
		}
		public void SetPuzzle()
		{
			type = (Type)Random.Range((int)Type.Red, (int)Type.Purple+1); // enumÀº int32¿¡¼­´Â ¹Ú½Ì ¾ð¹Ú½ÌÀÌ ¾ÈÀÏ¾î³ª¼­ ±¦Ãá
		}

		public void SetDirection(Puzzle destPuzzle, Hexa direction)
		{
			Swap(destPuzzle);
			destPuzzle.moveHexaDirection = direction;
			destPuzzle.CurState = State.Move;
		}
		public void Stop()
		{
			moveHexaDirection = new Hexa(0, 0);
			CurState = State.Check;
		}
		public bool DoDownMove()
		{
			if (CurState != State.Move)
				return false;

			Vector2 vector2Pos= new Vector2(PuzzleObj.transform.position.x, PuzzleObj.transform.position.y);
			if ((vector2Pos - hexa.Position*Interval).sqrMagnitude < 0.01f)
			{
				PuzzleObj.transform.position = hexa.Position*Interval;
				Stop();
			}
			else
			{ 
				Vector3 moveDirection = new Vector3(moveHexaDirection.Position.x, moveHexaDirection.Position.y);
				PuzzleObj.transform.Translate(moveDirection * Time.deltaTime* MoveVelocity);
			}

			return true;
		}

		void Swap(Puzzle DestPuzzle)
		{
			GameObject tempObj = PuzzleObj;
			Type tempType = type;

			PuzzleObj = DestPuzzle.PuzzleObj;
			type = DestPuzzle.type;

			DestPuzzle.PuzzleObj = tempObj;
			DestPuzzle.type = tempType;
			
		}
	}
}