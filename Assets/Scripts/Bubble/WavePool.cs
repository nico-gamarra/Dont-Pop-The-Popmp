using System;
using System.Collections.Generic;
using UnityEngine;

public class WavePool : MonoBehaviour
{
    [SerializeField] private WaveClick wavePrefab;
    [SerializeField] private int initialSize;

    private readonly Queue<WaveClick> _pool = new();

    private void OnEnable()
    {
        WaveClick.OnWaveHide += ReturnWave;
    }
    
    private void OnDisable()
    {
        WaveClick.OnWaveHide -= ReturnWave;
    }

    private void Awake()
    {
        for (int i = 0; i < initialSize; i++)
        {
            CreateNewWave();
        }
    }

    private WaveClick CreateNewWave()
    {
        WaveClick wave = Instantiate(wavePrefab, transform);
        wave.gameObject.SetActive(false);
        _pool.Enqueue(wave);
        return wave;
    }

    public WaveClick GetWave()
    {
        if (_pool.Count > 0)
            return _pool.Dequeue();

        return CreateNewWave();
    }

    private void ReturnWave(WaveClick wave)
    {
        wave.gameObject.SetActive(false);
        _pool.Enqueue(wave);
    }
}