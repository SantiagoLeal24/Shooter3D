using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    public float velocidad = 12f;
    public float gravedad = -9.81f * 2;
    public float jumpHeight = 3f;

    public Transform groundCheck;
    public float groudDistancia = 0.4f;
    public LayerMask groundMask;

    Vector3 velocity;

    bool isGrounded;
    bool isMoving;

    private Vector3 UltimaPosicion = new Vector3(0f, 0f, 0f);
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    
    void Update()
    {
        //Ground Check

        isGrounded = Physics.CheckSphere(groundCheck.position, groudDistancia, groundMask);

        //Resetear la velocidad

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Los inputs

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        //Vector de movimiento

        Vector3 mover = transform.right * x + transform.forward * z; //right = eje rojo, foward = eje azul

        // Mover al personaje

        controller.Move(mover * velocidad * Time.deltaTime);

        // Chequear el salto

        if (Input.GetButtonDown("Jump") && isGrounded)

        {
            //ir hacia arriba

            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravedad);
        }

        //Caerse

        velocity.y += gravedad * Time.deltaTime;

        //salto

        controller.Move(velocity * Time.deltaTime);

        if (UltimaPosicion != gameObject.transform.position && isGrounded == true)
        {
            isMoving = true;

            //para despues
        }
        else
        {
            isMoving = false;

            //para despues
        }

        UltimaPosicion = gameObject.transform.position;

    }
}
