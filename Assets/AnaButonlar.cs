using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class AnaButonlar : MonoBehaviour {

public void GirisYap()
    {
        SceneManager.LoadScene("girisyap");
    }
    public void KayitOl()
    {
        SceneManager.LoadScene("kayitol");
    }
}
