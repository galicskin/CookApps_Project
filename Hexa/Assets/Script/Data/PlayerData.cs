using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
	[CreateAssetMenu(fileName = "PlayerData", menuName = "ScriptableObjects/PlayerData")]
	public class PlayerData: RunTimeData
	{
		public int RainbowCount { get; private set; }
		public int MoveCount { get; private set; }
		public int TotalScore { get; private set; }

		public void InitGame()
		{
			RainbowCount = 0;
			TotalScore = 0;
			MoveCount = 0;
		}

		public void MovePuzzle()
		{
			++MoveCount;
		}

		public void UpdateRainbowScore(int count)
		{
			RainbowCount += count;
		}
	}
}