using UnityEngine;
using UnityEngine.InputSystem;

namespace PlayerBehaviours
{
    [CreateAssetMenu(fileName = "NewPlayerBind", menuName = "Ayahuasca/Player/PlayerBinds", order = 0)]
    public class PlayerBindKeys : ScriptableObject
    {
        public InputActionReference MoveAction;
        public InputActionReference InteractAction;
        public InputActionReference MainAction;
        public InputActionReference JumpAction;
    }
}