using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
	public class Puzzle
	{
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
		void Start()
        {
			
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
			type = (Type)Random.Range((int)Type.Red, (int)Type.Purple); // enum�� int32������ �ڽ� ��ڽ��� ���Ͼ�� ����
		}
	}
}