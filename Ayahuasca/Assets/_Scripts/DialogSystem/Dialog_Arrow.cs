using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialog_Arrow : MonoBehaviour
{
    public TextMeshProUGUI textDisplay;
    public string[] sentences;
    public GameObject Arrow;
    private int index;
    public float typingSpeed;
    public float NewSentenceSpeed;
    private bool showMessage = false;

    private void Start()
    {
        Arrow.SetActive(false);
    }

    private void Update()
    {
        if (textDisplay.text == sentences[index])
        {
            Arrow.SetActive(true);
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (textDisplay.text == "")
        {
            index = 0;
            showMessage = true;
            StartCoroutine(Type());
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        showMessage = false;
        StopCoroutine(Type());
        textDisplay.text = "";
        Arrow.SetActive(false);
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
        } 
        
    }

}
