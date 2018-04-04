using UnityEngine;
using UnityEngine.UI;

public class TankShooting : MonoBehaviour
{
    public int m_PlayerNumber = 1;       
    public Transform m_Shell;           
    public Transform m_FireTransform;    
    public Slider m_AimSlider;        
    public float m_MinLaunchForce = 1000000000000000f;
    public float m_MaxLaunchForce = 10000000000000000f; 
    public float m_MaxChargeTime = 0.75f;

    
    private string m_FireButton;         
    private float m_CurrentLaunchForce;

    private float m_ChargeSpeed;         
    private bool m_Fired;                


    private void OnEnable()
    {
        m_CurrentLaunchForce = m_MinLaunchForce;
        m_AimSlider.value = m_MinLaunchForce;
    }


    private void Start()
    {
        m_FireButton = "Fire" + m_PlayerNumber;

        m_ChargeSpeed = (m_MaxLaunchForce - m_MinLaunchForce) / m_MaxChargeTime;
    }

    private void Update()
    {
        // Track the current state of the fire button and make decisions based on the current launch force.
        m_AimSlider.value = m_MinLaunchForce;
        if(m_CurrentLaunchForce >= m_MaxLaunchForce && !m_Fired)
        {
            m_CurrentLaunchForce = m_MaxLaunchForce;
            Fire();
        }
        else if (hInput.GetButtonDown(m_FireButton))
        {
            m_Fired = false;
            m_CurrentLaunchForce = m_MinLaunchForce;
        }
        else if (hInput.GetButton(m_FireButton) && !m_Fired)
        {
            m_CurrentLaunchForce += m_ChargeSpeed * Time.deltaTime;
            m_AimSlider.value = m_CurrentLaunchForce;
        }
        else if(hInput.GetButtonUp(m_FireButton) && !m_Fired)
        {
            Fire();
        }
    }


    private void Fire()
    {
        // Instantiate and launch the shell.
        m_Fired = true;
        if (m_Shell.childCount > 0)
        {
            Rigidbody actualShell = m_Shell.GetChild(0).GetComponent<Rigidbody>();

            if (actualShell != null)
            {
                actualShell.transform.position = m_FireTransform.position;
                actualShell.transform.SetParent(null);
                actualShell.gameObject.SetActive(true);
                actualShell.transform.forward = m_FireTransform.forward;
                actualShell.velocity = m_MaxLaunchForce * m_FireTransform.forward;
                m_CurrentLaunchForce = m_MinLaunchForce;
            }
        }
    }
}