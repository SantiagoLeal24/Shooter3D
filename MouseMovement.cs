using UnityEngine;

//public class MouseMovement : MonoBehaviour
//{
//    public float sensibilidadMouse = 100f;

//    float rotacionX = 0f;
//    float rotacionY = 0f;

//    public float topClamp = -90f;
//    public float bottomClamp = 90f;

//    void Start()
//    {
//        //mouse invisible y en el medio

//        Cursor.lockState = CursorLockMode.Locked;
//    }

//    void Update()
//    {
//        //mouse input

//        float MouseX = Input.GetAxis("Mouse X") * sensibilidadMouse * Time.deltaTime;

//        float MouseY = Input.GetAxis("Mouse Y") * sensibilidadMouse * Time.deltaTime;

//        //mirar arriba o abajo

//        rotacionX -= MouseY;

//        //bloquear la rotacion

//        rotacionX = Mathf.Clamp(rotacionX, topClamp, bottomClamp);

//        //mirar a los costados

//        rotacionY -= MouseX;

//        //aplicar rotaciones al transform

//        transform.localRotation = Quaternion.Euler(rotacionX, rotacionY, 0f);

//    }
//}


public class MouseMovement : MonoBehaviour
{
    public float sensibilidadMouse = 100f;

    float rotacionX = 0f;
    

    public float topClamp = -90f;
    public float bottomClamp = 90f;

    // para rotar el cuerpo horizontalmente
    public Transform playerBody;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float MouseX = Input.GetAxis("Mouse X") * sensibilidadMouse * Time.deltaTime;
        float MouseY = Input.GetAxis("Mouse Y") * sensibilidadMouse * Time.deltaTime;

        // 1. ROTACIÓN VERTICAL (CÁMARA): Aplicada a ESTE Transform (Main Camera)
        rotacionX -= MouseY;
        rotacionX = Mathf.Clamp(rotacionX, topClamp, bottomClamp);

        // Aplicamos la rotación vertical SÓLO al transform de la cámara
        transform.localRotation = Quaternion.Euler(rotacionX, 0f, 0f);

       
        // Función Rotate
        playerBody.Rotate(Vector3.up * MouseX);
    }
}