using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderGetSize : MonoBehaviour
{
    private Text sizeText;
    public Slider slider;
    
    void Start()
    {
        sizeText = GetComponent<Text>();
        slider.gameObject.SetActive(false);
        
    }

    void Update()
    {
        sizeText.text = slider.value.ToString();
    }

    public void SliderHide(bool enabled)
    {
        slider.gameObject.SetActive(enabled);
    }


}
