using UnityEngine;

public class ShellExplosion : MonoBehaviour
{
    public LayerMask m_TankMask;          
    public float m_MaxDamage = 20f;                  
    public float m_ExplosionForce = 1000f;            
    public float m_MaxLifeTime = 2f;                  
    public float m_ExplosionRadius = 0.5f;

    Transform parent;

    private void Awake()
    {
        parent = transform.parent;
        gameObject.SetActive(false);
    }
    
    public void OnEnable()
    {
        Invoke("Deactivate", m_MaxLifeTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Find all the tanks in an area around the shell and damage them.
        Collider[] colliders = Physics.OverlapSphere(transform.position, m_ExplosionRadius, m_TankMask);
        for (int i = 0; i < colliders.Length; i++)
        {
            Rigidbody targetRigidBody = colliders[i].transform.parent.GetComponent<Rigidbody>();
            if (!targetRigidBody)
                continue;

            TankHealth targetHealth = targetRigidBody.GetComponent<TankHealth>();
            if (!targetHealth)
                continue;
            
            targetHealth.TakeDamage(20);
            Deactivate();
        }
    }

    void Deactivate()
    {
        gameObject.SetActive(false);
        transform.SetParent(parent);
    }
}