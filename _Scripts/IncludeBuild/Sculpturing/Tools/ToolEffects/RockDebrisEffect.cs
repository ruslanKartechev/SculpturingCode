using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sculpturing.Tools
{
    public class RockRigidbody
    {
        public Rigidbody rb;
        public Vector3 gravity;
        public Vector3 randomForce;

        public RockRigidbody() { }
        public RockRigidbody(Rigidbody _rb, Vector3 _grav, Vector3 _randForce)
        {
            rb = _rb;
            gravity = _grav;
            randomForce = _randForce;
            rb.useGravity = false;
        }
        public void Hide()
        {
            rb.gameObject.SetActive(false);
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
        public void Activate(ForceMode forceMode)
        {
            rb.gameObject.SetActive(true);
            rb.angularVelocity = Random.onUnitSphere * 15;
            rb.AddForce(gravity, ForceMode.Acceleration);
            if (forceMode == ForceMode.Force)
                rb.AddForce(randomForce);
            else if (forceMode == ForceMode.Impulse)
                rb.AddForce(randomForce, ForceMode.Impulse);
        }

    }
    public class RockDebrisEffect : MonoBehaviour
    {
        [SerializeField] private GameObject rocksPF;
        [SerializeField] private float min_count;
        [SerializeField] private float max_count;
        [Header("Local scale boundaries")]
        [SerializeField] private float min_size;
        [SerializeField] private float max_size;
        [Header("How Log before rocks Disappear")]
        [SerializeField] private float lifeTime;
        [Header("Radiuse in which rocks appear around a point")]
        [SerializeField] private float spawningRadius;
        [Header("Downward force range")]
        [SerializeField] private bool useGravity = true;
        [SerializeField] private float gravMultiplyer_min;
        [SerializeField] private float gravMultiplyer_max;
        [Header("Random force Parameters")]
        [SerializeField] private ForceMode forceMode = ForceMode.Force;
        [SerializeField] private float min_force;
        [SerializeField] private float max_force;

        private GameObject rocksParent;
        private List<RockRigidbody> rocksRbs = new List<RockRigidbody>();

        public void PreInst()
        {
            if(rocksParent == null)
                rocksParent = new GameObject("RockParticles");
            for (int i = 0; i < max_count * 4; i++)
            {
                GameObject temp = Instantiate(rocksPF);
                temp.SetActive(false);
                temp.transform.localScale *= Random.Range(min_size, max_size);

                Rigidbody rb = temp.AddComponent<Rigidbody>();
                float grabityMulpriler = 0;
                if (useGravity == true)
                    grabityMulpriler = Random.Range(gravMultiplyer_min, gravMultiplyer_max);
                float force = Random.Range(min_force, max_force);
                RockRigidbody rock = new RockRigidbody(rb, Physics.gravity * grabityMulpriler, Random.onUnitSphere * force);
                rock.Hide();
                rock.rb.gameObject.transform.parent = rocksParent.transform;
                rocksRbs.Add(rock);
                
            }
        }

        public void Spawn(Vector3 sourcePosition)
        {
            int count = (int)Random.Range(min_count, max_count);
            StartCoroutine(EmissionHandler(sourcePosition, count));
        }

        private IEnumerator EmissionHandler(Vector3 sourcePosition, int count)
        {
            List<RockRigidbody> mRocks = ChooseRandomRocks(count);
            if (mRocks == null)
            {
                 yield break;
            }
            for (int i = 0; i < count; i++)
            {

                mRocks[i].rb.gameObject.transform.position = sourcePosition + Random.onUnitSphere * spawningRadius;
                mRocks[i].Activate(forceMode);
                if (i % 3 == 0)
                    yield return null;
            }
            yield return new WaitForSeconds(lifeTime);
            for (int i = 0; i < count; i++)
            {
                mRocks[i].Hide();
            }
        }


        private List<RockRigidbody> ChooseRandomRocks(int count)
        {
            List<RockRigidbody> chosen = new List<RockRigidbody>();
            for (int i = 0; i < rocksRbs.Count; i++)
            {
                if (rocksRbs[i].rb.gameObject.activeInHierarchy == false)
                {
                    chosen.Add(rocksRbs[i]);
                    if (chosen.Count >= count)
                        return chosen;
                }
            }
            if (chosen.Count < count)
                return null;
            return chosen;
        }



        public void Clear()
        {
            Destroy(rocksParent);
        }


    }
}