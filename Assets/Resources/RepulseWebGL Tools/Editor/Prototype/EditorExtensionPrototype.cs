using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using File = System.IO.File;


namespace Repulse.Editor.Prototype
{
    /*//A custom enumeration to specify what value type we are looking for
    public enum ValueTypes
    {
        Float,
        Int,
        String
    }*/
    public class EditorExtensionPrototype : EditorWindow
    {
        /*
        //All parameters defined
        private string _keyPair;
        private string _valuePair;
        private GUIStyle _headerLabelStyle;
        private int _index = 0;
        private readonly string[] _options = {"Float", "Int", "String"};
        
        //Data manaaging platform
        private readonly JsonSerializer _jsonSerializer = new JsonSerializer();
        private List<DataTypes.DataTypes.StorageObject> _storageObjects = new List<DataTypes.DataTypes.StorageObject>();
        private ValueTypes _currentValueType = ValueTypes.Float;
        
        //Generating the Editor Window
        [MenuItem("Window/Repulse WebGL Tools")]
        public static void ShowWindow()
        {
            EditorExtensionPrototype extensionPrototype = (EditorExtensionPrototype)GetWindow(typeof(EditorExtensionPrototype));
            extensionPrototype.maxSize = new Vector2(300f, 500f);
            extensionPrototype.minSize = extensionPrototype.maxSize;
            extensionPrototype.Init();
        }

        private void Awake()
        {
            _jsonSerializer.SetEditorWindow(this);
        }

        //Checking if We have anything saved -> if so add them to the editor window
        public void Init()
        {
            var path = "Assets/Resources/PlayerPrefsJsonData/PlayerPrefsJson.json";

            if (File.Exists(path))
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(fs))
                    {
                        string tempJson = reader.ReadToEnd();

                        var tempObject = JsonUtility.FromJson<DataTypes.DataTypes.JsonSerializableClass>(tempJson);
                        
                        if (tempObject == null) return;
                        
                        var tempKeyValuePairArray = tempObject.ReturnStorageObjects();
                        
                        if (tempKeyValuePairArray != null)
                        {
                            foreach (var keyValuePair in tempKeyValuePairArray)
                            {
                                _storageObjects.Add(keyValuePair);
                                _jsonSerializer.AddToStorageObjects(keyValuePair.key,keyValuePair.value,keyValuePair.dataType);
                            }
                        }
                    }
                }
            }
        }
        
        //All the GUI stuff happening on screen
        private void OnGUI()
        {
            EditorGUILayout.HelpBox("Add all Key/Value pairs you have to be able to store in WEBGL persistent storage,Make Sure you select correct Value type or it will always be 0",MessageType.Info);
            
            _keyPair = EditorGUILayout.TextField("Key", _keyPair);
           
            _index =  EditorGUILayout.Popup(_index, _options);
            
            _currentValueType = (ValueTypes)_index;
            
            if (GUILayout.Button("Add key/value pair"))
            {
                if (!PlayerPrefs.HasKey(_keyPair))
                    return;

                switch (_currentValueType)
                {
                    case ValueTypes.Float: _valuePair = PlayerPrefs.GetFloat(_keyPair).ToString();
                        break;
                    
                    
                    case ValueTypes.Int: _valuePair = PlayerPrefs.GetInt(_keyPair).ToString();
                        break;
                   
                    case ValueTypes.String: _valuePair = PlayerPrefs.GetString(_keyPair);
                        break;
                }
                
                _jsonSerializer.AddToStorageObjects(_keyPair,_valuePair,(int)_currentValueType);
                
               // _keyValuePairs.Add(new KeyValuePair(_keyPair,_valuePair));
                
            }

            if (GUILayout.Button("Generate PlayerPrefs file"))
            {
                
                _jsonSerializer.SerializeJsonToFile();
            }
            
            foreach (var storageObject in _storageObjects)
            {
                GUILayout.Space(10f);
                EditorGUILayout.LabelField(storageObject.key.ToUpper());
                EditorGUILayout.LabelField(storageObject.value);
            }
            
        }


        public void UpdateList(List<DataTypes.DataTypes.StorageObject> storageObjects)
        {
            foreach (var storageObject in storageObjects)
            {
                if(!_storageObjects.Contains(storageObject))
                    _storageObjects.Add(storageObject);
            }
        }
        */
      
       
       
    }
}
