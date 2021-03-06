using Cinemachine;
using JnA.Core.Interaction;
using JnA.Core.ScriptableObjects;
using JnA.Utils;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;
namespace JnA.Platformer
{
    /// <summary>
    /// Player class. 
    /// Hook player input into platforming mechanic,
    /// Make sure player is the target of the active Cinemachine
    /// </summary>
    public class PlayerBase : MonoBehaviour
    {
        [SerializeField] protected ControllerBase controller;

        [SerializeField] InputActionAsset input;

        [SerializeField] PlayerEvent playerEvent;

        Interactable interact;

        InputAction moveAction, interactAction, runAction;

        protected virtual void Awake()
        {
            InputActionMap playerMap = input.FindActionMap(Constants.PLAYER_MAP, true);
            GetActions(playerMap);
        }

        protected virtual void GetActions(InputActionMap playerMap)
        {
            moveAction = playerMap.FindAction("Move", true);
            runAction = playerMap.FindAction("Run", true);
            interactAction = playerMap.FindAction(Constants.INTERACT_ACTION, true);
        }

        protected virtual void OnEnable()
        {
            playerEvent.SetInteract += SetInteract;
            playerEvent.TryExitInteract += TryExitInteract;

            // Listen for input
            AddListeners();
        }

        protected virtual void AddListeners()
        {
            moveAction.performed += Move;
            moveAction.canceled += EndMove;

            runAction.performed += StartRun;
            runAction.canceled += EndRun;

            interactAction.performed += Interact;
        }

        protected virtual void OnDisable()
        {
            playerEvent.SetInteract -= SetInteract;
            playerEvent.TryExitInteract -= TryExitInteract;

            moveAction.performed -= Move;
            moveAction.canceled -= EndMove;

            runAction.performed -= StartRun;
            runAction.canceled -= EndRun;

            interactAction.performed -= Interact;
        }

        protected virtual void Start()
        {
            // Become target for cinemachine
            FindObjectOfType<JnA.Core.MainVCam>().Follow(transform);
        }

        Vector3 GetPlayerPosition() => transform.position;

        void SetPlayerPosition(Vector3 p) => transform.position = p;

        void TriggerAnimation(string trigger) => controller.animator.SetTrigger(trigger);

        void SetInteract(Interactable interact)
        {
            if (this.interact != null)
                TryExitInteract(this.interact);
            this.interact = interact;
            this.interact.Show();
        }

        void TryExitInteract(Interactable interact)
        {
            if (this.interact == interact)
            {
                this.interact.Hide();
                this.interact = null;
            }
        }

        private void Move(InputAction.CallbackContext ctx)
        {
            Vector2 _axis = ctx.ReadValue<Vector2>();

            if (ShouldMove(_axis))
                controller.StartMove(_axis);
        }


        /// <summary>
        /// Determines if the given axis input should cause the controller to call StartMove
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        protected virtual bool ShouldMove(Vector2 axis) => axis.x != 0 || axis.y != 0;

        protected virtual void EndMove(InputAction.CallbackContext ctx)
        {
            controller.EndMove();
        }

        private void StartRun(InputAction.CallbackContext ctx)
        {
            controller.SetRun(true);
        }

        private void EndRun(InputAction.CallbackContext ctx)
        {
            controller.SetRun(false);
        }

        private void Interact(InputAction.CallbackContext context)
        {
            if (interact)
            {
                interact.Interact();
            }
        }
    }
}