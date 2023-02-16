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
        Sprite original;

        [SerializeField]
        BoxCollider2D myCollider2d;

        [SerializeField]
        int squareID;

        [SerializeField]
        private bool isSelected;

        [SerializeField]
        private bool isActive;

        public bool GetisSelected()
        {
            return isSelected;
        }

        public void SetisSelected(bool value)
        {
            isSelected = value;
        }

        public bool GetisActive()
        {
            return isActive;
        }

        public void SetisActive(bool value)
        {
            isActive = value;
        }

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
                    myImage.color = Color.white;
                    SetisActive(true);
                    break;
                default:
                    break;
            }
        }

        public void SetOriginalImage()
        {
            SetImage(original);
        }

        public void SetImage(Sprite img)
        {
            myImage.sprite = img;
        }

        public bool CanUseThisSquare()
        {
            if (GetisSelected() && !GetisActive())
                return true;
            return false;
        }

        public void ActiveSquare()
        {
            SetState(GridSquareState.Active);
            myCollider2d.enabled = false;
        }

        public void DeActiveSquare()
        {
            SetState(GridSquareState.Normal);
            myCollider2d.enabled = true;
            SetisSelected(false);
            SetisActive(false);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (GetisActive())
                return;
            SetState(GridSquareState.Hover);
            SetisSelected(true);
        }

        private void OnTriggerStay2D(Collider2D other)
        {
            SetisSelected(true);
            if (GetisActive())
                return;
            SetState(GridSquareState.Hover);
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (GetisActive())
                return;
            SetState(GridSquareState.Normal);
            SetisSelected(false);
        }
    }

    public enum GridSquareState
    {
        Normal,
        Hover,
        Active
    }
}
