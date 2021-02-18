using System.Collections.Generic;
using EncryptionLibrary;
using UnityEngine;
using static Plugins.GetAllFunctions;

namespace Resources.Scripts.Repulse.WEBGL
{
    public static class CoreWebGlPlugin
    {
        //Creating a local storage object to store the player prefs data
        private static readonly List<DataTypes.DataTypes.StorageObject> LocalStorageObjects = new List<DataTypes.DataTypes.StorageObject>();
        private static readonly string _keyName = Application.productName;
        
        //You call this if you want to manually save the Json to web storage
        public static void SaveToWebStorage(string jsonToSave)
        {
            SaveCookies(jsonToSave,_keyName);
        }

        //You call this if you want to automatically handle playerPrefs to WebStorage
        public static void SaveToWebStorage()
        {
            OnUnload();
        }

        //You call this to get data from web storage 
        //Returns a JSON string that needs to be managed to extract data
        public static string GetFromWebStorage()
        {
            var decryptedString = AesEncryption.DecryptString(GetCookies(_keyName));
            return decryptedString;
        }

        public static void LoadIntoPlayerPrefs()
        {
            LoadIntoPlayerPrefs(GetFromWebStorage());
        }

        public static void CheckStorageExists()
        {
            GenerateWebStorage(_keyName);
        }

        //This is the function called when browser refreshes or changes
        public static void OnUnload()
        {
            LocalStorageObjects.Clear();
            var jsonObject = UnityEngine.Resources.Load<TextAsset>("RepulseWebGL Tools/PlayerPrefsJsonData/PlayerPrefsJson").ToString();
            Debug.Log("Step 1:" + jsonObject);
            CheckPlayerPrefsForChanges(jsonObject);
        }

        
        //<SUMMARY>
        //These functions are helper functions for the OnUnload Function set to save all available player prefs to the storage
        //</SUMMARY>
        private static void CheckPlayerPrefsForChanges(string jsonObject)
        {
            var jsonSerializableObject = JsonUtility.FromJson<DataTypes.DataTypes.JsonSerializable>(jsonObject);
            if (jsonSerializableObject == null)
                return;
            
            Debug.Log("Step 2:" + jsonSerializableObject);
            UpdatePlayerPrefs(jsonSerializableObject);
            
        }

        private static void UpdatePlayerPrefs(DataTypes.DataTypes.JsonSerializable jsonSerializable)
        {
            foreach (var storageObject in jsonSerializable.storageObjects)
            {
                var value = " ";

                if (PlayerPrefs.HasKey(storageObject.key))
                {
                    switch (storageObject.dataType)
                    {
                        case 0: value = PlayerPrefs.GetFloat(storageObject.key).ToString();
                            break;
                        case 1: value = PlayerPrefs.GetInt(storageObject.key).ToString();
                            break;
                        case 2: value = PlayerPrefs.GetString(storageObject.key);
                            break;
                    }
                }
                
                UpdateObjectValue(value,storageObject);
                
            }
            
            CreateJsonSerializableClass();
        }

        private static void UpdateObjectValue(string value, DataTypes.DataTypes.StorageObject storageObject)
        {
            storageObject.value = value;
            LocalStorageObjects.Add(storageObject);
            Debug.Log("Updating Storage Object:" + storageObject);
        }

        private static void CreateJsonSerializableClass()
        {
            var tempJsonClass = new DataTypes.DataTypes.JsonSerializable(LocalStorageObjects);
            var tempJsonString = JsonUtility.ToJson(tempJsonClass);
            Debug.Log("Step 5:" + tempJsonString);
            var encryptedString =  AesEncryption.EncryptString(tempJsonString);
            SaveToWebStorage(encryptedString);
        }
        
        //<SUMMARY>
        //These functions are helper functions for the onLoad event to load everything from storage to player prefs
        //</SUMMARY>
        private static void LoadIntoPlayerPrefs(string playerPrefsJsonString)
        {
            var jsonSerializableClass = JsonUtility.FromJson<DataTypes.DataTypes.JsonSerializable>(playerPrefsJsonString);
            
            foreach (var storageObject in jsonSerializableClass.storageObjects)
            {
                switch (storageObject.dataType)
                {
                    case 0:
                        if(float.TryParse(storageObject.value,out var tempFloat)) 
                            PlayerPrefs.SetFloat(storageObject.key,tempFloat);
                        break;
                    case 1: if(int.TryParse(storageObject.value,out var tempInt))
                                PlayerPrefs.SetInt(storageObject.key,tempInt);
                        break;
                    case 2: PlayerPrefs.SetString(storageObject.key,storageObject.value);
                        break;
                }
            }
            
            Debug.Log("Successfully loaded player prefs to game");
            
            
        }
        
        
    }
}
