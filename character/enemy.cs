using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class enemy : MonoBehaviour
{
    public int enemyIndex;
    public bool iswalk;
    private Rigidbody2D rb;
    private Animator animator;
    private int attack_style;
    private bool isPlayerInTrigger;
    public int attackDelay;
    private int random_attack;
    public int hp;
    private List<character> chlist =   new List<character>();
    private List<enemy> newEnemylist = new List<enemy>();
    public int dmg;
    private SpriteRenderer spriteRenderer;
    private bool isdie = false;
    private int addhp;
    public bool isboss;
    private int[] atk_all = {0,0,0,0,0,0,0,1,1,0,0,1};
    private List<enemy> enemList =   new List<enemy>();
    public bool toplayer;

    //보스 공격 받으면 붉게 변하게 함
    private Material material;
    private int currHp;
    private Color originalColor;

    private float hp_percent;

    void Awake()
    {
        material = GetComponent<Renderer>().material;
        iswalk = true;   
        toplayer = false;
    }


    void Start()
    {
        if(!isboss){
            hp = currencyManager.currencymanager.enemy_hp[enemyIndex];
            dmg = currencyManager.currencymanager.enemy_dmg[enemyIndex];
            get_attackstyle();
        }
        else sethp();
        currHp = hp;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb.gravityScale = 0;
        originalColor = material.color;
    }
    void Update()
    {
        if(iswalk&&!isdie&&!isboss&&!toplayer){
            walk();
        }
        else if(isboss){
            if(hp <= 0){
                victory();
                sethp();
            }
        }

        
        if(chlist.Count == 0 && !toplayer && newEnemylist.Count == 0){
            isPlayerInTrigger = false;
            iswalk = true;
        }
        if(toplayer && enemList.Count == 0){
            isPlayerInTrigger = false;
            iswalk = true;
        }

        if(toplayer&&iswalk){
            new_walk();
        }

        if(isboss){
            if(hp != currHp){
                changeColor();
                currHp = hp;
            }
        }

        if(isboss && GameManager.gameManager.update_game){
            sethp();
        }
    }

    void get_attackstyle(){
        attack_style = currencyManager.currencymanager.attack_style_enemy[enemyIndex];
    }

    void walk(){
        rb.linearVelocity = new Vector2(currencyManager.currencymanager.enemy_move[enemyIndex],rb.linearVelocity.y);
        animator.SetBool("iswalk",true);
    }

    public void SetIndex(int index){
        enemyIndex = index;
    }

    IEnumerator Attack(){
        while(isPlayerInTrigger){
            rb.linearVelocity = Vector2.zero;
            anim_attack();
            do_dmg();
            yield return new WaitForSeconds(attackDelay);
        }
    }

    void do_dmg(){
        if(atk_all[enemyIndex] == 1){
            if(!toplayer) alldmg(dmg);
            else new_alldmg(dmg);
        }
        else{
            if(!toplayer) onedmg(dmg);
            else new_onedmg(dmg);
        }
    }

    void anim_attack(){
        animator.SetBool("iswalk",false);
        random_attack = Random.Range(1,attack_style+1);
        animator.SetInteger("isfight",random_attack);
    }

    public void OnChildTriggerEnter(Collider2D collision)
    {
        if(collision.CompareTag("Player")){
            character enemyScript = collision.GetComponent<character>();
            enemy newEnemyScript = collision.GetComponent<enemy>();
            if(enemyScript!=null && !chlist.Contains(enemyScript)){
                chlist.Add(enemyScript);
            }
            if(newEnemyScript != null && !newEnemylist.Contains(newEnemyScript)){
                newEnemylist.Add(newEnemyScript);
            }
            iswalk = false;
            isPlayerInTrigger = true;
            StartCoroutine(Attack());
        }
    }

    public void OnChildTriggerExit(Collider2D collision)
    {
        if(collision.CompareTag("Player")){
            character enemyScript = collision.GetComponent<character>();
            enemy newEnemyScript = collision.GetComponent<enemy>();
            if(enemyScript != null){
                chlist.Remove(enemyScript);
            }
            if(newEnemyScript != null){
                newEnemylist.Remove(newEnemyScript);
            }
        }   
    }

    void Die(){
        isdie = true;
        if(toplayer) GameManager.gameManager.isalive = false;
        currencyManager.currencymanager.kill_gold(enemyIndex);
        StartCoroutine(fadeDestroy());
    }

    IEnumerator fadeDestroy(){
        animator.SetTrigger("isdead");
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }

    public void takeDmg(int dmg){
        hp -= dmg;
        if(hp <= 0 && !isboss && !isdie){
            Die();
        }
    }

    public void alldmg(int dmg){
        if(chlist.Count > 0){
            for(int i = chlist.Count - 1; i>= 0;--i){
                if(chlist[i] != null){
                    chlist[i].takeDmg(dmg);
                }
            }
        }
        if(newEnemylist.Count > 0){
            for(int i = newEnemylist.Count - 1;i >=0 ;--i){
                if(newEnemylist[i] != null){
                    newEnemylist[i].takeDmg(dmg);
                }
            }
        }
    }

    public void onedmg(int dmg){
        if(chlist.Count > 0){
            if(chlist[0] != null){
                chlist[0].takeDmg(dmg);
            }
        }
        else if(newEnemylist.Count > 0){
            if(newEnemylist[0] != null){
                newEnemylist[0].takeDmg(dmg);
            }
        }
    }


    void victory(){
        GameManager.gameManager.dospawn = false;
        StartCoroutine(win());
        GameManager.gameManager.DeleteAllClones();
        if(GameManager.gameManager.stageIndex <= 100) GameManager.gameManager.stageIndex++;
        GameManager.gameManager.update_game = true;
        clear_gold();
    }

    void clear_gold(){
        if(GameManager.gameManager.stageIndex <= 10){
            currencyManager.currencymanager.gold += 100;
        }
        else if(GameManager.gameManager.stageIndex <= 15){
            currencyManager.currencymanager.gold += 200;
        }
        else{
            currencyManager.currencymanager.gold += 500;
        }
    }

    IEnumerator win(){
        GameManager.gameManager.winloseText.text = "Clear";
        GameManager.gameManager.winloseText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        GameManager.gameManager.winloseText.gameObject.SetActive(false);
        GameManager.gameManager.do_game = false;
        GameManager.gameManager.add_Gold();
        GameManager.gameManager.DeleteAllClones();
    }

    void sethp(){
        int start_hp;
        switch(GameManager.gameManager.stageIndex / 10){
            case 0:
                addhp = 150;
                start_hp = 350;
                break;
            case 1:
                addhp = 200;
                start_hp = 2500;
                break;
            case 2:
                addhp = 300;
                start_hp = 10000;
                break;
            case 8:
                addhp = 5000;
                start_hp = 20000;
                break;
            case 9:
                addhp = 10000;
                start_hp = 50000;
                break;
            case 10:
                addhp = 15000;
                start_hp = 150000;
                break;
            default:
                addhp = 1000;
                start_hp = 35000;
                break;
        }
        hp = addhp * GameManager.gameManager.stageIndex + start_hp;
    }

    void changeColor(){
        StartCoroutine(changeColor_boss());
    }

    IEnumerator changeColor_boss(){
        material.color = new Color(0.2f,0,0);
        yield return new WaitForSeconds(0.1f);
        material.color = originalColor;
    }
    
    void get_new_hp(){
        switch(currencyManager.currencymanager.magic_level[2]/2){
            case 0:
                hp_percent = 0.2f;
                break;
            case 1:
                hp_percent = 0.3f;
                break;
            case 2:
                hp_percent = 0.4f;
                break;
            case 3:
                hp_percent = 0.5f;
                break;
            case 4:
                hp_percent = 0.6f;
                break;
            case 5:
                hp_percent = 0.7f;
                break;
            case 6:
                hp_percent = 0.8f;
                break;
            case 7:
                hp_percent = 0.9f;
                break;
            default:
                hp_percent = 1;
                break;
        }
    }

    public void implayer(){
        GameManager.gameManager.isalive = true;
        toplayer  = true;
        get_new_hp();
        hp = (int)(hp*hp_percent);
        ch_exit();
        gameObject.layer = LayerMask.NameToLayer("player");
        gameObject.tag = "Player";
        chlist.Clear();
        spriteRenderer.flipX = false;
    }

    void ch_exit(){
        GameObject[] Players = GameObject.FindGameObjectsWithTag("Player");

        for(int i=Players.Length-1;i>=0;--i){
            if(Players[i] != null){
                character chscript = Players[i].GetComponent<character>();
                if(chscript != null){
                    chscript.exit_enemlist();
                }
            }
        }
    }

    void new_walk(){
        rb.linearVelocity = new Vector2(-currencyManager.currencymanager.enemy_move[enemyIndex],rb.linearVelocity.y);
        animator.SetBool("iswalk",true);
    }

    public void OnChildTriggerEnter_2player(Collider2D collision)
    {
        if(collision.CompareTag("enemy")){
            enemy enemyScript = collision.GetComponent<enemy>();
            if(enemyScript!=null && !enemList.Contains(enemyScript)){
                enemList.Add(enemyScript);
            }
            iswalk = false;
            isPlayerInTrigger = true;
            StartCoroutine(Attack());
        }
    }

    public void OnChildTriggerExit_2player(Collider2D collision)
    {
        if(collision.CompareTag("enemy")){
            enemy enemyScript = collision.GetComponent<enemy>();
            if(enemyScript != null){
                enemList.Remove(enemyScript);
            }
        }   
    }

    public void new_onedmg(int dmg){
        if(enemList.Count > 0){
            if(enemList[0] != null){
                enemList[0].takeDmg(dmg);
            }
        }
    }

    public void new_alldmg(int dmg){
        if(enemList.Count > 0){
            for(int i = enemList.Count - 1;i>=0;--i){
                if(enemList[i] != null)
                    enemList[i].takeDmg(dmg);
            }
        }
    }

}
