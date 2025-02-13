﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class HitCollider : NetworkBehaviour {

	public bool m_hasDealtDamage;
	public AudioClip m_hit1;
	public AudioClip m_hit2;
	public AudioClip m_hit3;
	public AudioClip m_clash1;
	public AudioClip m_swipe1;
	public AudioClip m_swipe2;
	public AudioSource m_swordAudSrc;
	public AudioSync m_audSync;
	private int m_damage = 10;
	private Animator m_animController;
	private Health m_healthScript;
	private CharacterMovement m_characterScript;
	private int m_randNum;

	void Start () {
		m_characterScript = GetComponent<CharacterMovement>();
		m_healthScript = GetComponent<Health>();
		m_animController = GetComponent<Animator>();
	}

	void OnTriggerEnter(Collider other) {
		if(other.gameObject.tag == "EnemySword" && !other.transform.root.GetComponent<HitCollider>().m_hasDealtDamage) {
			if(isLocalPlayer) {
				CharacterMovement hitMovement = other.transform.root.GetComponent<CharacterMovement>();
				if(hitMovement != null) {
					if(m_animController.GetBool("isBlocking")) {
						m_audSync.PlaySound(3);
						m_characterScript.m_numOfBlockedAttacks++;
						m_animController.SetBool("blockedAttack", true);
					} 
					if(m_characterScript.m_numOfBlockedAttacks > 3) {
						m_animController.SetBool("blockedAttack", true);
						m_animController.SetBool("isBlocking", false);
						m_characterScript.m_numOfBlockedAttacks = 0;
					}
					if(!m_animController.GetBool("isBlocking")) {
						HurtSound();
						m_healthScript.TakeDamage(m_damage);
						m_characterScript.m_numOfBlockedAttacks = 0;
					}
					other.transform.root.GetComponent<HitCollider>().m_hasDealtDamage = true;
				}
			}
		}	
	}

	public void ResetBlock() {
		m_animController.SetBool("isBlocking", false);
		m_hasDealtDamage = false;
	}

	public void HurtSound() {
		m_randNum = Random.Range(1, 4);
		if(m_randNum == 1) {
			m_audSync.PlaySound(0);
		}
		if(m_randNum == 2) {
			m_audSync.PlaySound(1);
		}
		if(m_randNum == 3) {
			m_audSync.PlaySound(2);
		}
	}
}
