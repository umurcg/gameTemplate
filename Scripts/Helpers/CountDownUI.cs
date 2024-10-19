using CorePublic.UI;
using UnityEngine;
using UnityEngine.Events;

namespace CorePublic.Helpers
{
    public class CountDownUI : MonoBehaviour
    {
        private TextController _textController;

        public int duration;
        private float _counter;
        public UnityEvent onCountDownFinished;
    
        // Start is called before the first frame update
        void Start()
        {
            _textController = gameObject.AddComponent<TextController>();
            _counter = duration;
        }

        // Update is called once per frame
        void Update()
        {
            if(_counter<=0) return;
        
            _counter -= Time.deltaTime;
            if (_counter < 0)
            {
                _counter = 0;
                if (onCountDownFinished!=null) onCountDownFinished.Invoke();
            }
            _textController.SetText(Mathf.CeilToInt(_counter).ToString());
     
        }
    }
}
