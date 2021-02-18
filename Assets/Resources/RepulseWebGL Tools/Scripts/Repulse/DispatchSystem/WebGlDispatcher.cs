using UnityEngine;

namespace Resources.Scripts.Repulse.DispatchSystem
{
    public class WebGlDispatcher : MonoBehaviour
    {
        //The core event handler for OnBrowserLoad
        public delegate void LoadedEvent();
        /* Hook onto this event from any script to be able to execute code when page refreshes or browser closes*/
        public static event LoadedEvent LoadDelegate;
        
        
        //The core event handler for Refreshing or browser close
        public delegate void UnloadEvent();
        /* Hook onto this event from any script to be able to execute code when page refreshes or browser closes*/
        public static event UnloadEvent UnloadDelegate;
        
        

        //The core event handler for detecting when mouse enters unity
        public delegate void MouseEnteredEvent();
        /* Hook onto this event from any script to be able to execute code when page refreshes or browser closes*/
        public static event MouseEnteredEvent MouseEnteredDelegate;

        
        
        //The core event handler for detecting when mouse exits unity
        public delegate void MouseLeftEvent();
        /* Hook onto this event from any script to be able to execute code when page refreshes or browser closes*/
        public static event MouseLeftEvent MouseLeftEventDelegate;

        public delegate void ApplicationSetToFullscreen();

        public static event ApplicationSetToFullscreen ApplicationSetToFullscreenDelegate;
        
        
        //<SUMMARY>
        //Even if these are shown as unused - they are but called from the browser
        //</SUMMARY>
        public void BrowserLoaded()
        {
            OnLoadDelegate();
        }

        public void BrowserUnloaded()
        {
           OnUnloadDelegate();
        }

        public void MouseEntered()
        {
           OnMouseEnteredDelegate();
        }

        public void MouseLeft()
        {
            OnMouseLeftEventDelegate();
        }

        public void ApplicationFullscreen()
        {
            OnApplicationSetToFullscreenDelegate();
        }
        
        private static void OnMouseLeftEventDelegate()
        {
            MouseLeftEventDelegate?.Invoke();
        }

        private static void OnUnloadDelegate()
        {
            UnloadDelegate?.Invoke();
        }

        private static void OnMouseEnteredDelegate()
        {
            MouseEnteredDelegate?.Invoke();
        }

        private static void OnLoadDelegate()
        {
            LoadDelegate?.Invoke();
        }

        private static void OnApplicationSetToFullscreenDelegate()
        {
            ApplicationSetToFullscreenDelegate?.Invoke();
        }
    }
}
