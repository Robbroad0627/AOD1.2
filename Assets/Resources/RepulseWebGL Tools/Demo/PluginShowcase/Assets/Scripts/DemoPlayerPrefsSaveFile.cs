using UnityEngine;

namespace Resources
{
    public class DemoPlayerPrefsSaveFile : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            PlayerPrefs.SetFloat("Health",100);
            PlayerPrefs.SetInt("Magika",50);
            PlayerPrefs.SetString("Name","Tom");
            PlayerPrefs.SetFloat("Random1",50f);
            PlayerPrefs.Save();
        }

    
    }
}
