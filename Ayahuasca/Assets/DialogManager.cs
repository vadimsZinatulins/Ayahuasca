using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DialogManager : MonoBehaviour
{
    public static DialogManager instance;
    public TextMeshProUGUI textDisplay;

    protected bool isTalking = false;
    public bool IsTalking { get { return isTalking; } }

    public UnityAction OnFinishTalking;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public void TypeDialog(string InDialogString, float InTypingSpeed)
    {
        if (!isTalking)
        {
            textDisplay.text = "";
            textDisplay.gameObject.SetActive(true);
            StartCoroutine(TypeDialogCoroutine(InDialogString, InTypingSpeed));
        }
    }

    private IEnumerator TypeDialogCoroutine(string InDialogString, float InTypingSpeed)
    {
        if (!isTalking)
        {
            isTalking = true;

            foreach (char letter in InDialogString.ToCharArray())
            {
                textDisplay.text += letter;
                yield return new WaitForSeconds(InTypingSpeed);
            }

            isTalking = false;
            OnFinishTalking?.Invoke();
        }
    }
}
