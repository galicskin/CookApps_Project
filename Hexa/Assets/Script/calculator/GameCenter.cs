using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
	public class GameCenter : MonoBehaviour
	{
		PuzzleGenerator puzzleGenerator;
		HexaCoordinateSystem<Puzzle> hexaCoordinateSystem;

        enum PuzzleState { 
        Move,
        Stop,
        Swap,
        }

        PuzzleState puzzleState = PuzzleState.Stop;
        void Start()
        {
            initSettingAllRandom(4);

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

        void initSettingAllRandom(int SideLength)
        {
            hexaCoordinateSystem = new HexaCoordinateSystem<Puzzle>(SideLength); // 각스테이지마다 다르게 설정할수 있도록해야함
            List<Puzzle> puzzles = hexaCoordinateSystem.Collections;

            for (int i = 0; i < puzzles.Count ; ++i)
            {
                puzzles[i] = new Puzzle();
            }
        }

        void SetPuzzleType(int q,int r, int s, Puzzle.Type type)
        {
            Puzzle SettingPuzzle;
            if (hexaCoordinateSystem.GetObjectInHexaTile(q, r, s, out SettingPuzzle))
            {
                SettingPuzzle.SetPuzzle(type);
            }

        }

        void CheckAllPuzzle()
        {
            
        }

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

    }
}