using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move_Bird : MonoBehaviour
{
    Rigidbody bird;
    float m_Speed;
    int n = 0;

    // Start is called before the first frame update
    void Start()
    {
      bird = GetComponent<Rigidbody>();
      m_Speed = 5.0f;
    }

    // Update is called once per frame
    void Update()
    {
      n = n + 1;

      bird.velocity = transform.forward * m_Speed;
      if (n == 5){
      bird.transform.Rotate(0,1,0, Space.Self);
      n = 0;
      }
    }
}
