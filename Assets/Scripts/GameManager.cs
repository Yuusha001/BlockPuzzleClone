using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlockPuzzle
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager instance { get; private set; }

        private void Awake()
        {
            instance = this;
        }

        public GameState currentState { get; private set; }

        [SerializeField]
        GameObject LoadingScreen,
            GamingScreen;

        [SerializeField]
        Grid gameField;

        public void SetGameState(GameState state)
        {
            currentState = state;
        }

        // Start is called before the first frame update
        void Start() { }

        // Update is called once per frame
        void Update()
        {
            switch (currentState)
            {
                case GameState.Loading:
                    LoadingScreen.SetActive(true);
                    GamingScreen.SetActive(false);
                    break;
                case GameState.Playing:
                    LoadingScreen.SetActive(false);
                    GamingScreen.SetActive(true);
                    if (!gameField.isCreated)
                        gameField.createField();
                    break;
                case GameState.GameOver:
                    break;
                default:
                    break;
            }
        }
    }

    public enum GameState
    {
        Loading,
        Playing,
        GameOver
    }

    public class GameEvent
    {
        public static Action CheckIfShapeCanPlaced;
        public static Action ReturnShapeToStartPosition;
    }
}
