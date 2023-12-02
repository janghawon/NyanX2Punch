using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerJump : NetworkBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private PlayerAnimation _playerAnimation;
    [SerializeField] private Rigidbody2D _rigid;
    [SerializeField] private ParticleSystem _jumpFx;
    [SerializeField] private Transform _JumpFxSpawnPos;
    [SerializeField] private float _jumpForce;
    [SerializeField] private PlayerState _playerState;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        _inputReader.JumpEvent += HandleJump;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;
        _inputReader.JumpEvent -= HandleJump;
    }

    private void HandleJump()
    {
        if (_playerState.IsOnJump || _playerState.IsOnDie) return;

        _playerState.IsOnJump = true;
        Instantiate(_jumpFx, _JumpFxSpawnPos.position, Quaternion.identity);
        _rigid.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }

    private void Update()
    {
        if (_rigid.velocity.y != 0)
            _playerAnimation.SetJump(true, _rigid.velocity.y);
        else
            _playerAnimation.SetJump(false, _rigid.velocity.y);
    }
}
