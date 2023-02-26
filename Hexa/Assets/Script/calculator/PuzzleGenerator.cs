using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
	public class PuzzleGenerator :MonoBehaviour
	{
        //Queue<GameObject> PuzzlePool = new Queue<GameObject>();
        List<Queue<GameObject>> PuzzlePool = new List<Queue<GameObject>>();

        GameObject objectSet=null;

        GameObject ObjectSet
        {
            get
            {
                if (objectSet == null)
                {
                    objectSet = new GameObject("objectSet");
                }
                return objectSet;
            }
        }


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

        public void Setting(Puzzle puzzle , HexaTile.CallbackClickHexaTile OnClick , HexaTile.CallbackOverPuzzle OnOverPuzzle,HexaTile.CallbackUpMouseHexaTile OnUpMouse)
        {
            Puzzle.SetInterval(Tile.transform.localScale.x * 0.5f,ObjectSet);
            Vector2 position2D = puzzle.hexa.Position * Puzzle.Interval;

            GameObject TileObj = Instantiate(Tile, new Vector3(position2D.x, position2D.y) + Tile.transform.position, Tile.transform.rotation, ObjectSet.transform);
            HexaTile tile = TileObj.AddComponent<HexaTile>();
            tile.SetPuzzle(puzzle);
            tile.OnClickHexaTile = OnClick;
            tile.OnOverHexaTile = OnOverPuzzle;
            tile.OnUpMouse = OnUpMouse;

            GameObject model = puzzleData.PuzzlePrefabs[(int)puzzle.type];
            GameObject Obj= Instantiate(model, new Vector3(position2D.x, position2D.y) + model.transform.position, model.transform.rotation, ObjectSet.transform);
            puzzle.PuzzleObj = Obj;


        }

        public void PutPool(Puzzle puzzle)
        {
            if (PuzzlePool.Count <= (int)puzzle.type)
            {
                Debug.LogWarning($"count : {(int)puzzle.type} , {puzzle.hexa.q},{puzzle.hexa.r},{puzzle.hexa.s}");
            }
            else
            { 
                PuzzlePool[(int)puzzle.type].Enqueue(puzzle.PuzzleObj);
            }
        }

        public GameObject GeneratePuzzleOnHead(Puzzle spawnPuzzle)
        {
            int type = Random.Range((int)Puzzle.Type.Red, (int)Puzzle.Type.Purple + 1);

            if (!PuzzlePool[type].TryDequeue(out spawnPuzzle.PuzzleObj))
            {
                spawnPuzzle.PuzzleObj = Instantiate(puzzleData.PuzzlePrefabs[type], Vector3.zero, Quaternion.identity,ObjectSet.transform);
            }
            spawnPuzzle.SetPuzzle((Puzzle.Type)type);
            spawnPuzzle.PuzzleObj.SetActive(true);
            spawnPuzzle.PuzzleObj.transform.position = spawnPuzzle.hexa.Position*Puzzle.Interval;
            return null;
        }
    }
}