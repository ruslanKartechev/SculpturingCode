using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using General;
using General.Data;
public class LevelFinishButton : MonoBehaviour, IClickable
{
    [Header("Button model")]
    [SerializeField] private GameObject btn;
    [Header("Transform settings")]
    [Tooltip("position in camera local coords")]
    [SerializeField] private Vector3 localRotation = new Vector3();
    [SerializeField] private float Zoffset;
    [SerializeField] private Vector2 ScreenOffset = new Vector2();
    [Header("Confetti")]
    [SerializeField] private GameObject confettiPF;
    [SerializeField] private float EventCallDelay = 0.5f;
    [Header("Animation settings")]
    [SerializeField] private Animator btnAnimator;
    [SerializeField] private float AnimatorPlayBackSpeed;
    [SerializeField] private Color pressedColor;
    [SerializeField] private Renderer innerBtn;
    [Space(15)]
    private bool IsDebugMode = true;

    private bool ShowButton = false;

    private Collider mCollider;
    private bool isActive = false;
    private Color defaultColor;
    private Vector3 BtnStartPosition = new Vector3();
    private const string animPressedName = "Pressed";
    private ISoundEffect soundEffect;

    private void Awake()
    {
        if (soundEffect == null)
            soundEffect = GetComponent<ISoundEffect>();
        mCollider = GetComponent<Collider>();
        Hide();
        defaultColor = innerBtn.materials[1].color;
        BtnStartPosition = innerBtn.gameObject.transform.localPosition;
        GameManager.Instance.eventManager.NewStageLoaded.AddListener(OnNewStage);
        GameManager.Instance.eventManager.LevelFinised.AddListener(OnStageFinished);
        GameManager.Instance.eventManager.StageFinished.AddListener(OnStageFinished);
        IsDebugMode = GameManager.Instance.data.MainGameData.IsDebug;

    }
    private void Show()
    {
        mCollider.enabled = true;
        transform.parent = Camera.main.transform.parent;
        Vector3 toolPos = new Vector3(Screen.width * ScreenOffset.x , Screen.height*ScreenOffset.y , Zoffset) ;
        transform.position = Camera.main.ScreenToWorldPoint(toolPos);
        transform.localEulerAngles = localRotation;
        isActive = true;
        btn.SetActive(true);
    }
    private void Hide()
    {
        mCollider.enabled = false;
        btn.SetActive(false);
    }

    private void OnThresholdReached()
    {
        GameManager.Instance.eventManager.ThresholdReached.RemoveListener(OnThresholdReached);
        Show();
    }

    private IEnumerator PressedEffect()
    {
        btnAnimator.speed = AnimatorPlayBackSpeed;
        btnAnimator.Play(animPressedName);
        innerBtn.materials[1].color = pressedColor;
        GameManager.Instance.controlls.StopInput();
        yield return new WaitForSeconds(EventCallDelay);
        GameManager.Instance.eventManager.StageFinished.Invoke();
        GameManager.Instance.controlls.ResumeInput();
    }
    private void OnNewStage(StageData data)
    {
        if (data.stageName == StageByName.Sculpturing || data.stageName == StageByName.Polishing)
        {
            ShowButton = false;
            GameManager.Instance.eventManager.ThresholdReached.AddListener(OnThresholdReached);
        }
        else if (data.stageName == StageByName.Painting || data.stageName == StageByName.Decorating)
            ShowButton = true;
        GameManager.Instance.eventManager.CameraPositionSet.AddListener(OnCameraSet);
    }
    private void OnCameraSet()
    {
        GameManager.Instance.eventManager.CameraPositionSet.RemoveListener(OnCameraSet);
        if (IsDebugMode || ShowButton)
            Show();
    }
    public void OnClick()
    {
        if (isActive)
        {
            isActive = false;
            if(soundEffect != null)
                soundEffect.PlayEffectOnce();
            StartCoroutine(PressedEffect());
        }
    }
    private void OnStageFinished()
    {
        StartCoroutine(RemovingTool());
    }
    private IEnumerator RemovingTool()
    {
        yield return null;
        btnAnimator.Play("Idle");
        yield return null;
        innerBtn.materials[1].color = defaultColor;
        innerBtn.gameObject.transform.localPosition = BtnStartPosition;
        yield return null;
        Hide();
    }
}
