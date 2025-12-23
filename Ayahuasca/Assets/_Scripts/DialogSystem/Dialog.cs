using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{
    public string[] sentences;
    protected int index;

    public float typingSpeed;
    public float NewSentenceSpeed;

    public void Talk()
    {
        Debug.Log("Talk");

        if (!DialogManager.instance.IsTalking)
        {
            index = 0;

            InventoryUI.Instance?.gameObject.SetActive(false);
            TriggerDialog();
        }
        else
        {

        }
    }

    public void TriggerDialog()
    {
        if (!DialogManager.instance.IsTalking)
        {
            DialogManager.instance.TypeDialog(sentences[index], typingSpeed);
            DialogManager.instance.OnFinishTalking += OnFinishTalking;
        }
        else
        {
            NextSentence();
        }
    }

    public void NextSentence()
    {

    }

    public void OnFinishTalking()
    {
        if (index < sentences.Length - 1)
        {
            index++;
            DialogManager.instance.TypeDialog(sentences[index], typingSpeed);
        }
        else
        {
            InventoryUI.Instance?.gameObject.SetActive(true);
        }
    }

}
