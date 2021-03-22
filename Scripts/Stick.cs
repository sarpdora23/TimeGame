using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using MilkShake;
using UnityEngine.SceneManagement;

public class Stick : MonoBehaviour
{
    public float speed;
    private bool rightrue;
    public bool iscontinue;
    public GameObject player;
    private Animation playeranim;
    public bool iscorrect;
    public GameObject[] silindirler;
    public int health;
    public GameObject green;
    public AudioClip attack;
    public AudioSource ausor;
    public Text healthtxt;
    public GameObject world;
    public ShakePreset earthquake;
    public AudioSource earthsource;
    public int tamiricinnekaldı;
    // Start is called before the first frame update
    void Start()
    {
        health = 4;
        tamiricinnekaldı = 5;
        playeranim = player.GetComponent<Animation>();
        ausor = gameObject.GetComponent<AudioSource>();
        yeniden();
        
    }

    // Update is called once per frame
    void Update()
    {
        healthtxt.text = "Repair: " + tamiricinnekaldı;
        if (iscontinue)
        {
             devam();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            durdur();
        }
        if(health <= 0)
        {
            SceneManager.LoadScene("Lost");
        }
        if(tamiricinnekaldı <= 0)
        {
            SceneManager.LoadScene("Win");
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Convertor")
        {
            rightrue = !rightrue;
        }
        else
        {
            Debug.Log("Yesil");
            iscorrect = true;
        }

    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log("Kirmizi");
        iscorrect = false;
    }
    public void devam()
    {
        if (rightrue)
        {
            transform.Translate(new Vector2(speed * Time.deltaTime, 0));
        }
        else
        {
            transform.Translate(new Vector2(speed * Time.deltaTime * -1, 0));
        }
    }
    public void durdur()
    {
        iscontinue = false;
        playeranim.Stop();
        playeranim.Play();
        StartCoroutine(countdown3());
        if (!iscorrect)
        {
            StartCoroutine(countdownEarth());
        }
        else
        {
            StartCoroutine(countdown2());
            
        }
        

    }
    public void yeniden()
    {
        gameObject.transform.localPosition = new Vector3(-0.5206854f, 0.35879f, -5.377114f);
        green.transform.localScale = new Vector3(Random.Range(0.2f, 0.6f), green.transform.localScale.y, green.transform.localScale.z);
        green.transform.position = new Vector3(Random.Range(-3.276f, 2.464f), -1.93107f, -2.629114f);
        iscontinue = true;
        speed = Random.Range(2, 5);
        rightrue = true;
    }
    IEnumerator countdown()
    {
        yield return new WaitForSeconds(2f);
        silindirler[health - 1].transform.DORotate(new Vector3(0, 0, -90), 1.2f);
        earthsource.Stop();
        silindirler[health - 1].transform.DOMove(new Vector3(silindirler[health - 1].transform.position.x + 6.7f, silindirler[health - 1].transform.position.y - 3.26f, 2.95f), 1.2f).OnComplete(()=> healthazalt());  
        yeniden();
    }
    IEnumerator countdown2()
    {
        yield return new WaitForSeconds(4f);
        tamiricinnekaldı--;
        yeniden();
    }
    void healthazalt()
    {
        Destroy(silindirler[health - 1]);
        health--;
    }
    IEnumerator countdown3()
    {
        yield return new WaitForSeconds(2.5f);
        ausor.clip = attack;
        ausor.Play();
        
    }
    IEnumerator countdownEarth()
    {
        yield return new WaitForSeconds(2.2f);
        world.GetComponent<Shaker>().Shake(earthquake);
        yield return new WaitForSeconds(0.6f);
        earthsource.Play();
        yield return new WaitForSeconds(2.2f);
        StartCoroutine(countdown());

    }

}
