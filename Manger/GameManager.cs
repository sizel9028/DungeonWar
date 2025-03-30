using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public bool loading_game = false;
    public bool do_game = false;
    public bool do_upgrade = false;
    public static GameManager gameManager;
    public int stageIndex=1;
    public ButtonManager buttonManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private int spawnkind;
    private int rand;
    private bool isspawn = false;
    public bool isend = false;
    public TextMeshProUGUI winloseText;
    public TextMeshProUGUI goldText;
    public TextMeshProUGUI goldText_1;

    private int maxSpawntime;
    private int minSpawntime;
    private float spawntime;
    public bool update_game;
    private int no_spawn;
    private int random_index;
    public Tilemap[] tilemaps;
    private int tilemapIndex;
    public bool dospawn;
    private int[] spawn_p = {500,100,50,40,30,25,20,15,10,8,6,4};
    public int spawncount;
    
    public bool isalive;

    private bool ischange_tilemap = true;

    public int magicKind;   // 사용할 magic count
    public bool do_change = false;

    private bool isgiveMoney = false;

    public int Game_Gold;
    public int do_Game_Gold;

    private void Awake()
    {
        if (gameManager == null){
            gameManager = this;
        }
        else{
            Destroy(gameObject);
        }
        magicKind = 0;
        winloseText.gameObject.SetActive(false);
        buttonManager = FindAnyObjectByType<ButtonManager>();
    }
    void Start()
    {
        get_spawntime();
        get_spawnkind();
        buttonManager.stagetext.text = "stage : " + "B" + stageIndex.ToString() + "F";
    }

    void get_spawnkind(){
        if(stageIndex < 3) spawnkind = 0;
        else if(stageIndex < 22) spawnkind = stageIndex / 3;  // spawnkind = 7까지
        else if(stageIndex < 25) spawnkind = 8;
        else if(stageIndex < 30) spawnkind = 10;
        else spawnkind = 11;
    }


    void get_spawntime(){
        if(stageIndex < 6){
            minSpawntime = 40;
            maxSpawntime = 60;
        }
        else if(stageIndex < 20){
            minSpawntime = 40;
            maxSpawntime = 60;
        }
        else if(stageIndex < 40){
            minSpawntime = 40;
            maxSpawntime = 60;
        }
        else if(stageIndex < 90){
            minSpawntime = 45;
            maxSpawntime = 60;
        }
        else if(stageIndex < 120){
            minSpawntime = 45;
            maxSpawntime = 55;
        }
        spawntime = UnityEngine.Random.Range(minSpawntime,maxSpawntime+1) / 10.0f;
    }

    IEnumerator enemyspawn(){
        if(!isspawn){
            isspawn = true;
            get_spawntime();
            get_rdindex();
            Enemyspawn.enemyspawn.SpawnEnemy(random_index);
            yield return new WaitForSeconds(spawntime);
            isspawn = false;
        }
    }

    void get_rdindex(){
        int total = 0;
        for(int i=no_spawn;i<spawnkind+1;++i) total += spawn_p[i];
        int rd_value = UnityEngine.Random.Range(1,total+1);

        for(random_index = no_spawn;random_index<12;++random_index){
            rd_value -= spawn_p[random_index];
            if(rd_value <= 0) break;
        }
    }
    
    void get_random_index(){   // 20 30 40 60 이로 가면
        if(stageIndex < 10) no_spawn = 0;
        else if(stageIndex < 20) no_spawn = 1;
        else if(stageIndex < 40) no_spawn = 2;
        else if(stageIndex < 60) no_spawn = 5;
        else if(stageIndex < 90) no_spawn = 8;
        else no_spawn = 11;
    }

    void set_gold(){
        goldText_1.text = currencyManager.currencymanager.gold + "g";
        goldText.text = Game_Gold +"g/" +do_Game_Gold + "g";
    }


    void Update()
    {
        if(dospawn){
            StartCoroutine(enemyspawn());
        }

        if(do_game || do_upgrade){
            set_gold();
            goldText.gameObject.SetActive(true);
            goldText_1.gameObject.SetActive(true);
        }
        else {goldText.gameObject.SetActive(false);
        goldText_1.gameObject.SetActive(false);}

        if(update_game){
            get_random_index();
            get_spawnkind();
            update_game = false;
        }

        if(ischange_tilemap && do_game){   // 타일맵을 바꾸는 용도도
            get_tilemap();
            ischange_tilemap = false;
        }

        if(!ischange_tilemap && !do_game){
            ischange_tilemap = true;
        }
        
        if(isalive && !do_game){
            isalive = false;
        }

        if(spawncount != 0 && !do_game){
            spawncount = 0;
        }

        if(!isgiveMoney && do_game){
            addMoney();
        }

        if(isgiveMoney && !do_game){
            isgiveMoney = false;   // 초기화화
        }

    }

    void FixedUpdate()
    {
        do_Game_Gold = Mathf.Min(do_Game_Gold,Game_Gold);   // 돈 유지
    }
    void addMoney(){
        StartCoroutine(giveMoney());
    }

    IEnumerator giveMoney(){
        if(!isgiveMoney){
            isgiveMoney = true;
            do_Game_Gold+=stageIndex/10*2 + 1;
            yield return new WaitForSeconds(1);
            isgiveMoney = false;
        }
    }

    void get_tilemap(){
        switch (stageIndex / 20)
        {
            case 0 :
                tilemapIndex = 0;
                break;
            case 1 :
                tilemapIndex = 1;
                break;
            default:
                tilemapIndex = 2;
                break;
        }
        rstilemaps();
        tilemaps[tilemapIndex].gameObject.SetActive(true);
    }

    void rstilemaps(){
        for(int i=1;i<3;++i){
            tilemaps[i].gameObject.SetActive(false);
        }
    }
    public void DeleteAllClones()
    {
        isend = true;
        GameObject[] playerobject = GameObject.FindGameObjectsWithTag("enemy");
        GameObject[] enemyobject = GameObject.FindGameObjectsWithTag("Player"); // 모든 오브젝트 찾기
        GameObject[] spikeobject = GameObject.FindGameObjectsWithTag("spike");
        foreach (GameObject obj in playerobject)
        {
            if (obj.name.Contains("(Clone)")) // "Clone" 포함된 오브젝트 찾기
            {
                Destroy(obj);
            }
        }
        foreach(GameObject obj in enemyobject){
            if (obj.name.Contains("(Clone)")) 
            {
                Destroy(obj);
            }
        }
        foreach(GameObject obj in spikeobject){
            if (obj.name.Contains("(Clone)")) 
            {
                Destroy(obj);
            }
        }
    }

    public void add_Gold(){
        currencyManager.currencymanager.gold += do_Game_Gold;
        do_Game_Gold = 0;
    }
    
    public void set_Gold(){
        int Game_Gold_1= (int)(Game_Gold * 0.6f);
        if(currencyManager.currencymanager.gold > Game_Gold_1){
            do_Game_Gold = Game_Gold_1;
            currencyManager.currencymanager.gold -= Game_Gold_1;
        }
        else{
            do_Game_Gold = currencyManager.currencymanager.gold;
            currencyManager.currencymanager.gold = 0;
        }
    }
    
    public void set_GameGold(){
        switch (currencyManager.currencymanager.magic_level[3]-2)
        {
            case -1:
                Game_Gold = 100;
                break;
            case 0:
                Game_Gold = 175;
                break;
            case 1:
                Game_Gold = 300;
                break;
            case 2:
                Game_Gold = 500;
                break;
            case 3:
                Game_Gold = 725;
                break;
            case 4:
                Game_Gold = 950;
                break;
            case 5:
                Game_Gold = 1300;
                break;
            case 6 :
                Game_Gold = 1550;
                break;
            case 7:
                Game_Gold = 1850;
                break;
            default:
                Game_Gold = 400 * currencyManager.currencymanager.magic_level[3];
                break;
        }
    }


}
