using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using TMPro;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private PlayerAttack _pAttack;
    public float movementSpeed;
    [SerializeField] private PlayerAnimation _playerAnimation;
    [SerializeField] private PlayerState _playerState;

    [SerializeField] private TextMeshPro _nickName;
    [SerializeField] private Color _hostColor;
    [SerializeField] private Color _clientColor;

    public Vector2 movementInput;
    private Vector3 prevpos;
    public bool canMovement;

    // 동적으로 생성 : Awake -> Spawn -> Start
    // 이미 씬에 배치된 오브젝트 : Awake -> Start -> Spawn

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        _inputReader.MovementEvent += HandleMovement;
        GameConnectManager.Instance.playerMList.Add(this);
        Color selectColor = IsHost ? _hostColor : _clientColor;
        SetNickNameServerRpc(OwnerClientId, selectColor);
    }

    [ServerRpc]
    private void SetNickNameServerRpc(ulong clientID, Color color)
    {
        UserData userData = HostSingleton.Instnace.GamaManager.NetServer.GetUserDataByClientID(clientID);
        SetNickNameClientRpc(userData.name, color);
    }

    [ClientRpc]
    private void SetNickNameClientRpc(string name, Color color)
    {
        _nickName.text = name;
        _nickName.color = color;
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
        if (_playerState.IsOnAttack || _playerState.IsOnDie || !canMovement) return;

        _playerAnimation.SetMove(prevpos.x - transform.position.x);
        _playerAnimation.FlipController();
        prevpos = transform.position;

        if (!IsOwner) return;
        transform.position += (Vector3)movementInput * movementSpeed;
    }
}
