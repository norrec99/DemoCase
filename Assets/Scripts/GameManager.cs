using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Material mat_normal;
    [SerializeField] private Material mat_selected;

    private GameObject currentSelected;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero, 100f, LayerMask.GetMask("Building", "Unit") );
            if (hit.collider != null)
            {
                if (currentSelected != null)
                {
                    SpriteRenderer srCurrent = currentSelected.GetComponent<SpriteRenderer>();
                    if (srCurrent != null)
                    {
                        srCurrent.material = mat_normal;
                    }
                }
                currentSelected = hit.transform.gameObject;
                SpriteRenderer sr = currentSelected.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.material = mat_selected;
                }
            }
            else
            {
                if (currentSelected != null)
                {
                    SpriteRenderer srCurrent = currentSelected.GetComponent<SpriteRenderer>();
                    if (srCurrent != null)
                    {
                        srCurrent.material = mat_normal;
                    }
                    srCurrent = null;
                }   
            }
        }
    }
}