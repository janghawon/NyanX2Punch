using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Rigidbody2D _rigidbody;
    public float movementSpeed;
    [SerializeField] private PlayerAnimation _playerAnimation;

    public Vector2 movementInput;

    // 동적으로 생성 : Awake -> Spawn -> Start
    // 이미 씬에 배치된 오브젝트 : Awake -> Start -> Spawn

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
        movementInput = move;
    }

    private void FixedUpdate()
    {
        _playerAnimation.SetMove(transform.position);
        if (!IsOwner) return;
        transform.position += (Vector3)movementInput * movementSpeed;
    }
}
