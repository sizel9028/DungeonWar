
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterSpawn : MonoBehaviour
{
    public GameObject[] characterPrefab;
    public Vector3 spawnPoint;
    public game_buttonManager btmanager;
    public static CharacterSpawn characterSpawn;
    

    public bool[] spawned = new bool[8];  // 거짓일때만 소환가능능

    void Awake()
    {
        btmanager = FindAnyObjectByType<game_buttonManager>();
        for(int i=0;i<8;++i){
            spawned[i] = false;   // 노스폰
        }
        if (characterSpawn == null){
            characterSpawn = this;
        }
        else{
            Destroy(gameObject);
        }
    }

    void get_randomPos(){
        float randy = Random.Range(-0.72f,-0.41f);
        spawnPoint = new Vector3(-4,randy,randy);
    }
    public void SpawnCharacter(int i)
    {
        get_randomPos();
        GameObject newCharacter = Instantiate(characterPrefab[i], spawnPoint, Quaternion.identity);
        character characterscript = newCharacter.GetComponent<character>();
        if (characterscript != null){
            characterscript.SetIndex(i);
        }
    }

}
