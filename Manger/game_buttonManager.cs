using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem.XR.Haptics;

public class game_buttonManager : MonoBehaviour
{

    public Button[] unitButtons;
    public CharacterSpawn characterSpawn;
    public Image[] image;
    public int[] respawntime = new int[8];
    private bool[] issafecount = new bool[8];
    private bool docount = false;

    private bool checkLevel = false;
    
    void Start()
    {
        characterSpawn = FindAnyObjectByType<CharacterSpawn>();
        for(int i=0;i<unitButtons.Length;++i){
            int index = i;
            unitButtons[i].onClick.AddListener(()=>OnButtonClick(index));
        }
    }

    void Update()
    {
        if(!GameManager.gameManager.do_game){
            SetButton(false);
        }
        else{
            SetButton(true);
        }

        if(GameManager.gameManager.do_game && GameManager.gameManager.spawncount >= (7 + GameManager.gameManager.stageIndex / 4)){
            docount = true;
        }
        else{
            docount = false;
        }

        if(docount){
            for(int i=0;i<image.Length;++i){
                afterspawnButton(i);
            }
        }
        else{
            for(int i=0;i<image.Length;++i){
                if(!CharacterSpawn.characterSpawn.spawned[i] && currencyManager.currencymanager.ch_level[i] > 0){
                    alphabutton(1f,i);
                }
            }
        }

        if(!checkLevel && GameManager.gameManager.do_game){
            checkLevel = true;
            for(int i=0;i<image.Length;++i){
                if(currencyManager.currencymanager.ch_level[i] == 0) alphabutton(0.5f,i);
                else alphabutton(1f,i);
            }
        }

        if(checkLevel && !GameManager.gameManager.do_game){
            checkLevel = false;   // check level을 초기화 시키기기
        } 

    }


    void SetButton(bool active){  // 소환 버튼을 보여주기기
        for(int i=0;i<image.Length;++i){
            image[i].gameObject.SetActive(active);
        }
    }

    void afterspawnButton(int index){
        alphabutton(0.5f,index);
    }

    void resetspawnButton(int index){
        CharacterSpawn.characterSpawn.spawned[index] = false;
        alphabutton(1f,index);
    }

    IEnumerator DelayedResetSpawnButton(int savedIndex) {
        yield return new WaitForSeconds(respawntime[savedIndex]);
        resetspawnButton(savedIndex);
    }  

    void alphabutton(float alpha,int index){
        Color color = image[index].color;
        color.a = alpha;
        image[index].color = color; 
        Image buttonimage = unitButtons[index].GetComponent<Image>();
        Color newColor = buttonimage.color;
        newColor.a = alpha;
        buttonimage.color = newColor;
    }

    void OnButtonClick(int index){  // 버튼을 눌렀을때 작동동
        if(currencyManager.currencymanager.isEnoughspawngold(currencyManager.currencymanager.spawn_gold[index])){
            if(!characterSpawn.spawned[index]&&!docount&&currencyManager.currencymanager.ch_level[index] > 0){
                GameManager.gameManager.spawncount++;
                currencyManager.currencymanager.Spendspawngold(currencyManager.currencymanager.spawn_gold[index]);
                characterSpawn.SpawnCharacter(index);
                characterSpawn.spawned[index] = true;
                afterspawnButton(index);
                StartCoroutine(DelayedResetSpawnButton(index));
            }
        }
    }
}
