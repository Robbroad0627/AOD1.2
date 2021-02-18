using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Resources.Editor.JsonSerialization;
using UnityEditor;
using UnityEngine;

namespace Resources.Editor
{
    
    //A custom enumeration to specify what value type we are looking for
    public enum ValueTypes
    {
        Float,
        Int,
        String
    }
    public class WebglEditorExtension : EditorWindow
    {
    
        //All parameters defined
        private string _keyPair;
        private string _idInput;
        private int _currentID;
        private GUIStyle _headerLabelStyle;
        private int _dataTypeSelected = 0;
        private readonly string[] _dataTypeOptions = {"Float", "Int", "String"};



        //Saving scroll position 
        private Vector2 _currentScrollPos;
        
        //Data managing platform
        private readonly JsonSerializer _jsonSerializer = new JsonSerializer();

        private List<KeyStorageEditor> _keyStorage = new List<KeyStorageEditor>();
        
        private readonly List<DataTypes.DataTypes.StorageObject> _storageObjects = new List<DataTypes.DataTypes.StorageObject>();
        
        private ValueTypes _currentValueType = ValueTypes.Float;
        
        //Styling properties
        private Texture2D _repulseLogo;
        private static Texture2D _backgroundTexture;
        private static Texture2D _tooltipInfoIcon;

        private void OnEnable()
        {
            _repulseLogo = UnityEngine.Resources.Load<Texture2D>("RepulseWebGL Tools/Textures/Repulse/repulsexlogo");
           _tooltipInfoIcon = UnityEngine.Resources.Load<Texture2D>("RepulseWebGL Tools/Textures/Repulse/info");

        }

        [Serializable]
        public struct KeyStorageEditor
        {
            public int id;
            public string key;
            public int valueType;

            public KeyStorageEditor(string key,int valueType,int id)
            {
                this.key = key;
                this.valueType = valueType;
                this.id = id;
            }

        }
        
        //Generating the Editor Window
        [MenuItem("Window/Repulse WebGL Tools")]
        public static void ShowWindow()
        {
            WebglEditorExtension extensionPrototype = (WebglEditorExtension)GetWindow(typeof(WebglEditorExtension),true,"Repulse WEBGL Tools");
            extensionPrototype.maxSize = new Vector2(300f, 600f);
            extensionPrototype.minSize = extensionPrototype.maxSize;
            extensionPrototype.Init();
        }

        
        //Setting the window to the serializer
        private void Awake()
        {
            _jsonSerializer.SetEditorWindow(this);
        }

        

        //Checking if We have anything saved -> if so add them to the editor window
        private void Init()
        {
            var path = "Assets/Resources/RepulseWebGL Tools/PlayerPrefsJsonData/PlayerPrefsJson.json";

            if (File.Exists(path))
            {
                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(fs))
                    {
                        string tempJson = reader.ReadToEnd();

                        var tempObject = JsonUtility.FromJson<DataTypes.DataTypes.JsonSerializable>(tempJson);
                        
                        if (tempObject == null) return;
                        
                        var tempKeyValuePairArray = tempObject.ReturnStorageObjects();
                        
                        if (tempKeyValuePairArray != null)
                        {
                            var tempId = 0;
                            foreach (var keyValuePair in tempKeyValuePairArray)
                            {
                                //_storageObjects.Add(keyValuePair);
                                _keyStorage.Add(new KeyStorageEditor(keyValuePair.key,keyValuePair.dataType,tempId++));
                                _jsonSerializer.AddToStorageObjects(keyValuePair.key,keyValuePair.value,keyValuePair.dataType);
                            }
                        }
                    }
                }
            }

