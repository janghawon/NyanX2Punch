using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerDie : NetworkBehaviour
{
    [SerializeField] private PlayerState _playerState;
    [SerializeField] private PlayerHealth _playerHealth;
    [SerializeField] private Rigidbody2D _rigid;

    private bool canMakeFx = true;
    
    [ServerRpc(RequireOwnership = false)]
    public void DieServerRpc(bool isHost, Vector3 dir)
    {
        DieClientRpc(isHost, dir);
    }

    [ClientRpc]
    public void DieClientRpc(bool isHost, Vector3 dir)
    {
        FeedbackManager.Instance.ShaekScreen(new Vector3(0.2f, 0.2f, 0));
        FeedbackManager.Instance.StopTime(0.5f, 0.2f);

        if (IsHost == isHost) return;

        _playerState.IsOnDie = true;
        dir.y = 0.5f;
        _rigid.AddForce(dir * 40, ForceMode2D.Impulse);
    }

    private void Update()
    {
        if (!_playerState.IsOnDie || Time.frameCount % 2 == 0 || !canMakeFx) return;
        FeedbackManager.Instance.MakeFxServerRpc(FXType.smoke, transform.position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.CompareTag("MapLimit") && _playerState.IsOnDie)
        {
            canMakeFx = false;
            for(int i = 0; i < 5; i++)
            {
                Vector3 spawnPos = (Vector3)Random.insideUnitCircle * 2 + transform.position;
                FeedbackManager.Instance.MakeFxServerRpc(FXType.die_smoke,  spawnPos);
            }
            
            GameConnectManager.Instance.GameEndTurmSet(1);
            GameConnectManager.Instance.UnSetPlayerServerRpc();
        }
    }
}
