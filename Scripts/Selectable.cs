using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    private Outline outline;
    // Start is called before the first frame update
    private void Awake()
    {
        outline = this.gameObject.GetComponentInChildren<Outline>();
        outline.enabled = false;

    }
    public IEnumerator SelectCoroutine()
    {
        outline.enabled = true;
        yield return new WaitForSeconds(1f);

        if (outline != null) outline.enabled = false;
       
        
    }
    public IEnumerator WrongSelectCoroutine()
    {
        outline.OutlineColor = Color.red;
        outline.enabled = true;
        yield return new WaitForSeconds(1.5f);
        outline.enabled = false;
        outline.OutlineColor = Color.white;
        
    }

    public void Select()
    {
        outline.enabled = true;
    }

    public void Deselect()
    {
        outline.enabled = false;
    }
}
