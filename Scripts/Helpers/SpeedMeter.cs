using UnityEngine;

namespace Helpers
{
    public class SpeedMeter : MonoBehaviour
    {
        public float avarageSpeed;
        private Vector3 _lastPosition;
    
        // Start is called before the first frame update
        void Start()
        {
            _lastPosition = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            var speed = (transform.position - _lastPosition).magnitude / Time.deltaTime;
            avarageSpeed = Mathf.Lerp(avarageSpeed, speed, Time.deltaTime);
            _lastPosition = transform.position;
        }

        private void OnGUI()
        {
            var screenPos = Camera.main.WorldToScreenPoint(transform.position+Vector3.up);
            var style = new GUIStyle();
            style.fontSize = 20;
            style.normal.textColor = Color.white;
            var text="Speed: " + avarageSpeed.ToString("F2");
            //Create rect with considering fov and aspect ratio
            var rect = new Rect(screenPos.x - 100, Screen.height - screenPos.y - 50, 200, 100);
            GUI.Label(rect, text, style);
            
            
            

        }
    }
}