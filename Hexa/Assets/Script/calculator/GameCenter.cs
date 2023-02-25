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

        Puzzle headPuzzle;
        Puzzle spawnPuzzle;


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
                        DropPuzzles();
                        SpawnPuzzle();
                    }
                    break;
                case GameState.Stop:
                    {

                    }
                    break;
                case GameState.Swap:
                    {
                        
                    }
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
                            puzzleState = GameState.Move;
                        }

                    }
                    break;

            }
        }
        private void FixedUpdate()
        {
            switch (puzzleState)
            { 
                case GameState.Move:
                    { 
                        bool isMove = false;
                            for (int i = 0; i < puzzleList.Count; ++i)
                            {
                                bool isPuzzleMove = puzzleList[i].DoDownMove();
                                if (!isMove && isPuzzleMove)
                                {
                                    isMove = true;
                                }
                            }
                            //if (!isMove)
                        //puzzleState = GameState.Stop;
                    }
                    break;
            }
        }
        void initSetting()
        {
            SetPuzzles(hexaSize);
            for (int i = 0; i < puzzleList.Count; ++i)
            {
                puzzleGenerator.Setting(puzzleList[i], TouchPuzzle , OnStartDrag,OnEndDrag);
            }
            boundaryPuzzle = new Puzzle(new Hexa(0, 0));
            boundaryPuzzle.SetPuzzle(Puzzle.Type.Boundary);
            SettingBottomHexas();
            headPuzzle = GetPuzzle(Hexa.Up * (hexaSize - 1));
            spawnPuzzle = new Puzzle(Hexa.Up * hexaSize);
        }

        void SettingBottomHexas()
        {
            int r = (hexaSize - 1);
            for (int q = -r; q < hexaSize * 2 - 1 -r; ++q)
            {
                BottomHexas.Add(new Hexa(q, q > 0 ? r - q : r));
            }
        }

        void SetPuzzles(int sideLength)
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
            for (int i = 0; i < puzzleList.Count; ++i)
            {
                if (puzzleList[i].CurState == Puzzle.State.Erase)
                {
                    puzzleGenerator.PutPool(puzzleList[i]);
                    puzzleList[i].SetPuzzle(Puzzle.Type.Empty);
                    puzzleList[i].PuzzleObj.SetActive(false);  // 만약 애니매이션이 존재한다면 애니매이션을 실행시키는 함수를 발동.
                    puzzleList[i].PuzzleObj = null;

                }
                puzzleList[i].None();
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
                        destHexa = destHexa + Hexa.Up;
                        destCursor = GetPuzzle(destHexa);
                    }
                    startHexa = startHexa + Hexa.Up;
                    DropPuzzle = GetPuzzle(startHexa);
                }
            }
        }


        void DropPuzzles()
        {
            for (int i = 0; i < puzzleList.Count; ++i)
            {
                Puzzle DownPuzzle = GetPuzzle(puzzleList[i].hexa + Hexa.Down);
                if (puzzleList[i].type != Puzzle.Type.Empty)
                {
                    continue;
                }
                else if (DownPuzzle.type != Puzzle.Type.Empty && DownPuzzle.CurState != Puzzle.State.Move)
                {
                    Puzzle DropPuzzle= GetPuzzle(puzzleList[i].hexa + Hexa.Up);

                    while (DropPuzzle.type != Puzzle.Type.Boundary)
                    {
                        if (DropPuzzle.PuzzleObj != null && DropPuzzle.CurState != Puzzle.State.Move)
                        {
                            DropPuzzle.SetDirection(puzzleList[i], Hexa.Down);
                            break;
                        }
                        else
                            DropPuzzle = GetPuzzle(DropPuzzle.hexa + Hexa.Up);
                    }
                    
                }
            }

            for (int i = 0; i < puzzleList.Count; ++i)
            {
                Puzzle DownPuzzle = GetPuzzle(puzzleList[i].hexa + Hexa.Down);
                if (puzzleList[i].type != Puzzle.Type.Empty)
                {
                    continue;
                }
                else if (DownPuzzle.type != Puzzle.Type.Empty && DownPuzzle.CurState != Puzzle.State.Move)
                {
                    Puzzle DropPuzzle = GetPuzzle(puzzleList[i].hexa + Hexa.RightUp);
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
        }


        void SpawnPuzzle()
        {
            if (headPuzzle.type == Puzzle.Type.Empty)
            {
                puzzleGenerator.GeneratePuzzleOnHead(spawnPuzzle);
                spawnPuzzle.SetDirection(headPuzzle, Hexa.Down);
            }
        }

        void DisplayHint()
        {
            
        }

        void TouchPuzzle()
        {
            //조건에 의해 puzzleStat = GameState.Swap()   
        }

        void TouchPuzzle(Puzzle puzzle)
        {

        }

        void OnStartDrag()
        {
            
        }
        void OnEndDrag()
        {
            
        }


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