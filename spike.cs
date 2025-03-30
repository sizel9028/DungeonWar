using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spike : MonoBehaviour
{
    private Animator animator;
    private gamemagicManager spikemanager;
    private bool doattack = false;
    private List<character> chlist =   new List<character>();
    private List<enemy> enemList =   new List<enemy>();
    private int dmg;
    private bool isupdate_dmg = false;


    void Awake()
    {
        spikemanager = FindAnyObjectByType<gamemagicManager>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if(spikemanager.doatk){
            animator.SetBool("doAttack",true);
        }
        else{
            animator.SetBool("doAttack",false);
        }
        
        if(spikemanager.doatk && !doattack){
            doattack = true;
            StartCoroutine(Attack());
        }

        if(!isupdate_dmg&&GameManager.gameManager.do_game){   // 매 스테이지별 스파이크 데미지 업데이트트
            get_dmg();
            spikemanager.doatk = false;
            isupdate_dmg = true;
        }

        if(!GameManager.gameManager.do_game && isupdate_dmg){
            isupdate_dmg = false;
        }
        
    }

    void get_dmg(){
        dmg = currencyManager.currencymanager.spike_Dmg();
    }

    IEnumerator Attack(){
        do_dmg(dmg);
        yield return new WaitForSeconds(1);
        doattack = false;
    }

    void do_dmg(int dmg){
        if(chlist.Count > 0){
            for(int i = chlist.Count - 1;i>=0;--i){
                if(chlist[i] != null){
                    chlist[i].takeDmg(dmg);
                }
            }
        }
        if(enemList.Count > 0){
            for(int i = enemList.Count - 1;i>=0;--i){
                if(enemList[i] != null){
                    enemList[i].takeDmg(dmg);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")){
            character chscript = collision.GetComponent<character>();
            if(chscript != null && !chlist.Contains(chscript)){
                chlist.Add(chscript);
            }
        }
        if(collision.CompareTag("enemy")){
            enemy enemyscript  = collision.GetComponent<enemy>();
            if(enemyscript != null && !enemList.Contains(enemyscript)){
                enemList.Add(enemyscript);
            }
        }
        
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player")){
            character chscript = collision.GetComponent<character>();
            if(chscript != null){
                chlist.Remove(chscript);
            }
        } 
        if(collision.CompareTag("enemy")){
            enemy enemyscript = collision.GetComponent<enemy>();
            if(enemyscript != null){
                enemList.Remove(enemyscript);
            }
        }
    }

}
