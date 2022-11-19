using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerClass
{
    public string class_name;
    public int health;
    public int strength;
    public int defense;
    public int speed;

    public  playerClass(string class_name){
        this.class_name = class_name;
        this.health = 100;
        this.strength = 30;
        this.defense = 20;
        this.speed = 50;
        }
}

public class Player
{
    string name;
    playerClass playerClass;
    GameObject player;

    public Player(string name="Cat Dude",string class_name="Knight"){
        playerClass = new playerClass(class_name);
        this.name = name;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void spawnPlayer(Tiles[] cells)
    {
        int start = UnityEngine.Random.Range(0, cells.Length);
        while (cells[start].visited==false)
        {
            start = UnityEngine.Random.Range(0, cells.Length);
        }
        player.transform.localPosition = new Vector3(cells[start].x, 20, cells[start].z);


    }



}
