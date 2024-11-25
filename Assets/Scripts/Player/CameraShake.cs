using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachineCamera;
    private CinemachineBasicMultiChannelPerlin noise;
    private float shakeIntensity = 4f;
    private float shakeTime = 0.5f;

    private void Awake()
    {
        cinemachineCamera = GetComponent<CinemachineVirtualCamera>();
        noise = cinemachineCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShakeCamera()
    {
        StartCoroutine(StartCameraShake());
    }
    private IEnumerator StartCameraShake()
    {
        float timer = 0f;
        noise.m_AmplitudeGain = shakeIntensity;

        while (timer < shakeTime)
        {
            timer += Time.deltaTime;
            yield return null;
        }

        noise.m_AmplitudeGain = 0f;
    }
}
