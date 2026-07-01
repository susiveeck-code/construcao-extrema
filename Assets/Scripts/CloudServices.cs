using System;
using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
using UnityEngine;

/*
    Jogo inicia
      │
      ▼
    Inicializa os serviços da Unity
      │
      ▼
    Conecta o jogador na nuvem
      │
      ▼
    Se der certo:
        mostra o PlayerID
    Se der erro:
        abre um popup
*/

public class CloudServices : MonoBehaviour
{
    [SerializeField] private GameObject erroLoginPopup;

    // Essa função vai executar tarefas demoradas
    private async void Awake()
    {
        try
        {
            // Essa linha inicializa todos os serviços da Unity.
            await UnityServices.InitializeAsync();

            // Agora ele chama a rotina de login.
            await SignInAnonymouslyAsync();
        }
        catch(Exception e)
        {             
            Debug.LogException(e);
            erroLoginPopup.SetActive(true);
        }
    }
    
    public async Task SignInAnonymouslyAsync()
    {
        // O jogador já está logado?
        if (AuthenticationService.Instance.IsSignedIn) return;

        try
        {
            // Aqui acontece realmente o login. A Unity cria um jogador automaticamente
            await AuthenticationService.Instance.SignInAnonymouslyAsync();
            Debug.Log("Sign in anonymously succeeded!");

            // A Unity cria um identificador único. Esse ID identifica somente aquele jogador
            Debug.Log($"PlayerID: {AuthenticationService.Instance.PlayerId}");

        }
        //catch (AuthenticationException ex)
        //{
        //    erroLoginPopup.SetActive(true);
        //}       
        catch (AuthenticationException ex)
        {
            Debug.LogException(ex);
            erroLoginPopup.SetActive(true);
        }
        catch (RequestFailedException ex)
        {
            Debug.LogException(ex);
            erroLoginPopup.SetActive(true);
        }
    }

    //public void TentarLoginNovamente()
    //{
    //    erroLoginPopup.SetActive(false);
    //    SignInAnonymouslyAsync();

    //}

    public async void TentarLoginNovamente()
    {
        erroLoginPopup.SetActive(false);

        try
        {
            if (UnityServices.State != ServicesInitializationState.Initialized)
            {
                await UnityServices.InitializeAsync();
            }

            await SignInAnonymouslyAsync();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            erroLoginPopup.SetActive(true);
        }
    }
}
