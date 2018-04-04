using UnityEngine;
using UnityEngine.UI;

public class TankHealth : MonoBehaviour
{
    //Add event for death
    public float m_StartingHealth = 100f;
    public Slider m_Slider;
    public Image m_FillImage;
    public Color m_FullHealthColor = Color.green;
    public Color m_ZeroHealthColor = Color.red;
    public float m_CurrentHealth;
    private bool m_Dead;

    private void OnEnable()
    {
        m_CurrentHealth = m_StartingHealth;
        m_Dead = false;

        SetHealthUI(m_CurrentHealth);
    }

    public void TakeDamage(float amount)
    {
        // Adjust the tank's current health, update the UI based on the new health and check whether or not the tank is dead.
        m_CurrentHealth -= amount;
        SetHealthUI(m_CurrentHealth+amount);
        if(m_CurrentHealth <= 0 && !m_Dead)
        {
            OnDeath();
        }
    }


    private void SetHealthUI(float originalHealth)
    {
        // Adjust the value and colour of the slider.
        m_Slider.value = Mathf.Lerp (originalHealth / m_StartingHealth, m_CurrentHealth / m_StartingHealth, m_CurrentHealth / m_StartingHealth);

        Debug.Log(m_CurrentHealth / m_StartingHealth);
        m_FillImage.color = Color.Lerp(m_ZeroHealthColor, m_FullHealthColor, m_CurrentHealth/m_StartingHealth);
    }


    private void OnDeath()
    {
        // Play the effects for the death of the tank and deactivate it.
        m_Dead = true;
        gameObject.SetActive(false);
    }
}