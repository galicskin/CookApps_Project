using System;
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
        List<Hexa> BottomHexas = new List<Hexa>();
        Puzzle boundaryPuzzle;
        int hexaSize = 4;
        // Debug
        List<Hexa> DebugList = new List<Hexa>();
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
            initSetting();

        }

        void Update()
        {
            switch (puzzleState)
            {
                case GameState.Move:
                    {
                        
                    }
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
                            MoveSetting();
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
        private void FixedUpdate()
        {
            for (int i = 0; i < puzzleList.Count; ++i)
            {
                puzzleList[i].DownMove();
            }
        }
        void initSetting()
        {
            SetPuzzleAllRandom(hexaSize);

            for (int i = 0; i < puzzleList.Count; ++i)
            {
                puzzleGenerator.Setting(puzzleList[i]);
            }
            boundaryPuzzle = new Puzzle(new Hexa(0, 0));
            boundaryPuzzle.SetPuzzle(Puzzle.Type.Boundary);
            SettingBottomHexas();
        }

        void SettingBottomHexas()
        {
            int r = (hexaSize - 1);
            for (int q = -r; q < hexaSize * 2 - 1 -r; ++q)
            {
                BottomHexas.Add(new Hexa(q, q > 0 ? r - q : r));
            }
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
                if (puzzleList[i].CurState != Puzzle.State.None)
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
            Hexa[] hexaAry = HexaCoordinateSystem.GetHexaVectors();


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
                if (puzzleList[i].CurState == Puzzle.State.Erase)
                {
                    puzzleList[i].SetPuzzle(Puzzle.Type.Empty);
                    puzzleGenerator.PuzzlePool.Enqueue(puzzleList[i].PuzzleObj);
                    puzzleList[i].PuzzleObj.SetActive(false);  // 만약 애니매이션이 존재한다면 애니매이션을 실행시키는 함수를 발동.
                    puzzleList[i].PuzzleObj = null;
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

        // 기존에 있던 퍼즐들은 무조건 아래로, 새로생긴 퍼즐은 맨 위에서 퍼짐. (바로직선아래 오른쪽 왼쪽순서)
        void MoveSetting()
        {
            for (int i = 0; i < BottomHexas.Count; ++i)
            {
                Hexa destHexa = BottomHexas[i];
                Puzzle destCursor = GetPuzzle(destHexa);
                while (destCursor.type != Puzzle.Type.Boundary)
                {
                    if (destCursor.type == Puzzle.Type.Empty)
                    {
                        break;
                    }
                    destHexa = destHexa + Hexa.Up;
                    destCursor = GetPuzzle(destHexa);
                }

                Hexa startHexa = destHexa + Hexa.Up;
                Puzzle DropPuzzle = GetPuzzle(startHexa);

                while (DropPuzzle.type != Puzzle.Type.Boundary)
                {
                    if (DropPuzzle.type != Puzzle.Type.Empty)
                    {
                        DropPuzzle.SetDirection(destCursor, Hexa.Down);
                        BottomHexas[i] = destHexa;
                        destHexa = destHexa + Hexa.Up;
                        destCursor = GetPuzzle(destHexa);
                    }
                    startHexa = startHexa + Hexa.Up;
                    DropPuzzle = GetPuzzle(startHexa);
                }
            }
        }

        void SearchDest()
        {
            for (int i= 0; i < BottomHexas.Count; ++i)
            {
                Hexa headPuzzleHexa = BottomHexas[i] + Hexa.Down;
                
            }
        }

        void DecideDest()
        {
            
        }

            /*
            for (int i = 0; i < puzzleList.Count; ++i)
            {
                if (puzzleList[i].type == Puzzle.Type.Empty && puzzleList[i].CurState != Puzzle.State.WaitPuzzle )
                {
                    Puzzle DropPuzzle = GetPuzzle(puzzleList[i].hexa + Hexa.Up);
                    if (DropPuzzle.PuzzleObj != null&& DropPuzzle.CurState != Puzzle.State.Move)
                    {
                        DropPuzzle.SetDirection(puzzleList[i], Hexa.Down);
                        continue;
                    }
                    DropPuzzle = GetPuzzle(puzzleList[i].hexa + Hexa.RightUp);
                    if (DropPuzzle.PuzzleObj != null && DropPuzzle.CurState != Puzzle.State.Move)
                    {
                        DropPuzzle.SetDirection(puzzleList[i], Hexa.LeftDown);
                        continue;
                    }
                    DropPuzzle = GetPuzzle(puzzleList[i].hexa + Hexa.LeftUp);
                    if (DropPuzzle.PuzzleObj != null && DropPuzzle.CurState != Puzzle.State.Move)
                    {
                        DropPuzzle.SetDirection(puzzleList[i], Hexa.RightDown);
                        continue;
                    }
                }
                
            }
            */


        void OnDrawGizmos()
        {
            for (int i = 0; i < puzzleList.Count; ++i)
            {
                if (puzzleList[i].CurState == Puzzle.State.Erase)
                    Gizmos.DrawSphere(puzzleList[i].PuzzleObj.transform.position, 1.0f);
            }
        }
    }
}