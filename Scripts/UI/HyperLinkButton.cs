using UnityEngine;
using UnityEngine.UI;

namespace CorePublic.UI
{
    [RequireComponent(typeof(Button))]
    public class HyperLinkButton : MonoBehaviour
    {
        public string url;
    
        // Start is called before the first frame update
        void Start()
        {
            Button button = GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                Application.OpenURL(url);
            });
        }

    }
}
