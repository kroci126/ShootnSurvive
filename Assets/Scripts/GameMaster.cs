using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{

    public static GameMaster gm;

    [SerializeField]
    private int maxLifes = 3;
    private static int _remainingLifes = 3;
    public static int RemainingLifes
    {
        get { return _remainingLifes; }
    }

    void Awake ()
    {
        if (gm == null)
        {
            gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        }
    }

    public Transform playerPrefab;
    public Transform spawnPoint;
    public float spawnDelay = 3;
    public Transform spawnPrefab;
    public string respawnCDName = "RespawnCD";
    public string spawnSoundName = "Spawn";

    public CameraShake cameraShake;

    [SerializeField]
    private GameObject gameOverUI;

    private AudioManager audioManager;

    void Start()
    {
        _remainingLifes = maxLifes;

        audioManager = AudioManager.instance;
        if(audioManager == null)
        {
            Debug.LogError("No Audio manager found in the scene");
        }
    }

    public void EndGame()
    {
        gameOverUI.SetActive(true);
    }

    public Transform enemyDeathParticles;

    public IEnumerator _RespawnPlayer ()
    {
        audioManager.PlaySound(respawnCDName);

        yield return new WaitForSeconds(spawnDelay);
        audioManager.PlaySound(spawnSoundName);
        Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        Transform clone = Instantiate(spawnPrefab, spawnPoint.position, spawnPoint.rotation);
        Destroy(clone.gameObject, 3f);
    }
    

    public static void KillPlayer (Player player)
    {
        Destroy(player.gameObject);
        _remainingLifes--;
        if(_remainingLifes <= 0)
        {
            gm.EndGame();
        }
        else
        {
            gm.StartCoroutine(gm._RespawnPlayer());
        }
        
    }

    public static void KillEnemy (Enemy enemy)
    {
        gm._KillEnemy(enemy);
    }

    public void _KillEnemy(Enemy _enemy)
    {
        //sound
        audioManager.PlaySound(_enemy.deathSoundName);

        //particles
        GameObject _clone = Instantiate(_enemy.deathParticles.gameObject, _enemy.transform.position, Quaternion.identity) as GameObject; 
        Destroy(_clone, 3f);

        //camerashake
        cameraShake.Shake(_enemy.shakeAmt, _enemy.shakeLength);
        Destroy(_enemy.gameObject);
    }
}
