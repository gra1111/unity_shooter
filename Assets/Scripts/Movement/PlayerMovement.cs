using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 6f;
    public float runspeed = 12f;
    public float currentspeed;

    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    public Transform groundCheck;
    public float groundDistance = 0.3f;
    public LayerMask groundMask;

    Vector3 velocity;
    bool isGrounded;

    [SerializeField] public Stamina stamina;

    // Control de saltos
    private int maxAirJumps = 0;   // 0 = solo salto en suelo (sin doble salto)
    private int jumpsUsed = 0;

    void Update()
    {
        // 1) Detecci�n de suelo: usamos el CharacterController y opcionalmente el CheckSphere
        bool controllerGrounded = controller.isGrounded;
        bool sphereGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Puedes probar con solo uno de los dos:
        // isGrounded = controllerGrounded;
        // o con combinaci�n (m�s estricto):
        isGrounded = controllerGrounded || sphereGrounded;

        // Pegar al suelo cuando estamos cayendo y tocamos suelo
        if (isGrounded && velocity.y < 0f)
        {
            velocity.y = -2f;
            jumpsUsed = 0;   // hemos tocado suelo: se resetean los saltos
        }

        // Inputs WASD
        float x = Input.GetAxis("Horizontal");   // A/D
        float z = Input.GetAxis("Vertical");     // W/S

        if (Input.GetKey(KeyCode.LeftShift) && isGrounded && stamina.currentStamina > 0)
        {
            currentspeed = runspeed;
        }
        else
        {
            currentspeed = speed;
        }

        // Movimiento respecto a la orientaci�n del jugador
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * currentspeed * Time.deltaTime);

        // Saltar: solo si estamos en suelo (o dentro del n�mero de saltos permitido)
        if (Input.GetButtonDown("Jump") && isGrounded && jumpsUsed <= maxAirJumps)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            jumpsUsed++;   // ya hemos usado el salto de esta "fase en el aire"
        }

        // Aplicar gravedad
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}
