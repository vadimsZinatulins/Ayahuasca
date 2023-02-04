using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogOneMessage : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    private int index;
    public float typingSpeed;
    private bool showMessage = false;


    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    private void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        showMessage = true;
        StartCoroutine(Type());
    }

    private void OnTriggerExit(Collider other)
    {
        showMessage = false;
        StopCoroutine(Type());
        textDisplay.text = "";
    }

    IEnumerator Type()
    {

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

    }

    public void NextSentence()
    {

        if (index < sentences.Length - 1)
        {
            index++;
            textDisplay.text = "";
            StartCoroutine(Type());
        } else
        {
            textDisplay.text = "";

        }
    }

}
