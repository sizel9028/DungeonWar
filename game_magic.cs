using System.Collections;
using UnityEngine;

public class game_magic : MonoBehaviour
{

    public bool domagic;

    void Awake()
    {
        domagic = false;
    }
    public void spawn_ch(int level){   // 용병 소환환
        int count;
        switch(level/4){
            case 0:
                count = 1;
                break;
            case 1:
                count = 2;
                break;
            case 2:
                count = 3;
                break;
            case 3:
                count = 4;
                break;
            default:
                count = 5;
                break;
        }
        StartCoroutine(do_spawn(count));
        domagic = true;
    }

    IEnumerator do_spawn(int count){
        for(int i=0;i<count;++i){
            CharacterSpawn.characterSpawn.SpawnCharacter(5);
            yield return new WaitForSeconds(0.3f);
        }
    }

    public void enemy2player(int level){
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");

        for(int i=enemies.Length-1;i>=0;--i){
            enemy enscript  = enemies[i].GetComponent<enemy>();
            if(enscript != null){
                if(!enscript.iswalk){
                    enscript.implayer();
                    domagic = true;
                    break;
                }
            }
        }
    }


}
