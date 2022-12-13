using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Utility
{
    [AddComponentMenu("UI/ButtonExtra")]
    public class N_ButtonExtra : MonoBehaviour, ISelectHandler,IPointerEnterHandler,IPointerExitHandler,IDeselectHandler
    {
        public LeanTweenType TweenType;
        private Button _button;
        public Vector3 targetAnimationScale = Vector3.one;
        public float easeInTime = 1;
        public float easeOutTime =1;
        private Vector3 initialAnimationScale;

        public bool invertButtonColor = false;
        private Color originalButtonColor;
        public Color invertedButtonColor;
        public bool invertTextColor = false;
        private Color originalTextColor;
        public Color invertedTextColor;

        void Awake()
        {
            _button = GetComponent<Button>();
            if (_button)
            {
                initialAnimationScale = transform.localScale;
                if (invertButtonColor)
                {
                    originalButtonColor = _button.image.color;
                }

                if (invertTextColor)
                {
                    originalTextColor = _button.GetComponentInChildren<TextMeshProUGUI>().color;
                }
            }

        }

        private void OnEnable()
        {
            LeanTween.cancel(gameObject);
            transform.localScale = initialAnimationScale;
        }

        private void OnValidate()
        {
            if (_button == null)
            {
                if (TryGetComponent<Button>(out Button btn))
                {
                    _button = btn;
                }
                else
                {
                    _button = gameObject.AddComponent<Button>();
                }
            }
        }

        public void OnSelect(BaseEventData eventData)
        {
            LeanTween.cancel(gameObject);
            LeanTween.scale(GetComponent<RectTransform>(), targetAnimationScale, easeInTime).setEase(TweenType);
            if (_button)
            {
                if (invertButtonColor)
                {
                    _button.image.color = invertedButtonColor;
                }

                if (invertTextColor)
                {
                    _button.GetComponentInChildren<TextMeshProUGUI>().color = invertedTextColor;
                }
            }
           
        }
        
        public void OnPointerEnter(PointerEventData eventData)
        {
            LeanTween.cancel(gameObject);
            LeanTween.scale(GetComponent<RectTransform>(), targetAnimationScale, easeInTime).setEase(TweenType);
            if (_button)
            {
                if (invertButtonColor)
                {
                    _button.image.color = invertedButtonColor;
                }

                if (invertTextColor)
                {
                    _button.GetComponentInChildren<TextMeshProUGUI>().color = invertedTextColor;
                }
            }
        }
        
        public void OnPointerExit(PointerEventData eventData)
        {
            LeanTween.cancel(gameObject);
            LeanTween.scale(GetComponent<RectTransform>(), initialAnimationScale, easeOutTime).setEase(TweenType);
            if (_button)
            {
                if (invertButtonColor)
                {
                    _button.image.color = originalButtonColor;
                }

                if (invertTextColor)
                {
                    _button.GetComponentInChildren<TextMeshProUGUI>().color = originalTextColor;
                }
            }
        }
        
        public void OnDeselect(BaseEventData eventData)
        {
            LeanTween.cancel(gameObject);
            LeanTween.scale(GetComponent<RectTransform>(), initialAnimationScale, easeOutTime).setEase(TweenType);
            if (_button)
            {
                if (invertButtonColor)
                {
                    _button.image.color = originalButtonColor;
                }

                if (invertTextColor)
                {
                    _button.GetComponentInChildren<TextMeshProUGUI>().color = originalTextColor;
                }
            }
        }
        
        public void LoadLevel(string name)
        {
            SceneManager.LoadScene(name);
        }

        public void ChangeMenu(N_Menu TargetMenu)
        {
            N_Menu.ChangeMenu(TargetMenu);
        }

        public void TravelBack()
        {
            N_Menu.TravelBack();
        }

        public void ExitGame()
        {
            Application.Quit();
        }
    }
}