using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MainMenuScript : MonoBehaviour
{
    public TMP_InputField inputField;
    public Scene gameScene;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PressButton() {
        Names.UsersName = inputField.text;
        if (inputField.text == "") 
        {
            inputField.text = "Nanashi";
        }

        SceneManager.LoadScene("main");
    
    }
}
