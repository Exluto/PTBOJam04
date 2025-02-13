﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class Health : NetworkBehaviour {

    public const int m_maxHealth = 100;

	[SyncVar(hook = "ChangeHealth")]
    public int m_currHealth;

	public CharacterMovement m_playerScript;
	public HUDScript m_hudScript;

	void Start() {
		if(!isServer) {
			CmdSetHealthAtStart();
		}
		m_currHealth = m_maxHealth;
		m_playerScript = GetComponent<CharacterMovement>();
	}

	void Update() {
		if(m_currHealth <= 0) {
			m_playerScript.Dead();
		}
	}

	[Command]
	public void CmdSetHealthAtStart() {
		m_currHealth = m_maxHealth;
	}

	public void TakeDamage(int damage) {
		if(!isServer) {
			CmdTakeDamage(damage);
		}
		m_currHealth -= damage;
	}

	[Command]
	public void CmdTakeDamage(int damage) {
		m_currHealth -= damage;
	}

	public int GetCurrentHealth() {
		return m_currHealth;
	}

	public void ChangeHealth(int health) {
		m_currHealth = health;
		m_hudScript.UpdateHUD(this.gameObject, health);
	}
}