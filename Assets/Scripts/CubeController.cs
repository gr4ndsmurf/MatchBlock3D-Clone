using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeController : MonoBehaviour
{
    public List<Cube> cubeList = new List<Cube>();
    public Cube currentCube;
    public Transform spawnPoint;
    private Touch touch;

    private Vector3 downPos, upPos;
    private bool dragStarted;
    
    private float speed = 5f;
    private void Start()
    {

        currentCube = PickRandomCube();
    }
    void Update()
    {
        if (Input.touchCount > 0)
        {
            touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                dragStarted = true;
                upPos = touch.position;
                downPos = touch.position;
                if (currentCube)
                {
                    currentCube.rb.freezeRotation = true;
                }
            }
        }
        if (dragStarted)
        {
            if (touch.phase == TouchPhase.Moved)
            {
                downPos = touch.position;
            }
            if (currentCube)
            {
                currentCube.rb.linearVelocity = CalculateDirection() * speed;
            }

            if (touch.phase == TouchPhase.Ended)
            {
                downPos = touch.position;
                dragStarted = false;
                if (!currentCube) return;
                currentCube.rb.linearVelocity = Vector3.zero;
                currentCube.SendCube();
                currentCube = null;
                StartCoroutine(SetCube());
            }
            
        }

    }
    private IEnumerator SetCube()
    {
        yield return new WaitForSeconds(1);
        currentCube = PickRandomCube();
    }
    public Cube PickRandomCube()
    {
        GameObject temp = Instantiate(cubeList[Random.Range(0, 6)].gameObject,spawnPoint.position,Quaternion.identity);
        return temp.GetComponent<Cube>();
    }
    Vector3 CalculateDirection()
    {
        Vector3 temp = (downPos - upPos).normalized;
        temp.z = 0;
        temp.y = 0;
        return temp;
    }
}