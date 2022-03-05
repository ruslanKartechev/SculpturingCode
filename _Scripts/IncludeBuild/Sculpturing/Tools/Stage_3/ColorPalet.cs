using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using General;
namespace Sculpturing.Tools
{
    public class ColorPalet : MonoBehaviour
    {
        
        [SerializeField] private List<ColorCan> cans = new List<ColorCan>();
        [Header("Transform Settings")]
        [SerializeField] private float Zoffset;
        [SerializeField] private Vector2 ScreenOffset = new Vector2();
        [Header("Scaling the cans")]
        public float CansOriginalScale = 10;
        public float CansEnlargedScale = 12;
        public float TimeToScale = 0.35f;

        private List<Transform> canObjs = new List<Transform>();
        private PaintinTool tool;
        private ColorCan currentCan = null;
        private List<Color> colors = new List<Color>();

        private ISoundEffect soundEffect;

        public void Init(PaintinTool _tool)
        {
            if (soundEffect == null)
                soundEffect = GetComponent<ISoundEffect>();
            SetPosition();
            tool = _tool;
            if (GameManager.Instance.data.MainGameData.toolsData.defaultPaletColors.Count < cans.Count)
                Debug.Log("Too few colors provided for the color palet");
            colors = GameManager.Instance.data.MainGameData.toolsData.defaultPaletColors;
            foreach(ColorCan can in cans)
            {
                canObjs.Add(can.transform);
            }
            for (int i =0; i < cans.Count;i++)
            {
                cans[i].Init(tool,this);
                if (colors[i] != null)
                    cans[i].SetColor(colors[i]);
                else
                    cans[i].SetColor(Color.black);
            }
        }
        private void SetPosition()
        {
            transform.parent = Camera.main.transform.parent;
            Vector3 toolPos = new Vector3(Screen.width * ScreenOffset.x, Screen.height * ScreenOffset.y, Zoffset);
            transform.position = Camera.main.ScreenToWorldPoint(toolPos);
            transform.rotation = Quaternion.LookRotation(transform.position - Camera.main.transform.position);
            transform.eulerAngles = new Vector3(GameManager.Instance.data.MainGameData.toolsData.ColorPaletXangle,
                    transform.eulerAngles.y, transform.eulerAngles.z);
        }
        public void OnColorChosen(ColorCan can)
        {
            if(currentCan != null)
            {
                StartCoroutine(Helpers.ChangeScale(currentCan.transform, CansOriginalScale, TimeToScale));
            }
            StartCoroutine(Helpers.ChangeScale(can.transform, CansEnlargedScale, TimeToScale));
            currentCan = can;
            soundEffect.PlayEffectOnce();
        }
        public Color GetRandomColor()
        {
            int rand = (int)Random.Range(0, colors.Count);
            return colors[rand];
        }    
        public void Disable()
        {
            foreach(ColorCan can in cans)
            {
                can.transform.localScale = Vector3.one * CansOriginalScale;
            }
            gameObject.SetActive(false);
        }
    }

}