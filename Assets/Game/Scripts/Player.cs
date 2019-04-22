using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    private CharacterController playercontroller;

    // player speed variable
    [SerializeField]
    private float pSpeed = 3.5f;
    //gravity
    private float gravity = 9.81f;
    [SerializeField]
    private GameObject muzzleFlash;
    [SerializeField]
    private GameObject hitMarkerPrefab;
    [SerializeField]
    private AudioSource weaponAudio;

    [SerializeField]
    private int currentAmmo;
    private int maxAmmo = 50;

    private bool isReloading = false;

    private UI_Manager uiManager;


    // Start is called before the first frame update
    void Start()
    {
        playercontroller = GetComponent<CharacterController>();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        currentAmmo = maxAmmo;

        uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();
    }

    // Update is called once per frame
    void Update()
    {


        if (Input.GetMouseButton(0) && currentAmmo > 0)
        {

            Shoot();            

        }
        else
        {
            muzzleFlash.SetActive(false);
            weaponAudio.Stop();
        }

        if (Input.GetKeyDown(KeyCode.R) && isReloading == false)
        {
            isReloading = true;
            StartCoroutine(ReloadWeapon());
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        CalculateMovement();
    }

    void Shoot()
    {
        muzzleFlash.SetActive(true);
        currentAmmo--;
        uiManager.UpdateAmmo(currentAmmo);
        if (weaponAudio.isPlaying == false)
        {
            weaponAudio.Play();
        }


        Ray rayOrigin = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hitInfo;

        if (Physics.Raycast(rayOrigin, out hitInfo))
        {
            Debug.Log("Hit: " + hitInfo.transform.name);
            Instantiate(hitMarkerPrefab, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));


        }
    }


    IEnumerator ReloadWeapon()
    {
        yield return new WaitForSeconds(2f);
        currentAmmo = maxAmmo;
        uiManager.UpdateAmmo(currentAmmo);
        isReloading = false;
    }

    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 direction = new Vector3(horizontalInput, 0, verticalInput);
        Vector3 velocity = direction * pSpeed;
        velocity.y -= gravity;
        velocity = transform.transform.TransformDirection(velocity);
        playercontroller.Move(velocity * Time.deltaTime);
    }
}
