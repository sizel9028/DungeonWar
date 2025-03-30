using UnityEngine;

public class magic : MonoBehaviour
{
    private Animator animator;
    private int magicIndex;


    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        animator.SetInteger("MagicType",magicIndex);
    }

    public void SetIndex(int index){
        magicIndex = index;
    }
    public void DestroySelf(){
        if(magicIndex < 3) heal_Magic.heal_magic.ismagic_alive_1 = false;
        else heal_Magic.heal_magic.ismagic_alive_2 = false;
        Destroy(gameObject);
    }
}
