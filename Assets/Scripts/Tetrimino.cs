using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour {

    float fall = 0;
    float pressedDownInTime = 0;
    public float slowDownPressed = 3;
    public float fallSpeed = 1;
    public bool allowRotation = true;
    public bool limitRotation = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        CheckUserInput();
	}
    
    void CheckUserInput()
    {
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);
            if (CheckIsValidPosition())
            {
                FindObjectOfType<Game>().UpdateGrid(this);
            }
            else
            {
                transform.position += new Vector3(-1, 0, 0);
            }
        }
        else if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);
            if (CheckIsValidPosition())
            {
                FindObjectOfType<Game>().UpdateGrid(this);
            }
            else
            {
                transform.position += new Vector3(1, 0, 0);
            }
        }
        else if ((Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) || (Time.time - fall >= fallSpeed))
        {
            transform.position += new Vector3(0, -1, 0);
            if (CheckIsValidPosition())
            {
                FindObjectOfType<Game>().UpdateGrid(this);
            }
            else
            {
                transform.position += new Vector3(0, 1, 0);
                FindObjectOfType<Game>().DeleteRow();
                enabled = false;
                FindObjectOfType<Game>().SpawnNextTetromino();
            }
            fall = Time.time;
        }
        else if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            pressedDownInTime += Time.deltaTime * 6;
            if (pressedDownInTime > slowDownPressed)
            {
                transform.position += new Vector3(0, -1, 0);
                if (CheckIsValidPosition())
                {
                    FindObjectOfType<Game>().UpdateGrid(this);
                }
                else
                {
                    transform.position += new Vector3(0, 1, 0);
                    FindObjectOfType<Game>().DeleteRow();
                    enabled = false;
                    FindObjectOfType<Game>().SpawnNextTetromino();
                }
            }
            else if (Input.GetKeyUp(KeyCode.S))
            {
                pressedDownInTime = 0;
                transform.position += new Vector3(0, 1, 0);
                FindObjectOfType<Game>().DeleteRow();
                enabled = false;
                FindObjectOfType<Game>().SpawnNextTetromino();
            }
            fall = Time.time;
        }
        else if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (allowRotation)
            {
                if (limitRotation)
                {
                    if (transform.rotation.eulerAngles.z >= 90)
                    {
                        transform.Rotate(0, 0, -90);
                    }
                    else
                    {
                        transform.Rotate(0, 0, 90);
                    }
                }
                else
                {
                    transform.Rotate(0, 0, 90);
                    if (CheckIsValidPosition())
                    {
                        FindObjectOfType<Game>().UpdateGrid(this);
                    }
                    else
                    {
                        if (limitRotation)
                        {
                            if (transform.rotation.eulerAngles.z >= 90)
                            {
                                transform.Rotate(0, 0, -90);
                            }
                            else
                            {
                                transform.Rotate(0, 0, 90);
                            }
                        }
                        else
                        {
                            transform.Rotate(0, 0, -90);
                        }
                    }
                }
            }
        }
    }

    bool CheckIsValidPosition()
    {
        foreach (Transform mino in transform)
        {
            Vector2 pos = FindObjectOfType<Game>().Round(mino.position);
            if (FindObjectOfType<Game>().CheckIsInsideGrid (pos) == false)
            {
                return false;
            }
            if (FindObjectOfType<Game>().GetTransformAtGridPosition(pos) != null && FindObjectOfType<Game>().GetTransformAtGridPosition(pos).parent != transform)
            {
                return false;
            }
        }
        return true;
    }
}
