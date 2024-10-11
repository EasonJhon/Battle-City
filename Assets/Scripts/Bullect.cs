using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullect : MonoBehaviour
{
    public float movespeed = 10;

    public bool isPlayerBullect;

    public void SetBulletSpeed(float speed)
    {
        movespeed = speed;
    }
    
    void Update()
    {
        transform.Translate(transform.up * movespeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Heart":
                collision.SendMessage("Die");
                Destroy(gameObject);
                break;
            case "Enemy":
                if (isPlayerBullect)
                {
                    collision.SendMessage("Die");
                    Destroy(gameObject);
                }

                break;
            case "Tank":
                if (isPlayerBullect == false)
                {
                    collision.SendMessage("Die");
                    Destroy(gameObject);
                }

                break;
            case "Wall":
                Destroy(collision.gameObject);
                Destroy(gameObject);
                break;
            case "Barriar":
                if (isPlayerBullect)
                {
                    collision.SendMessage("PlayAudio");
                }

                Destroy(gameObject);
                break;
            default:
                break;
        }
    }
}