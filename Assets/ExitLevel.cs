using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitLevel : MonoBehaviour
{
    [SerializeField] ParticleSystem _particleSystem;
    [SerializeField] GameObject _image;
    [SerializeField] float _timeAfterGameQuit;
    void Start()
    {
        
        StartCoroutine(ExitGame());
    }

   private IEnumerator ExitGame()
    {
        _particleSystem.Play();
        _image.SetActive(false);
        yield return new WaitForSeconds(_timeAfterGameQuit);
        Debug.Log("Quitting");
        Application.Quit();
    }
}
