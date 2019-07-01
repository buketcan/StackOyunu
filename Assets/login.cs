using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class login : MonoBehaviour {

    public InputField loginUsername;
    public InputField loginPassword;
 

    public IEnumerator Login()
    {
        Debug.Log("Trying to login");
        WWWForm DataToSend = new WWWForm();
        if (Application.installMode == ApplicationInstallMode.Editor)
        {
            DataToSend.AddField("username", loginUsername.text);
            DataToSend.AddField("password", loginPassword.text);

            WWW serverRequest = new WWW("http://localhost" + "/login.php", DataToSend);
            yield return serverRequest;
            Debug.Log(serverRequest.text);
            SceneManager.LoadScene("1");
        }
        else
        {
            Debug.Log("Application is not installed from market");
            Application.Quit();
        }
    }
    public void loginButton()
    {
        StartCoroutine(Login());
    }
}
