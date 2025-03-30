using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;
using System.Collections;
using System;


//메뉴화면 버튼 관리리
public class ButtonManager : MonoBehaviour
{

    public Button new_button;
    public Button continue_button;
    public Button upgrade_button;
    public Button start_button;
    public Button back_button;
    public Button save_button;
    public Button back_upgrade_button;
    public TextMeshProUGUI stagetext;
    public Button do_upgrade_button;
    //상점버튼, 도감버튼
    public Canvas canvas;
    private List<Button> menubuttons;
    private List<Button> gamebuttons;
    private bool isupgrade = true;
    public GameObject upgrade_Bt;
    public GameObject new_BackGround;

    private int[] first_ch_level = {1,0,0,0,0,0,0,0};
    private int[] first_ch_need = {25,50,450,250,100,75,800,1600};
    private int[] fisrt_mg_level = {1,1,1,1};
    private int[] fisrt_mg_need = {900,900,900,100};

    public Button exitButton;
    public TextMeshProUGUI explain_text;
    private int get_upgrade_index = 10;
    private string ch_status;
    private string ch_add;

    private bool change_stage=true;
    void Start()
    {
        stagetext.text = "stage : "+"B" + GameManager.gameManager.stageIndex + "F";  
        menubuttons = new List<Button> {new_button,continue_button,exitButton};
        gamebuttons = new List<Button> {upgrade_button,start_button,back_button,save_button};

        new_button.onClick.AddListener(button1click);
        continue_button.onClick.AddListener(button2click);
        upgrade_button.onClick.AddListener(button3click);
        start_button.onClick.AddListener(button4click);
        back_button.onClick.AddListener(button5click);
        save_button.onClick.AddListener(button6click);
        back_upgrade_button.onClick.AddListener(button7click);
        do_upgrade_button.onClick.AddListener(button8click);

        exitButton.onClick.AddListener(button9click);
    }

    void Update()
    {
        button_update();
        if(GameManager.gameManager.isend){
            stagetext.text = "stage : " + "B" + GameManager.gameManager.stageIndex + "F";  
            GameManager.gameManager.isend = false;
        }

        if(change_stage){
            stagetext.text = "stage : "+"B" + GameManager.gameManager.stageIndex + "F";  
            change_stage = false;
        }

        if(currencyManager.currencymanager.upgrade_chIndex != get_upgrade_index || !isupgrade){
            get_upgrade_index = currencyManager.currencymanager.upgrade_chIndex;
            if(currencyManager.currencymanager.upgrade_chIndex < 8){
                get_ch_add(currencyManager.currencymanager.ch_level[currencyManager.currencymanager.upgrade_chIndex]);
                get_ch_text(get_upgrade_index);
                set_explain_text(get_upgrade_index);
            }
            else if(currencyManager.currencymanager.upgrade_chIndex >= 8){
                get_ch_text(5);
                set_magic_text(currencyManager.currencymanager.upgrade_chIndex-8);
            }
        }

        if(!GameManager.gameManager.do_game){
            new_BackGround.SetActive(true);
        }
        else
            new_BackGround.SetActive(false);
    }

    void set_magic_text(int index){
        switch(index){
            case 0:
                explain_text.text = "가시함정 작동 스크롤("+currencyManager.currencymanager.magic_level[0]+"lv"+")\n가시함정을 작동시켜" +"\n" +"적과 아군에게 피해를 입힌다";
                break;
            case 1:
                explain_text.text = "용병 소환 스크롤("+currencyManager.currencymanager.magic_level[1]+"lv)\n" + ch_status +" 용병을 소환한다\n" + "소환 카운트에 포함안된다";
                break;
            case 2:
                explain_text.text = "매혹 스크롤("+currencyManager.currencymanager.magic_level[2]+"lv)\n"+"현재 아군과 싸우는 무작위 적 \n한명을 매혹시켜 아군으로 만든다";
                break;
            case 3:
                explain_text.text = "골드 스크롤("+currencyManager.currencymanager.magic_level[3]+"lv) ("+GameManager.gameManager.Game_Gold +"g)\n"+"던전 내부와 밖으로 골드로 옮겨\n골드를 옮길 수 있다";
                break;
        }
    }

