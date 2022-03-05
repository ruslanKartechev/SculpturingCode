using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using General;
using General.Data;
using PaintIn3D;
namespace Sculpturing.Tools
{

    public class PaintinTool : Tool, ITool
    {
        [Space(10)]
        [SerializeField] private GameObject paletPF;
        [Header("Paint Decal settings")]
        [SerializeField] private LayerMask paintableMask;
        [SerializeField] private float SpherecastRadius = 0.2f;
        [Space(10f)]
        [SerializeField] private Texture texture = null;
        [SerializeField] private Texture shape = null;
        [SerializeField] private float paintRadius = 0.1f;
        [SerializeField] private float paintHardness = 5;
       
        [Space(10)]
        [SerializeField] private Renderer modelRender;
        [Space(10)]
        [SerializeField] private ParticleSystem particels;
        [SerializeField] private float particlesAlpha = 100f;

        private Color currentColor;
        private ColorPalet Palet;
        private P3dPaintDecal paintDecal;
        private bool FirstColorChosen = false;
        public void Init()
        {
            HideTool();
            if(Palet == null)
            {
                Palet = Instantiate(paletPF).GetComponent<ColorPalet>();
                Palet.Init(this);
                InitDecal();
            }
            else
            {
                Palet.gameObject.SetActive(true);
                Palet.Init(this);
            }
                
            particels.Stop();
        }
        private void InitDecal()
        {
            paintDecal = GetComponent<P3dPaintDecal>();
            paintDecal.Texture = texture;
            paintDecal.Shape = shape;
            paintDecal.Hardness = paintHardness;
            paintDecal.Radius = paintRadius;
            paintDecal.Color = Palet.GetRandomColor();
        }
        public void SetColor(Color color)
        {
            if (FirstColorChosen == false)
                SetFirstColor();
            if(color != currentColor)
            {
                paintDecal.Color = color;
                currentColor = color;
                animator.Play("ColorChange", 0, 0);
            }
        }
        private void SubScribe()
        {
            GameManager.Instance.eventManager.MouseDown.AddListener(Working);
            GameManager.Instance.eventManager.MouseUp.AddListener(Idle);
        }
        private void UnSubscribe()
        {
            GameManager.Instance.eventManager.MouseDown.RemoveListener(Working);
            GameManager.Instance.eventManager.MouseUp.RemoveListener(Idle);
        }
        private void SetFirstColor()
        {
            FirstColorChosen = true;
            ShowTool();
            transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
            GameManager.Instance.gameUIController.progressPanel.SwitchHeader(1);
            GameManager.Instance.eventManager.ToolInited.Invoke(this);
            StartCoroutine(CheckTargets());
            SubScribe();
        }

        public void OnColorChangeAnim()
        {
            modelRender.materials[1].SetColor("_BaseColor", currentColor);
            modelRender.materials[1].SetColor("_EmissionColor", currentColor);
        }
        private IEnumerator CheckTargets()
        {
            Ray ray = new Ray() ;
            while (true)
            {
                if (IsActive)
                {
                    Vector3 screenPos = Camera.main.WorldToScreenPoint(actionPoint.position);
                    ray = Camera.main.ScreenPointToRay(screenPos);
                    PaintCast(ray, false);
                    ray.origin = particels.gameObject.transform.position;
                    ray.direction = particels.gameObject.transform.forward;
                    PaintCast(ray, false);
                    yield return null;
                }
                yield return null;
            }
        }

        private bool PaintCast(Ray ray, bool sphereCast = false)
        {
            bool didHit = false;
            RaycastHit[] hit;
            if (sphereCast)
            {
                 hit = Physics.SphereCastAll(ray, SpherecastRadius, 10f, paintableMask);
            }
            else
            {
                hit = Physics.RaycastAll(ray, 10f, paintableMask);
            }
            if(hit.Length > 0)
            {
                didHit = true;
                for(int i =0; i<hit.Length; i++)
                {
                    Paint(hit[i]);
                }
            }
            return didHit;
        }
        private void Paint(RaycastHit hit)
        {
            var preview = !Input.GetKey(KeyCode.Mouse0);
            var priority = 0; // If you're painting multiple times per frame, or using 'live painting', then this can be used to sort the paint draw order. This should normally be set to 0.
            var pressure = 1.0f; // If you're using modifiers that use paint pressure (e.g. from a finger), then you can set it here. This should normally be set to 1.
            var seed = 0; // If this paint uses modifiers that aren't marked as 'Unique', then this seed will be used. This should normally be set to 0.
            var rotation = Quaternion.LookRotation(-hit.normal); // Get the rotation of the paint. This should point TOWARD the surface we want to paint, so we use the inverse normal.

            paintDecal.HandleHitPoint(preview, priority, pressure, seed, hit.point, rotation);
        }


        public override void Working()
        {
            base.Working();
            if(FirstActionDone == false)
            {
                FirstActionDone = true;
                GameManager.Instance.eventManager.FirstToolAction.Invoke();
            }
            var particleMain = particels.main;
            currentColor.a = particlesAlpha;
            particleMain.startColor = currentColor;
            particleMain.loop = true;
            particels.Play();
            soundEffect.StartEffect();
        }
        public override void Idle()
        {
            base.Idle();
            particels.Stop();
            soundEffect.StopEffect();
        }
        public void Disable()
        {
            FirstColorChosen = false;
            UnSubscribe();
            Idle();
            StopAllCoroutines();
            Palet.Disable();
            gameObject.SetActive(false);
        }

    }

}