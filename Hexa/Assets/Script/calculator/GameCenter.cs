using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
	public class GameCenter : MonoBehaviour
	{
		PuzzleGenerator puzzleGenerator;
		HexaCoordinateSystem hexaCoordinateSystem;
        PlayerData playerData;
        PuzzleData puzzleData;
        List<Puzzle> puzzleList = new List<Puzzle>();
        enum PuzzleState { 
        Move,
        Stop,
        Swap,
        }

        PuzzleState puzzleState = PuzzleState.Stop;
        void Start()
        {
            playerData = Datamanager.Instance.PlayerData;
            puzzleData = Datamanager.Instance.PuzzleData;
            initSetting(4);

        }

        void Update()
        {
            switch (puzzleState)
            {
                case PuzzleState.Move:
                    { }
                    break;
                case PuzzleState.Stop:
                    { 
                    }
                    break;
                case PuzzleState.Swap:
                    { }
                    break;

            }
        }

        void initSetting(int sideLength)
        {
            SetPuzzleAllRandom(sideLength);
            GameObject Tile = puzzleData.TilePrefab;
            
            float interval = 15.0f;
            for (int i = 0; i < puzzleList.Count; ++i)
            {
                Setting(puzzleList[i], Tile,interval);
            }
        }

        void SetPuzzleAllRandom(int sideLength)
        {
            if (sideLength <= 0)
                return;

            hexaCoordinateSystem = new HexaCoordinateSystem(sideLength);
            puzzleList.Add(new Puzzle(hexaCoordinateSystem.hexaCollectionCycle[0][0]));
            for (int cycle = 1; cycle < sideLength; ++cycle)
            {
                for (int element = 0; element < cycle * 6; ++element)
                {
                    puzzleList.Add(new Puzzle(hexaCoordinateSystem.hexaCollectionCycle[cycle][element]));
                }
            }
        }
        void Setting(Puzzle puzzle ,GameObject Tile,float interval)
        {
            Vector2 position2D = puzzle.hexa.Position* Tile.transform.localScale.x*0.5f;
            Instantiate(Tile, new Vector3(position2D.x, position2D.y) + Tile.transform.position, Tile.transform.rotation);
            Instantiate(puzzleData.PuzzlePrefabs[(int)puzzle.type], new Vector3(position2D.x, position2D.y), Quaternion.identity);
        }

        void SetPuzzleType(List<Hexa> hexas , Puzzle.Type type) //이 함수를 통해 초기에 고정된타일을 설정한다.
        {
            for (int i = 0; i < hexas.Count; ++i)
            {
                puzzleList[hexaCoordinateSystem.GetIndex(hexas[i])].SetPuzzle(type);
            }
        }



        void CheckAllPuzzle()
        {
            
        }
        /*
        void CheckPuzzle(int q,int r,int s)
        {
            Puzzle puzzle;
            Puzzle.Type checkType;
            if (hexaCoordinateSystem.GetObjectInHexaTile(q, r, s, out puzzle))
            {
                checkType = puzzle.type;
            }

            int _q = 0;
            int _r = 0;
            int _s = 0;

            for (int i = 0; i < 6; ++i)
            {
                switch (i)
                {
                    case 0:
                        {
                            _q = 0;
                            _r = -1;
                            _s = 1;
                        }
                        break;
                    case 1:
                        {
                            _q = 1;
                            _r = -1;
                            _s = 0;
                        }
                        break;
                    case 2:
                        {
                            _q = 1;
                            _r = 0;
                            _s = -1;
                        }
                        break;
                    case 3:
                        {
                            _q = 0;
                            _r = 1;
                            _s = -1;
                        }
                        break;
                    case 4:
                        {
                            _q = -1;
                            _r = 1;
                            _s = 0;
                        }
                        break;
                    case 5:
                        {
                            _q = -1;
                            _r = 0;
                            _s = 1;
                        }
                        break;
                }


                if (hexaCoordinateSystem.GetObjectInHexaTile(_q, _r, _s, out puzzle))
                {
                    
                }
            }

        }
        */
    }
}