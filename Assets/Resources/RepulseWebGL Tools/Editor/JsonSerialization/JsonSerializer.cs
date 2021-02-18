using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Resources.Editor.JsonSerialization
{
    
    public class JsonSerializer
    {
        
        
        //All parameters
        private DataTypes.DataTypes _dataTypes = new DataTypes.DataTypes();
        private readonly List<DataTypes.DataTypes.StorageObject> _storageObjects = new List<DataTypes.DataTypes.StorageObject>();
        private WebglEditorExtension _editorWindow;
        
       
        
        //Creating the serialization method
        public  void SerializeJsonToFile()
        {
            string path = "Assets/Resources/RepulseWebGL Tools/PlayerPrefsJsonData";
            SerializeJsonInEditor(path);
        }

        private  void SerializeJsonInEditor(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            
            DataTypes.DataTypes.JsonSerializable temp = new DataTypes.DataTypes.JsonSerializable(_storageObjects);
            path = "Assets/Resources/RepulseWebGL Tools/PlayerPrefsJsonData/PlayerPrefsJson.json";
            
            using (FileStream fs = new FileStream(path, FileMode.OpenOrCreate))
            {
                using (StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(JsonUtility.ToJson(temp));
                }
            }
            AssetDatabase.Refresh();
        }
        

        public void AddToStorageObjects(string key, string value, int dataType)
        {
            var temp = new DataTypes.DataTypes.StorageObject(key, value, dataType);
            
            _storageObjects.Add(temp);
            
           
        }

        public void ResetDataList()
        {
            _storageObjects.Clear();
        }

        public void SetEditorWindow(WebglEditorExtension editorExtensionPrototype)
        {
            _editorWindow = editorExtensionPrototype;
        }
        

       
    }
}
