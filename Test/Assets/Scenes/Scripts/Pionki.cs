using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Pionek
{
    pionek = 0,
    szybki = 1,
    przejmujacy = 2,
    zatrzymujacy = 3,
    armata = 4,
    rozwalacz = 5,
    
}
public class Pionki : MonoBehaviour
{
    public int team;
    public int currentX { get; set; }
    public int currentY { get; set; }
    public Pionek type;
    public int zaznaczony { get; set; }
    public int przejmowanie { get; set; }
    public int przejecie { get; set; }
    public int trzymanie { get; set; }
    public int id { get; set; }
    public bool zbity { get; set; }
    public bool zbicie { get; set; }
    
    
}
