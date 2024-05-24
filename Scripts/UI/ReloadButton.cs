using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    [RequireComponent(typeof(Button))]
    public class ReloadButton: MonoBehaviour
    {
        private void Start()
        {
            GetComponent<Button>().onClick.AddListener(Reload);
        }

        private void Reload()
        {
            CoreManager.Instance.ReloadLevel();
        }
    }
}