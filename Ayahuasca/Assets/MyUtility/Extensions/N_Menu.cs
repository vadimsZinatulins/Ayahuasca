using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Utility;


public class N_Menu : MonoBehaviour
{
    private static N_Menu CurrentActiveMenu;
    private static SelectionType CurrentSelectionType;
    private static float ChangeCurrentCooldown;
    private static float ChangeCooldown = 0.5f;
    private static N_Menu TargetPauseMenu;
    public static event Action<N_Menu> OnChangeMenu;

    //#THINK: Not gonna change this to find the first Selectable it founds because it may found hidden menus i dont want.
    //So we send the list of box contents he's gonna look for the first one he sees
    public List<RectTransform> buttonsContents;
    public bool CanTravelBack = true;
    public bool StartsActive=false;
    public bool UnlockMouseOnActive = true;
    public static UnityEvent<bool> OnUnlockMouseOnActive;
    public static UnityEvent<bool> OnLockMouseOnActive;
    public bool isActive { get; private set; }
    public UnityEvent OnEnableMenu;
    public UnityEvent OnDisableMenu;

    public static Stack<N_Menu> MenuBackStack;

    public static void ResetValues()
    {
        MenuBackStack = null;
        TargetPauseMenu = null;
        CurrentActiveMenu = null;
        ChangeCurrentCooldown = 0;
        
    }
    public virtual void Awake()
    {
        if (MenuBackStack == null)
        {
            Debug.Log("First trigger");
            var playerInput = PlayerInput.GetPlayerByIndex(0);
            if (playerInput)
            {
                playerInput.actions["Back"].performed += _=> TriggerPauseMenu();
                playerInput.actions["Pause"].performed += _=> TriggerPauseMenu();
            }
            //#TODO: Get the travel back, from menu which triggers this first
            
            //Enables/disables all the menus in the beggining
            foreach (var menu in FindObjectsOfType<N_Menu>(true))
            {
                if (menu != this)
                {
                    menu.gameObject.SetActive(menu.StartsActive);
                }
            }
            
            gameObject.SetActive(StartsActive);

        }
    }

    public virtual void OnEnable()
    {
        if (MenuBackStack == null)
        {
            MenuBackStack = new Stack<N_Menu>();
        }
        if (CurrentActiveMenu != null)
        {
            CurrentActiveMenu.gameObject.SetActive(false);
        }

        if (UnlockMouseOnActive)
        {
            OnUnlockMouseOnActive?.Invoke(UnlockMouseOnActive);
        }
        else
        {
            OnLockMouseOnActive?.Invoke(UnlockMouseOnActive);
        }
        CurrentActiveMenu = this;
        CurrentActiveMenu.gameObject.SetActive(true);
        if (MenuBackStack != null && !MenuBackStack.Contains(this))
        {
            MenuBackStack.Push(this);
        }
    }

    private void Start()
    {
        ChangeMenuSelection();
    }

    private void ChangeMenuSelection()
    {
        if (MenuBackStack.Count > 0)
        {
            if (CurrentSelectionType == SelectionType.Controller_Keyboard)
            {
                Cursor.visible = false;
                if (buttonsContents.Count>0)
                {
                    for (int i = 0; i < buttonsContents.Count; i++)
                    {
                        for (int t = 0; t < buttonsContents[i].childCount; t++)
                        {
                            if (buttonsContents[i].GetChild(t).GetComponent<Selectable>())
                            {
                                EventSystem.current.SetSelectedGameObject(buttonsContents[i].GetChild(i).gameObject);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    var buttons = CurrentActiveMenu.GetComponentsInChildren<Selectable>();
                    if (buttons.Length > 0)
                    {
                        //Isto faz com que esteja a dar click no botão
                        EventSystem.current.SetSelectedGameObject(buttons[0].gameObject);
                    }
                }
            
            }
            else
            {
                Cursor.visible = true;
                Cursor.lockState = CursorLockMode.Confined;
                if (EventSystem.current)
                {
                    EventSystem.current.SetSelectedGameObject(null);
                }
                else
                {
                
                }

            }
        }
    }
    
    private void OnDisable()
    {
        if (CurrentActiveMenu == this)
        {
            CurrentActiveMenu = null;
        }
    }

    public static void UpdateMenuSelection(SelectionType selection, bool ForceChange=false)
    {
        if (CurrentActiveMenu == null) return;
        if (selection != CurrentSelectionType && ChangeCurrentCooldown+ ChangeCooldown<=Time.time || ForceChange)
        {
            CurrentSelectionType = selection;
            Debug.Log($"Changed selection to {selection}");
            CurrentActiveMenu.ChangeMenuSelection();
            ChangeCurrentCooldown = Time.time;

        }
    }

    public static void ChangeMenu(N_Menu targetMenu = null)
    {
        if (targetMenu != null)
        {
            if (CurrentActiveMenu)
            {
                CurrentActiveMenu.isActive = false;
                CurrentActiveMenu.gameObject.SetActive(false);
            }
            targetMenu.gameObject.SetActive(true);
            targetMenu.isActive = true;
            targetMenu.ChangeMenuSelection();
        }
        else
        {
            if (CurrentActiveMenu)
            {
                CurrentActiveMenu.isActive = false;
                CurrentActiveMenu.gameObject.SetActive(false);
            }
        }
        OnChangeMenu?.Invoke(targetMenu);
    }

    public static void TravelBack()
    {
        if (CurrentActiveMenu.CanTravelBack)
        {
            if (MenuBackStack.Count > 1)
            {
                MenuBackStack.Pop();
                ChangeMenu(MenuBackStack.Peek());
            }
            else if(MenuBackStack.Count > 0)
            {
                MenuBackStack.Pop();
                ChangeMenu();
            }
        }
    }
    
    public static void TriggerPauseMenu()
    {
        if (CurrentActiveMenu == null)
        {
            if (TargetPauseMenu)
            {
                ChangeMenu(TargetPauseMenu);
            }
        }
        else
        {
            TravelBack();
        }
    }
    
    //Getters
    public static N_Menu GetCurrentActiveMenu()
    {
        return CurrentActiveMenu;
    }
    
    public static N_Menu GetCurrentPauseMenu()
    {
        return TargetPauseMenu;
    }
    
    //Setters
    public static void SetCurrentPauseMenu(N_Menu PMenu)
    {
        TargetPauseMenu = PMenu;
    }
    
}
public enum SelectionType
{
    Mouse,
    Controller_Keyboard
}