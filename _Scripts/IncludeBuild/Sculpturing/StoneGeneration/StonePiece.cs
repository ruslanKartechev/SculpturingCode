using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RayFire;
using General.Data;
using General;
using UnityEngine.Events;
namespace Sculpturing
{
    [DisallowMultipleComponent]
    public class StonePiece : MonoBehaviour, IToolTarget
    {
        private Collider mCollider;
        private Renderer mRenderer;

        private int hitsToCrack;
        private int hitsToBreak;
        private int hitsTaken;

        public bool IsCracked { get; private set; }
        public bool IsBroken { get; private set; }
        public Mesh myMesh;

        private int scoreOnDestroy;
        private Rigidbody rigidBody;

        private float timeToFall = 1;
        private float ColliderRadius;

        private StonePieceLoader loader;
        public StonePieceData data = new StonePieceData();
        
        [SerializeField]private LayerMask mask;       
        
        public Transform PiecesRoot { get { return loader.PiecesRoot; } }
        public List<StonePiece> ChildPieces { get { return loader.ChildPieces; } }

        public void SetData(int tier, string meshPath)
        {
            data.Tier = tier;
            data.MeshPath = meshPath; 
            data.localPos = transform.localPosition;
            data.localEulers = transform.localEulerAngles;
            data.localScale = transform.localScale.x;
        }
        public void Init()
        {
            if (loader == null)
                loader = GetComponent<StonePieceLoader>();
            if(mRenderer == null)
                mRenderer = GetComponent<Renderer>();
            if(data.Tier != 0)
            {
                myMesh = (Mesh)Resources.Load(data.MeshPath);
                transform.localPosition = data.localPos;
                transform.localEulerAngles = data.localEulers;
                transform.localScale = Vector3.one * data.localScale;
            }

            if (data.Tier != 2)
            {
                hitsToBreak = GameManager.Instance.data.StonesData.stoneBreakingData.Find(x => x.Tier == data.Tier).HitsToBreak;
                hitsToCrack = GameManager.Instance.data.StonesData.stoneBreakingData.Find(x => x.Tier == data.Tier).HitsToCrack;
            }
            if (data.Tier == 0)
            {
                scoreOnDestroy = GameManager.Instance.data.ScoreData.scoresData.BigStoneBrokenScore;
                mCollider = GetComponent<Collider>();
            }
            if (data.Tier == 1)
            {
                scoreOnDestroy = GameManager.Instance.data.ScoreData.scoresData.StonePieceBrokenScore;
                ColliderRadius = GetComponent<SphereCollider>().radius;
                GameManager.Instance.eventManager.StoneBroken.AddListener(OnStoneBroken);
            }
        }
        public void LoadChildren(UnityAction action, bool silent)
        {
            if (silent)
                loader.LoadSilent(action);
            else
                loader.LoadImmitdiate();
        }
        public void InitAsMainStone()
        {
            mCollider = GetComponent<BoxCollider>();
            if(mCollider == null)
                mCollider = gameObject.AddComponent<BoxCollider>();
            hitsTaken = 0;
        }
        public void InitTier1()
        {
            IsCracked = false;
            IsBroken = false;
            //
            hitsTaken = 0;
            //
            mCollider = gameObject.AddComponent<SphereCollider>();
            mCollider.isTrigger = false;
            gameObject.layer = 8;
            mask = LayerMask.GetMask("StonePieces");
            // rigidBody init
            rigidBody = gameObject.AddComponent<Rigidbody>();
            rigidBody.useGravity = false;
            rigidBody.freezeRotation = true;
            rigidBody.constraints = RigidbodyConstraints.FreezeAll;
        }
        public void InitTier2()
        {
            rigidBody = GetComponent<Rigidbody>();
            if(rigidBody == null)
                rigidBody = gameObject.AddComponent<Rigidbody>();
            rigidBody.useGravity = true;
            rigidBody.isKinematic = true;
        }
        private void OnStoneBroken()
        {
            bool isInContact = false;
            if (RaycastHitCheck(Vector3.up) == true)
            {
                isInContact = true;
            }else if(RaycastHitCheck(-Vector3.up) == true)
            {
                isInContact = true;
            } else if(RaycastHitCheck(Vector3.right) == true)
            {
                isInContact = true;
            }
            else if(RaycastHitCheck(-Vector3.right) == true)
            {
                isInContact = true;
            } else if(RaycastHitCheck(Vector3.forward) == true)
            {
                isInContact = true;
            }
            else if(RaycastHitCheck(-Vector3.forward) == true)
            {
                isInContact = true;
            }
            if(isInContact == false)
            {
                Crack();
                Break_1();
            }
        }
        private bool RaycastHitCheck(Vector3 direction)
        {
            bool didHit = false;
            if(Physics.Raycast(transform.position, direction, ColliderRadius*1.1f, mask)) // mask or ~mask 
            {
                didHit = true;
            }
            return didHit;
        }
        public void TakeHit()
        {
            hitsTaken++;
            if (data.Tier == 0)
            {
                if(hitsTaken >= hitsToCrack)
                    Break_0();
                return;
            }
            if (IsCracked && hitsTaken >= hitsToBreak) 
            {
                Break_1();
                return;
            }
            if (hitsTaken >= hitsToCrack)
            {
                hitsTaken = 0;
                Crack();
                return;
            }
        }
        private void Crack()
        {
            if(mRenderer == null)
            {
                mRenderer = GetComponent<Renderer>();
            }
            mRenderer.enabled = false;
            if(loader.PiecesRoot.gameObject != null)
                loader.PiecesRoot.gameObject.SetActive(true);
            IsCracked = true;
        }
        private IEnumerator Break_2()
        {

            if (rigidBody == null)
                rigidBody = GetComponent<Rigidbody>();
            rigidBody.isKinematic = false;
            Vector3 force = Random.insideUnitSphere * Random.Range(1,5);
            rigidBody.AddRelativeForce(force, ForceMode.Impulse);
            transform.localScale *= 0.75f;
            yield return new WaitForSeconds(timeToFall);
            gameObject.SetActive(false);
        }
        private void Break_1()
        {
            if (mCollider == null)
                mCollider = GetComponent<Collider>();
            mCollider.enabled = false;
            foreach (StonePiece t2 in ChildPieces)
            {
                StartCoroutine(t2.Break_2());
            }
            GameManager.Instance.eventManager.ScoreAdded.Invoke(scoreOnDestroy);
            GameManager.Instance.eventManager.StoneBroken.RemoveListener(OnStoneBroken);
            GameManager.Instance.eventManager.StoneBroken.Invoke();
        }
        private void Break_0() 
        {
            GameManager.Instance.eventManager.ScoreAdded.Invoke(scoreOnDestroy);
            loader.PiecesRoot.gameObject.SetActive(true);
            mRenderer.enabled = false;
            mCollider.enabled = false;
            //gameObject.SetActive(false);
        }
        public void ClearComponent()
        {
            if(mCollider != null)
            DestroyImmediate(mCollider);
            Collider col = GetComponent<Collider>();
            if (col != null)
                DestroyImmediate(col);
            if (rigidBody != null)
            DestroyImmediate(rigidBody);
            DestroyImmediate(this);
        }
       

    }
}