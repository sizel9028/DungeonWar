using UnityEngine;

public class spikeManager : MonoBehaviour
{
    private int spike_count;
    private Vector3[] spawnPos; // 어디에 소환할건지. 나중에 / 2해야함.
    private bool doCount = true;
    public GameObject Spike;
    private int[] spawn_x;

    void get_spikeCount(){
        get_spike_count();
        spawn_x = new int[spike_count];
        spawnPos = new Vector3[spike_count];
        for(int i = 0;i<spike_count;++i){
            int randx = Random.Range(0,31);
            spawn_x[i] = randx;
            for(int j=0;j<i;++j){
                if(spawn_x[i] == spawn_x[j]){
                    --i;
                    continue;
                }
            }
            spawnPos[i] = new Vector3(randx / 2.0f, -0.8f,0);
        }
        for(int i = 0;i<spike_count;++i){
            spawn_Spike(i);
        }
    }

    void get_spike_count(){
        switch(currencyManager.currencymanager.magic_level[0]/2){
            case 0:
                spike_count = 1;
                break;
            case 1:
                spike_count = 2;
                break;
            case 2:
                spike_count = 3;
                break;
            case 3:
                spike_count = 4;
                break;
            case 4:
                spike_count = 5;
                break;
            case 5:
                spike_count = 6;
                break;
            case 6:
            case 7:
            case 8:
                spike_count = 7;
                break;
            case 9:
            case 10:
                spike_count = 8;
                break;
            case 11:
            case 12:
                spike_count = 9;
                break;

        }
    }
    void spawn_Spike(int i){
        Instantiate(Spike,spawnPos[i],Quaternion.identity);
    }

    void Update()
    {
        if(GameManager.gameManager.do_game && doCount){
            get_spikeCount();
            doCount = false;
        }

        if(!GameManager.gameManager.do_game && !doCount){
            doCount = true;
        }
    }


}
