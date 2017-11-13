using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BrickManager : MonoBehaviour {

    private int currentScore;
    private int currentHiScore;

    public Text Score;
    public Text HiScore;

    public GameObject Brick;
    public GameObject Bricks;
    private List<GameObject> Levels;
    public float[] gridCoords;
    public float yDistance;
    public float moveTime;

    public GameObject Level;

    [Range(5,15)]
    public float maxColummns;

    private bool moveBlocks = false;
    private float moveAmount;
    public int maxLocationTries;

    void NextLevel()
    {
        int numOfBlocks;
        if (int.TryParse(Score.text, out currentScore))
        {
            currentScore++;
        } else
        {
            currentScore = 1;
        }
        Score.text = currentScore.ToString();
        if (currentScore > currentHiScore)
        {
            currentHiScore = currentScore;
            PlayerPrefs.SetInt("hiScore", currentHiScore);
            PlayerPrefs.Save();
            HiScore.text = currentHiScore.ToString();
        }
        
        if (currentScore > 10)
            numOfBlocks = Random.Range(2, gridCoords.Length+1);
        else if (currentScore > 5)
            numOfBlocks = Random.Range(1, 3);
        else
            numOfBlocks = Random.Range(1, 2);
        
        Vector2[] blockPos = CalculateBlockPosition(numOfBlocks);
        
        Debug.Log("Done processing block positions");
        foreach (GameObject g in Levels)
        {
            g.transform.position -= new Vector3(0, yDistance, 0);
        }
        InstantiateNewBlocks(blockPos);
        
    }

    void InstantiateNewBlocks(Vector2[] blockPos)
    {
        GameObject newLevel = Instantiate(Level);
        Levels.Add(newLevel);
        newLevel.transform.parent = Bricks.transform;
        foreach (Vector2 pos in blockPos)
        {
            GameObject brick = Instantiate(Brick, newLevel.transform);
            brick.transform.position = pos;
        }
    }

    private void Start()
    {
        currentHiScore = PlayerPrefs.GetInt("hiScore");
        HiScore.text = currentHiScore.ToString();

        Levels = new List<GameObject>();
        //Levels.Add(Level);
        NextLevel();
    }

    Vector2[] CalculateBlockPosition(int numOfBlocks)
    {
        bool samePoint;
        Vector2[] blockPos = new Vector2[numOfBlocks];
        for (int x = 0; x < numOfBlocks; x ++ )
        {
            int tries = 0; //Keeps track of number of tries for one position
            samePoint = true;
            while (samePoint) //Continues running while the generated point is already taken
            {
                tries++; //Increments the number of tries
                blockPos[x] = new Vector2(gridCoords[Random.Range(0, gridCoords.Length)], 3); //Sets the position to a randomly chosen pos from the pos list
                if (x != 0) // If x = 0, then it's the first point and will always be valid
                {
                    if (x > maxColummns) //This should NEVER be called. Just to check for coding errors.
                    {
                        Debug.LogWarning("More Blocks than Columns!");
                        return blockPos;
                    }
                    for (int y = 0; y<x; y++) //Checks to see if the blockPos is already taken
                    {
                        if (blockPos[y].x != blockPos[x].x)
                            samePoint = false;
                    }
                    if (tries > maxLocationTries) //Occurs when the number of tries exceeds the maximum number of tries allowed
                    {
                        TooManyTries(blockPos, x);
                        samePoint = false;
                    }
                } else
                {
                    samePoint = false;
                }
            }
        }
        return blockPos;
    }
    Vector2 TooManyTries(Vector2[] blockPos, int currentPos)
    {
        int neededAttemps = currentPos;
        for (int posIndex = 0; posIndex < gridCoords.Length;posIndex++)
        {
            int successfulAttempts = 0;
            for (int assignedPosIndex = 0; assignedPosIndex < currentPos; assignedPosIndex++)
            {
                if (blockPos[assignedPosIndex].x != gridCoords[posIndex])
                {
                    successfulAttempts++;
                }
            }
            if (successfulAttempts >= neededAttemps)
            {
                return new Vector2(gridCoords[posIndex],3);
            }
        }
        return new Vector2(10, 10); //This should never occur as it means there are no openings for the block.
    }

    bool MoveBlocksDown()
    {
        if (Levels[0] == null)
        {
            return false ;
        }
        for (int x = 0; x < Levels.Count; x++)
        {
            Levels[x].transform.position = Vector2.Lerp(transform.position, new Vector2(transform.position.x, transform.position.y - yDistance),moveTime);
        }

        if (Levels[0].transform.position.y <= moveAmount)
        {
            return false;
        }
        return true;
    }
}
