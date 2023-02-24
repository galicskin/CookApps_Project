using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
	public class PuzzleGenerator :MonoBehaviour
	{
        PuzzleData puzzleData;
        GameObject tile;
        GameObject Tile 
        {
            get 
            {
                if (tile == null)
                {
                    tile = puzzleData.TilePrefab;
                }
                return tile;
            }

        }

       
        public void SetData(PuzzleData puzzleData)
        {
            this.puzzleData = puzzleData;
        }

        public void Setting(Puzzle puzzle)
        {
            Puzzle.SetInterval(Tile.transform.localScale.x * 0.5f);
            Vector2 position2D = puzzle.hexa.Position * Puzzle.Interval;
            Instantiate(Tile, new Vector3(position2D.x, position2D.y) + Tile.transform.position, Tile.transform.rotation);
            GameObject Obj= Instantiate(puzzleData.PuzzlePrefabs[(int)puzzle.type], new Vector3(position2D.x, position2D.y), Quaternion.identity);
            puzzle.PuzzleObj = Obj;
        }
    }
}