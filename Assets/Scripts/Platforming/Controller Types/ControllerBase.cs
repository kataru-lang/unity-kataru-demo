using System;
using System.Collections;
using JnA.Core;
using NaughtyAttributes;
using UnityEngine;

namespace JnA.Platformer
{
    /// <summary>
    /// Controller base class
    /// </summary>
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(Collider2D))]
    public class ControllerBase : MonoBehaviour
    {
        #region PER GAMEOBJECT FIELDS
        [Expandable] [SerializeField] protected ControllerDataBase data;
        [SerializeField] protected ParticleSystem attackParticles;

        [Header("Cosmetics")]
        public Animator animator;

        [Header("Physics")]
        [SerializeField] protected Rigidbody2D rbody;

        [SerializeField] protected PhysicsMaterial2D noFriction, infFriction;

        #endregion

        #region MECHANICS FIELDS
        public bool moving = false;
        protected bool running = false;
        public Vector2 axis = Vector2.zero;

        private Vector3 origScale;
        #endregion

        #region ANIMATION FIELDS
        /// <summary>
        /// Animator param for which idle state
        /// 0: front
        /// 1: back
        /// 2: side
        /// </summary>
        public static readonly int WALK_BOOL = Animator.StringToHash("walk"),
            FLIP_TRIGGER = Animator.StringToHash("flip"),
             SPEED_FLOAT = Animator.StringToHash("speed"),
             ATTACK_TRIGGER = Animator.StringToHash("attack"),
             HURT_TRIGGER = Animator.StringToHash("hurt"),
             DIE_BOOL = Animator.StringToHash("die");

        #endregion

        private void OnValidate()
        {
            animator = GetComponent<Animator>();
            rbody = GetComponent<Rigidbody2D>();
        }

        protected virtual void Start()
        {
            if (!moving)
                rbody.sharedMaterial = infFriction;
            origScale = transform.localScale;
        }

        protected virtual void FixedUpdate()
        {
            if (moving)
                Move();
        }

        protected virtual void Move()
        {
            if (IsMaxVelocity()) return;
            float mult = GetMoveMultiplier();
            Vector2 force = new Vector2(GetMoveSpeedX(mult), GetMoveSpeedY(mult));
            rbody.AddForce(force);
        }

        protected virtual bool IsMaxVelocity() => Mathf.Abs(rbody.velocity.x) >= (running ? data.maxRunVelocity : data.maxWalkVelocity);

        protected virtual float GetMoveMultiplier() => (running ? data.runForce : data.walkForce) * rbody.mass;

        protected virtual float GetMoveSpeedY(float mult) => mult * axis.y;

        protected virtual float GetMoveSpeedX(float mult) => mult * axis.x;

        public void StartMoveTo(Vector3 target)
        {
            StartMove(GetTargetAxis(target));
        }

        protected virtual Vector2 GetTargetAxis(Vector3 target)
        {
            return (target - transform.position).normalized;
        }

        public void UpdateTargetAxis(Vector3 target)
        {
            this.axis = GetTargetAxis(target);
        }

        public virtual void StartMove(Vector2 axis)
        {
            rbody.sharedMaterial = noFriction;
            this.axis = axis;
            moving = true;
            animator.SetBool(WALK_BOOL, true);

            // Handle flipping logic
            if (shouldFlip())
            {
                TryFlip();
            }
        }


        public virtual void EndMove()
        {
            this.axis = Vector2.zero;
            rbody.sharedMaterial = infFriction;
            moving = false;
            animator.SetBool(WALK_BOOL, false);
        }

        public void SetRun(bool running)
        {
            this.running = running;
            animator.SetFloat(SPEED_FLOAT, running ? 1f : 0);
        }

        public void ForceFlip()
        {
            Vector2 axis = this.axis;
            axis.x = transform.localScale.x < 0 ? 1 : -1;
            this.axis = axis;
            TryFlip();
        }

        public virtual void TryFlip() => animator.SetTrigger(FLIP_TRIGGER);

        // called during flip animation via animation event
        protected void Flip()
        {
            float axisX = this.axis.x < 0 ? -1 : 1;
            Vector3 scale = origScale;
            scale.x = Mathf.Abs(scale.x) * axisX;
            transform.localScale = scale;
        }

        protected virtual bool shouldFlip()
        {
            return axis.x < 0 != transform.localScale.x < 0;
        }
    }
}
