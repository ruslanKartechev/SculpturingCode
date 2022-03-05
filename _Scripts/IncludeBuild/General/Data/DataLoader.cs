using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using General;

namespace General.Data {
    public class DataLoader : MonoBehaviour
    {
        public void StartLoading()
        {
            StartCoroutine(Load());
        }


        private IEnumerator Load()
        {
            Debug.Log("Data Loading");
            yield return new WaitForSeconds(1f);
            Debug.Log("Data loaded");
            GameManager.Instance.eventManager.DataLoaded.Invoke();
        }
    }
}