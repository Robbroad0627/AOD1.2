using System.Runtime.InteropServices;

namespace Plugins
{
    public static class GetAllFunctions
    {
        [DllImport("__Internal")]
        public static extern void SaveCookies(string str,string key);

        [DllImport("__Internal")]
        public static extern string GetCookies(string key);

        [DllImport("__Internal")]
        public static extern void GenerateWebStorage(string key);
        
        
    }
}
