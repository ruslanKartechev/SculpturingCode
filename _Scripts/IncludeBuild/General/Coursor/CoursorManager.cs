using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace General.Coursor
{
    public class CoursorManager : MonoBehaviour
    {
        private CoursorUI coursorUI;
        private Coroutine currentBehaviour;
        public void Start()
        {

        }
        public void Init(CoursorUI UI)
        {
            coursorUI = UI;
        }
        public virtual void Show()
        {
            
        }
        public virtual void Hide()
        {

        }
        public virtual void SetCoursor(CoursorController.CoursorTypeNames name)
        {

        }

        private IEnumerator FollowingRoutine()
        {
            yield return null; 
        }
    }
}