using UnityEngine;
using System.Collections;

public class Brick : MonoBehaviour
{

    public int lives;
    private int remainingLives;

    void Start()
    {
        remainingLives = lives;
    }

    public int BallHit()
    {
        return --remainingLives;
    }

    public int GetRemainingLives()
    {
        return remainingLives;
    }

    public int GetTotalLives()
    {
        return lives;
    }
}
