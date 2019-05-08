﻿using System;
using System.Timers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class Earth : Component, IPointerClickHandler {

    [SerializeField]
    private GameObject initialGameObject;

    [SerializeField]
    private GameObject[] listOfAttacks;

    private IEnumerator coroutine;

    private void Start() {
        Name = "Earth";
        // Set true when you want earth spawner to be active
        if (true && SceneManager.GetActiveScene() != SceneManager.GetSceneByName("Tutorial")) {
            // Time needs to be dynamic
            this.coroutine = StartSpawningAttacks();
            StartCoroutine(this.coroutine);
        }
    }

    private IEnumerator StartSpawningAttacks() {
        Debug.Log("Starting to spawn attacks...");
        while (true) { 
            Debug.Log("Waiting...");
            yield return new WaitForSeconds(UserBehaviourProfile.Instance.SpawnTime);
            CreateRandomEnemy();
            Debug.Log("Done Waiting!");
        }
        CreateRandomEnemy();
    }

    private void CreateRandomEnemy() {
        Debug.Log("Spawning random enemy");
        int randomInt = UnityEngine.Random.Range(0, 2);
        float rand = UnityEngine.Random.Range(0f, 1.0f);
        // If condition is true create Web Attack
        if (randomInt == 0 && (rand < UserBehaviourProfile.Instance.WebAttackProb)) {
            // TODO Might have to tweak spawn time value
            UserBehaviourProfile.Instance.SpawnTime = 3.0f;
            Debug.Log("Creating WebAttack...");
            WebAttack webAttack = (new GameObject("WebAttack")).AddComponent<WebAttack>();
            webAttack.Run((Component) this.initialGameObject.GetComponent(typeof(Component)),
                typeof(Computer));
        }
        // If condition is true, create document attack
        if (randomInt == 1 && (rand) < UserBehaviourProfile.Instance.DocumentAttackProb) {
            Debug.Log("Preparing to create Document Attack...");
            if (UserBehaviourProfile.Instance.documentHacked == false) {
                Debug.Log("Creating DocumentAttack...");
                DocumentAttack documentAttack = (new GameObject("DocumentAttack")).AddComponent<DocumentAttack>();
                documentAttack.Run((Component) this.initialGameObject.GetComponent(typeof(Component)),
                    typeof(Document));
            } else {
                Debug.Log("Document has already been taken control of...");
            }
        }
    }
}

