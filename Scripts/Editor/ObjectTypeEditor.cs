namespace ObjectType.Editor
{
    public static class ObjectTypeEditor
    {
        
        //Add button under Tools/ObjectTypeLibrary
        [UnityEditor.MenuItem("Reboot/ObjectTypeLibrary")]
        public static void OpenObjectTypeLibrary()
        {
            var library = ObjectTypeLibrary.Find();
            UnityEditor.Selection.activeObject = library;
        }
    }
}