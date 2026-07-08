using System;
using TMPro;
using Unity.Services.Core;
using UnityEngine;

using System.Runtime.ConstrainedExecution;
using System.Threading.Tasks;
using Unity.Services.Authentication;


public class ControladorLogin : MonoBehaviour
{
    [SerializeField] private CloudServices cloudServices;
    [SerializeField] private TMP_Text usernameText;
    [SerializeField] private TMP_InputField usernameInputField;
    [SerializeField] private GameObject erroLoginPopup;
    [SerializeField] private TMP_Text recordeText;

    // Essa função vai executar tarefas demoradas
    private async void Awake()
    {
        try
        {
            // Essa linha inicializa todos os serviços da Unity.
            await UnityServices.InitializeAsync();

            // Agora ele chama a rotina de login.
            await cloudServices.SignInAnonymouslyAsync();

            // Atualiza o nome do usuário na tela e o seu record
            AtualizarUserNameUI();
            AtualizarRecordeUI();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            erroLoginPopup.SetActive(true);
        }
    }

    private void AtualizarUserNameUI()
    {
        string username = cloudServices.GetUserName();
        usernameText.text = username;
        usernameInputField.text = username.Substring(0, username.IndexOf("#"));
    }

    public async void SalvarNovoUsername()
    {
        await cloudServices.AtualizarUserName(usernameInputField.text);
        AtualizarUserNameUI();
    }

    public async void AtualizarRecordeUI()
    {
        int recorde = await cloudServices.GetPontuacaoJogador();
        recordeText.text = "MEU RECORDE: " + recorde;
    }
}
