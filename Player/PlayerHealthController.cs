using Assets.Scripts.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealthController : Health
{

    PlayerUIController uiCon;

    [SerializeField] AudioSource source;
    [SerializeField] AudioClip hitSound;

    // Start is called before the first frame update
    void Start()
    {
        uiCon = GetComponent<PlayerUIController>();
        source = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Damage(float damage)
    {
        hitpoints -= damage;
        uiCon.UpdateHealth(hitpoints, maxHitpoints);
        if (hitpoints < 0) Die();
    }
    public override void Damage(DamageData damage)
    {
        hitpoints-= damage.amount;
        uiCon.UpdateHealth(hitpoints, maxHitpoints);
        source.clip = hitSound;
        source.Play();
        if (hitpoints < 0) Die();
    }

    public override void Die()
    {
        //game over. make player load save or return to menu
        if (SaveController.CheckForSave()) SaveController.ReloadLevel();
        else LevelController.LoadLevelByName("Menu");
    }

    public void UpdateResistance(float[] _resistance)
    {
        for (int i = 0; i < _resistance.Length; i++)
        {
            resistance[i] = _resistance[i];
        }
    }
}
