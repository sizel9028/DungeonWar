using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class upgrade_unit : MonoBehaviour
{
    public Button[] upgradeButtons;
    public TextMeshProUGUI[] buttonTexts;
    public TextMeshProUGUI gold_Text;
    private bool first_log = true;
    public TextMeshProUGUI explainText;
    public Button[] magicUpBt;
    public TextMeshProUGUI[] magicTexts;    
    private string magic_status;

    void Start()
    {
        for(int i=0;i<upgradeButtons.Length;++i){
            int index = i;
            upgradeButtons[i].onClick.AddListener(()=>OnButtonClick(index));
        }
        for(int i=0;i<magicUpBt.Length;++i){
            int index = i;
            magicUpBt[i].onClick.AddListener(()=>OnButtonClick(index+8));
        }
    }

    void Update()
    {
        if(GameManager.gameManager.do_upgrade){
            get_gold();
            show_gold();
        }
        if(first_log){
            currencyManager.currencymanager.upgrade_gold = currencyManager.currencymanager.need_gold[0];
            first_log = false;
        }
    }



    void OnButtonClick(int index){
        if(index <= 7){
            currencyManager.currencymanager.upgrade_chIndex = index;
            currencyManager.currencymanager.upgrade_gold = currencyManager.currencymanager.need_gold[index];
        }
        else if(index > 7){
            if(index==9 || index == 10) GameManager.gameManager.magicKind = index - 9;
            currencyManager.currencymanager.upgrade_chIndex = index;
            Debug.Log("Clicked index: " + index);
            currencyManager.currencymanager.upgrade_gold = currencyManager.currencymanager.magic_need_gold[index-8];
        }
    }

    void get_activeText(int index){
        if(GameManager.gameManager.magicKind == index){
            magic_status = "활성화";
        }
        else{
            magic_status = "비활성화";
        }
    }

    

    void get_gold(){
        for(int i=0;i<8;++i){
            //buttonTexts[i].text = currencyManager.currencymanager.need_gold[i] + "g";  // 200g
            buttonTexts[i].text = "";
        }
        //magicTexts[0].text = currencyManager.currencymanager.magic_need_gold[0] + "g";
        magicTexts[0].text = "";   // 비어두기기
        for(int i=1;i<magicTexts.Length;++i){
            get_activeText(i-1);
            magicTexts[i].text =magic_status;
        }
    }

    void show_gold(){
        gold_Text.text = currencyManager.currencymanager.upgrade_gold + "g";
    }
}
