using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerDie : NetworkBehaviour
{
    [SerializeField] private PlayerState _playerState;
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private Rigidbody2D _rigid;
    [SerializeField] private GameObject _smokeParticle;

    public override void OnNetworkSpawn()
    {
        GameBar gb = GameObject.Find("Canvas/EmptyBar").GetComponent<GameBar>();
        if(IsOwner)
        {
            gb.myDie = this;
        }
        else
        {
            gb.enemyDie = this;
        }
    }

    public void Die()
    {
        _playerState.IsOnDie = true;
        _rigid.AddForce(_playerHealth.Dir * 50, ForceMode2D.Impulse);
    }

    private void Update()
    {
        if (!_playerState.IsOnDie) return;
        Instantiate(_smokeParticle, transform.position, Quaternion.identity);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("MapLimit") && _playerState.IsOnDie)
        {
            Debug.Log(1);
        }
    }
}
