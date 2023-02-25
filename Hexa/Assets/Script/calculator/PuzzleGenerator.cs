using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
	public class PuzzleGenerator :MonoBehaviour
	{
        //Queue<GameObject> PuzzlePool = new Queue<GameObject>();
        List<Queue<GameObject>> PuzzlePool = new List<Queue<GameObject>>();



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

            for (int i = 0; i < puzzleData.PuzzlePrefabs.Count; ++i)
            {
                PuzzlePool.Add(new Queue<GameObject>());
            }
        }

        public void Setting(Puzzle puzzle , HexaTile.CallbackClickHexaTile OnClick , HexaTile.CallbackStartDragPuzzle OnStartDrag, HexaTile.CallbackEndDragPuzzle OnEndDrag)
        {
            Puzzle.SetInterval(Tile.transform.localScale.x * 0.5f);
            Vector2 position2D = puzzle.hexa.Position * Puzzle.Interval;

            GameObject TileObj = Instantiate(Tile, new Vector3(position2D.x, position2D.y) + Tile.transform.position, Tile.transform.rotation);
            HexaTile tile = TileObj.AddComponent<HexaTile>();
            tile.SetPuzzle(puzzle);
            tile.OnClickHexaTile = OnClick;

            GameObject Obj= Instantiate(puzzleData.PuzzlePrefabs[(int)puzzle.type], new Vector3(position2D.x, position2D.y), Quaternion.identity);
            puzzle.PuzzleObj = Obj;
            
        }

        public void PutPool(Puzzle puzzle)
        {
            PuzzlePool[(int)puzzle.type].Enqueue(puzzle.PuzzleObj);
        }

        public GameObject GeneratePuzzleOnHead(Puzzle spawnPuzzle)
        {
            int type = Random.Range((int)Puzzle.Type.Red, (int)Puzzle.Type.Purple + 1);

            if (!PuzzlePool[type].TryDequeue(out spawnPuzzle.PuzzleObj))
            {
                spawnPuzzle.PuzzleObj = Instantiate(puzzleData.PuzzlePrefabs[type], Vector3.zero, Quaternion.identity);
            }
            spawnPuzzle.SetPuzzle((Puzzle.Type)type);
            spawnPuzzle.PuzzleObj.SetActive(true);
            spawnPuzzle.PuzzleObj.transform.position = spawnPuzzle.hexa.Position*Puzzle.Interval;
            return null;
        }
    }
}