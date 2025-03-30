using UnityEngine;

public class audioManager : MonoBehaviour
{

    public AudioSource[] audioSources;
    private bool is_game = false;
    void FixedUpdate()
    {
        if(GameManager.gameManager.loading_game && !is_game && !GameManager.gameManager.do_game){
            ChangeMusic(0); // 로딩 음악 재생
            is_game = true;
        }
        if(GameManager.gameManager.do_game && is_game){
            ChangeMusic(1); 
            is_game = false;
        }
    }

    void ChangeMusic(int index)
    {
        foreach (AudioSource audio in audioSources)
        {
            audio.Stop(); // 모든 오디오 정지
        }
        audioSources[index].Play(); // 선택한 오디오 실행
    }
}
