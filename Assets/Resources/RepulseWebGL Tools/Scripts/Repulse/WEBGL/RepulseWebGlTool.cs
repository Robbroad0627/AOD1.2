using System;
using Plugins;
using Resources.Scripts.Repulse.DispatchSystem;
using UnityEngine;
using UnityEngine.Serialization;

namespace Resources.Scripts.Repulse.WEBGL
{
   
    public enum Selection
    {
        Enabled,
        Disabled
    }
    public class RepulseWebGlTool : MonoBehaviour
    {
        
        /*<SUMMARY>
         In this we specify all settings we want to check for to enable or disable functionalities of the plugin
         </SUMMARY>
         */

        [Serializable]
        public class Options
        {
            public Selection setActive = Selection.Enabled;
            
        }

        public Options usePlayerPrefs = new Options();
        
        //Setting the settings depending on user choice
        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
            switch (usePlayerPrefs.setActive)
            {
                case Selection.Disabled: break;
                case Selection.Enabled:
                    CoreWebGlPlugin.CheckStorageExists();
                    WebGlDispatcher.UnloadDelegate += CoreWebGlPlugin.OnUnload;
                    WebGlDispatcher.LoadDelegate += CoreWebGlPlugin.LoadIntoPlayerPrefs;
                    break;
            }
            
        }
        
    }
}
