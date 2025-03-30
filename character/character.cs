using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Android;

public class character : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator animator;
    private Transform targetEnemy;
    private bool iswalk; // iswalk가 참이면 attack, 거짓이면 walk
    private bool near_player;
    private int attack_style;
    private int characterIndex;
    private int random_attack;
    public float attackDelay;
    private bool isattacking;
    public bool isfight;
    private bool isPlayerInTrigger;
    public int horselevel;
    private int horseskill;
    private bool ishorse;
    private bool ishorsewalk;
    public Vector3 enemyPos;
    public int magic_attack;
    private List<enemy> enemList =   new List<enemy>();
    public int dmg;
    public int hp;
    private bool isdie = false;
    private int healing;
    private bool rdy_magic = false;

    private int[] level_hp = {10,80,400,150,20,125,250,30};
    private int[] first_hp = {30,100,600,600,80,175,800,250};
    private int[] level_3hp = {10,200,1200,350,50,300,450,100};
    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        get_characterstatus();
        rb.gravityScale = 0;
    }

    void Start()
    {
        dmg = currencyManager.currencymanager.character_dmg[characterIndex];
        hp = currencyManager.currencymanager.charcter_hp[characterIndex];
        if(characterIndex == 3) ishorse = true;
        else ishorse = false;
        changewalk(true);  // walk bool값 변경경
        near_player = false;
        isattacking = false;
        isfight = false;
        isPlayerInTrigger = false;
        get_attackstyle();
        if(characterIndex == 3) get_horseskill();
        healing = currencyManager.currencymanager.ch_level[characterIndex] * 10;
    }

    void Update()
    {
        if(iswalk&&!ishorse&&!isdie){
            walk();
        }
        else if(ishorse&&ishorsewalk&&!isdie){
            horsewalk();
        }

        if(enemList.Count == 0){
            isPlayerInTrigger = false;
            changewalk(true);
        }

    }
    void FixedUpdate()
    {
        if (enemList.Count > 0){
            enemyPos = enemList[0].transform.position;
            rdy_magic = true;
        }

        if(rdy_magic && enemList.Count == 0)
            rdy_magic = false;
    }

    void get_attackstyle(){
        attack_style = currencyManager.currencymanager.attack_style_ch[characterIndex];
    }

    public void SetIndex(int index){
        characterIndex = index;
    }

    void get_characterstatus(){
        attackDelay = 1f;
    }
    void walk(){
        rb.linearVelocity = new Vector2(currencyManager.currencymanager.character_move[characterIndex],rb.linearVelocity.y);
        animator.SetBool("iswalk",true);
    }
    // 랜서 전용
    void get_horseskill(){
        horseskill = attack_style;
        attack_style = 1;
    }

    void horsewalk(){
        rb.linearVelocity = new Vector2(currencyManager.currencymanager.character_move[characterIndex],rb.linearVelocity.y);
        animator.SetInteger("walking",horseskill);
        animator.SetBool("iswalk",true);
    }

    void changewalk(bool set){
        iswalk = set;
        ishorsewalk = set;
    }
    //

    IEnumerator Attack(){
        while(isPlayerInTrigger){
            Debug.Log("sss");
            rb.linearVelocity = Vector2.zero;
            anim_attack();
            yield return new WaitForSeconds(attackDelay);
        }
    }

    void anim_attack(){
        Debug.Log("ss");
        animator.SetBool("iswalk",false);
        random_attack = Random.Range(1,attack_style+1);
        if((characterIndex == 4 || characterIndex == 7) && rdy_magic)
            heal_Magic.heal_magic.heal_magic_anim(characterIndex,attack_style,enemyPos);
        if(characterIndex == 4 || characterIndex == 7){
            animator.SetInteger("isfight",attack_style);
        }
        else 
            animator.SetInteger("isfight",random_attack);

        if(characterIndex % 4 == 3 || characterIndex == 1){ //lancer and magic and axeman
            alldmg(dmg);
        }
        else if(characterIndex == 4){
            alldmg(dmg);
            if(attack_style == 2)
                heal(healing);
        }
        else{
            onedmg(dmg);
        }
    }


    void die(){
        isdie = true;
        GameManager.gameManager.spawncount--;
        isdie = true;
        animator.SetTrigger("isdead");    
        StartCoroutine(fadeDestroy());
    }

    IEnumerator fadeDestroy(){
        animator.SetTrigger("isdead");
        rb.linearVelocity = Vector2.zero;
        yield return new WaitForSeconds(0.4f);
        Destroy(gameObject);
    }

    public void OnChildTriggerEnter(Collider2D collision)
    {
        if(collision.CompareTag("enemy")){
            Debug.Log("이새끼");
            enemy enemyScript = collision.GetComponent<enemy>();
            if(enemyScript!=null && !enemList.Contains(enemyScript)){
                enemList.Add(enemyScript);
            }
            changewalk(false);
            isPlayerInTrigger = true;
            StartCoroutine(Attack());
        }
    }

    public void OnChildTriggerExit(Collider2D collision)
    {
        Debug.Log("작동하니?");
        if(collision.CompareTag("enemy")){
            enemy enemyScript = collision.GetComponent<enemy>();
            if(enemyScript != null){
                enemList.Remove(enemyScript);
            }
        }   
    }

    public void exit_enemlist(){
        for(int i=enemList.Count-1;i>=0;--i){
            if(enemList[i].toplayer){
                enemList.RemoveAt(i);
            }
        }
    }



    public void takeDmg(int dmg){
        hp -= dmg;
        if(hp <= 0 && !isdie){
            die();
        }
    }


    public void onedmg(int dmg){
        if(enemList.Count > 0){
            if(enemList[0] != null){
                enemList[0].takeDmg(dmg);
            }
        }
    }

    public void alldmg(int dmg){
        if(enemList.Count > 0){
            for(int i = enemList.Count - 1;i>=0;--i){
                if(enemList[i] != null)
                    enemList[i].takeDmg(dmg);
            }
        }
    }

    void heal(int heal){
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");

        for(int i = players.Length - 1;i>=0;--i){
            if(players[i]!=null){
                character chscript = players[i].GetComponent<character>();
                if(chscript != null){
                    if(chscript.hp < currencyManager.currencymanager.charcter_hp[chscript.characterIndex]){
                        chscript.hp += heal;
                    }
                }
            }
        }
    }

}
