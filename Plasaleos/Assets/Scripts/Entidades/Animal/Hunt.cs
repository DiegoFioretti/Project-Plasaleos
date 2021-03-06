﻿using UnityEngine;

[RequireComponent(typeof(Animal))]
public class Hunt : MonoBehaviour, IState {
    [SerializeField] float m_chaseSpeed;
    [SerializeField] float m_reachDistance;
    [SerializeField] float m_looseDistance;
    Animal m_animal;
    Alien m_alienPrey;
    Vector2 m_transformRight;
    Vector2 m_groundNormal;
    bool m_grounded;

    private void Awake() {
        m_animal = GetComponent<Animal>();
    }

    private void OnEnable() {
        GetComponent<Animator>().SetBool("Chasing", true);
    }

    private void OnDisable() {
        GetComponent<Animator>().SetBool("Chasing", false);
    }

    public void StateUpdate(out IState nextState) {
        m_animal.CheckForFlip();
        m_animal.CheckForLanding();
        m_animal.TakeGravityEffect();
        RaycastHit2D hit;
        if (!m_alienPrey.isActiveAndEnabled || (Vector3.Distance(
                transform.position, m_alienPrey.transform.position) > m_looseDistance)) {

            nextState = GetComponent<Movement>();
        } else {
            if ((m_animal.FacingRight && !m_alienPrey.FacingRight && 
                m_alienPrey.transform.position.x < transform.position.x) ||
                (!m_animal.FacingRight && m_alienPrey.FacingRight && 
                m_alienPrey.transform.position.x > transform.position.x)) {

                m_animal.Flip();
            }
            if (m_animal.SearchPrey(out hit)) {
                if (hit.distance < m_reachDistance) {
                    m_alienPrey.Damage();
                    Vector3 offset = m_animal.EntityRight * 0.5f;
                    m_alienPrey.transform.position = transform.position + offset;
                    m_alienPrey.transform.SetParent(transform);
                    nextState = GetComponent<Devour>();
                } else {
                    m_alienPrey = hit.transform.GetComponent<Alien>();
                    m_alienPrey.Scare(m_animal.FacingRight);
                    nextState = this;
                }
            } else {
                nextState = this;
            }
        }
    }

    public void StateFixedUpdate() {
        float angle = Vector2.Angle(m_animal.EntityRight, -Physics2D.gravity);
        if (m_animal.Grounded && angle <= 90f) {
            m_animal.m_rb.AddForce(m_animal.EntityRight * m_chaseSpeed * 2f);
        }
    }

    public void Chase(Transform prey) {
        m_alienPrey = prey.GetComponent<Alien>();
        m_alienPrey.Scare(m_animal.FacingRight);
    }
}