using System;
using UnityEngine;

public class BubbleAnimations : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private Animator animator;
    [SerializeField] private WavePool wavePool;

    public void PlayWaveAnimation(Vector2 position)
    {
        WaveClick wave = wavePool.GetWave();
        wave.PlayWave(position);
    }
}
