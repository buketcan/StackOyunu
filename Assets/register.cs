using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class register : MonoBehaviour {
    public InputField RegisterUsername;
    public InputField RegisterPassword;
    public InputField RegisterEmail;
    public string baglanti = "http://localhost";

    public IEnumerator Register()
    {
        Debug.Log("Trying to register");
        WWWForm DataToSend = new WWWForm();
        if (Application.installMode == ApplicationInstallMode.Editor)
        {
            DataToSend.AddField("username", RegisterUsername.text);
            DataToSend.AddField("password", RegisterPassword.text);
            DataToSend.AddField("email", RegisterEmail.text);
            WWW serverRequest = new WWW(baglanti + "/unity.php", DataToSend);
            yield return serverRequest;
            Debug.Log(serverRequest.text);
            
        }
        else
        {
            Debug.Log("Application is not installed from market");
            Application.Quit();
        }
    }
    public void RegisterButton()
    {
        StartCoroutine(Register());
    }
}

