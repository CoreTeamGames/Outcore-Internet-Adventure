using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;

public class ControlSettings : MonoBehaviour
{
    [SerializeField] DevicesManager _manager;
    [SerializeField] Image _gamepadImage;
    SpriteAtlas _spriteAtlas;

    public void Awake()
    {
        _manager.OnChangeControlSchemeEvent += OnChangeControlScheme;
    }

    void OnChangeControlScheme(SpriteAtlas atlas)
    {
        _spriteAtlas = atlas;
        _gamepadImage.sprite = atlas.GetSprite("UI gamepad");
        _gamepadImage.SetNativeSize();
    }

} 