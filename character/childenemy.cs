using UnityEngine;

public class childenemy : MonoBehaviour
{
    public enemy en;

    void Awake()
    {
        en = GetComponentInParent<enemy>();
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(!en.toplayer)
            en.OnChildTriggerEnter(collision);
        else
            en.OnChildTriggerEnter_2player(collision);
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(!en.toplayer)
            en.OnChildTriggerExit(collision);
        else 
            en.OnChildTriggerExit_2player(collision);
    }
}
