using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    [Header("Inscribed")]
    public GameObject projectilePrefab;
    public float velocityMult = 10f;
    public GameObject projlinePrefab;

    [Header("Dynamic")]
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;
    private void Awake()
    {
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;
    }
    void OnMouseEnter()
    {
        //print("Slingshot:OnMouseEnter()"); 
        launchPoint.SetActive(true);
    }

    void OnMouseExit()
    {
        //print("Slingshot:OnMouseExit()");
        launchPoint.SetActive(false);
    }
    void OnMouseDown()
    {
        aimingMode = true;
        projectile = Instantiate(projectilePrefab);
        projectile.transform.position = launchPos;
        projectile.GetComponent<Rigidbody>().isKinematic = true;

    }
    void Update()
    {
        if (!aimingMode) { return; }

        //get current mouse position
        Vector3 mousePos2d = Input.mousePosition;
        mousePos2d.z = -Camera.main.transform.position.z;
        Vector3 mousePos3d = Camera.main.ScreenToWorldPoint(mousePos2d);

        //find delta from launch position
        Vector3 mouseDelta = mousePos3d - launchPos;
        //limit mouseDelta to the radius
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if (mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }
        //move projectile to this new position
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;

        if (Input.GetMouseButtonUp(0))
        { 
            aimingMode = false;
            Rigidbody projRb = projectile.GetComponent<Rigidbody>();
            projRb.isKinematic = false;
            projRb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            projRb.velocity = -mouseDelta * velocityMult;
            FollowCam.POI = projectile;
            Instantiate<GameObject>(projlinePrefab,projectile.transform);
            projectile = null;
        }
    }
}
