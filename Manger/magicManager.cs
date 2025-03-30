using System.Collections;
using UnityEngine;

public class heal_Magic : MonoBehaviour
{ 
    public static heal_Magic heal_magic;
    public GameObject[] magicPrefab;
    public float plus_ydir;
    private int index;
    public bool ismagic_alive_1;
    public bool ismagic_alive_2;

    void Awake()
    {
        if (heal_magic == null){
            heal_magic = this;
        }
        else{
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if(!GameManager.gameManager.do_game && (!ismagic_alive_1 && !ismagic_alive_2)){
            ismagic_alive_1 = false;
            ismagic_alive_2 = false;
        }
    }
    void get_enemyPos(Vector3 enemyPos){
        enemyPos = new Vector3(enemyPos.x,plus_ydir);
    }

    void get_index(int characterIndex,int attack_style){
        switch(characterIndex){
            case 4:
                characterIndex = 0;
                break;
            case 7:
                characterIndex = 1;
                break;
            default:
                break;
        }
        index = characterIndex*2+attack_style;   // 마법사 스킬 3 , 3일때 나감감
    }

    public void heal_magic_anim(int characterIndex,int attack_style,Vector3 enemyPos){
        if(!ismagic_alive_1 && characterIndex == 4){
            ismagic_alive_1 = true;
            get_enemyPos(enemyPos);
            get_index(characterIndex,attack_style);
            GameObject newMagic = Instantiate(magicPrefab[index/3],enemyPos, Quaternion.identity);
            magic magicscript = newMagic.GetComponent<magic>();
            if (magicscript != null){
                magicscript.SetIndex(index);
            }
        }
        else if(!ismagic_alive_2 && characterIndex == 7){
            ismagic_alive_2 = true;
            get_enemyPos(enemyPos);
            get_index(characterIndex,attack_style);
            GameObject newMagic = Instantiate(magicPrefab[index/3],enemyPos, Quaternion.identity);
            magic magicscript = newMagic.GetComponent<magic>();
            if (magicscript != null){
                magicscript.SetIndex(index);
            }
        }
    }
}
