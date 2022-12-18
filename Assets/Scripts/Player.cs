using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerClass
{
    public string class_name;
    public float health;
    public int strength;
    public int defense;
    public int speed;
    public Image healthBar;

    public playerClass(string class_name){
        this.class_name = class_name;
        this.health = 20;
        this.strength = 30;
        this.defense = 20;
        this.speed = 50;
        this.healthBar = GameObject.Find("HealthFront").GetComponent<Image>();
        }
}

public class Player
{
    string name;
    playerClass playerClass;
    GameObject player;
    float currentHealth;
    public static Player instance;

    public Player(string name="Cat Dude",string class_name="Knight"){
        instance = this;
        playerClass = new playerClass(class_name);
        this.name = name;
        player = GameObject.FindGameObjectWithTag("Player");
        currentHealth = playerClass.health;
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

    public void updateHealth(int amount)
    {
        currentHealth -= amount;
        playerClass.healthBar.fillAmount = (currentHealth / playerClass.health);
        if (currentHealth<0)
        {
            playerDied();
        }
    }

    void playerDied()
    {
        Debug.Log("Dead!!!!!");
    }

}
