using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using System;

public class PlayerHealth : NetworkBehaviour
{
    public int maxHealth;
    public NetworkVariable<int> currentHealth;
    private bool _isDead = false;
    private GameBar _gameBar;

    public Action<PlayerHealth> OnDie;
    public event Action<int> OnHealthChanged;

    public ulong LastHitDealerID { get; private set; }
    public Transform hitTrm;

    private void Awake()
    {
        currentHealth.Value = maxHealth;
        _gameBar = GameObject.Find("Canvas/EmptyBar").GetComponent<GameBar>();
    }

    public override void OnNetworkSpawn()
    {
        if (IsClient)
        {
            currentHealth.OnValueChanged += HandleChangeHealth;
            HandleChangeHealth(0, maxHealth);
            OnHealthChanged += _gameBar.OnChangeGameBarValue;
        }

        if (!IsServer) return;
        currentHealth.Value = maxHealth;
    }

    public override void OnNetworkDespawn()
    {
        if (IsClient)
        {
            currentHealth.OnValueChanged -= HandleChangeHealth;
            OnHealthChanged -= _gameBar.OnChangeGameBarValue;
        }
    }

    private void HandleChangeHealth(int prev, int newValue)
    {
        if (_isDead) return;
        OnHealthChanged?.Invoke(newValue);
    }

    public void TakeDamage(int damageValue, ulong dealerID)
    {
        LastHitDealerID = dealerID;
        ModifyHealth(-damageValue);
    }

    public void RestoreHealth(int healthValue)
    {
        ModifyHealth(healthValue);
    }

    public void ModifyHealth(int value)
    {
        if (_isDead) return;

        currentHealth.Value = Mathf.Clamp(currentHealth.Value + value, 0, maxHealth);
        if (currentHealth.Value == 0)
        {
            OnDie?.Invoke(this);
            _isDead = true;
        }
    }
}
