using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
	public class Datamanager : MonoBehaviour
	{
		static Datamanager instance; 
		public static Datamanager Instance
		{
			get 
			{
				if (instance == null)
				{
					GameObject DataManagerObj = new GameObject("_Datamanager");
					instance = DataManagerObj.AddComponent<Datamanager>();
				}
				return instance;
			}
		}

		PuzzleData puzzleData;
		public PuzzleData PuzzleData
		{
			get 
			{
				if (puzzleData == null)
				{
					puzzleData = Resources.Load<PuzzleData>("ScriptableObject/PuzzleData");
					
				}
				return puzzleData;
			}
		}

		PlayerData playerData;
		public PlayerData PlayerData
		{
			get
			{
				if (puzzleData == null)
				{
					playerData = Resources.Load<PlayerData>("ScriptableObject/PlayerData");
					//��¥ �����̶�� ������ ���� ���̵� ���� ������ ����ȭ�� �ؾ���.
				}
				return playerData;
			}
		}

	}
}