using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SizeObj : MonoBehaviour
{
    public float Size { get { return size; } }
    public Text textSize;

    [SerializeField] private float step = 0.3f;
    private float size;
    private Vector3 scaleStep;

    void Start()
    {
        textSize = GetComponentInChildren<Text>();
        
    }
    void Update()
    {        
        textSize.text = size.ToString();
    }

    public void RandomaizeSize(int min, int max)
    {
        size = Random.Range(min, max);
        scaleStep = new Vector3(step, step, step);
        transform.localScale += scaleStep * size;
        
    }
    public void ChangeSize(float count)
    {
        scaleStep = new Vector3(step, step, step);        
        transform.localScale = Vector3.one + scaleStep * count;
        size = count;
        
    }
    public void DownSize()
    {
        scaleStep = new Vector3(step, step, step);
        transform.localScale -= scaleStep;
        size--;
    }
    

}