            _currentID = _keyStorage.Count;
        }
        
        //All the GUI stuff happening on screen
        private void OnGUI()
        {
            GUILayout.ExpandWidth(false);
            
            //Background rendering
            _backgroundTexture = new Texture2D(1, 1, TextureFormat.RGBA32, false);
            _backgroundTexture.SetPixel(0,0,new Color(0.09f,0.05f,0.05f));
            _backgroundTexture.Apply();
            
            
            GUI.DrawTexture(new Rect(0,0,maxSize.x,maxSize.y),_backgroundTexture,ScaleMode.StretchToFill);
            
            //Top part with Repulse logo and label
            GUIStyle logoLabelStyle = new GUIStyle(GUI.skin.label) {contentOffset = new Vector2(70, 30), fontSize = 18};

            if (_repulseLogo != null)
            {
                EditorGUILayout.LabelField("REPULSE",logoLabelStyle);
                EditorGUI.DrawPreviewTexture(new Rect(10, 10, 60, 60), _repulseLogo);
                GUILayout.Space(60);
            }
            
            //The Tooltip style
            GUIStyle tooltipStyle = GUI.skin.GetStyle("HelpBox");
            if(_tooltipInfoIcon!=null)
            {
                
                EditorGUI.DrawPreviewTexture(new Rect(30,115,25,25),_tooltipInfoIcon);
            }
            tooltipStyle.margin = new RectOffset(15, 15, 15, 15);
            tooltipStyle.padding = new RectOffset(55, 10, 10, 10);
            EditorGUILayout.TextArea("Add all Key/Value pairs you want to be stored in WEBGL persistent storage,Make sure you select correct Value type or it will always be stored as 0 if wrong. ",tooltipStyle);
            
            //Key name style
            GUIStyle keyLabelStyle = new GUIStyle(GUI.skin.textField);
            keyLabelStyle.margin = new RectOffset(35, 35, 5, 10);
            EditorGUIUtility.labelWidth = 40f;
            
            _keyPair = EditorGUILayout.TextField("Key", _keyPair,keyLabelStyle);
           
            
            //The datatype selection
            GUIStyle dataTypePopupStyle = GUI.skin.GetStyle("Popup");
            dataTypePopupStyle.fixedHeight = 25f;
            dataTypePopupStyle.margin = new RectOffset(30, 33, 5, 15);
            dataTypePopupStyle.padding = new RectOffset(5, 0, 0, 0);
            _dataTypeSelected =  EditorGUILayout.Popup(_dataTypeSelected, _dataTypeOptions,dataTypePopupStyle);
            _currentValueType = (ValueTypes)_dataTypeSelected;


            //Buttons and styling
            GUIStyle buttonsGuiStyle = new GUIStyle(GUI.skin.button);
            buttonsGuiStyle.margin = new RectOffset(25, 25, 5, 0);
            
            if (GUILayout.Button("Add key/value pair",buttonsGuiStyle))
            {
                
                _keyStorage.Add(new KeyStorageEditor(_keyPair,_dataTypeSelected,_currentID++));
                
                _jsonSerializer.AddToStorageObjects(_keyPair,"EMPTY",(int)_currentValueType);
                
            }

            if (GUILayout.Button("Generate PlayerPrefs file",buttonsGuiStyle))
            {
                
                _jsonSerializer.SerializeJsonToFile();
            }

            if (GUILayout.Button("Undo", buttonsGuiStyle))
            {
                _jsonSerializer.ResetDataList();
                _keyStorage.Clear();
                Init();
            }

            if(!Directory.Exists("Assets/WebGLTemplates"))
                if (GUILayout.Button("Import WebGL Templates", buttonsGuiStyle))
                {
                    CopyTemplates();
                }
            
           


            //Scroll area
            GUIStyle deleteIDButtonStyle = new GUIStyle(GUI.skin.button);
            deleteIDButtonStyle.fixedWidth = 50f;
            /*deleteIDButtonStyle.margin = new RectOffset(0, 0, -500, 0);*/

            EditorGUILayout.LabelField("Playerprefs Keys:");
            _currentScrollPos = EditorGUILayout.BeginScrollView(_currentScrollPos, false, true);
            foreach (var keyStorageEditor in _keyStorage.ToList())
            {
                var selectedValueType = (ValueTypes) keyStorageEditor.valueType;
                GUILayout.Space(10f);
                
                EditorGUILayout.LabelField("ID:", keyStorageEditor.id.ToString());
                EditorGUILayout.LabelField("Key: ", keyStorageEditor.key.ToUpper());
                EditorGUILayout.LabelField("Type: ", selectedValueType.ToString());
                
                if (GUILayout.Button("Delete",deleteIDButtonStyle))
                {
                    
                    int idToDeleteAsInt = keyStorageEditor.id;

                    foreach (var key in _keyStorage)
                    {
                        if (key.id == idToDeleteAsInt)
                        {
                            _keyStorage.Remove(key);
                            ResetIDs();
                            _currentID--;
                            break;
                        }
                    }
                }
                

            }
            EditorGUILayout.EndScrollView();

            
            
        }


        private void ResetIDs()
        {

            var tempID = 0;
            var tempList = new List<KeyStorageEditor>();
            
            _jsonSerializer.ResetDataList();
            
            foreach (var keyStorageEditor in _keyStorage)
            {
                var tempKeyStorageEditor = new KeyStorageEditor(keyStorageEditor.key, keyStorageEditor.valueType, tempID++);
                tempList.Add(tempKeyStorageEditor);
                _jsonSerializer.AddToStorageObjects(tempKeyStorageEditor.key,"EMPTY",tempKeyStorageEditor.valueType);
            }
            _keyStorage.Clear();

            _keyStorage = tempList;
            
            

        }

        private void CopyTemplates()
        {
            var targetPath = "Assets/WebGLTemplates";
            var sourcePath = "Assets/Resources/Templates";

            if (!Directory.Exists(targetPath))
            {
                foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
                {
                    Directory.CreateDirectory(dirPath.Replace(sourcePath, targetPath));
                }

                foreach (var file in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
                {
                    File.Copy(file,file.Replace(sourcePath,targetPath),true);
                }
                
                AssetDatabase.Refresh();
            }
        }
    }
}
