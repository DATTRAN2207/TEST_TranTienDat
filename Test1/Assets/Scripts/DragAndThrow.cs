using UnityEngine;
using UnityEngine.SceneManagement;

public class DragAndThrow : MonoBehaviour
{
    private Vector3 throwVector;
    private Rigidbody2D _rb;
    private LineRenderer _lr;
    private int bounce = 0;
    public int maxBounce = 5;
    public LayerMask layerMask = new LayerMask();
    private bool isDown = false;
    [SerializeField]
    private CircleCollider2D _collider;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _lr = GetComponent<LineRenderer>();
    }

    void OnMouseDown()
    {
        isDown = true;
        CalculateThrowVector();
        SetArrow();
    }
    /*private void OnMouseDrag()
    {
        CalculateThrowVector();
        SetArrow();
    }*/

    private void FixedUpdate()
    {
        if (!isDown)
            return;
        CalculateThrowVector();
        SetArrow();
    }

    private void SetArrow()
    {
        _lr.positionCount = 4;

        Vector2 direction = throwVector;
        Vector2 origin = _rb.gameObject.transform.position;

        for (int i = 0; i < 4; i++)
        {
 
            RaycastHit2D hit = Physics2D.Raycast(origin, direction, Mathf.Infinity, layerMask);

            if (hit.collider != null)
            {
                _lr.SetPosition(i, origin);
                float ch = Mathf.Sqrt(2 * _collider.radius * _collider.radius);
                Vector2 vt2 = hit.point - direction.normalized * ch;
                direction = Vector2.Reflect(direction, hit.normal);
                origin = vt2;
            }

            
        }
        _lr.enabled = true;
    }

    private void CalculateThrowVector()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 distance = mousePos - transform.position;

        throwVector = -distance.normalized;
    }

    private void OnMouseUp()
    {
        isDown = false;
        _lr.enabled = false;
        _rb.AddForce(throwVector / 20f);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            bounce++;
            if (bounce >= maxBounce)
            {
                EndGame();
            }
        }
        if (collision.gameObject.CompareTag("Finish"))
        {
            WinGame();
        }
    }

    private void WinGame()
    {
        SceneManager.LoadScene("Level2");
    }

    void EndGame()
    {
        Debug.Log("Game Over");
    }
}
