using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float mainThrust = 1000f;
    [SerializeField] float rotationThrust = 1f;
    [SerializeField] AudioClip mainEngine;
    
    [SerializeField] ParticleSystem mainEngineParticles;
    [SerializeField] ParticleSystem leftThrusterParticles;
    [SerializeField] ParticleSystem rightThrusterParticles;

    Rigidbody rb;
    // AudioSource audioSource;
    bool isAlive;
 
 
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        BasicMovement();
        JumpProcess();
        // ProcessRotation();
    }
    private void BasicMovement()
    {
        float moveSpeed = 10f;
        float xValue = Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime;
        float zValue = Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime;

        //set yValue = 0 because I don't object is flying
        transform.Translate(xValue, 0, zValue);
    }

    void JumpProcess()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            StartJumping();
        }
        else
        {
            StopThrusting(); //Stop making mainEngine effect
        }
    }

    void StopThrusting()
    {
        // audioSource.Stop();
        mainEngineParticles.Stop();
    }

    void StartJumping()
    {
        rb.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
        // if (!audioSource.isPlaying)
        // {
        //     audioSource.PlayOneShot(mainEngine);
        // }
        // if (!mainEngineParticles.isPlaying) //Check if main engine is start yet
        // {
        //     mainEngineParticles.Play(); //Start making mainEngine effect
        // }
    }

    void ProcessRotation()
    {
        if (Input.GetKey(KeyCode.A))
        {
            RotateLeft();
        }
        else if (Input.GetKey(KeyCode.D))
        {
            RotateRight();
        }
        else
        {
            StopRotating();
        }
    }

    void RotateLeft()
    {
        applyRotation(rotationThrust);
        if (!rightThrusterParticles.isPlaying) //Check if rightThruster is not rotating
        {
            rightThrusterParticles.Play(); //Perform right rotation effect
        }
    }

    void RotateRight()
    {
        applyRotation(-rotationThrust);
        if (!leftThrusterParticles.isPlaying) //Check if leftThruster is not rotating
        {
            leftThrusterParticles.Play();
        }
    }

    private void StopRotating()
    {
        rightThrusterParticles.Stop();
        leftThrusterParticles.Stop();
    }

    void applyRotation(float rotationThisFrame)
    {
        rb.freezeRotation = true; // Freezing rotation so we can manually rotate
        transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime);
        rb.freezeRotation = false; // unfreezing rotation so physics system can take over
        
    }

}