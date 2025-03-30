using UnityEngine;

public class childecharacter : MonoBehaviour
{

    public character ch;

    void Awake()
    {
        ch = GetComponentInParent<character>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        ch.OnChildTriggerEnter(collision);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        ch.OnChildTriggerExit(collision);
    }
}
