using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Krzyzowka : MonoBehaviour
{
    public Material material;
    public Material[] materials;
    private GameObject[,] tiles;
    public bool tura;
    private int[,] pole = new int[3, 3];
    private int u = 0;
    private GameObject gameController;
    private int k = 0;
    private int op = 0;
    private int k2 = 0;
    private int op2 = 0;
    
    public void Game()
    {
        gameController = GameObject.FindGameObjectWithTag("GameController");
        if (tura == false) CzyKlikniety();
        Bot();
        Wygrana();
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
    public void GenerateAllTile()
    {
        tiles = new GameObject[3, 3];
        for (int i = 0; i < 3; i++)
        {
            for (int j = 0; j < 3; j++)
            {
                tiles[i, j] = GenerateSingleTile(20, j, i);
                tiles[i, j].transform.position = new Vector3(j - 3, 0, i);
            }
        }
    }
    private void CzyKlikniety()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;// Klasa do zczytywania pozycji klikniecia

            if (Physics.Raycast(ray, out hit))
            {
                Debug.Log(hit.transform.name);
                for (int i = 0; i <= 2;i++)
                {
                    for (int j = 0; j <= 2; j++)
                    {
                        string n = j.ToString() + i.ToString();
                        if(hit.transform.name == n)
                        {
                            if (pole[j, i] == 0)
                            {
                                //Debug.Log(hit.transform.name+" "+pole[i, j]);
                                ChangeColor(materials[0], hit.transform.name, true);
                                tura = true;
                                op += 1;
                                Debug.Log(k);
                                if (op == 9)
                                {
                                    Usun2();
                                    op = 0;
                                }
                            }
                            
                        }
                    }
                }
                
            }
        }
    }
    private void ChangeColor(Material materiales, string name, bool b)
    {
       for(int i = 0; i<=2; i++)
        {
            for(int j = 0; j<=2; j++)
            {
                string n = i.ToString() + j.ToString();
                if (name == n && tiles[j, i] != null)
                {
                    tiles[j, i].GetComponent<MeshRenderer>().material = materiales;
                    if (b == false) pole[i, j] = -1;
                    if (b == true) pole[i, j] = 1;
                }
            }
        }        
    }
    private void Bot()
    {
        if (tura == true)
        {
            int l1, l2;
            do
            {
                l1 = Random.Range(0, 3);
                l2 = Random.Range(0, 3);
                u += 1;
            }
            while (pole[l1, l2] != 0 || u <= 9);
            ChangeColor(materials[1], l1.ToString() + l2.ToString(), false);
            tura = false;

            op += 1;
            Debug.Log(k);
            if (op == 9)
            {
                Usun2();
                op2 = 0;
            }
            else
            {
                Debug.Log("Dziala");
            }
        }
    }
    private void Wygrana()
    {
        if (pole[0, 0] == 1 && pole[0, 1] == 1 && pole[0, 2] == 1) Usun();
        if (pole[0, 0] == 1 && pole[1, 0] == 1 && pole[2, 0] == 1) Usun();
        if (pole[0, 0] == 1 && pole[1, 1] == 1 && pole[2, 2] == 1) Usun();
        if (pole[0, 1] == 1 && pole[1, 1] == 1 && pole[2, 1] == 1) Usun();
        if (pole[0, 2] == 1 && pole[1, 2] == 1 && pole[2, 2] == 1) Usun();
        if (pole[1, 0] == 1 && pole[1, 1] == 1 && pole[1, 2] == 1) Usun();
        if (pole[0, 2] == 1 && pole[1, 1] == 1 && pole[2, 0] == 1) Usun();
        if (pole[2, 0] == 1 && pole[2, 1] == 1 && pole[2, 2] == 1) Usun();

        if (pole[0, 0] == -1 && pole[0, 1] == -1 && pole[0, 2] == -1) Usun2();
        if (pole[0, 0] == -1 && pole[1, 0] == -1 && pole[2, 0] == -1) Usun2();
        if (pole[0, 0] == -1 && pole[1, 1] == -1 && pole[2, 2] == -1) Usun2();
        if (pole[0, 1] == -1 && pole[1, 1] == -1 && pole[2, 1] == -1) Usun2();
        if (pole[0, 2] == -1 && pole[1, 2] == -1 && pole[2, 2] == -1) Usun2();
        if (pole[1, 0] == -1 && pole[1, 1] == -1 && pole[1, 2] == -1) Usun2();
        if (pole[0, 2] == -1 && pole[1, 1] == -1 && pole[2, 0] == -1) Usun2();
        if (pole[2, 0] == -1 && pole[2, 1] == -1 && pole[2, 2] == -1) Usun2();
    }
    private void Usun()
    {
        for(int i = 0; i <= 2; i++)
        {
            for(int j = 0; j <= 2; j++)
            {
                Destroy(tiles[i, j]);
            }
        }
        gameController.GetComponent<GameManager>().Wygraj(100);
        SceneManager.LoadScene("WIN");
    }
    private void Usun2()
    {
        for (int i = 0; i <= 2; i++)
        {
            for (int j = 0; j <= 2; j++)
            {
                Destroy(tiles[i, j]);
            }
        }
        SceneManager.LoadScene("LOSE");
    }
}