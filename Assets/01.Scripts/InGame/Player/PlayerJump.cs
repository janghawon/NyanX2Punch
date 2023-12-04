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
        FeedbackManager.Instance.MakeFxServerRpc(FXType.jump, _JumpFxSpawnPos.position);
        _rigid.AddForce(Vector2.up * _jumpForce, ForceMode2D.Impulse);
    }

    private void FixedUpdate()
    {
        if (!_playerState.IsOnJump) return;

        if(_rigid.velocity.y == 0)
        {
            _playerState.IsOnJump = false;
        }
    }

    public void JumpEnd()
    {
        _playerState.IsOnJump = false;
    }
}
