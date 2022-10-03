using System.Collections;
using UnityEngine;
using SuperUltra.GazolineRacing;

namespace SuperUltra.GazolineRacing
{

    public class PlayerController : MonoBehaviour
    {
        private CharacterController controller;
        private Vector3 move;
        public float forwardSpeed;
        public float maxSpeed;

        private int desiredLane = 1;//0:left, 1:middle, 2:right
        public float laneDistance = 2.5f;//The distance between tow lanes

        public bool isGrounded;
        public LayerMask groundLayer;
        public Transform groundCheck;

        public float gravity = -12f;
        public float jumpHeight = 2;
        private Vector3 velocity;

        public Animator animator;
        private bool isSliding = false;

        public float slideDuration = 1.5f;

        bool toggle = false;

        void Start()
        {
            controller = GetComponent<CharacterController>();
            Time.timeScale = 1.2f;
        }

        private void FixedUpdate()
        {


            //Increase Speed
            if (toggle)
            {
                toggle = false;
                if (forwardSpeed < maxSpeed)
                    forwardSpeed += 0.1f * Time.fixedDeltaTime;
            }
            else
            {
                toggle = true;
                if (Time.timeScale < 2f)
                    Time.timeScale += 0.005f * Time.fixedDeltaTime;
            }
        }

        void Update()
        {


            animator.SetBool("isGameStarted", true);
            move.z = forwardSpeed;

            isGrounded = Physics.CheckSphere(groundCheck.position, 0.17f, groundLayer);
            animator.SetBool("isGrounded", isGrounded);
            if (isGrounded && velocity.y < 0)
                velocity.y = -1f;


            controller.Move(velocity * Time.deltaTime);

            //Gather the inputs on which lane we should be
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                desiredLane++;
                if (desiredLane == 4)
                    desiredLane = 3;

            }
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                desiredLane--;
                if (desiredLane == -1)
                    desiredLane = 0;
            }

            //Calculate where we should be in the future
            Vector3 targetPosition = transform.position.z * transform.forward + transform.position.y * transform.up;
            if (desiredLane == 0)
                targetPosition += Vector3.left * laneDistance;
            else if (desiredLane == 2)
                targetPosition += Vector3.right * laneDistance;

            //transform.position = targetPosition;
            if (transform.position != targetPosition)
            {
                Vector3 diff = targetPosition - transform.position;
                Vector3 moveDir = diff.normalized * 30 * Time.deltaTime;
                if (moveDir.sqrMagnitude < diff.magnitude)
                    controller.Move(moveDir);
                else
                    controller.Move(diff);
            }

            controller.Move(move * Time.deltaTime);
        }


    }
}