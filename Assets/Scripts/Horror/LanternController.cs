using UnityEngine;
using UnityEngine.UI;

public class LanternController : MonoBehaviour
{
    [Header("Light Settings")]
    [SerializeField] private Light lanternLight;
    [SerializeField] private float offRange = 5f;
    [SerializeField] private float onRange = 10f;
    [SerializeField] private float offIntensity = 15f;
    [SerializeField] private float onIntensity = 20f;

    [Header("Light Duration Settings")]
    [SerializeField] private float minLightTime = 15f;
    [SerializeField] private float maxLightTime = 20f;
    [SerializeField] private float timeBeforeCanExtinguish = 5f;

    [Header("Hold Settings")]
    [SerializeField] private float holdTime = 3f;
    private float holdTimer = 0f;

    [Header("UI")]
    [SerializeField] private Image holdProgressImage;
    [SerializeField] private GameObject buttonE;
    [SerializeField] private SingleCharacterSpawner characterSpawner;
    [SerializeField] private AudioSource enableSound;
    [SerializeField] private AudioSource disableSound;
    [SerializeField] private AudioSource brokeSound;

    public bool isLit = false;
    private float lightTimer = 0f;
    private float time = 0f;
    private bool isFirstEnabled = false;
    private Transform playerCamera;

    void Start()
    {
        playerCamera = Camera.main.transform;
    }

    void Update()
    {
        HandleHoldInput();
        if (isLit) UpdateLightTimers();
    }

    void HandleHoldInput()
    {
        if (Input.GetKey(KeyCode.E))
        {
            holdTimer += Time.deltaTime;
            if (holdProgressImage != null) holdProgressImage.fillAmount = holdTimer / holdTime;
            if (holdTimer >= holdTime && !isLit)
            {
                if (!isFirstEnabled)
                {
                    characterSpawner.Spawn();
                    isFirstEnabled = true;
                }
                TurnOnLantern();
            }
        }
        else
        {
            holdTimer = 0f;
            if (holdProgressImage != null) holdProgressImage.fillAmount = 0f;
        }
    }

    void TurnOnLantern()
    {
        isLit = true;
        lightTimer = Random.Range(minLightTime, maxLightTime);
        time = lightTimer;
        if (lanternLight != null)
        {
            lanternLight.range = onRange;
            lanternLight.intensity = onIntensity;
        }
        holdProgressImage.fillAmount = 0f;
        enableSound.Play();
        buttonE.SetActive(false);
    }

    void TurnOffLantern()
    {
        isLit = false;
        if (lanternLight != null)
        {
            lanternLight.range = offRange;
            lanternLight.intensity = offIntensity;
        }
    }

    void UpdateLightTimers()
    {
        lightTimer -= Time.deltaTime;
        if (lightTimer <= 0f)
        {
            disableSound.Play();
            TurnOffLantern();
        }
        else if (time - lightTimer >= timeBeforeCanExtinguish && IsLookingAtLantern())
        {
            brokeSound.Play();
            TurnOffLantern();
        }
    }

    bool IsLookingAtLantern()
    {
        if (playerCamera == null) return false;
        Vector3 toLantern = (transform.position - playerCamera.position).normalized;
        float angle = Vector3.Angle(playerCamera.forward, toLantern);
        return angle >= 120f;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !isLit) buttonE.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) buttonE.SetActive(false);
    }
}