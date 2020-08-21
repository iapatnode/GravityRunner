using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasScript : MonoBehaviour
{
    public Text SignText;

    // Start is called before the first frame update
    void Start()
    {
        SignText.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Makes text pop up when called
    public void EnableText(int textNumber)
    {
        if (textNumber == 1)
        {
            SignText.text = "Use Z to switch gravity\nand avoid spikes";
        }
        else if (textNumber == 2)
        {
            SignText.text = "Switch levers to open doors";
        }
        else
        {
            SignText.text = "Push rocks onto buttons\nto move platforms\nand avoid quicksand";
        }
        SignText.enabled = true;
        Invoke("DisableText", 2);
    }

    //Makes text disappear
    private void DisableText()
    {
        SignText.enabled = false;
    }
}
