using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Rotate : MonoBehaviour
{
    public GameObject pan;
    public GameObject prefab;
    private Vector2 pre;
    private Vector2 first;
    private Vector2 S_pos;
    private Vector3 now_rotation;

    private Rigidbody2D rb;
    private float rspeed = 0;
    private float first_time;
    private float last_time;

    private int num;
    private int num_ang;

    private Color[] colors = { new Color(1,0,0), new Color(1, 0.3f, 0), new Color(1,1, 0), new Color(0, 1, 0), new Color(0, 0, 1) , new Color(0, 0.4f, 1), new Color(0.4f, 0, 1) };

    void Start()
    {
        num = 3;
        S_pos = Camera.main.WorldToScreenPoint(pan.transform.position);
        rb = pan.transform.GetComponent<Rigidbody2D>();
        num_ang = 360 / num;
        Set();

    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            pre = Input.mousePosition;
            first = Input.mousePosition;
            first_time = Time.deltaTime;

        }
        if (Input.GetMouseButton(0))
        {
            Vector2 now = Input.mousePosition;
            Vector2 temp1 = new Vector2(S_pos.x - pre.x, S_pos.y - pre.y);
            Vector2 temp2 = new Vector2(S_pos.x - now.x, S_pos.y - now.y);
            float angle = Vector2.SignedAngle(temp1, temp2);
            pre = now;
            pan.transform.rotation = Quaternion.Euler(0, 0, now_rotation.z + angle);
            now_rotation = pan.transform.rotation.eulerAngles;
        }
        if (Input.GetMouseButtonUp(0))
        {
            Vector2 now = Input.mousePosition;
            Vector2 temp1 = new Vector2(S_pos.x - first.x, S_pos.y - first.y);
            Vector2 temp2 = new Vector2(S_pos.x - now.x, S_pos.y - now.y);
            float angle = Vector2.SignedAngle(temp1, temp2); 
            last_time = Time.deltaTime;
            rspeed = angle/(last_time-first_time)/90f;

        }
        pan.transform.rotation = Quaternion.Euler(0, 0, now_rotation.z - rspeed);
        rspeed *= 0.98f;
        if (Mathf.Abs(rspeed) < 10f)
        {
            rspeed = 0;
            float angle = now_rotation.z % num_ang;
            if (angle > num_ang - angle)
            {
                pan.transform.rotation = Quaternion.Euler(0, 0, now_rotation.z + num_ang - angle);
            }
            else
            {
                pan.transform.rotation = Quaternion.Euler(0, 0, now_rotation.z - angle);
            }

            now_rotation = pan.transform.rotation.eulerAngles;
        }
    }
    
    public void plus()
    {
        if (num < 7)
        {
            num++;
            num_ang = 360 / num;
            GameObject temp = Instantiate(prefab);
            temp.transform.parent = pan.transform;
            temp.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            Set();
        }
    }
    public void minus()
    {
        if (num >2)
        {
            num--;
            num_ang = 360 / num;
            GameObject temp = pan.transform.GetChild(0).gameObject;
            temp.transform.parent = null;
            Destroy(temp);
            Set();
        }
    }
    
    public void Set()
    {
        GameObject temp = pan.transform.GetChild(0).gameObject;
        temp.SetActive(false);
        temp.SetActive(true);
        SpriteRenderer render = temp.GetComponent<SpriteRenderer>();
        render.color = colors[0];
        temp.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 0.35f);
        for (int i = 1; i < num; i++)
        {
            temp = pan.transform.GetChild(i).gameObject;
            temp.SetActive(false);
            temp.SetActive(true);
            render = temp.GetComponent<SpriteRenderer>();
            render.color = colors[i];
            temp.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.35f * Mathf.Sin(Mathf.PI / 180 * num_ang * i), 0.35f * Mathf.Cos(Mathf.PI / 180 * num_ang * i));
        }
    }

}
