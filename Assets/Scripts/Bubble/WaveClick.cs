using System;
using UnityEngine;

public class WaveClick : MonoBehaviour
{
    public static event Action<WaveClick> OnWaveHide;
    
    [SerializeField] private WavePool wavePool;
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void PlayWave(Vector3 position)
    {
        transform.position = position;
        gameObject.SetActive(true);
        _animator.Play("Wave", 0, 0f);
    }
    
    public void OnWaveAnimationEnd()
    {
        gameObject.SetActive(false);
        OnWaveHide?.Invoke(this);
    }
}