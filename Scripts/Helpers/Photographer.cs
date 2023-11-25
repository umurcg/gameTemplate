
﻿#if UNITY_EDITOR
using UnityEditor;
#endif
using System.IO;
using UnityEngine;

namespace Helpers
{
    public class Photographer : MonoBehaviour
    {
#if UNITY_EDITOR

        public string fileName = "shot";
        public string folderPath = "PhotographerShots";
        
        public bool breakAfterShot = true;
        public KeyCode keyCode = KeyCode.S;

        public enum Format { PNG,JPEG }
        public Format format;
        
        // Update is called once per frame
        private void Update()
        {
            if (Input.GetKeyDown(keyCode))
                TakeScreenShot();
        }

        [ContextMenu("Take Screen Shot")]
        public void TakeScreenShot()
        {
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                AssetDatabase.ImportAsset(folderPath);
            }

            var width = Camera.main.pixelWidth;
            var height = Camera.main.pixelHeight;

            var subFolder = folderPath + "/" + width + "_" + height;
            
            if (!Directory.Exists(subFolder))
            {
                Directory.CreateDirectory(subFolder);
                AssetDatabase.ImportAsset(subFolder);
            }
        
            var uniqueFileName = AssetDatabase.GenerateUniqueAssetPath(subFolder+"/"+fileName+"."+format.ToString().ToLower());
            ScreenCapture.CaptureScreenshot(uniqueFileName);
            AssetDatabase.ImportAsset(folderPath);
            
            Debug.Log("Took screen shot and saved to: " + subFolder+"/"+uniqueFileName);
            AssetDatabase.ImportAsset(subFolder, ImportAssetOptions.ForceUpdate);
            
            if (breakAfterShot && Application.isPlaying) Debug.Break();
        }

#endif
        
    }
}
