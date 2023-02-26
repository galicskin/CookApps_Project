using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace GHJ_Lib
{
    public class MainUI : MonoBehaviour
    {
        int rainBowScore = 0;
        [SerializeField] Text RainBowScore;

        int moveCount = 0;
        [SerializeField] Text MoveCount;

        public PlayerData playerData { get; private set; }

        void Start()
        {
            playerData = Datamanager.Instance.PlayerData;
        }

        void Update()
        {
            if (rainBowScore != playerData.RainbowCount)
                UpdateRainBowScore(playerData.RainbowCount);

            if (moveCount != playerData.MoveCount)
                UpdateMoveCount(playerData.MoveCount);
        }
        void UpdateRainBowScore(int score)
        {
            rainBowScore = score;
            RainBowScore.text = rainBowScore.ToString();
        }

        public void UpdateMoveCount(int move)
        {
            moveCount = move;
            MoveCount.text = moveCount.ToString();
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }

}

