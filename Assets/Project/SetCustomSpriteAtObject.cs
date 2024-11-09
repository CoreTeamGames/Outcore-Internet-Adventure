using UnityEngine;

    [RequireComponent(typeof(SpriteRenderer))]
public class SetCustomSpriteAtObject : MonoBehaviour
{
    [SerializeField] string _spriteName = "Custom character1";
    SpriteRenderer _spriteRenderer;
    Texture2D _texture;
    Sprite _sprite;
    string _pathToCustomSpritesFolder;

    public void OnEnable()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
        SetCustomSprite();
    }
    public void SetCustomSprite()
    {
        //_pathToCharactersFolder = $"{Application.dataPath}\\Localization";
        _pathToCustomSpritesFolder = @"C:\Users\fnaf9\Desktop\OUTCORE INTERNET ADVENTURE\Custom characters";
        var bytes = System.IO.File.ReadAllBytes(_pathToCustomSpritesFolder + '\\' + _spriteName + ".png");
        _texture = new Texture2D(1, 1);
        _texture.LoadImage(bytes);
        _texture.filterMode = FilterMode.Point;
        _sprite = Sprite.Create(_texture, new Rect(Vector2.zero, new Vector2(_texture.width, _texture.height)), new Vector2(.5f, 0), 100);      
        _spriteRenderer.sprite = _sprite;
    }
}
