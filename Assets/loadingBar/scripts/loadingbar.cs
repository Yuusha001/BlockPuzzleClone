using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace BlockPuzzle
{
    public class loadingbar : MonoBehaviour
    {
        private RectTransform rectComponent;
        private Image imageComp;
        public float speed = 0.0f;

        // Use this for initialization
        void Start()
        {
            rectComponent = GetComponent<RectTransform>();
            imageComp = rectComponent.GetComponent<Image>();
            imageComp.fillAmount = 0.0f;
            GameManager.instance.SetGameState(GameState.Loading);
        }

        void Update()
        {
            if (imageComp.fillAmount != 1f)
            {
                imageComp.fillAmount = imageComp.fillAmount + Time.deltaTime * speed;
            }
            else
            {
                GameManager.instance.SetGameState(GameState.Playing);
                //imageComp.fillAmount = 0.0f;
            }
        }
    }
}
