using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GHJ_Lib
{
    public class HexaTile : MonoBehaviour 
    {
        public delegate void CallbackClickHexaTile(Puzzle puzzle);
        public delegate void CallbackStartDragPuzzle();
        public delegate void CallbackEndDragPuzzle();


        public CallbackClickHexaTile OnClickHexaTile;
        public CallbackStartDragPuzzle OnStartDragPuzzle;
        public CallbackEndDragPuzzle OnEndDragPuzzle;
        Puzzle puzzle;
        Vector2 curPosition;
        float srtTouchRange;
        Camera mainCam;
        
        enum ControlType {  None,Drag , Over }
        ControlType controlType = ControlType.None;
        public void Start()
        {
            mainCam = Camera.main;
            curPosition = new Vector2(transform.position.x, transform.position.y);
            srtTouchRange = Puzzle.Interval * 0.5f * Puzzle.Interval * 0.5f;
        }
       

        public void SetPuzzle(Puzzle puzzle)
        {
            this.puzzle = puzzle;
        }
        private void OnMouseDown()
        {
            Vector2 curPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            if ((curPos - curPosition).sqrMagnitude < srtTouchRange)
            {
                OnClickHexaTile(puzzle);
                controlType = ControlType.Drag;
            }
        }

        private void OnMouseOver()
        {
            switch (controlType)
            {
                case ControlType.Over:
                { }
                break;
                case ControlType.Drag:
                {
                    
                }
                break;
                case ControlType.None:
                {
                    Vector2 curPos = mainCam.ScreenToWorldPoint(Input.mousePosition);
                        if ((curPos - curPosition).sqrMagnitude < srtTouchRange)
                        {
                            //스왑애니매이션 진행
                        }
                }
                break;
            }
        }

        private void OnMouseUp()
        {
            if (controlType == ControlType.Drag)
            {
                //애니매이션이 실행 됐다면 검사진행
            }
        }


    }

}

