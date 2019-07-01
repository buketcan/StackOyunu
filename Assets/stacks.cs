﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class stacks : MonoBehaviour {

    int stack_uzunlugu;
    int count = 4;
    int skor = 0;
    const float max_Deger = 5f;
    const float hız_degeri = 0.15f;
    const float buyukluk = 3f;
    Vector2 stack_boyut = new Vector2(buyukluk, buyukluk);
    float hata_payi = 0.2f;
    float hiz = hız_degeri;
    GameObject[] go_stack;
    int stack_index;
    bool x_ekseninde_hareket;
    Vector3 Camera_pos;
    Vector3 eski_stack_pos;
    float hassasiyet;
    bool stack_alındı = false;
    bool dead = false;
    int combo = 0;
    Color32 renk;
    public Color32 renk1;
    public Color32 renk2;
    public Color32 renk3;
    public Color32 renk4;
    public Text textimiz;
    public Text high_score_Text;
    public GameObject g_panel;
    int sayac = 0;
    Camera camera;
    int high_score;
	
	void Start () {
        high_score = PlayerPrefs.GetInt("highscore");//Kayıtlı olan en yüksek skorumuzu alıyoruz
        high_score_Text.text = high_score.ToString();//Yüksek skoru yazıyoruz
        textimiz.text = skor.ToString() ;//yeni oyundaki skorumuzu yazıyoruz
        camera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();//camerayı alıyoruz
        camera.backgroundColor = renk2;//camera arkaplanını değiştiriyoruz
        renk = renk1;//ilk stack rengimizi seçiyoruz.
        stack_uzunlugu = transform.GetChildCount();//kaç stack olduğunu öğreniyoruz
        go_stack = new GameObject[stack_uzunlugu];//
        for (int i = 0; i < stack_uzunlugu; i++)
        {
            go_stack[i] = transform.GetChild(i).gameObject;
            go_stack[i].GetComponent<Renderer>().material.color = renk;
        }
        stack_index = stack_uzunlugu-1;
	}
    void ArtikParcaOl(Vector3 konum, Vector3 scale,Color32 renkde)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.transform.localScale = scale;
        go.transform.position = konum;
        go.GetComponent<Renderer>().material.color = renkde;
        go.AddComponent<Rigidbody>();
    }
	
	void Update () {
        if (!dead)
        {
            if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Oyun();

                }
                Hareketlendir();
                transform.position = Vector3.Lerp(transform.position, Camera_pos, 0.1f);
            }
            else if (Application.platform == RuntimePlatform.Android)
            {
                if (Input.touchCount > 0&&Input.GetTouch(0).phase==TouchPhase.Began)
                {
                    Oyun();
                }
                Hareketlendir();
                transform.position = Vector3.Lerp(transform.position, Camera_pos, 0.1f);
            }
            
        }
	}
    public void Oyun()
    {
        if (Stack_Kontrol())
        {
            Stack_Al_Koy();
            count++;
            skor++;
            textimiz.text = skor.ToString();
            if (skor > high_score)
            {
                high_score = skor;
            }
            byte deger = 25;
            renk = new Color32((byte)(renk.r + deger), (byte)(renk.g + deger), (byte)(renk.b + 25), renk.a);
            renk2 = new Color32((byte)(renk2.r + deger), (byte)(renk2.g + deger), (byte)(renk2.b + 25), renk2.a);
            if (sayac > 4)
            {
                sayac = 0;
                renk1 = renk2;
                renk2 = renk3;
                renk3 = renk4;
                renk4 = renk;
                renk = renk1;
            }
            sayac++;
        }
        else
        {
            Bitir();
        }
    }
    void Stack_Al_Koy()
    {
        eski_stack_pos = go_stack[stack_index].transform.localPosition;
        if (stack_index <= 0)
        {
            stack_index = stack_uzunlugu ;
        }
        stack_alındı = false;
        stack_index--;
        x_ekseninde_hareket = !x_ekseninde_hareket;
        Camera_pos = new Vector3(0, -count+3, 0);
        go_stack[stack_index].transform.localScale = new Vector3(stack_boyut.x, 1, stack_boyut.y);
        go_stack[stack_index].GetComponent<Renderer>().material.color =  Color32.Lerp(    go_stack[stack_index].GetComponent<Renderer>().material.color,renk,0.5f);

        camera.backgroundColor = Color32.Lerp(camera.backgroundColor, renk2, 0.08f);
    }
    void Hareketlendir()
    {
        if (x_ekseninde_hareket)
        {
            if (!stack_alındı)
            {
                go_stack[stack_index].transform.localPosition = new Vector3(max_Deger, count , hassasiyet);
                stack_alındı = true;
            }
            if (go_stack[stack_index].transform.localPosition.x > max_Deger)
            {
                hiz = hız_degeri * -1;
            }
            else if (go_stack[stack_index].transform.localPosition.x < -max_Deger)
            {
                hiz = hız_degeri;
            }
            go_stack[stack_index].transform.localPosition += new Vector3(hiz, 0, 0);
        }
        else 
        {
            if (!stack_alındı)
            {
                go_stack[stack_index].transform.localPosition = new Vector3(hassasiyet, count, max_Deger);
                stack_alındı = true;
            }
            if (go_stack[stack_index].transform.localPosition.z > max_Deger)
            {
                hiz = hız_degeri * -1;
            }
            else if (go_stack[stack_index].transform.localPosition.z < -max_Deger)
            {
                hiz = hız_degeri;
            }
            go_stack[stack_index].transform.localPosition += new Vector3(0, 0, hiz);
        }
    }
    bool Stack_Kontrol()
    {
        if (x_ekseninde_hareket)
        {
            float fark = eski_stack_pos.x - go_stack[stack_index].transform.localPosition.x;
            if (Mathf.Abs(fark) > hata_payi)
            {     combo = 0;
                Vector3 konum;
                if (go_stack[stack_index].transform.localPosition.x > eski_stack_pos.x)
                {
                    konum = new Vector3(go_stack[stack_index].transform.position.x + go_stack[stack_index].transform.localScale.x / 2, go_stack[stack_index].transform.position.y, go_stack[stack_index].transform.position.z);
                }
                else
                {
                    konum = new Vector3(go_stack[stack_index].transform.position.x - go_stack[stack_index].transform.localScale.x / 2, go_stack[stack_index].transform.position.y, go_stack[stack_index].transform.position.z);
                }
                Vector3 boyut = new Vector3(fark, 1, stack_boyut.y);
                stack_boyut.x -= Mathf.Abs(fark);
                if (stack_boyut.x < 0)
                {
                    return false;
                }
                go_stack[stack_index].transform.localScale = new Vector3(stack_boyut.x, 1, stack_boyut.y);
                float mid = go_stack[stack_index].transform.localPosition.x / 2 + eski_stack_pos.x / 2;
                go_stack[stack_index].transform.localPosition = new Vector3(mid, count, eski_stack_pos.z);
                hassasiyet = go_stack[stack_index].transform.localPosition.x;
                ArtikParcaOl(konum, boyut, go_stack[stack_index].GetComponent<Renderer>().material.color);
            }
            else
            {
                combo++;
                if (combo > 3)
                {
                    stack_boyut.x += 0.3f;
                    if (stack_boyut.x > buyukluk)
                    {
                        stack_boyut.x = buyukluk;
                       
                       
                    }
                    go_stack[stack_index].transform.localScale = new Vector3(stack_boyut.x, 1, stack_boyut.y);
                    go_stack[stack_index].transform.localPosition = new Vector3(eski_stack_pos.x, count, eski_stack_pos.z);
                }
                else 
                {
                    go_stack[stack_index].transform.localPosition = new Vector3(eski_stack_pos.x, count, eski_stack_pos.z);
                }
                hassasiyet = go_stack[stack_index].transform.localPosition.x;
            }
        }
        else
        {

            float fark = eski_stack_pos.z - go_stack[stack_index].transform.localPosition.z;
            if (Mathf.Abs(fark) > hata_payi)
            {
                combo = 0;
                Vector3 konum;
                if (go_stack[stack_index].transform.localPosition.z > eski_stack_pos.z)
                {
                    konum = new Vector3(go_stack[stack_index].transform.position.x, go_stack[stack_index].transform.position.y, go_stack[stack_index].transform.position.z + go_stack[stack_index].transform.localScale.z / 2);
                }
                else
                {
                    konum = new Vector3(go_stack[stack_index].transform.position.x, go_stack[stack_index].transform.position.y, go_stack[stack_index].transform.position.z - go_stack[stack_index].transform.localScale.z / 2);
                }
                Vector3 boyut = new Vector3(stack_boyut.x, 1, fark);
                stack_boyut.y -= Mathf.Abs(fark);
                if (stack_boyut.y < 0)
                {
                    return false;
                }
                go_stack[stack_index].transform.localScale = new Vector3(stack_boyut.x, 1, stack_boyut.y);
                float mid = go_stack[stack_index].transform.localPosition.z / 2 + eski_stack_pos.z / 2;
                go_stack[stack_index].transform.localPosition = new Vector3(eski_stack_pos.x, count, mid);
                hassasiyet = go_stack[stack_index].transform.localPosition.z;
                ArtikParcaOl(konum, boyut, go_stack[stack_index].GetComponent<Renderer>().material.color);
                combo++;
            }
            else
            {
                combo++;
                if (combo > 3)
                {
                    stack_boyut.y += 0.3f;
                    if (stack_boyut.y > buyukluk)
                    {
                        stack_boyut.y = buyukluk;
                        
                      
                    }
                    go_stack[stack_index].transform.localScale = new Vector3(stack_boyut.x, 1, stack_boyut.y);
                 
                    go_stack[stack_index].transform.localPosition = new Vector3(eski_stack_pos.x, count, eski_stack_pos.z);
                }
                else
                {
                   
                    go_stack[stack_index].transform.localPosition = new Vector3(eski_stack_pos.x, count, eski_stack_pos.z);
                }
                hassasiyet = go_stack[stack_index].transform.localPosition.z;
            }
        }
        return true;
    }
    void Bitir()
    {
       // Debug.Log("asd");
            dead = true;
            go_stack[stack_index].AddComponent<Rigidbody>();
            g_panel.SetActive(true);
            PlayerPrefs.SetInt("highscore", high_score);
            high_score_Text.text = high_score.ToString();
            textimiz.text = "";
    }
    public void Yeni_Oyun()
    { 
    Application.LoadLevel(Application.loadedLevel);
    }
}