    void get_ch_add(int index){
        if(index >= 15){
            ch_add = "한계에 도달\n강화스탯의 증가가 매우 감소한다";
        }
        else{
            ch_add = "";
        }
    }

    void set_explain_text(int index){
        switch(index){
            case 0:
                explain_text.text  = ch_status + " 궁수("+currencyManager.currencymanager.ch_level[0]+"lv)\n" + ch_add;
                break;
            case 1:
                explain_text.text = ch_status + " 집행인("+currencyManager.currencymanager.ch_level[1]+"lv)\n" + ch_add;
                break;
            case 2:
                explain_text.text = ch_status + " 방패병("+currencyManager.currencymanager.ch_level[2]+"lv)";
                break;
            case 3:
                explain_text.text = ch_status + " 기사("+currencyManager.currencymanager.ch_level[3]+"lv)";
                break;
            case 4:
                explain_text.text = ch_status + " 신관("+currencyManager.currencymanager.ch_level[4]+"lv)";
                break;
            case 5:
                explain_text.text = ch_status + " 용병("+currencyManager.currencymanager.ch_level[5]+"lv)";
                break;
            case 6:
                explain_text.text = ch_status + " 소드마스터("+currencyManager.currencymanager.ch_level[6]+"lv)";
                break;
            case 7:
                explain_text.text = ch_status + " 마법사("+currencyManager.currencymanager.ch_level[7]+"lv)";
                break;
        }
    }

    void get_ch_text(int index){
        switch(currencyManager.currencymanager.ch_level[index]/3){
            case 0:
                ch_status = "견습";
                break;
            case 1:
                ch_status = "초급";
                break;
            case 2:
                ch_status = "중급";
                break;
            case 3:
                ch_status = "고급";
                break;
            case 4:
                ch_status = "엘리트";
                break;
            case 5:
                ch_status = "정예";
                break;
            default:
                ch_status = "황실";
                break;

        }
    }

    void button_update(){
        if(GameManager.gameManager.do_game){
            canvas.gameObject.SetActive(false);
        }
        else if(GameManager.gameManager.do_upgrade){
            canvas.gameObject.SetActive(true);
            SetButtonStates(false,menubuttons);
            SetButtonStates(false,gamebuttons);
            back_upgrade_button.gameObject.SetActive(true);
            do_upgrade_button.gameObject.SetActive(true);
            upgrade_Bt.gameObject.SetActive(true);
            stagetext.gameObject.SetActive(false);
        }
        else if(GameManager.gameManager.loading_game){
            canvas.gameObject.SetActive(true);
            SetButtonStates(false,menubuttons);
            SetButtonStates(true,gamebuttons);
            back_upgrade_button.gameObject.SetActive(false);
            do_upgrade_button.gameObject.SetActive(false);
            upgrade_Bt.gameObject.SetActive(false);
            stagetext.gameObject.SetActive(true);
        }
        else{
            canvas.gameObject.SetActive(true);
            SetButtonStates(true,menubuttons);
            SetButtonStates(false,gamebuttons);
            back_upgrade_button.gameObject.SetActive(false);
            do_upgrade_button.gameObject.SetActive(false);
            upgrade_Bt.gameObject.SetActive(false);
            stagetext.gameObject.SetActive(false);
        }
    }

    void SetButtonStates(bool isActive,List<Button> buttons){
        foreach(Button btn in buttons){
            btn.gameObject.SetActive(isActive);
        }
    }

    void button1click()
    {
        change_stage = true;
        GameManager.gameManager.loading_game = true;
        // 초기화 추가
        GameManager.gameManager.stageIndex = 1;
        currencyManager.currencymanager.gold = 0;
        Array.Copy(first_ch_level, currencyManager.currencymanager.ch_level, first_ch_level.Length);
        Array.Copy(first_ch_need,currencyManager.currencymanager.need_gold,first_ch_need.Length);
        Array.Copy(fisrt_mg_level,currencyManager.currencymanager.magic_level,fisrt_mg_level.Length);
        Array.Copy(fisrt_mg_need,currencyManager.currencymanager.magic_need_gold,fisrt_mg_need.Length);
        currencyManager.currencymanager.upgrade_gold = currencyManager.currencymanager.need_gold[0];
        explain_text.text  = ch_status + " 궁수("+currencyManager.currencymanager.ch_level[0]+"lv)";
        GameManager.gameManager.update_game = true;
        GameManager.gameManager.set_GameGold();
    }

