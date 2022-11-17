using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace CyberPunkCoding
{
    public class Pawn : Objective
    {
        private static Pawn instance;
        public static Pawn Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<Pawn>();
                return instance;
            }
        }

        [SerializeField] private Animator animator;
        [SerializeField] private LayerMask interactionLayerMask;
        [SerializeField] private float stepLength = 2.75f;
        [SerializeField] private float stepDuration = 1f;
        [SerializeField] private Ease stepEase;
        private Vector3 startPos;
        private Quaternion startRotation;
        
        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }

            instance = this;
            startPos = transform.position;
            startRotation = transform.rotation;
        }

        private IEnumerator followCommandCoroutine;
        private static readonly int WalkingAnim = Animator.StringToHash("Walking");

        public void FollowCommand(Command[] commands, System.Action onCommandsExecuted)
        {
            if (followCommandCoroutine != null)
                return;
            followCommandCoroutine = FollowCommandsCoroutine(commands, onCommandsExecuted);
            StartCoroutine(followCommandCoroutine);
        }

        IEnumerator FollowCommandsCoroutine(Command[] commands, System.Action onCommandsExecuted)
        {
            float animationDuration = stepDuration - 0.15f;
            for (var i = 0; i < commands.Length; i++)
            {
                var command = commands[i];
                switch (command)
                {
                    case Command.Forward:
                        int consecutive = 0;
                        while (i + consecutive < commands.Length)
                        {
                            if (commands[i + consecutive] == Command.Forward)
                            {
                                if (Physics.Linecast(transform.position + transform.forward * consecutive * stepLength,
                                        transform.position + transform.forward * (consecutive + 1) * stepLength,
                                        interactionLayerMask, QueryTriggerInteraction.Ignore))
                                    break;
                                
                                consecutive++;
                                continue;
                            }
                            break;
                        }
                        if (consecutive == 0)
                            break;
                        
                        animator.SetBool(WalkingAnim, true);
                        transform.DOMove(transform.position + transform.forward * stepLength * consecutive, (stepDuration * consecutive) - 0.1f)
                                .SetEase(stepEase)
                                .onComplete +=
                            () => { animator.SetBool(WalkingAnim, false); };
                        i += consecutive - 1;
                        yield return new WaitForSeconds(consecutive - 1 * stepDuration);
                        break;
                    case Command.TurnRight:
                        transform.DORotate(transform.eulerAngles + Vector3.up * 90f, animationDuration)
                            .SetEase(stepEase);
                        break;
                    case Command.TurnLeft:
                        transform.DORotate(transform.eulerAngles - Vector3.up * 90f, animationDuration)
                            .SetEase(stepEase);
                        break;
                    case Command.Hit:
                        Debug.DrawRay(transform.position, transform.forward * stepLength, Color.green, 1f);
                        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, stepLength,
                                interactionLayerMask))
                            if (hit.collider.TryGetComponent(out HitTarget hitTarget))
                                hitTarget.GetHit();
                        break;
                }

                yield return new WaitForSeconds(stepDuration);
            }

            ZoneTarget zoneTarget = FindObjectOfType<ZoneTarget>();
            if (zoneTarget) valid = zoneTarget.BoxCollider.bounds.Contains(transform.position);
            else valid = true;

            onCommandsExecuted?.Invoke();
            followCommandCoroutine = null;
        }

        public override void Reset()
        {
            base.Reset();
            transform.position = startPos;
            transform.rotation = startRotation;
        }
    }
}