using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weaponInfo : MonoBehaviour
{
    public Vector3 offsetFromCenter=new Vector3(0.3f, 0, 0);
    public GameObject bullet;
    public float fireRateInSeconds=0.1f;
    public int maxBullets = 10;
    public float reloadTime = 2f;
    public float bulletSpread;
    public float bulletDamage;
    //public bool explosiveBullets;
    public float explosionRadius = 0f;
    public bool bulletsStay;

    public GameObject shootingPoint;
    public Vector2 posTaken;
    PlayerController controller;
    public GameObject targetPrefab;
    public string bulletSound;

    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        controller.backupPos += Controller_backupPos;
        controller.rewindPos += Controller_rewindPos;

    }

    private void Controller_rewindPos(object sender, System.EventArgs e)
    {
        transform.position = posTaken;
    }

    private void Controller_backupPos(object sender, System.EventArgs e)
    {
        posTaken = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
        UnityEditor.Handles.color = Color.yellow;

        UnityEditor.Handles.DrawWireDisc(transform.position, Vector3.back, explosionRadius);

    }
}
