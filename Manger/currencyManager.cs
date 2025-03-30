using UnityEngine;

//gold,stageIndex get
public class currencyManager : MonoBehaviour
{
    // 캐릭터의 업그레이드된 hp를 알아보기기
    public int gold;
    public int[] ch_level = new int[8];
    public static currencyManager currencymanager;
    public float[] character_move;
    public int[] attack_style_ch;
    public float[] enemy_move;
    public int[] attack_style_enemy;
    public int[] character_dmg;
    public int[] charcter_hp;
    public bool is_upgrade = true;
    private int[] level_hp_ch = {5,10,300,50,75,50,150,60};
    private int[] level_dmg_ch = {0,2,1,10,5,3,40,20};
    private int[] level_3_dmg_ch = {10,15,1,50,5,45,100,70};
    private int[] level_3_hp_ch = {50,150,900,300,50,250,450,100};
    private int[] low_level_hp = {10,140,0,0,0,50,0,0};
    private int[] low_level_dmg = {7,10,0,0,0,2,0,0};
    private int[] first_hp_ch = {30,100,1500,700,80,175,2000,400};
    private int[] first_dmg_ch = {2,5,15,20,15,12,180,100};
    public int[] enemy_hp = new int[12];
    public int[] enemy_dmg = new int[12];
    private int[] level_hp_en = {100,150,150,300,100,600,300,400,800,1500,1000,1200};
    private int[] level_dmg_en = {1,2,4,2,3,2,7,5,7,2,16,10};
    public int[] need_gold={25,50,750,350,200,125,1600,3200};
    private int[] upgradegold = {25,50,350,275,400,150,400,500};
    public int[] magic_need_gold = new int[4];
    public int upgrade_gold;
    public int upgrade_chIndex;
    private int[] kill_coin = {15,20,75,75,75,125,125,150,225,375,425,750};
    public int[] spawn_gold;
    private int[] const_attack_style = {2,3,3,3,2,3,3,2};
    public bool get_attack_sty = true;
    public int[] magic_level = new int[4];  //8 9 10 ch_level에서 담당당         // 0 : spike 1 : en2player 2 : spawn
    private int[] magic_level_dmg={120,3};
    //private int[] magic_remagicTime;


    private void Awake()
    {
        if (currencymanager == null) currencymanager = this;
        else Destroy(gameObject);

        spawn_gold = new int[8] {25,50,200,150,100,75,300,500};
    }

    public void kill_gold(int enemyIndex){
        GameManager.gameManager.do_Game_Gold += kill_coin[enemyIndex];
    }


    public bool isEnoughGold(int gold1){
        return gold >= gold1;
    }

    public bool isEnoughspawngold(int gold1){
        return GameManager.gameManager.do_Game_Gold >= gold1;
    }

    public void SpendGold(int gold1){
        gold -= gold1;
    }

    public void Spendspawngold(int gold1){
        GameManager.gameManager.do_Game_Gold -= gold1;
    }

    public int spike_Dmg(){
        return magic_level_dmg[0] * magic_level[0];
    }

    void get_hp_ch(){
        for(int i=0;i<8;++i){
            if(ch_level[i] < 15) charcter_hp[i] = first_hp_ch[i] + level_hp_ch[i] * ch_level[i] + (ch_level[i]/3) * level_3_hp_ch[i]
            + ch_level[i] * low_level_hp[i];
            else charcter_hp[i] = first_hp_ch[i] + level_hp_ch[i] * ch_level[i] + (ch_level[i]/3) * level_3_hp_ch[i]
            + 15 * low_level_hp[i];
        }
    }

    void get_atk_ch(){
        for(int i=0;i<8;++i){
            if(ch_level[i] < 15) character_dmg[i] = first_dmg_ch[i] + level_dmg_ch[i] * ch_level[i] + 
            (ch_level[i]/3)*level_3_dmg_ch[i] + ch_level[i]*low_level_dmg[i];
            else character_dmg[i] = first_dmg_ch[i] + level_dmg_ch[i] * ch_level[i] + 
            (ch_level[i]/3)*level_3_dmg_ch[i] + 15*low_level_dmg[i];
        }
    }

    void get_en(){  // en status
        for(int i=0;i<12;++i){
            enemy_dmg[i] = 2 + GameManager.gameManager.stageIndex * level_dmg_en[i];
            enemy_hp[i] = 50 + GameManager.gameManager.stageIndex * level_hp_en[i];
        }
    }

    public void up_cost(int index){
        if(ch_level[index]!=1){
            if(need_gold[index]>2000){
                need_gold[index] = (int)(need_gold[index]*1.1);
            }
            else{
                need_gold[index] = (int)(need_gold[index]*1.3);
            }
        }
        else{
            need_gold[index] = upgradegold[index];
        }
    }

    public void magic_up_cost(int index){
        if(index < 3 )magic_need_gold[index] = (int)(magic_need_gold[index]*1.2);
        else if(index == 3) upgrade_gold_scroll();
    }

    void get_attack_style(){
        for(int i=0;i<8;++i){
            int tmp = ch_level[i] / 3 + 1;
            if(tmp > const_attack_style[i]) attack_style_ch[i] = const_attack_style[i];
            else attack_style_ch[i] = tmp;
        }
    }

    void upgrade_gold_scroll(){
        if(magic_level[3] < 6){
            magic_need_gold[3] = (int)(GameManager.gameManager.Game_Gold * 1.5f);
        }
        else
            magic_need_gold[3] = (int)(GameManager.gameManager.Game_Gold * 0.6f);
    }


    void Update()
    {
        if(is_upgrade||GameManager.gameManager.loading_game){
            get_atk_ch();
            get_hp_ch();
            get_en();
            get_attack_sty = true;
            is_upgrade = false;
        }

        if(GameManager.gameManager.loading_game && get_attack_sty){
            get_attack_style();
            get_attack_sty = false;  // 어택변경을 한번만 적용용
        }
    }

}
