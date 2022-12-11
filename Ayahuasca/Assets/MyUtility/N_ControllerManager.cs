using System;
using QFSW.QC;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Utility
{
    public class N_ControllerManager : MonoBehaviour
    {
        public static PlayerInput playerInput;
        private void Awake()
        {
            playerInput = PlayerInput.GetPlayerByIndex(0);
            //N_Menu.UpdateMenuSelection(SelectionType.Mouse,true);
        }
        
        public void OnUseMovement(InputAction.CallbackContext context)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (QuantumConsole.Instance.IsActive) return;
#endif
            N_Menu.UpdateMenuSelection(SelectionType.Controller_Keyboard);
        }

        public void OnUseCursor(InputAction.CallbackContext context)
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            if (QuantumConsole.Instance.IsActive) return;
#endif
            if (playerInput.currentControlScheme == "PC")
            {
                N_Menu.UpdateMenuSelection(SelectionType.Mouse);
            }
        }
    }
}