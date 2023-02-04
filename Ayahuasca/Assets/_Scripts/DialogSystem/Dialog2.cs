using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialog2 : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    private int index;
    public float typingSpeed;

 
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    private void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(Type());
    }

    private void OnTriggerExit(Collider other)
    {
        textDisplay.text = "";
    }

    IEnumerator Type()
    {

        foreach(char letter in sentences[index].ToCharArray())
        {
            textDisplay.text += letter;
            yield return new WaitForSeconds(typingSpeed);
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
