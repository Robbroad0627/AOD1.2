using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Resources.DataTypes
{
    public class DataTypes 
    {
        //Creating a storage object containing the Key/Value pair and the correct data type
        [Serializable]
        public struct StorageObject
        {
            public string key;
            public string value;
            [FormerlySerializedAs("_dataType")] public int dataType;

            public StorageObject(string key,string value,int dataType)
            {
                this.key = key;
                this.value = value;
                this.dataType = dataType;
            }
            
        }

        [Serializable]
        public class JsonSerializable
        {
            public StorageObject[] storageObjects;

            public JsonSerializable(List<StorageObject> storageObjects)
            {
                this.storageObjects = storageObjects.ToArray();
            }

            public StorageObject[] ReturnStorageObjects()
            {
                if (storageObjects == null)
                {
                    return null;
                }

                return storageObjects;
            }
        }
    }
}
