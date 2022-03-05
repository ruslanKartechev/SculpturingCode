using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace General.Coursor
{
    public class CoursorUI : MonoBehaviour
    {
        [SerializeField] private Transform mainCoursor;
        [SerializeField] private List<Image> CoursorImages;
        [SerializeField] private string CoursorTag;
        private CoursorManager manager;
        public void Init(CoursorManager _manager)
        {
            manager = _manager;
        }

        public void SetCoursorImage(List<Image> _images)
        {

        }
        public void ChangeScale(Vector3 scaleVector)
        {
            mainCoursor.localScale = scaleVector;
        }
        public void ChangeScale(float scaleScalar)
        {
            mainCoursor.localScale = Vector3.one * scaleScalar;
        }
        public void SetPosition(Vector3 position)
        {
            mainCoursor.position = position;
        }
        public Vector3 GetPosition()
        {
            return mainCoursor.position;
        }


    }
}
