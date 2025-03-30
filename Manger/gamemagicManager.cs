using System.Collections;
using System.Net.Mail;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.UI;

public class gamemagicManager : MonoBehaviour
{
    public Button change2player;
    public game_magic Game_Magic;
    public int level;

    public Button spikeButton;
    private bool ischangeSpike = false;
    public bool doatk;
    public Button spawnButton;
    private bool btUpdate = true;
    public Image[] images;
    public Button[] magicButtons;

    private bool isrdy = true;
    private bool ch_isrdy = false;
    private bool changealpha = true;
    private int magicDelay;


    //public int mymagic = -1; 이거를 불러서 씀. mymaigc에 따라서 magic setactive
    void Awake()
    {
        Game_Magic = FindAnyObjectByType<game_magic>();
    }

    void Start()
    {
        change2player.onClick.AddListener(changeplayer);
        spikeButton.onClick.AddListener(spikeOn);
        spawnButton.onClick.AddListener(spawnPlayer);
    }

    void Update()
    {
        if(GameManager.gameManager.do_game && !btUpdate){
            all_hide();
            images[0].gameObject.SetActive(true);
            if(GameManager.gameManager.magicKind == 0 && currencyManager.currencymanager.magic_level[1] > 0){
                images[1].gameObject.SetActive(true);
            }
            else if(GameManager.gameManager.magicKind == 1 && currencyManager.currencymanager.magic_level[2] > 0){
                images[2].gameObject.SetActive(true);
            }
            btUpdate = true;
        }

        if(btUpdate && !GameManager.gameManager.do_game){
            btUpdate = false;
            all_hide();
            stop_delay();
            isrdy = true;
            changealpha = true;
            ch_isrdy = false;
            ischangeSpike = false;
            GameManager.gameManager.isalive = false;
        }

        if(ch_isrdy && !GameManager.gameManager.isalive && !isrdy){
            isrdy = true;
        }

        if(isrdy && changealpha){
            alphabutton(1,1);
            alphabutton(1,2);
            changealpha = false;
        }
    }

    void alphabutton(float alpha,int index){
        Color color = images[index].color;
        color.a = alpha;
        images[index].color = color; 
        Image buttonimage = magicButtons[index].GetComponent<Image>();
        Color newColor = buttonimage.color;
        newColor.a = alpha;
        buttonimage.color = newColor;
    }

    void all_hide(){
        for(int i=0;i<3;++i){
            images[i].gameObject.SetActive(false);
        }
    }

    void changeplayer(){
        if(isrdy){
            isrdy = false;
            ch_isrdy = false;
            changealpha = true;
            Game_Magic.enemy2player(currencyManager.currencymanager.magic_level[2]);
            if(Game_Magic.domagic){
                alphabutton(0.5f,2);
                setmagicDelay_2();
                StartCoroutine(chplayerdelay(magicDelay));
            }
            else{
                isrdy = true;
                ch_isrdy = true;
            }
        }
    }

    void setmagicDelay_2(){
        switch(currencyManager.currencymanager.magic_level[2]/3){
            case 0:
                magicDelay = 60;
                break;
            case 1:
                magicDelay = 50;
                break;
            case 2:
                magicDelay = 40;
                break;
            case 3:
            case 4:
                magicDelay = 30;
                break;
            case 5:
                magicDelay = 25;
                break;
            default:
                magicDelay = 20;
                break;
        }
    }

    IEnumerator chplayerdelay(int delay){
        yield return new WaitForSeconds(delay);
        ch_isrdy = true;
        Game_Magic.domagic = false;
    }

    void spikeOn(){
        if(!ischangeSpike){
            ischangeSpike = true;
            StartCoroutine(delay_change(2));
        }
    }

    IEnumerator delay_change(int delay){
        yield return new WaitForSeconds(delay);
        if(doatk == false) doatk = true;
        else doatk = false;
        ischangeSpike = false;
    }

    void spawnPlayer(){
        if(isrdy){
            isrdy = false;
            changealpha = true;
            alphabutton(0.5f,1);
            Game_Magic.spawn_ch(currencyManager.currencymanager.magic_level[1]);  // do spawn
            setmagicDelay_1();
            StartCoroutine(delay(magicDelay));
        }
    }

    IEnumerator delay(int delay){
        yield return new WaitForSeconds(delay);
        isrdy = true;
        Game_Magic.domagic = false;
    }

    void setmagicDelay_1(){
        switch(currencyManager.currencymanager.magic_level[1]/3){
            case 0:
                magicDelay = 60;
                break;
            case 1:
                magicDelay = 45;
                break;
            case 2:
                magicDelay = 35;
                break;
            case 3:
                magicDelay = 25;
                break;
            case 4:
                magicDelay = 20;
                break;
            default:
                magicDelay = 15;
                break;
        }
    }

    void stop_delay(){
        StopAllCoroutines();
    }
}
