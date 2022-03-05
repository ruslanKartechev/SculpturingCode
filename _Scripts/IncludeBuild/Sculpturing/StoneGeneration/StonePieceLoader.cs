using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using General.Data;
using UnityEngine.Events;
using General;
namespace Sculpturing
{
    [DisallowMultipleComponent]
    public class StonePieceLoader : MonoBehaviour
    {
        public StonePieceLoadData data;
        public Transform PiecesRoot { get; private set; }
        public List<Transform> Children { get; private set; }
        public List<StonePiece> ChildPieces { get; private set; }
        public UnityEvent childernLoaded;
        public void AddChildPath(string path)
        {
            if (data == null)
                data = new StonePieceLoadData();
            data.piecesPaths.Add(path);
        }
        public void InitEmpty()
        {
            data = null;
        }
        public void LoadImmitdiate()
        {
            InitLoad();
            PiecesRoot.gameObject.SetActive(true);
            for (int i = 0; i < data.piecesPaths.Count; i++)
            {
                GameObject temp = (GameObject)Resources.Load(data.piecesPaths[i]);
                temp = Instantiate(temp, PiecesRoot);
                GameManager.Instance.toolsManager.materialsManager.SetMaterialAndColor(temp, MaterialTypes.StonePieces);
                Children.Add(temp.transform);
                StonePiece piece = temp.GetComponent<StonePiece>();
                if (piece == null)
                    Debug.Log("stone piece component not found in loaded piece");
                else
                {
                    Children.Add(piece.transform);
                    ChildPieces.Add(piece);
                    piece.Init(); // ??? not needed
                }
            }
        }
        public void LoadSilent(UnityAction action)
        {
            InitLoad();
            childernLoaded.AddListener(action);
            StartCoroutine(LoadingRoutine());
        }
        private void InitLoad()
        {
            if (data == null) { Debug.Log("Loading data is nulll"); return; }
            if (PiecesRoot != null)
                DestroyImmediate(PiecesRoot.gameObject);
            PiecesRoot = new GameObject(gameObject.name + " Root").transform;
            PiecesRoot.parent = transform.parent;
            PiecesRoot.position = transform.position;
            PiecesRoot.rotation = transform.rotation;
            PiecesRoot.localScale = Vector3.one;
            Children = new List<Transform>();
            ChildPieces = new List<StonePiece>();
            childernLoaded = new UnityEvent();
        }
        public IEnumerator LoadingRoutine()
        {
            PiecesRoot.gameObject.SetActive(true);
            for (int i = 0; i < data.piecesPaths.Count; i++)
            {
                GameObject temp = (GameObject)Resources.Load(data.piecesPaths[i]);
                temp = Instantiate(temp, PiecesRoot);
                GameManager.Instance.toolsManager.materialsManager.SetMaterialAndColor(temp, MaterialTypes.StonePieces);
                Children.Add(temp.transform);
                StonePiece piece = temp.GetComponent<StonePiece>();
                if (piece == null)
                    Debug.Log("stone piece component not found in loaded piece");
                else
                {
                    Children.Add(piece.transform);
                    ChildPieces.Add(piece);
                    piece.Init(); // ??? not needed
                }
                yield return null;
            }
            yield return null;
            childernLoaded.Invoke();
        }



    }
}