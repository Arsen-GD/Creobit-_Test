using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Text textName;
    public Text greatingText;
    public GameObject inputField;
    public InputField InputName;
    public GameObject levels;

    void Start()
    {
        levels.gameObject.SetActive(false);
        inputField.gameObject.SetActive(true);
        greatingText.gameObject.SetActive(false);
       

    }

    
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void LoadLevel(int numberLvl)
    {
        SceneManager.LoadScene(numberLvl);
    }
    

    public void SetName()
    {
        if (InputName.text == "")
        {
            //Debug.Log("Имя не ввдено");
        }
        else
        {
            inputField.gameObject.SetActive(false);
            textName.text = "" + InputName.text;
            PlayerPrefs.SetString("Name",textName.text);
            levels.gameObject.SetActive(true);
            Greating();

        }
    }

    public void Greating()
    {
        greatingText.text = "Welcome " + textName.text;
        greatingText.gameObject.SetActive(true);
    }
}
