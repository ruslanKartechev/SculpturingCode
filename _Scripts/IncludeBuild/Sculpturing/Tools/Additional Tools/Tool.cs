
using UnityEngine;
using General;
namespace Sculpturing.Tools
{
    public class Tool : MonoBehaviour
    {

        public GameObject toolRotatable;
        [SerializeField] protected Transform actionPoint;
        [SerializeField] protected Transform handlePoint;
        [Header("Tool mover settings")]
        public bool UseToolMover = true;
        public float SensitivityModificator;
        public float AdditionalOffset;
        [Header("Offset")]
        public bool UseOffset = false;
        public Vector3 OffsetVector = new Vector3();
        [Header("Animations")]
        [SerializeField] protected float WorkingAnimationSpeed = 1.5f;
        [SerializeField] protected string WorkingAnimName = "Working";
        [SerializeField] protected string IdleAnimName = "Idle";
        [SerializeField] protected int mainLayer = 0;
        [SerializeField] protected int addLayer = 1;
        protected Animator animator;
        protected bool IsWorkingState = false;
        protected ToolMover mover;
        protected bool IsActive = false;
        protected bool FirstActionDone = false;
        protected bool WasInited = false;
        protected ISoundEffect soundEffect;
        public Transform HandlePoint
        {
            get { return handlePoint; }
        }
        public void SetMover(ToolMover _mover) => mover = _mover;

        private void Start()
        {
            animator = GetComponent<Animator>();
            if (soundEffect == null)
                soundEffect = GetComponent<ISoundEffect>();
        }

        protected void PlayActiveAnimation()
        {
            if(IsWorkingState == false)
            {
                animator.speed = WorkingAnimationSpeed;
                animator.Play(WorkingAnimName, mainLayer, 0);
                IsWorkingState = true;
            }
        }
        protected void PlayIdleAmimation()
        {
            if(IsWorkingState == true)
            {
                animator.Play(IdleAnimName, mainLayer, 0);
                IsWorkingState = false;
            }
        }
        public virtual void Working()
        {
            if (animator != null)
                PlayActiveAnimation();
            IsActive = true;
        }
        public virtual void Idle()
        {
            if (animator != null)
                PlayIdleAmimation();
            IsActive = false;
        }

        protected void ShowTool()
        {
            toolRotatable.gameObject.SetActive(true);
        }
        protected void HideTool()
        {
            toolRotatable.gameObject.SetActive(false);
        }

    }
}