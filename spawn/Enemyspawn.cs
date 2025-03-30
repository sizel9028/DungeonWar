
using UnityEngine;

public class Enemyspawn : MonoBehaviour
{
    public GameObject[] enemyPrefab;
    public Vector3 enemySpawnPos;
    public static Enemyspawn enemyspawn;

    void Awake()
    {
        if (enemyspawn == null){
            enemyspawn = this;
        }
        else{
            Destroy(gameObject);
        }
    }

    void get_randomPos(){
        float randy = Random.Range(-0.72f,-0.41f);
        enemySpawnPos = new Vector3(20,randy,randy);
    }

    public void SpawnEnemy(int i)
    {
        get_randomPos();
        GameObject newEnemy = Instantiate(enemyPrefab[i], enemySpawnPos, Quaternion.identity);
        enemy enemyscript = newEnemy.GetComponent<enemy>();
        if (enemyscript != null){
            enemyscript.SetIndex(i);
        }
    }

}
