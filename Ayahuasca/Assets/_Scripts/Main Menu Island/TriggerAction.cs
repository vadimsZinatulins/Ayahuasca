using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TriggerAction : MonoBehaviour
{
    class FillPerPlayer 
    {
        private bool isActive;
        private float currentFill;

        public float CurrentFill => currentFill;
        public bool IsActive => isActive;

        public void update(float multiplier) 
        {
            if(isActive)
            {
                currentFill = Mathf.Clamp01(currentFill + Time.deltaTime * multiplier);
            }
            else 
            {
                currentFill = Mathf.Clamp01(currentFill - Time.deltaTime * multiplier);
            }
        }

        public void Enable()
        {
            isActive = true;
        }

        public void Disable() 
        {
            isActive = false;
        }
    }

    [SerializeField]
    private Slider progresUntilActionTrigger;

    [SerializeField]
    protected UnityEvent actionToExecute;

    [SerializeField]
    private bool requiresBothPlayers;

    [SerializeField]
    private float timeToFillPerPlayer = 1.0f;

    private FillPerPlayer[] fillMeters = new FillPerPlayer[2];

    // Start is called before the first frame update
    void Start()
    {
        fillMeters[0] = new FillPerPlayer();
        fillMeters[1] = new FillPerPlayer();
    }

    // Update is called once per frame
    void Update()
    {
        float speed = 1.0f / timeToFillPerPlayer;

        fillMeters[0].update(speed);
        fillMeters[1].update(speed);

        float average = requiresBothPlayers ? 2.0f : 1.0f;
        float currentValue = Mathf.Clamp01((fillMeters[0].CurrentFill + fillMeters[1].CurrentFill) / average);
        
        if(currentValue > 0.0f)
        {
            if(!progresUntilActionTrigger.gameObject.activeSelf)
            {
                progresUntilActionTrigger.gameObject.SetActive(true);
            }

            progresUntilActionTrigger.value = currentValue;
        } 
        else if (progresUntilActionTrigger.gameObject.activeSelf)
        {
            progresUntilActionTrigger.gameObject.SetActive(false);
        }

        
        if(currentValue >= 1.0f)
        {
            actionToExecute.Invoke();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.name.Contains("Avati")) 
        {
            fillMeters[0].Enable();
        } 
        else if(other.gameObject.name.Contains("Sami")) 
        {
            fillMeters[1].Enable();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if(other.gameObject.name.Contains("Avati")) 
        {
            fillMeters[0].Disable();
        } 
        else if(other.gameObject.name.Contains("Sami")) 
        {
            fillMeters[1].Disable();
        }
    }
}
