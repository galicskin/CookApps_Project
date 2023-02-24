using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GHJ_Lib
{
	public class GameCenter : MonoBehaviour
	{
		PuzzleGenerator puzzleGenerator;
        HexaLogicCalculator calculator;
        HexaCoordinateSystem hexaCoordinateSystem;


        PlayerData playerData;
        PuzzleData puzzleData;
        List<Puzzle> puzzleList = new List<Puzzle>();
        Puzzle boundaryPuzzle;
        // Debug
        List<Puzzle> checkList = new List<Puzzle>();


        enum GameState { 
        Move,
        Stop,
        Swap,
        CheckAll,
        }

        GameState puzzleState = GameState.CheckAll;
        void Start()
        {
            playerData = Datamanager.Instance.PlayerData;
            puzzleData = Datamanager.Instance.PuzzleData;
            GameObject GeneratorObj = new GameObject("PuzzleGenerator");
            puzzleGenerator = GeneratorObj.AddComponent<PuzzleGenerator>();
            puzzleGenerator.SetData(puzzleData);
            initSetting(4);

        }

        void Update()
        {
            switch (puzzleState)
            {
                case GameState.Move:
                    { }
                    break;
                case GameState.Stop:
                    {

                    }
                    break;
                case GameState.Swap:
                    { }
                    break;
                case GameState.CheckAll:
                    {
                        if (Input.GetKeyDown(KeyCode.A))
                        {
                            CheckAllPuzzle();
                        }
                        if (Input.GetKeyDown(KeyCode.B))
                        { 
                            ErasePuzzle();
                        }
                        if (Input.GetKeyDown(KeyCode.C))
                        {
                            for (int i = 0; i < puzzleList.Count; ++i)
                            {
                                int index = hexaCoordinateSystem.GetIndex(puzzleList[i].hexa);
                                Debug.Log(index);
                            }
                            
                        }

                    }
                    break;

            }
        }

        void initSetting(int sideLength)
        {
            SetPuzzleAllRandom(sideLength);
            for (int i = 0; i < puzzleList.Count; ++i)
            {
                puzzleGenerator.Setting(puzzleList[i]);
            }
            boundaryPuzzle = new Puzzle(new Hexa(0, 0));
            boundaryPuzzle.SetPuzzle(Puzzle.Type.Boundary);
        }

        void SetPuzzleAllRandom(int sideLength)
        {
            if (sideLength <= 0)
                return;

            hexaCoordinateSystem = new HexaCoordinateSystem(sideLength);
            puzzleList = hexaCoordinateSystem.SetHexaCollect<Puzzle>(sideLength);
        }
        void SetPuzzleType(List<Hexa> hexas , Puzzle.Type type) //이 함수를 통해 초기에 고정된타일을 설정한다.
        {
            for (int i = 0; i < hexas.Count; ++i)
            {
                GetPuzzle(hexas[i]).SetPuzzle(type);
            }
        }

        void CheckAllPuzzle()
        {
            for (int i = 0; i < puzzleList.Count; ++i)
            {
                if (puzzleList[i].checkState != Puzzle.CheckState.None)
                    continue;
                puzzleList[i].Check();
                Hexa CheckHexa = puzzleList[i].hexa;
                CheckLinear(CheckHexa);
                CheckBoomerang(CheckHexa);
            }
        }

        void CheckBoomerang(Hexa center)
        {
            Puzzle CetnerPuzzle = GetPuzzle(center);
            Puzzle[] BoomerangPuzzleList = new Puzzle[9];
            BoomerangPuzzleList[0] = GetPuzzle(center + new Hexa(-1, 0));
            BoomerangPuzzleList[1] = GetPuzzle(center + new Hexa(0, -1));
            BoomerangPuzzleList[2] = GetPuzzle(center + new Hexa(1, -1));
            BoomerangPuzzleList[3] = GetPuzzle(center + new Hexa(2, -1));
            BoomerangPuzzleList[4] = GetPuzzle(center + new Hexa(1, 0));
            BoomerangPuzzleList[5] = GetPuzzle(center + new Hexa(0, 1));
            BoomerangPuzzleList[6] = GetPuzzle(center + new Hexa(-1, 1));
            BoomerangPuzzleList[7] = GetPuzzle(center + new Hexa(-2, 1));
            BoomerangPuzzleList[8] = BoomerangPuzzleList[0];

            for (int i = 0; i < 4; ++i)
            {
                int index = i * 2;
                if (CetnerPuzzle.type == BoomerangPuzzleList[index].type
               && CetnerPuzzle.type == BoomerangPuzzleList[index+1].type
               && CetnerPuzzle.type == BoomerangPuzzleList[index+2].type)
                {
                    CetnerPuzzle.Erase();
                    BoomerangPuzzleList[index].Erase();
                    BoomerangPuzzleList[index+1].Erase();
                    BoomerangPuzzleList[index+2].Erase();
                }
            }

        }

        void CheckLinear(Hexa center)
        {
            Puzzle.Type centerType = GetPuzzle(center).type;
            Hexa[] hexaAry = hexaCoordinateSystem.GetHexaVectors();

            for (int i = 0; i < hexaAry.Length; ++i)
            {
                Puzzle frontPuzzle = GetPuzzle(center + hexaAry[i]);
                Puzzle BackPuzzle = GetPuzzle(center - hexaAry[i]);

                if (centerType == frontPuzzle.type)
                {
                    frontPuzzle.Check();
                    if (centerType == GetPuzzle(frontPuzzle.hexa + hexaAry[i]).type || centerType == BackPuzzle.type)
                    {
                        SelectLinear(hexaAry[i], center, centerType);
                    }
                }
                else if (centerType == BackPuzzle.type)
                {
                    BackPuzzle.Check();
                    if (centerType == GetPuzzle(BackPuzzle.hexa - hexaAry[i]).type)
                    { 
                        SelectLinear(hexaAry[i], center, centerType);
                    }
                }
            }
        }
        
        Puzzle GetPuzzle(Hexa hexa)
        {
            int index =hexaCoordinateSystem.GetIndex(hexa);
            if (index >= puzzleList.Count)
                return boundaryPuzzle;
            return puzzleList[index];
        }

        void SelectLinear(Hexa vector,Hexa Center, Puzzle.Type centerType)
        {

            GetPuzzle(Center).Erase();
            checkList.Add(GetPuzzle(Center));
            Hexa direction = vector;
            Puzzle Checkpuzzle = GetPuzzle(Center + direction);
            while (centerType == Checkpuzzle.type)
            {
                Checkpuzzle.Erase();
                checkList.Add(Checkpuzzle);
                direction = direction + vector;
                Checkpuzzle = GetPuzzle(Center + direction);
            }
            direction = vector;
            Checkpuzzle = GetPuzzle(Center - direction);
            while (centerType == Checkpuzzle.type)
            {
                Checkpuzzle.Erase();
                checkList.Add(Checkpuzzle);
                direction = direction + vector;
                Checkpuzzle = GetPuzzle(Center - direction);
            }
            if (checkList.Count < 3)
            { }
        }

        void ErasePuzzle()
        {
            bool isExistEmpty = false;
            for (int i = 0; i < puzzleList.Count; ++i)
            {
                if (puzzleList[i].checkState == Puzzle.CheckState.Erase)
                {
                    puzzleList[i].SetPuzzle(Puzzle.Type.Empty);
                    puzzleList[i].PuzzleObj.SetActive(false);  // 만약 애니매이션이 존재한다면 애니매이션을 실행시키는 함수를 발동.
                    isExistEmpty = true;
                }
                puzzleList[i].None();
            }

            if (isExistEmpty)
            {
                puzzleState = GameState.Move;
            }
            else
            {
                puzzleState = GameState.Stop;
            }
        }

        void OnDrawGizmos()
        {
            for (int i = 0; i < puzzleList.Count; ++i)
            {
                if (puzzleList[i].checkState == Puzzle.CheckState.Erase)
                    Gizmos.DrawSphere(puzzleList[i].PuzzleObj.transform.position, 1.0f);
            }
        }
    }
}