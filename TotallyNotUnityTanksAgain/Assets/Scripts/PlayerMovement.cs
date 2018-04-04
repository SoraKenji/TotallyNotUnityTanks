using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class PlayerMovement : MonoBehaviour  {
    public Transform body;
    public int PlayerNumber = 1;
    public float speed = 12f;
    public float turnSpeed = 180f;
    public float angleToChange = 45;
    public float originalYPosition;
    public Slider m_dashSlider;

    public Transform centerPoint;

    private float originalSpeed;
    private float originalTurnspeed;
    private string m_MovementAxisName; // The name of the input axis for moving forward and back.
    private string m_FlipName;
    private string m_DashName;
    private string m_LeftTurn;
    private string m_SemiFlipName;
    private string m_TurnAxisName;              // The name of the input axis for turning.
    private Rigidbody m_Rigidbody;              // Reference used to move the tank.
    private float m_MovementInputValue;         // The current value of the movement input.
    private float m_TurnInputValue;             // The current value of the turn input.
    private float m_TurnCraft;
    private bool specialMove;
    int turning = 0;
    public RechargableHability _dashHability = new RechargableHability();

    private void Awake()
    {
        originalSpeed = speed;
        _dashHability.SetRechargeHability();
        m_Rigidbody = GetComponent<Rigidbody>();
    }


    private void OnEnable()
    {
        // When the tank is turned on, make sure it's not kinematic.
        m_Rigidbody.isKinematic = false;

        // Also reset the input values.
        m_MovementInputValue = 0f;
        m_TurnInputValue = 0f;
    }
    
    private void OnDisable()
    {
        // When the tank is turned off, set it to kinematic so it stops moving.
        m_Rigidbody.isKinematic = true;
    }


    private void Start()
    {
        // The axes names are based on player number.
        originalTurnspeed = turnSpeed;
        originalYPosition = transform.position.y;
        m_MovementAxisName = "Vertical" + PlayerNumber;
        m_SemiFlipName = "InputSemiFlip" + PlayerNumber;
        m_FlipName = "InputFlip" + PlayerNumber;
        m_DashName = "Dash" + PlayerNumber;
        m_TurnAxisName = "Horizontal" + PlayerNumber;
        m_LeftTurn = "TurnLeft" + PlayerNumber;
    }

    private void Update()
    {
        // Store the value of both input axes.
        if (!specialMove)
        {
            m_MovementInputValue = 1;
            m_TurnInputValue = hInput.GetAxis(m_TurnAxisName);
            m_TurnCraft = hInput.GetAxis(m_LeftTurn);
            body.localEulerAngles = new Vector3(0, 0, -m_TurnCraft * 45);


            if (hInput.GetButtonDown(m_SemiFlipName))
            {
                StopAllCoroutines();
                StartCoroutine(semiTotallyFlip());
            }

            if (hInput.GetButtonDown(m_FlipName))
            {
                StopAllCoroutines();
                StartCoroutine(totallyFlip());
            }
            if (hInput.GetButtonDown(m_DashName))
            {
                _dashHability.Active();
            }
            else if (hInput.GetButton(m_DashName))
            {
                //StopAllCoroutines();
                m_dashSlider.value = _dashHability.usingHability(m_dashSlider.value);
                if (_dashHability.isActive)
                {
                    speed = 1.5f * originalSpeed;
                }
                else
                {
                    speed = originalSpeed;
                }
            }
            if (hInput.GetButtonUp(m_DashName))
            {
                speed = originalSpeed;
                _dashHability.Deactive();
            }
        }
        else
        {
            if (_dashHability.isActive)
                _dashHability.isActive = false;
        }
        if (!_dashHability.isActive && m_dashSlider.value < 1)
        {
            m_dashSlider.value = _dashHability.recharging(m_dashSlider.value);
        }
        Vector3 center = new Vector3(0, transform.position.y, 0);
        Vector3 distance = center + transform.position;
        Debug.DrawLine(center, distance, Color.red);
    }

    public void makeItFlip()
    {
        StopAllCoroutines();
        StartCoroutine(flipping());
    }

    private void FixedUpdate()
    {
        // Adjust the rigidbodies position and orientation in FixedUpdate.
        Move();
        Turn();
    }

    IEnumerator flipping()
    {
        yield return StartCoroutine(semiTotallyFlip());
        lookAtCenter();
    }

    private void Move()
    {
        // Create a vector in the direction the tank is facing with a magnitude based on the input, speed and the time between frames.
        Vector3 movement = transform.forward * m_MovementInputValue * speed * Time.deltaTime;
        // Apply this movement to the rigidbody's position.
        m_Rigidbody.MovePosition(m_Rigidbody.position + movement);
    }

    public void lookAtCenter()
    {
        Vector3 center = new Vector3(0, transform.position.y, 0);
        Vector3 distance = center - transform.position;
        
        transform.forward = distance.normalized;
    }


    private void Turn()
    {
        // Determine the number of degrees to be turned based on the input, speed and time between frames.
        float turn = m_TurnInputValue * turnSpeed * Time.deltaTime;
        if (m_TurnInputValue * m_TurnCraft < 0)
        {
            turn = (m_TurnInputValue * originalTurnspeed * Time.deltaTime) / 2;
        }
        else if (Mathf.Abs(m_TurnCraft) > 0)
        {
            turn *= (1 + Mathf.Abs(m_TurnCraft));
        }
        // Make this into a rotation in the y axis.
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        // Apply this rotation to the rigidbody's rotation.
        m_Rigidbody.MoveRotation(m_Rigidbody.rotation * turnRotation);
    }

    IEnumerator totallyFlip()
    {
        specialMove = true;
        float radio = 2;
        float timePass = Time.deltaTime * 2;
        float angleYx = Mathf.Sin(transform.eulerAngles.y * Mathf.Deg2Rad);
        float angleYz = Mathf.Cos(transform.eulerAngles.y * Mathf.Deg2Rad);
        while ((100 * timePass * Mathf.Deg2Rad) < 2 * Mathf.PI)
        {
            transform.forward = new Vector3(angleYx * radio * Mathf.Cos(100 * timePass * Mathf.Deg2Rad),
                                            radio * Mathf.Sin(100 * timePass * Mathf.Deg2Rad),
                                            angleYz * radio * Mathf.Cos(100 * timePass * Mathf.Deg2Rad));
            timePass += Time.deltaTime * 2;
            yield return null;
        }

        specialMove = false;
    }

    IEnumerator semiTotallyFlip()
    {
        specialMove = true;
        float radio = 2;
        float timePass = 0;
        float angleYx = Mathf.Sin(transform.eulerAngles.y * Mathf.Deg2Rad);
        float angleYz = Mathf.Cos(transform.eulerAngles.y * Mathf.Deg2Rad);
        while ((100 * timePass * Mathf.Deg2Rad) < (3 * Mathf.PI / 2))
        {
            transform.forward = new Vector3(angleYx * radio * Mathf.Cos(100 * timePass * Mathf.Deg2Rad),
                                            radio * Mathf.Sin(100 * timePass * Mathf.Deg2Rad),
                                            angleYz * radio * Mathf.Cos(100 * timePass * Mathf.Deg2Rad));
            timePass += Time.deltaTime * 2;
            yield return null;
        }
        
        radio = -2;
        timePass = Mathf.PI / 4;
        while ((100 * timePass * Mathf.Deg2Rad) > 0)
        {
            transform.forward = new Vector3(angleYx * radio * Mathf.Cos(100 * timePass * Mathf.Deg2Rad),
                                            radio * Mathf.Sin(100 * timePass * Mathf.Deg2Rad),
                                            angleYz * radio * Mathf.Cos(100 * timePass * Mathf.Deg2Rad));
            timePass -= Time.deltaTime * 2;
            yield return null;
        }
        specialMove = false;
    }

    IEnumerator RechargingHability()
    {
        m_dashSlider.value = _dashHability.recharging(m_dashSlider.value);
        while (m_dashSlider.value < 1)
        {
            m_dashSlider.value = _dashHability.recharging(m_dashSlider.value);
            yield return null;
        }
    }
}
