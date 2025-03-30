using System.Collections;
using UnityEngine;

public class checkEnemy : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "enemy"){
            StartCoroutine(end_game());
            GameManager.gameManager.DeleteAllClones();
            if(GameManager.gameManager.stageIndex != 1) --GameManager.gameManager.stageIndex;
        }
    }

    IEnumerator end_game(){
        GameManager.gameManager.dospawn = false;
        GameManager.gameManager.winloseText.text = "Lose";
        GameManager.gameManager.winloseText.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        GameManager.gameManager.winloseText.gameObject.SetActive(false);
        GameManager.gameManager.do_game = false;
        GameManager.gameManager.add_Gold();
        GameManager.gameManager.DeleteAllClones();
    }
}
