using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mover : MonoBehaviour
{
    // Start is called before the first frame update
    //Variables globales
    public float velocidad;
    public float fuerza_Salto;
    private Rigidbody2D rigidbody;
    private BoxCollider2D boxCollider;
    private CapsuleCollider2D capsuleCollider;
    public LayerMask capaSuelo;
    public int saltosMaximos;
    private int saltosRestantes;


    //Header animaciones
    [Header("Animacion")]
    private Animator anim;
    
    void Start()
    {
        //Inicializamos
        rigidbody = GetComponent<Rigidbody2D>();
        boxCollider = GetComponent<BoxCollider2D>();
        capsuleCollider = GetComponent<CapsuleCollider2D>();
        saltosRestantes = saltosMaximos;
        
        velocidad = 2f;
        //animator
        anim = GetComponent<Animator>();
        
    }
    
    
    //Dirección del movimiento
    public bool moveRight = true;
    
    // Update is called once per frame
    void Update()
    {
        //movimiento
        moveCharacter();
        //aplicar animaciones
        anim.SetFloat("Horizontal", Mathf.Abs(rigidbody.velocity.x));
        anim.SetFloat("Salto", Mathf.Abs(rigidbody.velocity.y));
        //Salto
        ProcesarSalto();
    }

    bool EstaEnSuelo()
    {
        RaycastHit2D raycasHit = Physics2D.BoxCast(capsuleCollider.bounds.center, new Vector2(capsuleCollider.bounds.size.x, capsuleCollider.bounds.size.y), 0f, Vector2.down, 0.2f, capaSuelo);
        return raycasHit.collider != null;
    }
    //Funcion para mover el personaje 2d
    void moveCharacter()
    {
        //Movimiento en el eje x
        float horizontal = Input.GetAxis("Horizontal");
        rigidbody = GetComponent<Rigidbody2D>();
        
        //Movemos el personaje en X
        rigidbody.velocity = new Vector2(horizontal * velocidad, rigidbody.velocity.y);
        orientacionCharacter(horizontal);
        
    }

    
    void ProcesarSalto()
    {
        if (EstaEnSuelo())
        {
            saltosRestantes = saltosMaximos;
        }
        if (Input.GetKeyDown(KeyCode.Space) && saltosRestantes > 0) /* personaje está en el suelo*/
        {
            saltosRestantes--;
            rigidbody.AddForce(Vector2.up * fuerza_Salto, ForceMode2D.Impulse);
        }
    }
    
    
    //Gestionar la orientacion del personaje
    void orientacionCharacter(float horizontal)
    {
        //Si el personaje se mueve a la derecha
        if ((moveRight==true && horizontal<0) || (moveRight == false && horizontal > 0))
        { 
            //Lo debemos voltear
            moveRight = !moveRight;
            transform.localScale = new Vector2(transform.localScale.x * -1, transform.localScale.y);
        }
    }

    //Método para invocar las coliciones
    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Reconocer si el objeto choca con otro que tenga la propiedad respawn
        if (collision.gameObject.CompareTag("Respawn"))
        {
            //Lo que pasa si chocamos
            
            //Mover a otra posición
            transform.position = new Vector3(-6 , -1, 0);
        }
    }
}
