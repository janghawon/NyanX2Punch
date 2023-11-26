using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

public enum AuthState
{
    NotAuthenticated,
    Authenicating,
    Authenticated,
    Error,
    TimeOut
}

public static class AuthenticationWrapper
{
    public static AuthState State { get; private set; } = AuthState.NotAuthenticated;
    public static event Action<string> OnMessageEvent;

    public static async Task<AuthState> DoAuth(int maxTries = 5)
    {
        if(State == AuthState.Authenticated)
        {
            return State;
        }

        if(State == AuthState.Authenicating)
        {
            OnMessageEvent?.Invoke("인증이 진행 중 입니다.");
            return await Authenticaation();
        }

        await SignInAnoymouslyAsync(maxTries);
        return State;
    }

    private static async Task SignInAnoymouslyAsync(int maxTries)
    {
        State = AuthState.Authenicating;

        int tries = 0;
        while(State == AuthState.Authenicating && tries < maxTries)
        {
            try
            {
                await AuthenticationService.Instance.SignInAnonymouslyAsync();

                if(AuthenticationService.Instance.IsSignedIn && AuthenticationService.Instance.IsAuthorized)
                {
                    State = AuthState.Authenticated;
                    break;
                }
            }
            catch (AuthenticationException ex)
            {
                OnMessageEvent?.Invoke(ex.Message);
                State = AuthState.Error;
                break;
            }
            catch (RequestFailedException ex)
            {
                OnMessageEvent?.Invoke(ex.Message);
                State = AuthState.Error;
                break;
            }
            ++tries;
            await Task.Delay(1000);
        }

        if(State != AuthState.Authenticated && tries == maxTries)
        {
            OnMessageEvent?.Invoke($"Auth timeout : {tries} tries");
            State = AuthState.TimeOut;
        }
    }

    private static async Task<AuthState> Authenticaation()
    {
        while(State == AuthState.Authenicating)
        {
            await Task.Delay(200); // 0.2초마다 한번씩 확인
        }
        return State;
    }
}
