using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public LayerMask planetLayer;
    public Fly[] flies;

    private void Reset()
    {
        flies = FindObjectsOfType<Fly>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray camRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(camRay.origin, camRay.direction, 1000f, planetLayer);
            if (hit.collider != null)
            {
                foreach (var fly in flies)
                {
                    fly.Seek(hit.transform);
                }
            }
        }
    }
}
