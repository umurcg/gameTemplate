using System;
using CorePublic.Managers;
using UnityEngine;
using UnityEngine.Events;

namespace Samples.Demo
{
    public class Box: MonoBehaviour
    {
        private BoxController _boxController;
        private float angleCounter = 0;
        
        
        private void Start()
        {
            _boxController = BoxController.Instance;
        }

        private void Update()
        {
            float speed = _boxController.Speed;
            transform.Rotate(Vector3.up, speed * Time.deltaTime);
            angleCounter += speed * Time.deltaTime;
            if (angleCounter >= 360)
            {
                angleCounter = 0;
                CoreManager.Instance.EarnMoney(_boxController.Income);
            }
            
        }
    }
}