    void button2click(){
        change_stage = true;
        GameManager.gameManager.loading_game = true;
        GameManager.gameManager.stageIndex = PlayerPrefs.GetInt("StageIndex",1);
        currencyManager.currencymanager.gold = PlayerPrefs.GetInt("Gold",100);
        currencyManager.currencymanager.ch_level = loadArray(8,"ch_Level",first_ch_level);
        currencyManager.currencymanager.need_gold = loadArray(8,"need_Gold",first_ch_need);
        currencyManager.currencymanager.magic_need_gold = loadArray(4,"need_Gold_mg",fisrt_mg_need);
        currencyManager.currencymanager.magic_level = loadArray(4,"mg_Level",fisrt_mg_level);
        currencyManager.currencymanager.upgrade_gold = currencyManager.currencymanager.need_gold[0];
        explain_text.text  = ch_status + " 궁수("+currencyManager.currencymanager.ch_level[0]+"lv)";
        GameManager.gameManager.update_game = true;
        GameManager.gameManager.set_GameGold();
    }

    void button3click(){
        //upgrade를 보여주기
        change_stage = true;
        GameManager.gameManager.do_upgrade = true;
    }

    void button4click(){
        GameManager.gameManager.set_Gold();
        GameManager.gameManager.do_game = true;
        GameManager.gameManager.dospawn = true;
    }

    void button5click(){
        GameManager.gameManager.loading_game = false;
    }

    void button6click(){
        //save
        PlayerPrefs.SetInt("StageIndex",GameManager.gameManager.stageIndex);
        PlayerPrefs.SetInt("Gold",currencyManager.currencymanager.gold);
        saveArray(currencyManager.currencymanager.ch_level,"ch_Level");
        saveArray(currencyManager.currencymanager.need_gold,"need_Gold");
        saveArray(currencyManager.currencymanager.magic_level,"mg_Level");
        saveArray(currencyManager.currencymanager.magic_need_gold,"need_Gold_mg");
    }

    void saveArray(int[] arr,string keyBase){
        for(int i=0;i<arr.Length;++i){
            PlayerPrefs.SetInt(keyBase+i,arr[i]);
        }
        PlayerPrefs.Save();
    }

    int[] loadArray(int length,string keyBase,int[] array){
        int[] arr = new int[length];
        for(int i=0;i<length;++i){
            arr[i] = PlayerPrefs.GetInt(keyBase+i,array[i]);
        }
        return arr;
    }



    void button7click(){
        GameManager.gameManager.do_upgrade = false;
        currencyManager.currencymanager.is_upgrade = true;
    }

    void button8click(){
        // upgrade 하기
        if(isupgrade){
            if(currencyManager.currencymanager.isEnoughGold(currencyManager.currencymanager.upgrade_gold)){
                StartCoroutine(do_upgrade());
            }
        }
    }

    IEnumerator do_upgrade(){
        isupgrade = false;
        currencyManager.currencymanager.SpendGold(currencyManager.currencymanager.upgrade_gold);
        if(currencyManager.currencymanager.upgrade_chIndex < 8){
            currencyManager.currencymanager.ch_level[currencyManager.currencymanager.upgrade_chIndex]++;
            currencyManager.currencymanager.up_cost(currencyManager.currencymanager.upgrade_chIndex);
            currencyManager.currencymanager.upgrade_gold = currencyManager.currencymanager.need_gold[currencyManager.currencymanager.upgrade_chIndex];
        }
        else if(currencyManager.currencymanager.upgrade_chIndex >= 8){
            currencyManager.currencymanager.magic_level[currencyManager.currencymanager.upgrade_chIndex-8]++;
            currencyManager.currencymanager.magic_up_cost(currencyManager.currencymanager.upgrade_chIndex - 8);
            currencyManager.currencymanager.upgrade_gold = currencyManager.currencymanager.magic_need_gold[currencyManager.currencymanager.upgrade_chIndex-8];
            if(currencyManager.currencymanager.upgrade_chIndex == 11) GameManager.gameManager.set_GameGold();
        }

        yield return new WaitForSeconds(0.5f);
        isupgrade = true;
    }

    void button9click(){
        //exit app
        Application.Quit();
    }

}
