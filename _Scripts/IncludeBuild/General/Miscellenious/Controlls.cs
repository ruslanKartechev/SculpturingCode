using UnityEngine;
namespace General
{
    public class Controlls : MonoBehaviour
    {

        private bool takeInput = false;
        [SerializeField] private LayerMask clickableMask;
        private IClickable currTarget;
        public void Init()
        {
            GameManager.Instance.eventManager.GamePlay.AddListener(ResumeInput);
            Input.simulateMouseWithTouches = true;
            StopInput();
        }

        public void StopInput()
        {
            GameManager.Instance.eventManager.InputStopped.Invoke();
            takeInput = false;
        }
        public void ResumeInput()
        {
            GameManager.Instance.eventManager.InputResumed.Invoke();
            takeInput = true;
        }
        private void Update()
        {


            if (Input.GetMouseButtonDown(0))
            {
                if (CheckClickable() == true)
                    return;
            }

            if (takeInput)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    GameManager.Instance.eventManager.MouseDown.Invoke();
                }
                if (Input.GetMouseButtonUp(0))
                {
                    GameManager.Instance.eventManager.MouseUp.Invoke();
                }
            }
        }

        private bool CheckClickable()
        {
            Ray ray;
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();
            if (Physics.Raycast(ray, out hit, 50f,clickableMask))
            {
                IClickable target = hit.transform.GetComponent<IClickable>();
                if (target != null)
                {
                    target.OnClick();
                    GameManager.Instance.eventManager.ClickableHit.Invoke();
                    return true;
                }
            }
            return false;
        }


        public Vector3 MouseScreenPosition()
        {
            return Input.mousePosition;
        }
    }
}