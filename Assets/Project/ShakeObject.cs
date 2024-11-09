using UnityEngine;
using NaughtyAttributes;
using System.Collections;

public class ShakeObject : MonoBehaviour
{
    [SerializeField] Vector2 _minimalShakeVector;
    [SerializeField] Vector2 _maximalShakeVector;
    [SerializeField] bool _enableLerp = false;
    [SerializeField] float _delay;
    [Range(0, 1)] [ShowIf("_enableLerp")]
    [SerializeField] float _scaling;

    void Start()
    {
        StartCoroutine(Shake());
    }

    IEnumerator Shake()
    {
        float _scale = 1;
        if (_enableLerp)
            _scale = _scaling;

        float _x = Random.Range(_minimalShakeVector.x, _maximalShakeVector.x);
        float _y = Random.Range(_minimalShakeVector.y, _maximalShakeVector.y);
        transform.localPosition = Vector2.Lerp(transform.position, new Vector2(_x, _y), _scale);
        yield return new WaitForSeconds(_delay);
        StartCoroutine(Shake());
    }
}
