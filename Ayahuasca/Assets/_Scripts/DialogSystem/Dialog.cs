using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    protected int index;
    public float typingSpeed;
    public float NewSentenceSpeed;
    protected bool showMessage = false;

    public bool IsTalking { get; set; }

    public void Talk() {
        Debug.Log("Talk");
        
        this.index = 0;
        this.showMessage = true;
        
        InventoryUI.Instance?.gameObject.SetActive(false);
        StartCoroutine(Type());
    }

    private IEnumerator Type()
    {
        IsTalking = true;

        foreach(char letter in sentences[index].ToCharArray())
        {
            if (showMessage == true)
            {
                textDisplay.text += letter;
                yield return new WaitForSeconds(typingSpeed);
            }
            else
            {
                break;
            }
            
        }

        if (showMessage == true)
        {
            yield return new WaitForSeconds(NewSentenceSpeed);
            NextSentence();
        }
    }

    public void NextSentence()
    {
        if (index < sentences.Length - 1)
        {
            index++;
            textDisplay.text = "";
            StartCoroutine(Type());
        } else {
            IsTalking = false;
            textDisplay.text = "";
            showMessage = false;

            InventoryUI.Instance?.gameObject.SetActive(true);
        }
    }

}
