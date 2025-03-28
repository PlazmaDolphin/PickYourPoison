using UnityEngine;

public class DrawGizmos : MonoBehaviour
{
    public Color boxColor = Color.red;
    private BoxCollider2D boxCollider;

    void OnDrawGizmos()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider != null)
        {
            Gizmos.color = boxColor;
            Gizmos.DrawWireCube(boxCollider.bounds.center, boxCollider.bounds.size);
        }
    }
}
