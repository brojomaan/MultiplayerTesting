using UnityEngine;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            if (Instance != null)
            {
                Debug.LogError("More than one GameManager instance!");
                Destroy(Instance);
            }

            Instance = this;
        }

        public void ClickedOnSomething()
        {
            Debug.Log($"ClickedOnSomething");
        }
    }
}
