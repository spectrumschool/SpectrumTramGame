﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HalteManager : MonoBehaviour
{
    public GameObject haltePrefab;
    public Transform[] tracks;

    // Use this for initialization
    void Start ()
    {
        StartCoroutine(SpawnHalteCR());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator SpawnHalteCR()
    {
        while (true)
        {

            //welk spoor: random tussen 0 en 3
            int randomSpoor = UnityEngine.Random.Range(0, 4);
            //positie spoor, x waarde
            // y waarde buiten beeld
            Vector3 spawnPos = new Vector3(tracks[randomSpoor].localPosition.x, 8, 0);
            //halte aanmaken op deze positie
            Transform tfHalte = GameObject.Instantiate(haltePrefab, Vector3.zero, Quaternion.identity, this.transform).transform;
            tfHalte.localPosition = spawnPos;

            //random naam toewijzen
            int randomNaam = UnityEngine.Random.Range(0, GameManager.instance.haltenamen.Length);
            tfHalte.GetComponent<Halte>().haltenaam.text = GameManager.instance.haltenamen[randomNaam];

            yield return new WaitForSeconds(7.0f);
        }
    }
}
