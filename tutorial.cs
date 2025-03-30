using System.Collections;
using UnityEngine;

public class tutorial : MonoBehaviour
{
    public GameObject[] gameObjects;
    private bool isTouch = false;
    private int tuto_Index = 0;
    private bool do_next_tuto = false;
    private bool first_tuto = true;

    void Start()
    {
        all_hide();
    }

    void Update()
    {
        if(Input.touchCount > 0 && GameManager.gameManager.stageIndex == 1){
            Touch touch = Input.GetTouch(0);

            if(touch.phase == TouchPhase.Began){
                isTouch = true;
            }
            else if(touch.phase == TouchPhase.Ended){
                isTouch = false;
            }
        }
        else if(GameManager.gameManager.stageIndex == 1){
            if(Input.GetMouseButtonDown(0)){
                isTouch = true;
                Debug.Log("touch");
            }
            else if(Input.GetMouseButtonUp(0)){
                isTouch = false;
            }
        }

        if(GameManager.gameManager.do_game && GameManager.gameManager.stageIndex == 1 && first_tuto){
            dotuto();
            first_tuto = false;
        }

        if(isTouch && do_next_tuto && GameManager.gameManager.stageIndex == 1 && GameManager.gameManager.do_game){
            dotuto();
        }

        if(GameManager.gameManager.stageIndex == 1 && !GameManager.gameManager.do_game && (tuto_Index != 0 | !first_tuto)){
            tuto_Index = 0;
            first_tuto = true;
        }
    }
    void dotuto(){
        if(tuto_Index < 5) {
            Time.timeScale = 0;
            StartCoroutine(delay_tuto());
            Debug.Log("do");
        }
        else{
            all_hide();
            Time.timeScale = 1;
        }
    }

    IEnumerator delay_tuto(){
        do_next_tuto = false;
        if(tuto_Index == 4) GameManager.gameManager.do_Game_Gold += 99;
        if(tuto_Index > 0) gameObjects[tuto_Index-1].SetActive(false);
        gameObjects[tuto_Index].SetActive(true);
        yield return new WaitForSecondsRealtime(1);
        do_next_tuto = true;
        tuto_Index++;
    }

    void all_hide(){
        foreach(GameObject GO in gameObjects){
            GO.SetActive(false);
        }
    }
}
