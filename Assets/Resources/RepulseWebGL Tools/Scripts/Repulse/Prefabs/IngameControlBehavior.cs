using Resources.Scripts.Repulse.WEBGL;
using UnityEngine;

namespace Resources.RepulseWebGL_Tools.Scripts.Repulse.Prefabs
{
    public class IngameControlBehavior : MonoBehaviour
    {
        public void Minimize()
        {
            Screen.fullScreen=false;
        }

        public void Maximize()
        {
            Screen.fullScreen = true;
        }
    }
}
