using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace GHJ_Lib
{
    public class HexaTile : MonoBehaviour
    {
        public delegate void CallbackClickHexaTile(Puzzle puzzle);
        public delegate void CallbackOverPuzzle(Puzzle puzzle);
        public delegate void CallbackUpMouseHexaTile();
        enum ControlType { None, Drag, Over }

        public CallbackClickHexaTile OnClickHexaTile;
        public CallbackOverPuzzle OnOverHexaTile;
        public CallbackUpMouseHexaTile OnUpMouse;
        Puzzle puzzle;
        [SerializeField] Puzzle.Type Type;
        Vector2 curPosition;
        float srtTouchRange;
        Camera mainCam;
        ControlType controlType = ControlType.None;
        public void Start()
        {
            mainCam = Camera.main;
            curPosition = new Vector2(transform.position.x, transform.position.y);
            srtTouchRange = Puzzle.Interval * 0.5f * Puzzle.Interval * 0.5f;
        }

        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.A))
            {
                if(puzzle!= null)
                    Type = puzzle.type;
            }
        }

        public void SetPuzzle(Puzzle puzzle)
        {
            this.puzzle = puzzle;
        }

        private void OnMouseDown()
        {

            OnClickHexaTile(puzzle);
            controlType = ControlType.Drag;
            
        }
        private void OnMouseOver()
        {
            switch (controlType)
            {
                case ControlType.Over:
                {
                        
                }
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
                            OnOverHexaTile(puzzle);
                        }
                }
                break;
            }
        }
        private void OnMouseEnter()
        {
            puzzle.Glow();
        }
        private void OnMouseExit()
        {
            puzzle.CancelGlow();
        }
        private void OnMouseUp()
        {
            if (controlType == ControlType.Drag)
            {
                controlType = ControlType.None;
                OnUpMouse();
            }
        }
       
    }

    
}

