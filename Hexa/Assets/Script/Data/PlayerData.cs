using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
	[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData")]
	public class PlayerData: RunTimeData
	{
		public int TopsCount { get; private set; }
		public int MoveCount { get; private set; }
		public int TotalScore { get; private set; }

		public void InitGame(int maxMove)
		{
			TopsCount = 0;
			TotalScore = 0;
			MoveCount = maxMove;
		}
	}
}