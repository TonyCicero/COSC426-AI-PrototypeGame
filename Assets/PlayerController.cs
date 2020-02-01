using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator _animator;
    float rotSpeed = 40;
    [SerializeField]
    float moveSpeed = 5;
    [SerializeField]
    float camSpeed = 4;
    [SerializeField]
    Transform camT;
    [SerializeField]
    PInventory inv;
    [SerializeField]
    Health health;
    public Rigidbody rb;

    [SerializeField]
    float mineDist = 2.0f;
    [SerializeField]
    float attackDist = 3.0f;
    [SerializeField]
    Transform mineHeight;

    [SerializeField]
    AudioSource FootSteps;
    [SerializeField]
    AudioClip[] FootStepClips;

    [SerializeField]
    AudioSource sfx;
    [SerializeField]
    AudioClip whoosh;



    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_animator == null) return;
        var x = Input.GetAxis("Horizontal");
        var y = Input.GetAxis("Vertical");
        transform.Rotate(0, Input.GetAxis("Mouse X") * Time.deltaTime * rotSpeed, 0);
        camT.Rotate(Input.GetAxis("Mouse Y") * Time.deltaTime * camSpeed, 0, 0);
        


        if (!_animator.GetCurrentAnimatorStateInfo(0).IsName("Walk")) //dont allow the animation to move the player
        {
            rb.isKinematic = true;
            FootSteps.Stop(); // Stop Playing Footsteps sfx
            
        }
        else
        {
            Move(x, y);
            rb.isKinematic = false;
            if (!FootSteps.isPlaying)
            {
                FootSteps.clip = FootStepClips[Random.Range(0, FootStepClips.Length - 1)];
                FootSteps.Play();
            }
        }

        FootSteps.volume = (Mathf.Abs(x) + Mathf.Abs(y))/2; //set footstep volume based on movement

        if (Input.GetButtonDown("Pickup"))  // pickup/drink action
        {
            RaycastHit hit;
            if (Physics.Raycast(mineHeight.position, transform.TransformDirection(new Vector3(0, -1, 1)), out hit, mineDist))
            {
                Debug.DrawRay(mineHeight.position, transform.TransformDirection(new Vector3(0, -1, 1)) * hit.distance, Color.yellow);
                Debug.Log(hit.collider.tag);
                if (hit.collider.tag == "Water") // drink the water
                {
                    _animator.SetTrigger("Gather");
                    health.hydration += 5;
                    if (health.hydration > 100)
                    {
                        health.hydration = 100;
                    }
                    health.update_Txt();
                }
            }
        }

            if (Input.GetButtonDown("Fire1")) // attack/action
        {
            if (inv.hand.handSlot.id == 1) // if id = 1, play attack animation
            {
                if (health.Stamina(5)) //uses stamina
                {
                    sfx.clip = whoosh;
                    sfx.Play();
                    _animator.SetTrigger("Attack");
                    RaycastHit hit;
                    if (Physics.Raycast(mineHeight.position, transform.TransformDirection(Vector3.forward), out hit, attackDist))
                    {
                        Debug.DrawRay(mineHeight.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                        Debug.Log(hit.collider.tag);
                        if (hit.collider.tag == "Enemy")
                        {
                            hit.collider.gameObject.GetComponent<EnemyHealth>().hit(inv.hand.handSlot.attack);
                        }
                    }
                }
            }else if (inv.hand.handSlot.id == 5) // if id = 5, play mining animation
            {
                if (health.Stamina(5)) //uses stamina
                {
                    _animator.SetTrigger("Pickaxe");
                    RaycastHit hit;
                    if (Physics.Raycast(mineHeight.position, transform.TransformDirection(Vector3.forward), out hit, mineDist)) {
                        Debug.DrawRay(mineHeight.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
                        Debug.Log(hit.collider.tag);
                        if (hit.collider.tag == "Mineable")
                        {
                            StartCoroutine(hit.collider.gameObject.GetComponent<mineable>().hit(hit.point));
                        }else if (hit.collider.tag == "Enemy")
                        {
                            hit.collider.gameObject.GetComponent<EnemyHealth>().hit(inv.hand.handSlot.attack);
                        }

                    }
                }
            }
        }
    }
    private void Move(float x, float y)
    {
        _animator.SetFloat("x", x);
        _animator.SetFloat("y", y);

        rb.MovePosition(transform.position + transform.right * moveSpeed * x * Time.deltaTime + transform.forward * moveSpeed * y * Time.deltaTime);
        //rb.MovePosition(transform.position + transform.forward * moveSpeed * y * Time.deltaTime);
    }
}
