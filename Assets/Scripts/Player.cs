using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [System.Serializable]
    public class PlayerStats
    {
        public int maxHealth = 100;

        private int _curHealth;
        public int curHealth
        {
            get { return _curHealth; }
            set { _curHealth = Mathf.Clamp(value, 0, maxHealth); }
        }

        public void Init()
        {
            curHealth = maxHealth;
        }
    }

    public PlayerStats Stats = new PlayerStats();

    public int fallBoundary = -20;

    public string deathSoundName = "Death";
    public string damagedSoundName = "Grunt";

    private AudioManager audioManager;

    [SerializeField]
    private StatusIndicator statusIndicator;

    void Start()
    {
        Stats.Init();

        if (statusIndicator == null)
        {
            Debug.LogError("No status indicator reference on player");
        }
        else
        {
            statusIndicator.SetHealth(Stats.curHealth, Stats.maxHealth);
        }

        audioManager = AudioManager.instance;
    }

    void Update ()
    {
        if (transform.position.y <= fallBoundary)
        {
            DamagePlayer(9999999);
        }
    }

    public void DamagePlayer (int damage)
    {
        Stats.curHealth -= damage;
        if (Stats.curHealth <= 0)
        {
            //sound
            audioManager.PlaySound(deathSoundName);

            GameMaster.KillPlayer(this);
        }
        else
        {
            //sound
            audioManager.PlaySound(damagedSoundName);
        }

        statusIndicator.SetHealth(Stats.curHealth, Stats.maxHealth);
    }

    
}
