using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.Rendering;

namespace Helpers
{
    //Reboot Mono class contains common helper functions that works with MonoBehavior
    //Not that this script must be instantiated in the game.

    /// <summary>
    ///     Common
    /// </summary>
    public class Utils : MonoBehaviour
    {
        private static Utils _instance;

        /// <summary>
        ///     Retrieve singleton component with this function.
        /// </summary>
        /// <returns></returns>
        public static Utils Request()
        {
            if (!_instance)
            {
                var instanceObject = new GameObject("reboot_miscellaneous");
                _instance = instanceObject.AddComponent<Utils>();
            }

            return _instance;
        }

        /// <summary>
        ///     Finds and returns game object within a tag recursively.
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static GameObject FindGameObjectInChildWithTag(GameObject parent, string tag)
        {
            var t = parent.transform;

            for (var i = 0; i < t.childCount; i++)
            {
                var child = t.GetChild(i);

                if (child.gameObject.CompareTag(tag)) return child.gameObject;

                if (child.childCount > 0)
                {
                    var grandchild = FindGameObjectInChildWithTag(child.gameObject, tag);
                    if (grandchild != null)
                        return grandchild.gameObject;
                }
            }

            return null;
        }


        /// <summary>
        ///     Changes a standard material to opaque
        /// </summary>
        /// <param name="mat"></param>
        public static void ChangeMaterialToOpaque(Material mat)
        {
            mat.SetInt("_SrcBlend", (int)BlendMode.One);
            mat.SetInt("_DstBlend", (int)BlendMode.Zero);
            mat.SetInt("_ZWrite", 1);
            mat.DisableKeyword("_ALPHATEST_ON");
            mat.DisableKeyword("_ALPHABLEND_ON");
            mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            mat.renderQueue = -1;
        }

        /// <summary>
        ///     Creates a test object in a position
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        public static GameObject TestPosition(Vector3 position)
        {
            var test = GameObject.CreatePrimitive(PrimitiveType.Cube);
            test.name = "TestObject";
            test.transform.position = position;
            return test;
        }

        /// <summary>
        ///     Finds a random position in specified bounds
        /// </summary>
        /// <param name="bounds"></param>
        /// <returns></returns>
        public static Vector3 RandomPointInBounds(Bounds bounds)
        {
            return new Vector3(
                Random.Range(bounds.min.x, bounds.max.x),
                Random.Range(bounds.min.y, bounds.max.y),
                Random.Range(bounds.min.z, bounds.max.z)
            );
        }

        public static Color RandomColor()
        {
            return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        }

        /// <summary>
        ///     Clamps angle to given min and max
        /// </summary>
        /// <param name="angle"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static float ClampAngle(float angle, float min, float max)
        {
            if (angle < -360F)
                angle += 360F;
            if (angle > 360F)
                angle -= 360F;
            return Mathf.Clamp(angle, min, max);
        }

        /// <summary>
        ///     Remaps a float value from one range to another
        /// </summary>
        /// <param name="value"></param>
        /// <param name="from1"></param>
        /// <param name="to1"></param>
        /// <param name="from2"></param>
        /// <param name="to2"></param>
        /// <returns>float</returns>
        public static float Remap(float value, float from1, float to1, float from2, float to2)
        {
            return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
        }

        /// <summary>
        ///     Returns a random vector3 between min and max. (Inclusive)
        /// </summary>
        /// <returns>The <see cref="UnityEngine.Vector3" />.</returns>
        /// <param name="min">Minimum.</param>
        /// <param name="max">Max.</param>
        /// https://gist.github.com/Ashwinning/269f79bef5b1d6ee1f83
        public static Vector3 GetRandomVector3Between(Vector3 min, Vector3 max)
        {
            return min + Random.Range(0, 1) * (max - min);
        }

        /// <summary>
        ///     Gets the random vector3 between the min and max points provided.
        ///     Also uses minPadding and maxPadding (in metres).
        ///     maxPadding is the padding amount to be added on the other Vector3's side.
        ///     Setting minPadding and maxPadding to 0 will make it return inclusive values.
        /// </summary>
        /// <returns>The <see cref="UnityEngine.Vector3" />.</returns>
        /// <param name="min">Minimum.</param>
        /// <param name="max">Max.</param>
        /// <param name="minPadding">Minimum padding.</param>
        /// <param name="maxPadding">Max padding.</param>
        /// https://gist.github.com/Ashwinning/269f79bef5b1d6ee1f83
        public static Vector3 GetRandomVector3Between(Vector3 min, Vector3 max, float minPadding, float maxPadding)
        {
            // min padding as a value between 0 and 1
            var point1 = min + minPadding * (max - min);
            var point2 = max + maxPadding * (min - max);
            return GetRandomVector3Between(point1, point2);
        }
        
        public static void SaveSerializable(string fileName, object objectToSerialize)
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Create(Application.persistentDataPath+"/" + fileName+".txt");
            bf.Serialize(file, JsonUtility.ToJson(objectToSerialize));
            file.Close();
        }
        
        
        public static T LoadSerializable<T>(string fileName)
        {
            BinaryFormatter bf = new BinaryFormatter();

            string path = Application.persistentDataPath+"/" + fileName + ".txt";

            if (!File.Exists(path))
            {
                Debug.LogError("File not found in " + path);
                return default(T);
            }
            
            FileStream file = File.Open(path, FileMode.Open);
            var deserializedFile= bf.Deserialize(file);
            T loadedObject = JsonUtility.FromJson<T>(deserializedFile.ToString());
            file.Close();
            return loadedObject;
        }
        
#if UNITY_EDITOR
        
        public static void SaveAsset(UnityEngine.Object asset, string path,string name)
        {
            if (!UnityEditor.AssetDatabase.IsValidFolder(path))
            {
                Debug.LogError("Path doesnt exists: "+path);
                return;
            }
            
            var assetPathAndName = UnityEditor.AssetDatabase.GenerateUniqueAssetPath(path+"/"+name+".asset");

            UnityEditor.AssetDatabase.CreateAsset(asset, assetPathAndName);
            UnityEditor.AssetDatabase.SaveAssets();
            UnityEditor.AssetDatabase.Refresh();
            
            UnityEditor.EditorUtility.FocusProjectWindow();
            UnityEditor.Selection.activeObject = asset;
        }
#endif
        
    }
}