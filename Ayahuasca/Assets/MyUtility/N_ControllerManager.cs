using System;
using QFSW.QC;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Utility
{
    public class N_ControllerManager : MonoBehaviour
    {
        public static N_ControllerManager Instance;
        public static PlayerInput playerInput;
        
        public string PC_ControlSchemeName = "PC";
        public string Controller_ControlSchemeName = "Controller";

        public string UI_ActionMapName = "UI";
        public string Game_ActionMapName = "Game";
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(this.gameObject);
            }
            Instance = this;
            playerInput = PlayerInput.GetPlayerByIndex(0);
            //N_Menu.UpdateMenuSelection(SelectionType.Mouse,true);
        }

        public void ChangeActionMap(string controlScheme)
        {
            playerInput.currentActionMap.Disable();
            playerInput.SwitchCurrentActionMap(controlScheme);
            playerInput.currentActionMap.Enable();
        }

        public void OnUseMovement(InputAction.CallbackContext context)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (QuantumConsole.Instance.IsActive) return;
#endif
            if (playerInput.currentControlScheme == PC_ControlSchemeName ||
                playerInput.currentControlScheme == Controller_ControlSchemeName)
            {
                N_Menu.UpdateMenuSelection(SelectionType.Controller_Keyboard);
            }
        }

        public void OnUseCursor(InputAction.CallbackContext context)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (QuantumConsole.Instance.IsActive) return;
#endif
            if (playerInput.currentControlScheme == PC_ControlSchemeName)
            {
                N_Menu.UpdateMenuSelection(SelectionType.Mouse);
            }
        }
    }
}