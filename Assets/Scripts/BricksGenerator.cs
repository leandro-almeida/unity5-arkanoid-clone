using UnityEngine;
using System.Collections;

public class BricksGenerator : MonoBehaviour
{

    public GameObject[] bricksPrefab;
    public int numberOfBrickLines;
    public int numberOfBrickColumns;

    // Use this for initialization
    void Start()
    {
        // gerar bricks
        Vector3 brickPos;
        Vector2 brickSize = bricksPrefab[0].GetComponent<BoxCollider2D>().size;

        for (int i = 0; i < numberOfBrickLines; i++)
        {
            for (int j = 0; j < numberOfBrickColumns; j++)
            {
                // calcula a posicao do brick
                brickPos = new Vector3(transform.position.x + (brickSize.x / 2) + (brickSize.x * j), transform.position.y + (brickSize.y / 2) + (brickSize.y * i), 0);

                // instancia
                (Instantiate(GetRandomBrickPrefab(), brickPos, Quaternion.identity) as GameObject).transform.parent = transform;
            }
        }

    }

    private GameObject GetRandomBrickPrefab()
    {
        return bricksPrefab[Random.Range(0, bricksPrefab.Length)];
    }

}
