using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Text playerNameText;
    public Text playerScore;
    public Text playerEnergy;
    public Text hint;


    [SerializeField] private GameObject buttons;
    [SerializeField] private int score;
    [SerializeField] private int energy;
    public int Energy { get { return energy; } }
    public int Score { get { return score; } }

    void Start()
    {
        buttons.gameObject.SetActive(false);
        hint.gameObject.SetActive(false);
        playerEnergy.gameObject.SetActive(false);
        playerNameText.text = PlayerPrefs.GetString("Name");

        energy = 0;
        score = 0;
    }


    void Update()
    {
        playerScore.text = "Score: " + score.ToString();
        playerEnergy.text = "Energy: " + energy.ToString();

    }
    public void AddScore()
    {
        score++;
    }
    public void RemoveEnergy()
    {
        energy--;
    }
    public void AddStartEnergy(int count)
    {
        
        energy = count;
    }
    

    public void HideUI(bool enable)
    {
        buttons.SetActive(enable);
    }
    public IEnumerator HinttCoroutine(string text)
    {
        hint.text = text;
        hint.gameObject.SetActive(true);
        yield return new WaitForSeconds(2f);
        hint.gameObject.SetActive(false);


    }
}
