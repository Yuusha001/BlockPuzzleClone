using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlockPuzzle
{
    public class GridSquare : MonoBehaviour
    {
        [SerializeField]
        GridSquareState currentState;

        [SerializeField]
        UnityEngine.UI.Image myImage;

        [SerializeField]
        BoxCollider2D myCollider2d;

        [SerializeField]
        int squareID;

        public bool isSelected { get; set; }
        public bool isActive { get; set; }

        public GridSquareState CurrentState
        {
            get => currentState;
            set => currentState = value;
        }
        public int SquareID
        {
            get => squareID;
            set => squareID = value;
        }

        void SetState(GridSquareState state)
        {
            CurrentState = state;
            switch (state)
            {
                case GridSquareState.Normal:
                    myImage.color = Color.white;
                    break;
                case GridSquareState.Hover:
                    myImage.color = Color.gray;
                    break;
                case GridSquareState.Active:
                    isActive = true;
                    myImage.color = Color.green;
                    break;
                default:
                    break;
            }
        }

        public bool CanUseThisSquare()
        {
            if (isSelected && !isActive)
                return true;
            return false;
        }

        public void ActiveSquare()
        {
            SetState(GridSquareState.Active);
            myCollider2d.enabled = false;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (isActive)
                return;
            SetState(GridSquareState.Hover);
            isSelected = true;
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            isSelected = true;
            if (isActive)
                return;
            SetState(GridSquareState.Hover);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (isActive)
                return;
            SetState(GridSquareState.Normal);
            isSelected = false;
        }
    }

    public enum GridSquareState
    {
        Normal,
        Hover,
        Active
    }
}
