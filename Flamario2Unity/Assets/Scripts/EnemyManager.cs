using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    private GameObject mario;
    [SerializeField]
    private GameObject koopa;
    private Transform marioTransform;
    private bool koopaActivated = false;

    void Start()
    {
        marioTransform = mario.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        //At the moment Koopa is spawned in the section where mario is, after 
        //Mario steps forward a little
        if (marioTransform.position.x > 70 && koopaActivated == false)
        {
            koopaActivated = true;
            koopa.SetActive(true);
        }
    }
}
