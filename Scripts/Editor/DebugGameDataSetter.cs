using CorePublic.Managers;
using UnityEditor;
using UnityEngine;

namespace CorePublic.Editor
{
    public class DebugGameDataSetter: EditorWindow
    {
        public int levelToSet=0;
        public int moneyToSet=0;
        
        [MenuItem("Reboot/Tools/Debug Game Data Setter")]
        public static void ShowWindow()
        {
            GetWindow<DebugGameDataSetter>("DebugGameDataSetter");
        }

        private void OnGUI()
        {
            levelToSet = EditorGUILayout.IntField("Level", levelToSet);
            moneyToSet = EditorGUILayout.IntField("Money", moneyToSet);
            
            if (GUILayout.Button("Set Game Data"))
            {
                SetGameData();
            }
        }

        private void SetGameData()
        {
            CoreManager coreManager = FindObjectOfType<CoreManager>();
            coreManager.SetLevel(levelToSet,true);
            float moneyDif = moneyToSet - coreManager.GameMoney;
            if (moneyDif > 0)
            {
                coreManager.EarnMoney((int)moneyDif);
            }
            else
            {
                coreManager.SpendMoney((int)-moneyDif);
            }
        }
    }
}