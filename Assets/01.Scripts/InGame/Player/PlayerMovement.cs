using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private float _movementSpeed;
    [SerializeField] private PlayerAnimation _playerAnimation;

    private Vector2 _movementInput;

    // �������� ���� : Awake -> Spawn -> Start
    // �̹� ���� ��ġ�� ������Ʈ : Awake -> Start -> Spawn

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        _inputReader.MovementEvent += HandleMovement;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;
        _inputReader.MovementEvent -= HandleMovement;
    }

    private void HandleMovement(Vector2 move)
    {
        _movementInput = move;
    }

    private void FixedUpdate()
    {
        _playerAnimation.SetMove(_rigidbody.velocity.magnitude);
        _playerAnimation.FlipController(_rigidbody.velocity.x);
        if (!IsOwner) return;
        _rigidbody.velocity = _movementInput * _movementSpeed;
    }
}
