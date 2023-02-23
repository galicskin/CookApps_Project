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
			type = (Type)Random.Range((int)Type.Red, (int)Type.Purple); // enum�� int32������ �ڽ� ��ڽ��� ���Ͼ�� ����
		}
	}
}