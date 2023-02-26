using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
	public class Puzzle 
	{
		static float interval;
		public static float Interval { get { return ObjectSet.transform.localScale.x * interval; } }
		static GameObject ObjectSet;
		public enum Type {
			Red = 0,
			Blue,
			Green,
			Orange,
			Yellow,
			Purple,
			Top,
			TopSpin,
			Boundary,
			Empty,
			Obstacle
		}

		public enum State {
			None,
			Check,
			Erase,
			Move,
			Swap
		}
		public Puzzle(Hexa hexa)
		{
			this.hexa = hexa;
			SetPuzzle();
		}

		public GameObject PuzzleObj = null;
		public Hexa hexa { get; protected set; }
		public Type type { get; protected set; }
		public State CurState { get; protected set; } = State.None;
        public static void SetInterval(float TileSideLength,GameObject scaleObject)
		{
			interval = TileSideLength;
			ObjectSet = scaleObject;
		}
		Hexa moveHexaDirection=new Hexa(0,0);
		//Puzzle destPuzzle;
		protected float MoveVelocity = 50.0f;

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
			SwapObject(destPuzzle);
			destPuzzle.moveHexaDirection = direction;
			destPuzzle.CurState = State.Move;
		}

		public void Swap(Puzzle puzzle)
		{
			SwapObject(puzzle);
			moveHexaDirection =  hexa- puzzle.hexa;
			CurState = State.Swap;

			puzzle.moveHexaDirection = puzzle.hexa - hexa;
			puzzle.CurState = State.Swap;

		}

		public void Stop()
		{
			moveHexaDirection = new Hexa(0, 0);
			CurState = State.Check;
		}

		public void Do(State DoingState, State endState)
		{
			if (CurState != DoingState)
				return ;
			if (type == Type.Top)
			{
				int d = 11;
			}
			Vector2 vector2Pos = new Vector2(PuzzleObj.transform.position.x, PuzzleObj.transform.position.y);
			if ((vector2Pos - hexa.Position * Interval).sqrMagnitude < 0.01f)
			{
				PuzzleObj.transform.position = hexa.Position * Interval;
				moveHexaDirection = new Hexa(0, 0);
				CurState = endState;
			}
			else
			{
				Vector3 moveDirection = new Vector3(moveHexaDirection.Position.x, moveHexaDirection.Position.y);
				PuzzleObj.transform.Translate(moveDirection * Time.deltaTime * MoveVelocity);
			}

			return ;
		}

		public void Glow()
		{
			if (PuzzleObj != null)
				PuzzleObj.transform.GetChild(0).gameObject.SetActive(true);
		}

		public void CancelGlow()
		{
			if (PuzzleObj != null)
				PuzzleObj.transform.GetChild(0).gameObject.SetActive(false);
		}

		protected void SwapObject(Puzzle DestPuzzle)
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