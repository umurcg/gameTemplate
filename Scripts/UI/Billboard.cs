using UnityEngine;

namespace CorePublic.UI
{
    public class Billboard: MonoBehaviour
    {
        private Camera main;

        void Update()
        {
            if(!main) main = Camera.main;
            transform.LookAt(main.transform.position, Vector3.up);
        }
    }
}