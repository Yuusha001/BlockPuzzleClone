using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlockPuzzle
{
    public class ScoreManager : MonoBehaviour
    {
        [SerializeField]
        UnityEngine.UI.Text scoreTxt;
        int currentScore;

        private void Start()
        {
            AddScore(0);
        }

        private void OnEnable()
        {
            GameEvent.AddScore += AddScore;
        }

        private void OnDisable()
        {
            GameEvent.AddScore -= AddScore;
        }

        private void AddScore(int index)
        {
            currentScore += index;
            scoreTxt.text = "Your Score : " + currentScore.ToString();
        }
    }
}
