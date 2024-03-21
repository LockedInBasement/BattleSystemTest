using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIDamageIndicator : MonoBehaviour
{
    public TMP_Text damageText;

    public float moveSpeed;

    public float lifeTime= 3f;

    private RectTransform myRectTransform;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, lifeTime);

        myRectTransform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        myRectTransform.anchoredPosition += new Vector2(0f,-moveSpeed*Time.deltaTime);
    }
}
