using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Plansza : MonoBehaviour
{

    private int x = 8;
    private int y = 8;
    private GameObject[,] tiles;
    private Camera currentCamera;
    public Material material;
    public Material material2;
    public Material material3;
    private GameObject boty;
    public GameObject[] prefabs;
    public Material[] materials;
    public Pionki[,] pionki;
    private Bot bot;
    private bool tura = false;
    public GameObject kula;
    public int my;
    public int oni;
    private bool zbicie;
    private bool wygrana;
    private bool przejecie;
    private bool trzymanie;
    public GameObject czastki;
    private GameObject gameController;
    private GameObject plansza;
    private int ptk;

    public void Awak()
    {
        boty = GameObject.FindGameObjectWithTag("Bot");
        bot = boty.GetComponent<Bot>();
        gameController = GameObject.FindGameObjectWithTag("GameController");
        //plansza = GameObject.FindGameObjectWithTag("Plansza");
        
    }
    public void Game()
    {
      if (oni <= 0 && wygrana == false)
        {
            Debug.Log("Wygrales");
            Usun();
            Debug.Log("Wygrywasz");
            wygrana = true;
            my = 8;
            oni = 8;
            gameController.GetComponent<GameManager>().Wygraj(ptk);
            gameController.GetComponent<GameManager>().Start2("Win");
            //SceneManager.LoadScene("WIN");
        }
        if (my <= 0 && wygrana == false)
        {
            Debug.Log("Przegrales");
            Usun();
            wygrana = true;
            my = 8;
            oni = 8;
            gameController.GetComponent<GameManager>().Start2("Lose");
            //SceneManager.LoadScene("LOSE");
        }
        GameMeneger();
        for (int i = 0; i <= 7; i++)
        {
            for (int m = 0; m <= 7; m++)
            {
                int j = 7;
                int n = 7;
                if (pionki[i, j] != null && pionki[m, n] != null && m != i)
                {
                    if (pionki[i, j].currentX == pionki[m, n].currentX && pionki[i, j].currentY == pionki[m, n].currentY && pionki[i, j].id != pionki[m, n].id)
                    {
                        StartCoroutine(Zbicie(pionki[i, j]));
                    }
                }
                    
            }
        }
        SetPrzejecie();
        for (int i = 0; i <= 7; i++)
        {
            for (int j = 0; j <= 7; j++)
            {
                if (pionki[i, j] != null)
                {
                    if (pionki[i, j].zbicie == true && pionki[i, j].zbity == false)
                    {
                        if (pionki[i, j].team == 0) my -= 1;
                        if (pionki[i, j].team == 1) oni -= 1;
                        pionki[i, j].zbity = true;
                    }
                }
            }
        }
    }
    
        
    //Generate Board
    public void GenerateAllTile(float tileSize, int x, int y)
    {
        tiles = new GameObject[x, y];
        for (int i = 0; i < x; i++)
        {
            for (int j = 0; j < y; j++)
            {
                tiles[i, j] = GenerateSingleTile(tileSize, j, i);
                tiles[i, j].transform.position = new Vector3(j, 0, i);
            }
        }
    }
    private GameObject GenerateSingleTile(float tileSize, int tx, int ty)
    {
        GameObject tileObject = new GameObject(string.Format("{0}{1}", tx, ty));
        tileObject.transform.parent = transform;
        Mesh mesh = new Mesh();
        tileObject.AddComponent<MeshFilter>().mesh = mesh;
        tileObject.AddComponent<MeshRenderer>().material = material;


        Vector3[] vertices = new Vector3[4];
        vertices[0] = new Vector3(tx * tileSize, 1, ty * tileSize);
        vertices[1] = new Vector3((tx + 1) * tileSize, 1, ty * tileSize);
        vertices[2] = new Vector3(tx * tileSize, 1, (ty + 1) * tileSize);
        vertices[3] = new Vector3((tx + 1) * tileSize, 1, (ty + 1) * tileSize);

        int[] tris = new int[] { 0, 2, 1, 2, 3, 1 };

        mesh.vertices = vertices;
        mesh.triangles = tris;
        mesh.RecalculateNormals();

        tileObject.AddComponent<BoxCollider>();
       

        return tileObject;
    }
    //Generate Pieces
    public void GenerateAllPieces()
    {
        pionki = new Pionki[x, y];

        int niebiescy = 0, czerwoni = 1;
        //Niebiescy
        pionki[1, 0] = GenerateSinglePieces(Pionek.armata, niebiescy);
        pionki[1, 0].id = 1;
        pionki[1, 0].transform.rotation = Quaternion.Euler(-45, 0, -90);
        pionki[2, 0] = GenerateSinglePieces(Pionek.zatrzymujacy, niebiescy);
        pionki[2, 0].id = 2;
        pionki[3, 0] = GenerateSinglePieces(Pionek.przejmujacy, niebiescy);
        pionki[3, 0].id = 3;
        pionki[4, 0] = GenerateSinglePieces(Pionek.rozwalacz, niebiescy);
        pionki[4, 0].id = 4;
        pionki[5, 0] = GenerateSinglePieces(Pionek.szybki, niebiescy);
        pionki[5, 0].id = 5;
        pionki[6, 0] = GenerateSinglePieces(Pionek.pionek, niebiescy);
        pionki[6, 0].id = 6;

        //Czerwoni
        pionki[1, 7] = GenerateSinglePieces(Pionek.pionek, czerwoni);
        pionki[1, 7].id = 9;
        pionki[2, 7] = GenerateSinglePieces(Pionek.zatrzymujacy, czerwoni);
        pionki[2, 7].id = 10;
        pionki[3, 7] = GenerateSinglePieces(Pionek.przejmujacy, czerwoni);
        pionki[3, 7].id = 11;
        pionki[4, 7] = GenerateSinglePieces(Pionek.rozwalacz, czerwoni);
        pionki[4, 7].id = 12;
        pionki[5, 7] = GenerateSinglePieces(Pionek.szybki, czerwoni);
        pionki[5, 7].id = 13;
        pionki[6, 7] = GenerateSinglePieces(Pionek.pionek, czerwoni);
        pionki[6, 7].id = 14;
    }
    private Pionki GenerateSinglePieces(Pionek type, int team)
    {
        Pionki cp = Instantiate(prefabs[(int)type], transform).GetComponent<Pionki>();
        cp.type = type;
        cp.team = team;
        cp.GetComponent<MeshRenderer>().material = materials[team];

        return cp;
    }
 
    
   
    //Position
    public void StandardPositionPieces()
    {
        for (int i = 0; i <= 6; i++)
        {
            for (int j = 0; j <= 7; j++)
            {
                if (pionki[i, j] != null && tiles[i, j] != null)
                {
                    PositionSinglePieces(i, j, i, j, true);
                }
            }
        }
        pionki[2, 0].transform.position = new Vector3(19, 2, 3);
        pionki[2, 0].transform.rotation = Quaternion.Euler(0, 90, 0);
        ptk = 10;
    }
    public void RandomPositionPieces()
    {
        for (int i = 0; i <= 7; i++)
        {
            if (pionki[i, 0] != null && tiles[i, 0] != null)
            {
                PositionSinglePieces(i, 0, i, 0, true);
            }
        }
        for (int i = 0; i <= 7; i++)
        {
            int l = Random.Range(1, 7);
            int l2 = Random.Range(1, 7);
            if (pionki[i, 7] != null && tiles[l, l2] != null)
            {
                PositionSinglePieces(i, 7, l, l2, true);
            }
        }
        pionki[2, 0].transform.position = new Vector3(19, 2, 3);
        pionki[2, 0].transform.rotation = Quaternion.Euler(0, 90, 0);
        ptk = 100;
      
    }
    public void HalfReversPosition()
    {
        for (int i = 0; i <= 7; i++)
        {
            for (int j = 0; j <= 7; j++)
            {
                if (pionki[j, i] != null && tiles[j, i] != null)
                {
                    PositionSinglePieces(j, i, i, j, true);
                }
            }
        }
        ptk = 150;
      
    }
    public void ReversPosition()
    {
        for (int i = 0; i <= 7; i++)
        {
            if (pionki[i, 0] != null && tiles[i, 0] != null)
            {
                PositionSinglePieces(i, 0, i, 7, true);
            }
        }
        for (int i = 0; i <= 7; i++)
        {
            if (pionki[i, 7] != null && tiles[i, 7] != null)
            {
                PositionSinglePieces(i, 7, i, 0, true);
            }
        }
        ptk = 50;
        
    }
    private void PositionSinglePieces(int x, int y, int x2, int y2, bool force = false)
    {
        pionki[x, y].currentX = x2;
        pionki[x, y].currentY = y2;
        pionki[x, y].transform.position = new Vector3(x2 * 7f + 2.8f, 5, y2 * 7f + 2.8f);
        
    }
    //Ruch Pionka
    private void CzyKlikniety()
    {
        if (Input.touchCount == 1)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;// Klasa do zczytywania pozycji klikniecia

            if (Physics.Raycast(ray, out hit))
            {
                //Debug.Log(hit.transform.name);
                string x = "0" + hit.transform.name[1];//Musz� by� dwa znaki aby by� string do sprawdzenia
                string y = "0" + hit.transform.name[0];
                for (int i = 0; i <= 7; i++)
                {
                    for (int j = 0; j <= 6; j++)
                    {
                        if (pionki[i, j] != null && tiles[i, j] != null)
                        {
                            
                            if (int.Parse(y) == pionki[i, j].currentX && int.Parse(x) == pionki[i, j].currentY)
                            {
                                Debug.Log(pionki[i, j].name);
                                //Flagi
                                if (pionki[i, j].zaznaczony == 1)
                                {
                                    pionki[i, j].zaznaczony = 2;
                                    if(i == 6)
                                    {
                                        pionki[i, j].zaznaczony = 1;
                                        Debug.Log("Powinno dzialac");
                                    }
                                }
                                //if (pionki[i, j].name == "Pionek(Clone)") pionki[i, j].zaznaczony = 1;
                                else
                                {
                                    pionki[i, j].zaznaczony = 1;
                                }
                            }
                        }

                    }
                }
            }
        }

    }
    /*private void StartCortuine(StartCoroutine(CzyKlikniety());)
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;// Klasa do zczytywania pozycji klikniecia

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.name);
                string x = "0" + hit.transform.name[1];//Musz� by� dwa znaki aby by� string do sprawdzenia
                string y = "0" + hit.transform.name[0];
                for (int i = 0; i <= 7; i++)
                {
                    for (int j = 0; j <= 7; j++)
                    {
                        if (pionki[i, j] != null && tiles[i, j] != null)
                        {
                            //Debug.Log(pionki[i, j].name);
                            if (int.Parse(y) == pionki[i, j].currentX && int.Parse(x) == pionki[i, j].currentY)
                            {
                                //Flagi
                                if (pionki[i, j].zaznaczony == 1)
                                {
                                    pionki[i, j].zaznaczony = 2;
                                }
                                else
                                {
                                    pionki[i, j].zaznaczony = 1;
                                }
                            }
                        }

                    }

                }
            }
        }
    }*/
    public IEnumerator Ruch(Pionki p, int x, int y)
    {
        for (int i = 0; i <= 20; i++)
        {
            if(p != null)
                p.transform.position += new Vector3(x * 0.048f, 0, y * 0.048f);
                yield return new WaitForSeconds(0.02f);
        }
    }
    private IEnumerator Zbicie(Pionki p)
    {
        GameObject c = Instantiate(czastki);
        if(p.team == 0)c.GetComponent<ParticleSystemRenderer>().material = materials[0];
        if(p.team == 1)c.GetComponent<ParticleSystemRenderer>().material = materials[1];
        c.transform.position = p.transform.position;
        p.zbicie = true;
        Debug.Log(p);
        Destroy(p);
        Destroy(p.GetComponent<MeshRenderer>());
        Destroy(p.GetComponent<MeshFilter>());
        Debug.Log(p);
        yield return new WaitForSeconds(5);
        Destroy(c);
    }
    private void ChangePosition(Pionki p, int x, int y)
    {
        if (p.currentX <= 8 && p.currentY <= 8 && p.trzymanie == 0)
        {
            p.currentX = p.currentX + x;
            p.currentY = p.currentY + y;
            for (int i = 0; i <= 6; i++)
            {
                StartCoroutine(Ruch(p, x, y));
                //p.transform.position = p.transform.position + new Vector3(x, 0, y);
                
                tura = false;
                //Time.fixedDeltaTime = 0.5f;
            }

        }
        for (int m = 0; m <= 7; m++)
        {
            for (int n = 0; n <= 7; n++)
            {
                if (p != null && pionki[m, n] != null)
                {
                    if (p.currentX == pionki[m, n].currentX && p.currentY == pionki[m, n].currentY && p.id != pionki[m, n].id)
                    {
                        if (pionki[m, n].trzymanie != 0)
                        {
                            if (p.currentX == pionki[m, n].currentX && p.currentY == pionki[m, n].currentY && p.id != pionki[m, n].id)
                            {
                                StartCoroutine(Zbicie(p));
                            }
                        }
                        else
                        {
                            StartCoroutine(Zbicie(pionki[m, n]));
                        }
                    }
                }
            }
        }
        for (int m = 0; m <= 7; m++)
        {
            for (int n = 0; n <= 7; n++)
            {
                if (pionki[m, n] != null && tiles[m, n] != null)
                {
                    if (p.przejmowanie != 0 && pionki[m, n].przejecie != 0)
                    {
                        if (pionki[m, n].currentX + x <= 7 && pionki[m, n].currentY + y <= 7 && pionki[m, n].currentX + x >= 0 && pionki[m, n].currentY + y >= 0)
                        {
                            pionki[m, n].currentX = pionki[m, n].currentX + x;
                            pionki[m, n].currentY = pionki[m, n].currentY + y;
                            for (int i = 0; i <= 6; i++)
                            {
                                pionki[m, n].transform.position = pionki[m, n].transform.position + new Vector3(x, 0, y);
                            }
                        }
                    }
                }
            }
        }

        
    }
    private void ChangeColor(int ax, int ay, Material materiales)
    {
        if (ax > -1 && ay > -1 && ax <= 7 && ay <= 7 && tiles[ay, ax] != null)
        {
            tiles[ay, ax].GetComponent<MeshRenderer>().material = materiales;
        }
    }
    private bool CzyWybrany(int x, int y)
    {
        if (Input.touchCount == 1)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;// Klasa do zczytywania pozycji klikniecia

            if (Physics.Raycast(ray, out hit))
            {
                string xy = x.ToString() + y.ToString();
                if (hit.transform.name == xy)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    private void Przejecie(int i, int j, int x, int y)
    {
        
        if (CzyWybrany(pionki[i, j].currentX + x, pionki[i, j].currentY + y))
        {
            for (int m = 0; m <= 7; m++)
            {
                for (int n = 0; n <= 7; n++)
                {
                    if (pionki[m, n] != null && tiles[m, n] != null)
                    {
                        if (pionki[i, j].currentX + x == pionki[m, n].currentX && (pionki[i, j].currentY + y) == pionki[m, n].currentY)
                        {
                            pionki[i, j].przejmowanie = 3;
                            pionki[m, n].GetComponent<MeshRenderer>().material = material3; 
                            pionki[m, n].przejecie = 3;
                            Debug.Log(pionki[m, n].przejecie);
                        }
                    }
                }
            }
        }
    }
    private void SetPrzejecie()
    {
       for(int i = 0; i <= 7; i++)
        {
            for(int j = 0; j <= 7; j++)
            {
                if(pionki[i, j] != null && tiles[i, j] != null)
                {
                    if(pionki[i, j].trzymanie != 0 && trzymanie == false)
                    {
                        pionki[i, j].trzymanie -= 1;
                        Debug.Log(pionki[i, j].trzymanie);
                        trzymanie = true;
                        if (pionki[i, j].trzymanie == 0)
                        {
                            StartCoroutine(AnimacjaCzepiania(pionki[i, j], -1));
                        }
                    }
                    
                }
            }
        }
        
    }
    private void Usun()
    {
        for(int i = 0; i<=7; i++)
        {
            for(int j = 0; j<=7; j++)
            {
                if(pionki[i, j] != null)
                {
                    
                    Destroy(pionki[i, j]);
                    Destroy(pionki[i, j].GetComponent<MeshRenderer>());
                       Destroy(pionki[i, j].GetComponent<MeshFilter>());
                
                }
                if(tiles[i, j] != null) Destroy(tiles[i, j]);

            }
        }
        //Destroy(plansza);
    }
    public IEnumerator AnimacjaKuli(GameObject k)
    {
        for (int i = 0; i <= 20; i++)
        {
            k.transform.position += new Vector3(0, 0.45f, 0.65f);
            yield return new WaitForSeconds(0.02f);
        }
        for (int i = 0; i <= 20; i++)
        {
            k.transform.position += new Vector3(0, -0.45f, 0.65f);
            yield return new WaitForSeconds(0.02f);
        }
        Destroy(k);
    }
    public IEnumerator AnimacjaKuli(GameObject k, Pionki p)
    {
        for (int i = 0; i <= 20; i++)
        {
            k.transform.position += new Vector3(0, 0.45f, 0.65f);
            yield return new WaitForSeconds(0.02f);
        }
        for (int i = 0; i <= 20; i++)
        {
            k.transform.position += new Vector3(0, -0.45f, 0.65f);
            yield return new WaitForSeconds(0.02f);
        }
        Destroy(k);
        StartCoroutine(Zbicie(p));

    }
    public IEnumerator AnimacjaCzepiania(Pionki p, int pm)
    {
        float y = 2.25f;
        for (int i = 0; i <= 20; i++)
        {
            p.transform.position += new Vector3(0, -0.045f*pm, 0);
            p.transform.rotation = Quaternion.Euler(-90, y*pm, 0);
            yield return new WaitForSeconds(0.02f);
            y += 2.25f;
        }
        
    }
    public IEnumerator AnimacjaRozwalania(Pionki p)
    {
        float x = 6;
        for(int i = 0; i<=15; i++)
        {
            p.transform.rotation = Quaternion.Euler(-89.98f + x, 0, 0);
            yield return new WaitForSeconds(0.02f);
            x += 6;
        }
        Destroy(tiles[p.currentY + 1, p.currentX]);
        Debug.Log(tiles[p.currentY + 1, p.currentX]);
        tiles[p.currentY + 1, p.currentX] = null;
        Debug.Log(tiles[p.currentY + 1, p.currentX]);
        x = 6;
        for (int i = 0; i <= 15; i++)
        {
            p.transform.rotation = Quaternion.Euler(0 - x, 0, 0);
            yield return new WaitForSeconds(0.02f);
            x += 6;
        }
    }
 
    private void GameMeneger()
    {
            if (wygrana == false)
            {
                if (tura == true)
                {
                    int x, y;
                    CzyKlikniety();
                    for (int n = 0; n <= 7; n++)
                    {
                        for (int m = 0; m <= 7; m++)
                        {
                            ChangeColor(n, m, material);
                        }
                    }
                    for (int i = 0; i <= 6; i++)
                    {
                        int j = 0;
                        if (pionki[i, j] != null && tiles[i, j] != null)
                        {
                            
                            if (pionki[i, j].zaznaczony == 1 && pionki[i, j].trzymanie <= 1)
                            {
                                x = pionki[i, j].currentX;
                                y = pionki[i, j].currentY;
                                for (int n = 0; n <= 7; n++)
                                {
                                    for (int m = 0; m <= 7; m++)
                                    {
                                        if (pionki[m, n] != null)
                                        {
                                            pionki[m, n].zaznaczony = 0;
                                        }
                                    }
                                }
                                pionki[i, j].zaznaczony = 1;
                                ChangeColor(x + 1, y + 1, material2);
                                if (CzyWybrany(x + 1, y + 1) == true)
                                {
                                    ChangePosition(pionki[i, j], 1, 1);
                                    for (int n = 0; n <= 7; n++)
                                    {
                                        for (int m = 0; m <= 7; m++)
                                        {
                                            ChangeColor(n, m, material);
                                            if (pionki[m, n] != null)
                                            {
                                                pionki[m, n].zaznaczony = 0;
                                            }
                                        }
                                    }
                                }

                                ChangeColor(x, y + 1, material2);
                                if (CzyWybrany(x, y + 1) == true)
                                {
                                    ChangePosition(pionki[i, j], 0, 1);
                                    for (int n = 0; n <= 7; n++)
                                    {
                                        for (int m = 0; m <= 7; m++)
                                        {
                                            ChangeColor(n, m, material);
                                            if (pionki[m, n] != null)
                                            {
                                                pionki[m, n].zaznaczony = 0;
                                            }
                                        }
                                    }
                                }

                                ChangeColor(x + 1, y, material2);
                                if (CzyWybrany(x + 1, y) == true)
                                {
                                    ChangePosition(pionki[i, j], 1, 0);
                                    for (int n = 0; n <= 7; n++)
                                    {
                                        for (int m = 0; m <= 7; m++)
                                        {
                                            ChangeColor(n, m, material);
                                            if (pionki[m, n] != null)
                                            {
                                                pionki[m, n].zaznaczony = 0;
                                            }
                                        }
                                    }
                                }

                                ChangeColor(x - 1, y, material2);
                                if (CzyWybrany(x - 1, y) == true)
                                {
                                    ChangePosition(pionki[i, j], -1, 0);
                                    for (int n = 0; n <= 7; n++)
                                    {
                                        for (int m = 0; m <= 7; m++)
                                        {
                                            ChangeColor(n, m, material);
                                            if (pionki[m, n] != null)
                                            {
                                                pionki[m, n].zaznaczony = 0;
                                            }
                                        }
                                    }
                                }

                                ChangeColor(x - 1, y + 1, material2);
                                if (CzyWybrany(x - 1, y + 1) == true)
                                {
                                    ChangePosition(pionki[i, j], -1, 1);
                                    for (int n = 0; n <= 7; n++)
                                    {
                                        for (int m = 0; m <= 7; m++)
                                        {
                                            ChangeColor(n, m, material);
                                            if (pionki[m, n] != null)
                                            {
                                                pionki[m, n].zaznaczony = 0;
                                            }
                                        }
                                    }
                                }

                                ChangeColor(x - 1, y - 1, material2);
                                if (CzyWybrany(x - 1, y - 1) == true)
                                {
                                    ChangePosition(pionki[i, j], -1, -1);
                                    for (int n = 0; n <= 7; n++)
                                    {
                                        for (int m = 0; m <= 7; m++)
                                        {
                                            ChangeColor(n, m, material);
                                            if (pionki[m, n] != null)
                                            {
                                                pionki[m, n].zaznaczony = 0;
                                            }
                                        }
                                    }

                                }

                                ChangeColor(x, y - 1, material2);
                                if (CzyWybrany(x, y - 1) == true)
                                {
                                    ChangePosition(pionki[i, j], 0, -1);
                                    for (int n = 0; n <= 7; n++)
                                    {
                                        for (int m = 0; m <= 7; m++)
                                        {
                                            ChangeColor(n, m, material);
                                            if (pionki[m, n] != null)
                                            {
                                                pionki[m, n].zaznaczony = 0;
                                            }
                                        }
                                    }

                                }
                                ChangeColor(x + 1, y - 1, material2);
                                if (CzyWybrany(x + 1, y - 1) == true)
                                {
                                    ChangePosition(pionki[i, j], 1, -1);
                                    for (int n = 0; n <= 7; n++)
                                    {
                                        for (int m = 0; m <= 7; m++)
                                        {
                                            ChangeColor(n, m, material);
                                            if (pionki[m, n] != null)
                                            {
                                                pionki[m, n].zaznaczony = 0;
                                            }
                                        }
                                    }
                                }
                            }

                            if (pionki[i, j].zaznaczony == 2 && pionki[i, j].trzymanie <= 1)
                            {
                                if (pionki[i, j].name == "Armata(Clone)")
                                {
                                    ChangeColor(pionki[i, j].currentX, pionki[i, j].currentY + 4, material2);
                                    if (CzyWybrany(pionki[i, j].currentX, pionki[i, j].currentY + 4))
                                    {
                                        pionki[i, j].zaznaczony = 0;
                                        for (int m = 0; m <= 7; m++)
                                        {
                                            for (int n = 0; n <= 7; n++)
                                            {
                                                if (pionki[m, n] != null)
                                                {
                                                    GameObject kola = Instantiate(kula);
                                                    kola.transform.position = pionki[i, j].transform.position;
                                                    if (pionki[i, j].currentX == pionki[m, n].currentX && (pionki[i, j].currentY + 4) == pionki[m, n].currentY)
                                                    {
                                                        StartCoroutine(AnimacjaKuli(kola, pionki[m, n]));
                                                        pionki[m, n].zbicie = true;
                                                        Debug.Log("OKej");
                                                        //Debug.Log(pionki[m, n]);
                                                        //Debug.Log(pionki[m, n]);
                                                        
                                                    }
                                                    else
                                                    {
                                                        StartCoroutine(AnimacjaKuli(kola));
                                                    }
                                                }
                                            }
                                        }
                                    }


                                }
                                else if (pionki[i, j].name == "Czepiacz(Clone)")
                                {
                                    if (pionki[i, j].trzymanie == 0)
                                    {
                                        ChangeColor(pionki[i, j].currentX, pionki[i, j].currentY+1, material2);
                                    }
                                    if (CzyWybrany(pionki[i, j].currentX, pionki[i, j].currentY+1) == true)
                                    {
                                        StartCoroutine(AnimacjaCzepiania(pionki[i, j], 1));
                                    ChangePosition(pionki[i, j], 0, 1);
                                        pionki[i, j].trzymanie = 4;
                                        Debug.Log(pionki[i, j].trzymanie);
                                    }

                            }
                                else if (pionki[i, j].name == "Przejmowanie(Clone)")
                                {
                                    ChangeColor(pionki[i, j].currentX + 3, pionki[i, j].currentY, material2);
                                    Przejecie(i, j, 3, 0);
                                    ChangeColor(pionki[i, j].currentX + 3, pionki[i, j].currentY + 3, material2);
                                    Przejecie(i, j, 3, 3);
                                    ChangeColor(pionki[i, j].currentX - 3, pionki[i, j].currentY, material2);
                                    Przejecie(i, j, -3, 0);
                                    ChangeColor(pionki[i, j].currentX - 3, pionki[i, j].currentY + 3, material2);
                                    Przejecie(i, j, -3, 3);
                                    ChangeColor(pionki[i, j].currentX, pionki[i, j].currentY + 3, material2);
                                    Przejecie(i, j, 0, 3);
                                    ChangeColor(pionki[i, j].currentX, pionki[i, j].currentY - 3, material2);
                                    Przejecie(i, j, 0, -3);
                                    ChangeColor(pionki[i, j].currentX + 3, pionki[i, j].currentY - 3, material2);
                                    Przejecie(i, j, 3, -3);
                                    ChangeColor(pionki[i, j].currentX - 3, pionki[i, j].currentY - 3, material2);
                                    Przejecie(i, j, -3, -3);
                                }
                                else if (pionki[i, j].name == "Rozwalacz(Clone)")
                                {
                                    ChangeColor(pionki[i, j].currentX, pionki[i, j].currentY + 1, material2);
                                    if (CzyWybrany(pionki[i, j].currentX, pionki[i, j].currentY + 1) == true)
                                    {
                                        for (int m = 0; m <= 7; m++)
                                        {

                                            for (int n = 0; n <= 7; n++)
                                            {
                                                if (pionki[m, n] != null)
                                                {
                                                    if (pionki[i, j].currentX == pionki[m, n].currentX && (pionki[i, j].currentY + 1) == pionki[m, n].currentY)
                                                    {

                                                        pionki[m, n].zbicie = true;
                                                        StartCoroutine(AnimacjaRozwalania(pionki[i, j]));
                                                        StartCoroutine(Zbicie(pionki[m, n]));
                                                        Debug.Log("OKej");
                                                        //Debug.Log(pionki[m, n]);
                                                        //Debug.Log(pionki[m, n]);

                                                    }
                                                }
                                                else
                                                {
                                                    StartCoroutine(AnimacjaRozwalania(pionki[i, j]));
                                                }
                                            }
                                        }
                                       
                                    }
                                }
                                else if (pionki[i, j].name == "Szybki(Clone)")
                                {
                                    x = pionki[i, j].currentX;
                                    y = pionki[i, j].currentY;
                                    for (int n = 0; n <= 7; n++)
                                    {
                                        for (int m = 0; m <= 7; m++)
                                        {
                                            if (pionki[m, n] != null)
                                            {
                                                pionki[m, n].zaznaczony = 0;
                                            }
                                        }
                                    }
                                    pionki[i, j].zaznaczony = 2;
                                    ChangeColor(x + 2, y + 2, material2);
                                    if (CzyWybrany(x + 2, y + 2) == true)
                                    {
                                        ChangePosition(pionki[i, j], 2, 2);
                                        for (int n = 0; n <= 7; n++)
                                        {
                                            for (int m = 0; m <= 7; m++)
                                            {
                                                ChangeColor(n, m, material);
                                                if (pionki[m, n] != null)
                                                {
                                                    pionki[m, n].zaznaczony = 0;
                                                }
                                            }
                                        }
                                    }

                                    ChangeColor(x, y + 2, material2);
                                    if (CzyWybrany(x, y + 2) == true)
                                    {
                                        ChangePosition(pionki[i, j], 0, 2);
                                        for (int n = 0; n <= 7; n++)
                                        {
                                            for (int m = 0; m <= 7; m++)
                                            {
                                                ChangeColor(n, m, material);
                                                if (pionki[m, n] != null)
                                                {
                                                    pionki[m, n].zaznaczony = 0;
                                                }
                                            }
                                        }
                                    }

                                    ChangeColor(x + 2, y, material2);
                                    if (CzyWybrany(x + 2, y) == true)
                                    {
                                        ChangePosition(pionki[i, j], 2, 0);
                                        for (int n = 0; n <= 7; n++)
                                        {
                                            for (int m = 0; m <= 7; m++)
                                            {
                                                ChangeColor(n, m, material);
                                                if (pionki[m, n] != null)
                                                {
                                                    pionki[m, n].zaznaczony = 0;
                                                }
                                            }
                                        }
                                    }

                                    ChangeColor(x - 2, y, material2);
                                    if (CzyWybrany(x - 2, y) == true)
                                    {
                                        ChangePosition(pionki[i, j], -2, 0);
                                        for (int n = 0; n <= 7; n++)
                                        {
                                            for (int m = 0; m <= 7; m++)
                                            {
                                                ChangeColor(n, m, material);
                                                if (pionki[m, n] != null)
                                                {
                                                    pionki[m, n].zaznaczony = 0;
                                                }
                                            }
                                        }
                                    }

                                    ChangeColor(x - 2, y + 2, material2);
                                    if (CzyWybrany(x - 2, y + 2) == true)
                                    {
                                        ChangePosition(pionki[i, j], -2, 2);
                                        for (int n = 0; n <= 7; n++)
                                        {
                                            for (int m = 0; m <= 7; m++)
                                            {
                                                ChangeColor(n, m, material);
                                                if (pionki[m, n] != null)
                                                {
                                                    pionki[m, n].zaznaczony = 0;
                                                }
                                            }
                                        }
                                    }

                                    ChangeColor(x - 2, y - 2, material2);
                                    if (CzyWybrany(x - 2, y - 2) == true)
                                    {
                                        ChangePosition(pionki[i, j], -2, -2);
                                        for (int n = 0; n <= 7; n++)
                                        {
                                            for (int m = 0; m <= 7; m++)
                                            {
                                                ChangeColor(n, m, material);
                                                if (pionki[m, n] != null)
                                                {
                                                    pionki[m, n].zaznaczony = 0;
                                                }
                                            }
                                        }

                                    }

                                    ChangeColor(x, y - 2, material2);
                                    if (CzyWybrany(x, y - 2) == true)
                                    {
                                        ChangePosition(pionki[i, j], 0, -2);
                                        for (int n = 0; n <= 7; n++)
                                        {
                                            for (int m = 0; m <= 7; m++)
                                            {
                                                ChangeColor(n, m, material);
                                                if (pionki[m, n] != null)
                                                {
                                                    pionki[m, n].zaznaczony = 0;
                                                }
                                            }
                                        }

                                    }
                                    ChangeColor(x + 2, y - 2, material2);
                                    if (CzyWybrany(x + 2, y - 2) == true)
                                    {
                                        ChangePosition(pionki[i, j], 2, -2);
                                        for (int n = 0; n <= 7; n++)
                                        {
                                            for (int m = 0; m <= 7; m++)
                                            {
                                                ChangeColor(n, m, material);
                                                if (pionki[m, n] != null)
                                                {
                                                    pionki[m, n].zaznaczony = 0;
                                                }
                                            }
                                        }
                                    }
                                }
                                else if(pionki[i, j].name == "Pionek(Clone)")
                                {
                                    Debug.Log("Dzialaj");
                                    x = pionki[i, j].currentX;
                                    y = pionki[i, j].currentY;
                                    for (int n = 0; n <= 7; n++)
                                    {
                                        for (int m = 0; m <= 7; m++)
                                        {
                                            if (pionki[m, n] != null)
                                            {
                                                pionki[m, n].zaznaczony = 0;
                                            }
                                        }
                                    }
                                    pionki[i, j].zaznaczony = 1;
                                    ChangeColor(x + 1, y + 1, material2);
                                    if (CzyWybrany(x + 1, y + 1) == true)
                                    {
                                        ChangePosition(pionki[i, j], 1, 1);
                                        for (int n = 0; n <= 7; n++)
                                        {
                                            for (int m = 0; m <= 7; m++)
                                            {
                                                ChangeColor(n, m, material);
                                                if (pionki[m, n] != null)
                                                {
                                                    pionki[m, n].zaznaczony = 0;
                                                }
                                            }
                                        }
                                    }

                                    ChangeColor(x, y + 1, material2);
                                    if (CzyWybrany(x, y + 1) == true)
                                    {
                                        ChangePosition(pionki[i, j], 0, 1);
                                        for (int n = 0; n <= 7; n++)
                                        {
                                            for (int m = 0; m <= 7; m++)
                                            {
                                                ChangeColor(n, m, material);
                                                if (pionki[m, n] != null)
                                                {
                                                    pionki[m, n].zaznaczony = 0;
                                                }
                                            }
                                        }
                                    }

                                    ChangeColor(x + 1, y, material2);
                                    if (CzyWybrany(x + 1, y) == true)
                                    {
                                        ChangePosition(pionki[i, j], 1, 0);
                                        for (int n = 0; n <= 7; n++)
                                        {
                                            for (int m = 0; m <= 7; m++)
                                            {
                                                ChangeColor(n, m, material);
                                                if (pionki[m, n] != null)
                                                {
                                                    pionki[m, n].zaznaczony = 0;
                                                }
                                            }
                                        }
                                    }

                                    ChangeColor(x - 1, y, material2);
                                    if (CzyWybrany(x - 1, y) == true)
                                    {
                                        ChangePosition(pionki[i, j], -1, 0);
                                        for (int n = 0; n <= 7; n++)
                                        {
                                            for (int m = 0; m <= 7; m++)
                                            {
                                                ChangeColor(n, m, material);
                                                if (pionki[m, n] != null)
                                                {
                                                    pionki[m, n].zaznaczony = 0;
                                                }
                                            }
                                        }
                                    }

                                    ChangeColor(x - 1, y + 1, material2);
                                    if (CzyWybrany(x - 1, y + 1) == true)
                                    {
                                        ChangePosition(pionki[i, j], -1, 1);
                                        for (int n = 0; n <= 7; n++)
                                        {
                                            for (int m = 0; m <= 7; m++)
                                            {
                                                ChangeColor(n, m, material);
                                                if (pionki[m, n] != null)
                                                {
                                                    pionki[m, n].zaznaczony = 0;
                                                }
                                            }
                                        }
                                    }

                                    ChangeColor(x - 1, y - 1, material2);
                                    if (CzyWybrany(x - 1, y - 1) == true)
                                    {
                                        ChangePosition(pionki[i, j], -1, -1);
                                        for (int n = 0; n <= 7; n++)
                                        {
                                            for (int m = 0; m <= 7; m++)
                                            {
                                                ChangeColor(n, m, material);
                                                if (pionki[m, n] != null)
                                                {
                                                    pionki[m, n].zaznaczony = 0;
                                                }
                                            }
                                        }

                                    }

                                    ChangeColor(x, y - 1, material2);
                                    if (CzyWybrany(x, y - 1) == true)
                                    {
                                        ChangePosition(pionki[i, j], 0, -1);
                                        for (int n = 0; n <= 7; n++)
                                        {
                                            for (int m = 0; m <= 7; m++)
                                            {
                                                ChangeColor(n, m, material);
                                                if (pionki[m, n] != null)
                                                {
                                                    pionki[m, n].zaznaczony = 0;
                                                }
                                            }
                                        }

                                    }
                                    ChangeColor(x + 1, y - 1, material2);
                                    if (CzyWybrany(x + 1, y - 1) == true)
                                    {
                                        ChangePosition(pionki[i, j], 1, -1);
                                        for (int n = 0; n <= 7; n++)
                                        {
                                            for (int m = 0; m <= 7; m++)
                                            {
                                                ChangeColor(n, m, material);
                                                if (pionki[m, n] != null)
                                                {
                                                    pionki[m, n].zaznaczony = 0;
                                                }
                                            }
                                        }
                                    }
                                }
                            }

   
                        }   

                    }
                }
                else if (tura == false)
                {
                    bot.WylosowanyRuch(pionki, tiles);
                    tura = true;
                    trzymanie = false;
                }
               
            }
    }
}
