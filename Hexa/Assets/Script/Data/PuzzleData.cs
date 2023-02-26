using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
	[CreateAssetMenu(fileName = "PuzzleData", menuName = "ScriptableObjects/PuzzleData")]
	public class PuzzleData: StaticData
	{
		public List<GameObject> PuzzlePrefabs = new List<GameObject>();
		public GameObject TilePrefab;
	}
}