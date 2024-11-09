using UnityEngine;


public class CursorDragger : MonoBehaviour
{

    public Vector2 offset;
    public bool rotateOnDrag = true;
    public bool isDrag = false;
    public Rigidbody2D rigidbody2D;
    public Camera camera;
    public SpriteRenderer renderer;

        public void OnMouseDown()
        {
            rigidbody2D.freezeRotation = !rotateOnDrag;
        
        }

        public void OnMouseDrag()
        {
        Vector2 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
        rigidbody2D.velocity = Vector2.zero;
        rigidbody2D.MovePosition(new Vector2(mousePos.x + offset.x, mousePos.y + offset.y));
        }

    public void OnMouseUp()
    {
        Vector2 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
        rigidbody2D.velocity = (( mousePos + offset) - rigidbody2D.position) * 2;
        renderer.flipX = rigidbody2D.velocity.x < 0f;
        rigidbody2D.freezeRotation = true;
        rigidbody2D.rotation = 0;
    }
}