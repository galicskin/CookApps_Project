using System;
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
        List<Hexa> BottomHexas = new List<Hexa>();
        List<Puzzle> TopList = new List<Puzzle>();
        int TopCount;

        Puzzle headPuzzle;
        Puzzle spawnPuzzle;

        Puzzle pickedPuzzle = null;
        Puzzle overedPuzzle = null;

        Puzzle boundaryPuzzle;
        int hexaSize = 4;
        bool isTouch = false;
        bool IsFailToSwap = false;

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
            playerData.InitGame();
            puzzleGenerator.SetData(puzzleData);
            initSetting();
            //ScreenSetting();

        }


        void Update()
        {
            switch (puzzleState)
            {
                case GameState.Move:
                    {
                        DropPuzzles();
                        SpawnPuzzle();
                        bool isMove = false;
                        for (int i = 0; i < puzzleList.Count; ++i)
                        {
                            if (!isMove && puzzleList[i].CurState ==Puzzle.State.Move)
                            {
                                isMove = true;
                                break;
                            }
                        }

                        if (!isMove)
                        {
                            SetNonePuzzle();
                            puzzleState = GameState.CheckAll;
                        }
                    }
                    break;
                case GameState.Stop:
                    {
                        if (pickedPuzzle == null || overedPuzzle == null)
                            break;
                        bool IsPickPuzzleSwapMove = pickedPuzzle.CurState == Puzzle.State.Swap;
                        bool IsOveredPuzzleSwapMove = pickedPuzzle.CurState == Puzzle.State.Swap;

                        if (!IsOveredPuzzleSwapMove && !IsPickPuzzleSwapMove)
                        {
                            if (!isTouch)
                                puzzleState = GameState.Swap;
                        }
                    }
                    break;
                case GameState.Swap:
                    {
                        if (pickedPuzzle == null || overedPuzzle == null)
                            return;
                        bool IsPickPuzzleSwapMove = pickedPuzzle.CurState == Puzzle.State.Swap;
                        bool IsOveredPuzzleSwapMove = pickedPuzzle.CurState == Puzzle.State.Swap;
                        if (!IsOveredPuzzleSwapMove && !IsPickPuzzleSwapMove)
                        {
                            if (IsFailToSwap)
                            {
                                SuccessSwap();
                                puzzleState = GameState.Stop;
                            }
                            else if (CheckPuzzlesMove())
                            {
                                SuccessSwap();
                                SetNonePuzzle();
                                puzzleState = GameState.CheckAll;
                            }
                            else
                            {
                                pickedPuzzle.Swap(overedPuzzle);
                                IsFailToSwap = true;
                            }
                        }
                    }
                    break;
                case GameState.CheckAll:
                    {
                        UpdateTopList();

                        if (CheckPuzzlesMove())
                            puzzleState = GameState.Move;
                        else
                            puzzleState = GameState.Stop;

                        ErasePuzzle();
                        CheckTopAround();
                        SetNonePuzzle();
                        MoveSetting();
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
                        for (int i = 0; i < puzzleList.Count; ++i)
                        {
                            puzzleList[i].Do(Puzzle.State.Move, Puzzle.State.Check);
                        }
                    }
                    break;
                case GameState.Stop:
                    {
                        if (pickedPuzzle == null || overedPuzzle == null)
                            return;
                        pickedPuzzle.Do(Puzzle.State.Swap, Puzzle.State.None);
                        overedPuzzle.Do(Puzzle.State.Swap, Puzzle.State.None);
                    }
                    break;
                case GameState.Swap:
                    {
                        if (pickedPuzzle == null || overedPuzzle == null)
                            return;
                        pickedPuzzle.Do(Puzzle.State.Swap, Puzzle.State.None);
                        overedPuzzle.Do(Puzzle.State.Swap, Puzzle.State.None);
                    }
                    break;
            }
        }

        void SuccessSwap()
        {
            pickedPuzzle = null;
            overedPuzzle = null;
            IsFailToSwap = false;
            playerData.MovePuzzle();
        }

        void initSetting()
        {
            SetPuzzles(hexaSize);
            List<Hexa> TopHexas = new List<Hexa>();
            TopHexas.Add(new Hexa(0, 1));
            TopHexas.Add(new Hexa(1, -3));
            TopHexas.Add(new Hexa(2, -3));
            TopHexas.Add(new Hexa(-1, -2));
            TopHexas.Add(new Hexa(-2, -1));
            TopHexas.Add(new Hexa(-2, 3));
            TopHexas.Add(new Hexa(-1, 3));
            TopHexas.Add(new Hexa(0, 3));
            TopHexas.Add(new Hexa(1, 2));
            TopHexas.Add(new Hexa(2, 1));
            SetPuzzleType(TopHexas, Puzzle.Type.Top);

            // >> Top 추가
            for (int i = 0; i < TopHexas.Count; ++i)
            {
                TopList.Add(GetPuzzle(TopHexas[i]));
            }
            TopCount = TopList.Count; 
            // <<

            for (int i = 0; i < puzzleList.Count; ++i)
            {
                puzzleGenerator.Setting(puzzleList[i], TouchPuzzle , OnOver,OnTouchUp);
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

        bool CheckPuzzlesMove()
        {
            bool IsExistErasePuzzle = false;
            for (int i = 0; i < puzzleList.Count; ++i)
            {
                if (puzzleList[i].CurState != Puzzle.State.None )
                    continue;
                puzzleList[i].Check();
                Hexa CheckHexa = puzzleList[i].hexa;
                bool IsEraseLinear = CheckLinear(CheckHexa);
                bool IsEraseBoomerang = CheckBoomerang(CheckHexa);

                if (!IsExistErasePuzzle)
                    IsExistErasePuzzle = IsEraseLinear || IsEraseBoomerang;

            }
            return IsExistErasePuzzle;
        }

        bool CheckBoomerang(Hexa center)
        {
            bool IsExistErasePuzzle = false;
            Puzzle CetnerPuzzle = GetPuzzle(center);
            if (CetnerPuzzle.type >= Puzzle.Type.Top)
                return IsExistErasePuzzle;
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
                    IsExistErasePuzzle = true;
                    CetnerPuzzle.Erase();
                    BoomerangPuzzleList[index].Erase();
                    BoomerangPuzzleList[index+1].Erase();
                    BoomerangPuzzleList[index+2].Erase();
                }
            }
            return IsExistErasePuzzle;

        }


        bool CheckLinear(Hexa center)
        {
            bool IsExistErasePuzzle = false;
            Puzzle.Type centerType = GetPuzzle(center).type;
            if (centerType >= Puzzle.Type.Top)
                return IsExistErasePuzzle;
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
                        IsExistErasePuzzle = true;
                        SelectLinear(hexaAry[i], center, centerType);
                    }
                }
                else if (centerType == BackPuzzle.type)
                {
                    BackPuzzle.Check();
                    if (centerType == GetPuzzle(BackPuzzle.hexa - hexaAry[i]).type)
                    {
                        IsExistErasePuzzle = true;
                        SelectLinear(hexaAry[i], center, centerType);
                    }
                }
            }

            return IsExistErasePuzzle;
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
        }

        void ErasePuzzle()
        {
            for (int i = 0; i < puzzleList.Count; ++i)
            {
                if (puzzleList[i].CurState == Puzzle.State.Erase && puzzleList[i].PuzzleObj !=null)
                {
                    puzzleGenerator.PutPool(puzzleList[i]);
                    puzzleList[i].PuzzleObj.SetActive(false);  // 만약 애니매이션이 존재한다면 애니매이션을 실행시키는 함수를 발동.
                    puzzleList[i].PuzzleObj = null;
                    puzzleList[i].SetPuzzle(Puzzle.Type.Empty);

                }
            }
            
        }

        void CheckTopAround()
        {
            int RainBowCount = 0;

            for (int i = 0; i < TopList.Count; ++i)
            {
                Hexa[] hexas = Hexa.GetAround(TopList[i].hexa);
                bool IsHit = false;
                for (int j = 0; j < hexas.Length; ++j)
                {
                    Puzzle AroundPuzzle = GetPuzzle(hexas[j]);

                    if (AroundPuzzle.CurState == Puzzle.State.Erase)
                    {
                        IsHit = true;
                        if (HittedTop(TopList[i]))
                            ++RainBowCount;
                        break;
                    }
                    if (IsHit)
                        break;
                }

            }

            if (RainBowCount > 0 )
            {
                TopCount -= RainBowCount;
                playerData.UpdateRainbowScore(RainBowCount);
            }
        }

      

        bool HittedTop(Puzzle puzzle)
        {
            
            if (puzzle.type == Puzzle.Type.Top)
            {
                
                puzzle.SetPuzzle(Puzzle.Type.TopSpin);
                puzzle.PuzzleObj.transform.GetChild(1).GetComponent<Animator>().SetBool("IsSpin", true);
            }
            else if (puzzle.type == Puzzle.Type.TopSpin)
            {
                
                puzzle.PuzzleObj.SetActive(false);  // 만약 애니매이션이 존재한다면 애니매이션을 실행시키는 함수를 발동.
                puzzle.PuzzleObj = null;
                puzzle.SetPuzzle(Puzzle.Type.Empty);
                return true;
            }

            return false;
        }

        void SetNonePuzzle()
        {
            for (int i = 0; i < puzzleList.Count; ++i)
            {
                puzzleList[i].None();
            }
        }

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

        void UpdateTopList()
        {
            TopList.Clear();
            for (int i = 0; i < puzzleList.Count; ++i)
            {
                if (puzzleList[i].type == Puzzle.Type.Top || puzzleList[i].type == Puzzle.Type.TopSpin)
                {
                    TopList.Add(puzzleList[i]);
                    if (TopList.Count == TopCount)
                        return;
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

        void TouchPuzzle(Puzzle puzzle)
        {
            if (puzzleState == GameState.Stop && puzzle.type < Puzzle.Type.Purple+1)
            { 
                pickedPuzzle = puzzle;
                isTouch = true;
            }
        }

        void OnOver(Puzzle puzzle )
        {
            if (pickedPuzzle != null && overedPuzzle==null && puzzle.type < Puzzle.Type.Purple + 1)
            {
                overedPuzzle = puzzle;
                pickedPuzzle.Swap(overedPuzzle);
            }
        }

        void OnTouchUp()
        {
            isTouch = false;
        }

    }
}