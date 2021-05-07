using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class KarakterKontrol : MonoBehaviour
{
    public static KarakterKontrol instance;
    public Rigidbody rb;
    public float speed;
    public float lerpValue;
    public GameObject prev;
    public Material karakterMat;
    public List<GameObject> cubes = new List<GameObject>();
    public Text finishText;
    public CamControl camControl;

    private Camera cam;
    private bool isGameEnded=false;


    private void Awake()
    {
        if (instance==null)
        {
            instance = this;
        }
    }

    void Start()
    {
        cam = Camera.main;

       
    }

   
    void FixedUpdate()
    {
        if (Input.GetButton("Fire1"))
        {

            Movement();
        }

        if (!isGameEnded)
        {
           
            rb.velocity = Vector3.forward * Time.deltaTime * speed;
        }


    }

    private void Movement()
    {

        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray,out hit,1000))
        {
            Vector3 movementVec = new Vector3(hit.point.x, transform.position.y, transform.position.z);
            //transform.position = Vector3.Lerp(transform.position, movementVec,lerpValue);
            transform.DOMoveX(movementVec.x, 0.25f);
            
        }

    }

    public void Topla(GameObject gameObject)
    {
       
        Vector3 karakterPos = transform.localPosition;
        
        karakterPos.y += 0.44f;
        // transform.position = Vector3.Lerp(transform.position, karakterPos, 4);
        // transform.position = karakterPos;
        transform.DOMove(karakterPos, 0.095f);

        gameObject.transform.SetParent(transform);

        Vector3 pos = prev.transform.position;
       // pos.y -= 1;
       // gameObject.transform.DOMove(pos, 0.4f);
        gameObject.transform.DOMove(pos, 0.095f);
        camControl.offset.y += 0.1f;

        gameObject.GetComponent<MeshRenderer>().material = karakterMat;
        prev = gameObject;

        cubes.Add(gameObject);
        prev.GetComponent<BoxCollider>().isTrigger = false;
    }

   

   
    public void CokluTopla(List<GameObject> gameObjects )
    {
        
        for (int i = 0; i < gameObjects.Count; i++)
        {
            GameObject gameObject = gameObjects[i];
            Vector3 karakterPos = transform.localPosition;
            karakterPos.y += 0.44f;
            transform.position = karakterPos;



            gameObject.transform.SetParent(transform);

            Vector3 pos = prev.transform.localPosition;
            pos.y -= 1;
            gameObject.transform.localPosition = Vector3.Lerp(gameObject.transform.localPosition,pos,4);
            camControl.offset.y += 0.1f;
            gameObjects[i].gameObject.GetComponent<MeshRenderer>().material = karakterMat;
            cubes.Add(gameObject);

            prev = gameObject;

            prev.GetComponent<BoxCollider>().isTrigger = false;
        }
    }


    public IEnumerator Azalt(List<GameObject> gameObjects)
    {
        for (int i = 0; i < gameObjects.Count; i++)
        {
            transform.GetChild(transform.childCount - 1).SetParent(null);


            Vector3 karakterPos = transform.localPosition;
            karakterPos.y -= 0.44f;
            transform.transform.position = karakterPos;

            if (cubes.Count>0)
            {
                cubes.RemoveAt(cubes.Count - 1);

            }

            yield return new WaitForSeconds(0.1f);


        }

        if (cubes.Count>0 && cubes.Count>gameObjects.Count-1)
        {
            prev = cubes[cubes.Count - 1];
            cubes[cubes.Count - 1].AddComponent<Control>();

        }
        else
        {
            speed = 0;
            finishText.text = "You Failed!";

        }



        

    }



    











}
