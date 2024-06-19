using UnityEngine.SceneManagement;
using NaughtyAttributes;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    [Scene]
    [SerializeField] string _sceneForLoad;
    [SerializeField] CanvasGroup _loadingBarCanvasGroup;
    [SerializeField] Slider _loadingBarSlider;
    [SerializeField] float _fadeDuration = 0.5f;
    AsyncOperation _loadOperation;

    public void LoadScene(string sceneForLoad = "")
    {
        if (sceneForLoad != "")
        {
            _sceneForLoad = sceneForLoad;
        }
        _loadingBarCanvasGroup.DOFade(1, _fadeDuration).SetUpdate(true).OnComplete(() =>
        {
            StartCoroutine(LoadAsyncScene());
        });
    }

    IEnumerator LoadAsyncScene()
    {
        _loadOperation = SceneManager.LoadSceneAsync(_sceneForLoad, LoadSceneMode.Single);
        DontDestroyOnLoad(gameObject);
        TimeService.SetTimeAsStandard();
        _loadOperation.allowSceneActivation = false;
        while (!_loadOperation.isDone)
        {
            _loadingBarSlider.value = _loadOperation.progress;
            if (_loadOperation.progress >= 0.9f)
            {
                _loadOperation.allowSceneActivation = true;
            }
            yield return null;
        }
        _loadingBarCanvasGroup.DOFade(0, _fadeDuration).SetUpdate(true).OnComplete(() => { Destroy(gameObject); });
    }
}