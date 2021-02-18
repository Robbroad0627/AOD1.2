using System;
using Resources.DataTypes;
using Resources.Scripts.Repulse.DispatchSystem;
using Resources.Scripts.Repulse.WEBGL;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;

public class DemoFunctionallity : MonoBehaviour
{
    [Serializable]
    private struct BrowserSize
    {
        public int height;
        public int width;
    }
    
    //Browser Stats Panel
    public Text width;
    public Text height;
    
    //Player Prefs Panel
    public ScrollView playerPrefsScrollView;
    
    
    //Update Player Prefs Panel
    public Button incrementPlayerHealth;
    public Button decrementPlayerHealth;
    public InputField playerName;
    public Button submitButton;
    
    //Scrollview Item
    public Text HealthValue;
    public Text NameValue;
    
    //The ingame controls
    public GameObject ingameControls;
    
    private void Awake()
    {
        WebGlDispatcher.LoadDelegate += UpdateScrollView;
       // WebGlDispatcher.ApplicationSetToFullscreenDelegate += ApplicationIsInFullscreen;
    }

    private void Update()
    {
        ApplicationIsInFullscreen();
    }

    //Enabling fullscreen controls if in fullscreen
    private void ApplicationIsInFullscreen()
    {
        if(Screen.fullScreen)
            ingameControls.SetActive(true);
        else
            ingameControls.SetActive(false);
        
    }

    //Code is used from the browser
    public void LoadBrowserVariables(string json)
    {
        var browserSize = JsonUtility.FromJson<BrowserSize>(json);
        
        UpdateBrowserVariables(browserSize.width.ToString(),browserSize.height.ToString());
    }

    public void UpdateBrowserVariables(string width,string height)
    {
        this.width.text = width;
        this.height.text = height;
    }

    public void UpdateScrollView()
    {
        var tempString = CoreWebGlPlugin.GetFromWebStorage();
        var storageObjects = JsonUtility.FromJson<DataTypes.JsonSerializable>(tempString).ReturnStorageObjects();
        foreach (var storageObject in storageObjects)
        {
            if (storageObject.key == "Health")
            {
                HealthValue.text = storageObject.value.ToString();
            }

            if (storageObject.key == "Name")
            {
                NameValue.text = storageObject.value.ToString();
            }
        }
        
    }

    public void IncrementPlayerHealth()
    {
        var tempPlayerHealth = PlayerPrefs.GetFloat("Health");
        HealthValue.text = (++tempPlayerHealth).ToString();
        PlayerPrefs.SetFloat("Health",tempPlayerHealth);
    }

    public void DecrementPlayerHealth()
    {
        var tempPlayerHealth = PlayerPrefs.GetFloat("Health");
        HealthValue.text = (--tempPlayerHealth).ToString();
        PlayerPrefs.SetFloat("Health",tempPlayerHealth);
    }

    public void SubmitPlayerPrefs()
    {
        PlayerPrefs.SetString("Name",playerName.text);
        PlayerPrefs.Save();
        CoreWebGlPlugin.SaveToWebStorage();
        UpdateScrollView();
    }

    public void Minimize()
    {
        Screen.fullScreen = false;
    }
    
    public void Maximize()
    {
        Screen.fullScreen = true;
    }
}
