using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace General.Coursor
{
    public class CoursorController : MonoBehaviour
    {
        public CoursorManager manager;
        public CoursorUI UI;
        public struct CoursorTypeNames
        {
            public const string Default = "Default";
        }
        private void Start()
        {
            if (manager == null)
            {
                manager = FindObjectOfType<CoursorManager>();
            }
            if (manager == null) { Debug.Log("Coursor manager not found"); return; }
            if (UI == null)
            {
                UI = FindObjectOfType<CoursorUI>();
            }
            if (UI == null) { Debug.Log("Coursor UI not found"); return; }
        }
        public void SetCoursor(CoursorTypeNames name)
        {
            manager.SetCoursor(name);
        }
        public void ShowCoursor()
        {
            manager.Show();
        }
        public void HideCoursor()
        {
            manager.Hide();
        }
    }
}