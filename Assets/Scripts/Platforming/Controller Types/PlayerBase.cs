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

        [Header("Player-specific")]

        [InfoBox("Can we see all 3 sides?", EInfoBoxType.Normal)]
        [SerializeField] protected bool canTurn = true;

        [SerializeField] InputActionAsset input;

        [SerializeField] PlayerEvent playerEvent;

        Interactable interact;

        InputAction moveAction, interactAction, attackAction, runAction;

        protected virtual void Awake()
        {
            InputActionMap playerMap = input.FindActionMap(Constants.PLAYER_MAP, true);
            GetActions(playerMap);
        }

        protected virtual void GetActions(InputActionMap playerMap)
        {
            moveAction = playerMap.FindAction("Move", true);
            runAction = playerMap.FindAction("Run", true);
            attackAction = playerMap.FindAction("Attack", true);
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

        RuntimeAnimatorController GetAnimatorController() => controller.animator.runtimeAnimatorController;

        void SetAnimatorController(RuntimeAnimatorController a) => controller.animator.runtimeAnimatorController = a;

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

            MoveY(_axis.y);

            if (ShouldMove(_axis))
                controller.StartMove(_axis);
        }


        /// <summary>
        /// Determines if the given axis input should cause the controller to call StartMove
        /// </summary>
        /// <param name="axis"></param>
        /// <returns></returns>
        protected virtual bool ShouldMove(Vector2 axis) => true;

        /// <summary>
        /// Called when moving in Y direction.
        /// </summary>
        /// <param name="y"></param>
        protected virtual void MoveY(float y) { }

        protected virtual void EndMove(InputAction.CallbackContext ctx)
        {
            // tell controller we've stopped moving
            controller.axis.y = 0;
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