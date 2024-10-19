using System;
using UnityEditor;
using UnityEngine;

namespace CorePublic.Helpers
{
    [ExecuteInEditMode]
    public class GUIDGenerator : MonoBehaviour
    {
        public string GUID => _guid;
        [SerializeField] private string _guid;

#if UNITY_EDITOR

        private void Reset()
        {
            GenerateGUID();
        }

        [ContextMenu("Generate GUID")]
        private void GenerateGUID()
        {
            _guid = Guid.NewGuid().ToString();
            EditorUtility.SetDirty(this);
        }
#endif
    }
}
