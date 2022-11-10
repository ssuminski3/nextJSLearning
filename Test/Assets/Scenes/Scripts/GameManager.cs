using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
    private GameObject plansza;
    public GameObject planszaPrefab;
    private bool pGame;
    public GameObject KoloPrefab;
    private GameObject Kolo;
    private bool krzyrzyk;
    private GameObject canvas;
    private int read;
    private int poziom;
    private int punkty;
    private string x;
    private int reverse;
    private int random;
    private int halfreverse;
    public Text txt;
    public Text txtp;
    public Text txtReverse;
    public Text txtRandom;
    public Text txtHalfReverse;
    // Start is called before the first frame update
    void Start()
    {
        Odczyt();
        //Start2();
        
    }
    public void Start2(string level = "SampleScene")
    {
        SceneManager.LoadScene(level);
    }
    public void Start2()
    {
        SceneManager.LoadScene("SampleScene");
    }
    private void Usun()
    {
        canvas = GameObject.FindGameObjectWithTag("Canvas");
        Destroy(canvas);
    }
    // Update is called once per frame
    void Update()
    {
        if(pGame == true)
        {
            plansza.GetComponent<Plansza>().Game();
        }
        if(krzyrzyk == true)
        {
            Kolo.GetComponent<Krzyzowka>().Game();
        }
    }
    //Start a game
    public void KoloKrzyzryk()
    {
        if(punkty >= 100)
        {
            Kolo = Instantiate(KoloPrefab, transform);
            Kolo.GetComponent<Krzyzowka>().GenerateAllTile();
            krzyrzyk = true;
            Usun();
            punkty -= 100;
            Debug.Log(punkty);
            Kolo.transform.position += new Vector3(0f, 30f, -65.3f);
            Zapis();
        }
    }
    public void Standard()
    {
        plansza = Instantiate(planszaPrefab, transform);
        plansza.GetComponent<Plansza>().Awak();
        plansza.GetComponent<Plansza>().GenerateAllTile(6, 8, 8);
        plansza.GetComponent<Plansza>().GenerateAllPieces();
        plansza.GetComponent<Plansza>().StandardPositionPieces();
        pGame = true;
        plansza.transform.position += new Vector3(0, 40, -80f);
        //plansza.transform.rotation = Quaternion.Euler(0, 0, 0);s
        Usun();
    }
    public void Random()
    {
        if (random == 2)
        {
            plansza = Instantiate(planszaPrefab, transform);
            plansza.GetComponent<Plansza>().Awak();
            plansza.GetComponent<Plansza>().GenerateAllTile(6, 8, 8);
            plansza.GetComponent<Plansza>().GenerateAllPieces();
            plansza.GetComponent<Plansza>().RandomPositionPieces();
            pGame = true;
            plansza.transform.position += new Vector3(0, 40, -80f);
            Usun();
        }
        else
        {
            if (punkty >= 1000)
            {
                random = 2;
                punkty = punkty - 1000;
                Zapis();
            }
            else
            {
                Debug.Log("You Don't Have Enought Points");
            }
        }
    }
    public void Reverse()
    {
        if (reverse == 2)
        {
            plansza = Instantiate(planszaPrefab, transform);
            plansza.GetComponent<Plansza>().Awak();
            plansza.GetComponent<Plansza>().GenerateAllTile(6, 8, 8);
            plansza.GetComponent<Plansza>().GenerateAllPieces();
            plansza.GetComponent<Plansza>().ReversPosition();
            pGame = true;
            plansza.transform.position += new Vector3(0, 40, -80f);
            Usun();
        }
        else
        {
            if (punkty >= 100)
            {
                reverse = 2;
                punkty = punkty - 100;
                Zapis();
            }
            else
            {
                Debug.Log("You Don't Have Enought Points");
            }
        }
    }
    public void HalfReverse()
    {
        if (halfreverse == 2)
        {
            plansza = Instantiate(planszaPrefab, transform);
            plansza.GetComponent<Plansza>().Awak();
            plansza.GetComponent<Plansza>().GenerateAllTile(6, 8, 8);
            plansza.GetComponent<Plansza>().GenerateAllPieces();
            plansza.GetComponent<Plansza>().HalfReversPosition();
            pGame = true;
            plansza.transform.position += new Vector3(0, 40, -80f);
            Usun();
        }
        else
        {
            if(punkty >= 10000)
            {
                halfreverse = 2;
                punkty = punkty - 10000;
                Zapis();
            }
            else
            {
                Debug.Log("You Don't Have Enough Points");
            }
        }
    }
    //Zapis i odczyt
    public void Odczyt()
    {
        
        if(File.Exists(Application.persistentDataPath+"/poziom.txt"))
        {
            Debug.Log(Application.persistentDataPath+"/poziom.txt");
            StreamReader reader = new StreamReader(Application.persistentDataPath+"/poziom.txt");
            string text = reader.ReadToEnd();
            while(text[read] != ';')
            {
                x += text[read];
                poziom = int.Parse(x);
                read += 1;
            }
            read++;
            x = "";
            while (text[read] != ';')
            {
                x += text[read];
                punkty = int.Parse(x);
                read += 1;
            }
            read++;
            x = "";
            while (text[read] != ';')
            {
                x += text[read];
                reverse = int.Parse(x);
                read += 1;
            }
            read++;
            x = "";
            while (text[read] != ';')
            {
                x += text[read];
                random = int.Parse(x);
                read += 1;
            }
            read++;
            x = "";
            while (text[read] != ';')
            {
                x += text[read];
                halfreverse = int.Parse(x);
                read += 1;
            }
            txt.text = '$'+punkty.ToString();
            txtp.text = "POZIOM: "+poziom.ToString();

            if (reverse != 2) { txtReverse.text = "$100"; }
            else { txtReverse.text = "Reverse"; }

            if (random != 2) { txtRandom.text = "$1000"; }
            else { txtRandom.text = "Random"; }

            if (halfreverse != 2) { txtHalfReverse.text = "$10000"; }
            else { txtHalfReverse.text = "Half Reverse"; }
        }
        else{
            using (StreamWriter sw = File.CreateText(Application.persistentDataPath+"/poziom.txt"))
            {
                sw.WriteLine("0;0;0;0;0;");
            }
        }
    }
   public void Zapis()
    {
        string y = poziom + ";" + punkty + ";" + reverse + ";" + random + ";" + halfreverse + ";";
        if(File.Exists(Application.persistentDataPath+"/poziom.txt"))
        {
            StreamWriter writer = new StreamWriter(Application.persistentDataPath+"/poziom.txt");
            Debug.Log(y);
            writer.WriteLine(y);
            writer.Close();
        }
        else{
            using (StreamWriter sw = File.CreateText(Application.persistentDataPath+"/poziom.txt"))
            {
                sw.WriteLine(y);
            }
        }
    }
    public void Wygraj(int punktowanie)
    {
        punkty += punktowanie;
        poziom++;
        Zapis();
    }
}