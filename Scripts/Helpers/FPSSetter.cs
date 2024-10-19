using UnityEngine;

namespace CorePublic.Helpers
{
    public class FPSSetter : MonoBehaviour
    {
        [SerializeField] private int aimFPS = 60;
    
        // Start is called before the first frame update
        void Start()
        {
            Application.targetFrameRate = aimFPS;
        }

    }
}
