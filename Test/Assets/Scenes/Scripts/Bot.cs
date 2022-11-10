using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bot : MonoBehaviour
{
    public GameObject czastki;
    public Material[] materials;


    private IEnumerator Zbicie(Pionki p)
    {
        GameObject c = Instantiate(czastki);
        if (p.team == 0) c.GetComponent<ParticleSystemRenderer>().material = materials[0];
        if (p.team == 1) c.GetComponent<ParticleSystemRenderer>().material = materials[1];
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
    public IEnumerator Ruch(Pionki p, int x, int y)
    {
        p.transform.position = p.transform.position + new Vector3(x, 0, y);
        yield return new WaitForSeconds(20f);
    }
    private void ChangePosition(Pionki p, int x, int y, Pionki[,] pionki, GameObject[,] tiles)
    {
        for (int m = 0; m <= 7; m++)
        {
            for (int n = 0; n <= 7; n++)
            {
                if (pionki[m, n] != null)
                {
                    if (p.przejmowanie != 0 && pionki[m, n].przejecie != 0)
                    {
                        if (pionki[m, n].currentX + x <= 7 && pionki[m, n].currentY + y <= 7 && pionki[m, n].currentX + x >= 0 && pionki[m, n].currentY + y >= 0)
                        {
                            pionki[m, n].currentX = pionki[m, n].currentX + x;
                            pionki[m, n].currentY = pionki[m, n].currentY + y;
                            for (int i = 0; i <= 6; i++)
                            {
                                StartCoroutine(Ruch(p, x, y));
                                //pionki[m, n].transform.position = pionki[m, n].transform.position + new Vector3(x, 0, y);
                            }
                        }
                    }
                }
            }
        }
        if (p.currentX <= 7 && p.currentY <= 7 && tiles[p.currentY + y, p.currentX + x] != null)
        {
            p.currentX = p.currentX + x;
            p.currentY = p.currentY + y;
            for (int i = 0; i <= 6; i++)
            {
                //p.transform.position = p.transform.position + new Vector3(x, 0, y);
                StartCoroutine(Ruch(p, x, y));
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
                            StartCoroutine(Zbicie(p));
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
                if (pionki[m, n] != null)
                {
                    if (pionki[m, n].trzymanie != 0)
                    {
                        if (p.currentX == pionki[m, n].currentX && p.currentY == pionki[m, n].currentY && p.id != pionki[m, n].id)
                        {
                            StartCoroutine(Zbicie(p));
                        }
                    }
                }
            }
        }
    }
    public void WylosowanyRuch(Pionki[,] pionki, GameObject[,] tiles)
    {
        int i = 0;
        int l;
        do
        {
            l = Random.Range(-1, 8);
            if (l == -1 || l == 8) l = Random.Range(-1, 8);
            i++;
        }
        while (pionki[l, 7] == null || i == 7);
        if (pionki[l, 7] != null)
        {
            Pionki wybrany = pionki[l, 7];
            int l2 = 1;//Random.Range(1, 2);
            wybrany.zaznaczony = l2;
            if (wybrany.zaznaczony == 1)
            {
                int[] mozliweX;
                int[] mozliweY;
                if (wybrany.currentX == 7)
                {
                    mozliweX = new int[] { -1, 0, 0 };
                }
                if (wybrany.currentX == 0)
                {
                    mozliweX = new int[] { 0, 1, 1 };
                }
                else
                {
                    mozliweX = new int[] { -1, 0, 1 };
                }

                if (wybrany.currentY == 7)
                {
                    mozliweY = new int[] { -1, 0, 0 };
                }
                if (wybrany.currentY == 0)
                {
                    mozliweY = new int[] { 0, 1, 1 };
                }
                else
                {
                    mozliweY = new int[] { -1, 0, 1 };
                }

                int l4 = Random.Range(0, 2);
                int l5 = Random.Range(0, 2);
                if (wybrany != null)
                {
                    ChangePosition(wybrany, mozliweX[l4], mozliweY[l5], pionki, tiles);
                }
            }
        }
        else
        {
            Debug.Log("Dziala");
        }
    }
}
