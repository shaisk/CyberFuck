using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class CarPhysics : MonoBehaviour
{

    WheelJoint2D[] wheelJoints;
    JointMotor2D frontWheel;
    JointMotor2D backwheel;

    public float maxSpeed = -1000f;
    private float maxBackSpeed = 1500f;
    public float acceleration = 450f;
    private float dacceleration = -100f;
    public float brakeforce = 3000f;
    public float fuelSize;
    public float fuelUsage;
    public float fuelAdd = 3;
    private float currentFuel;
    private float gravity = 9.8f;
    private float angleCar = 0f;
    public bool grouned = false;
    public float wheelSize = 0.35f;
    public LayerMask map;
    public Transform bwheel;
    public Text coinsText;
    public int coinsInt = 0;
    public GameObject fp;
    public GameObject lp;
    public GameObject centerMass;
    public GameObject fuelProgressBar;
    private string[] tuning;
    private char del = 'f';
    public float timeStart = 5f;
    private int touchedUD;
    public float timeToLose = 5f;

    public Click[] ControlCar;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Rigidbody2D>().centerOfMass = centerMass.transform.localPosition;
        wheelJoints = gameObject.GetComponents<WheelJoint2D>();
        backwheel = wheelJoints[1].motor;
        frontWheel = wheelJoints[0].motor;

        tuning = data.t[data.car].Split(del);
        switch (data.car)
        {
            case 0:
                maxSpeed = maxSpeed - 350 * int.Parse(tuning[0]);
                brakeforce = brakeforce + 200 * int.Parse(tuning[1]);
                acceleration = acceleration + 150 * int.Parse(tuning[2]);
            break;

            case 1:
                maxSpeed = maxSpeed - 250 * int.Parse(tuning[0]);
                brakeforce = brakeforce + 100 * int.Parse(tuning[1]);
                acceleration = acceleration + 100 * int.Parse(tuning[2]);
                break;                
        }
        currentFuel = fuelSize;
    }

    void Update()
    {
        grouned = Physics2D.OverlapCircle(bwheel.transform.position, wheelSize, map);
        coinsText.text = coinsInt.ToString();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (currentFuel <= 0 || ((angleCar <= 130f && angleCar >= -105f) == false))
        {
            if (timeToLose > 0)
            {
                timeToLose -= Time.deltaTime;
            }
            else
            {
                lp.SetActive(true);
            }
        }
        else timeToLose = 5f;

        frontWheel.motorSpeed = backwheel.motorSpeed;
        angleCar = transform.localEulerAngles.z;
        if (angleCar >= 180)
        {
            angleCar = angleCar - 360;
        }
        print(angleCar);
        print(currentFuel);


        if (grouned == true)
        {
            if (ControlCar[0].clicked == true)
            {
                backwheel.motorSpeed = Mathf.Clamp(backwheel.motorSpeed - (acceleration - gravity * Mathf.PI * (angleCar / 2) * (GetComponent<Rigidbody2D>().velocity.sqrMagnitude * 1.75f)) * Time.fixedDeltaTime, maxSpeed, maxBackSpeed);
                currentFuel -= fuelUsage * Time.deltaTime;
            }

            else if ((backwheel.motorSpeed < 0) || (ControlCar[0].clicked == false && backwheel.motorSpeed == 0 && angleCar < 0))
            {
                backwheel.motorSpeed = Mathf.Clamp(backwheel.motorSpeed - (dacceleration - gravity * Mathf.PI * (angleCar / 2)) * Time.fixedDeltaTime, maxSpeed, 0);
                currentFuel -= (fuelUsage / 2.5f) * Time.deltaTime;
            }

            if ((ControlCar[0].clicked == false && backwheel.motorSpeed > 0) || (ControlCar[0].clicked == false && backwheel.motorSpeed == 0 && angleCar > 0))
            {
                backwheel.motorSpeed = Mathf.Clamp(backwheel.motorSpeed - (-dacceleration - gravity * Mathf.PI * (angleCar / 2)) * Time.fixedDeltaTime, 0, maxBackSpeed);
            }
        }

        else
        {
            if (ControlCar[0].clicked == false && backwheel.motorSpeed < 0) backwheel.motorSpeed = Mathf.Clamp(backwheel.motorSpeed - dacceleration * Time.fixedDeltaTime, maxSpeed, 0);
            else if (ControlCar[0].clicked == false && backwheel.motorSpeed > 0) backwheel.motorSpeed = Mathf.Clamp(backwheel.motorSpeed + dacceleration * Time.fixedDeltaTime, 0, maxBackSpeed);
            if (ControlCar[0].clicked == true)
            {
                backwheel.motorSpeed = Mathf.Clamp(backwheel.motorSpeed - (acceleration - gravity * (angleCar / 2) * (GetComponent<Rigidbody2D>().velocity.sqrMagnitude * 1.75f)) * Time.fixedDeltaTime, maxSpeed, maxBackSpeed);
                currentFuel -= (fuelUsage / 1.25f) * Time.deltaTime;
            }
        }
        
        if (ControlCar[1].clicked == true && backwheel.motorSpeed > 0)
            backwheel.motorSpeed = Mathf.Clamp(backwheel.motorSpeed - brakeforce * Time.fixedDeltaTime, 0, maxBackSpeed);
        else if (ControlCar[1].clicked == true && backwheel.motorSpeed < 0)
            backwheel.motorSpeed = Mathf.Clamp(backwheel.motorSpeed + brakeforce * Time.fixedDeltaTime, maxSpeed, 0);
        
        wheelJoints[1].motor = backwheel;
        wheelJoints[0].motor = frontWheel;

        fuelProgressBar.transform.localScale = new Vector2(currentFuel / fuelSize, 1);

        if (touchedUD == 1)
        {
            if (timeStart > 0)
            {
                timeStart -= Time.deltaTime;
            }
            else
            {
                maxSpeed = maxSpeed / 4;
                acceleration = acceleration / 4;
                touchedUD--;
                timeStart = 5f;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D trigger)
    {
        if (trigger.gameObject.tag == "coins")
        {
            coinsInt++;
            Destroy(trigger.gameObject);
        }
        else if (trigger.gameObject.tag == "finish")
        {
            fp.SetActive(true);
        }

        else if (trigger.gameObject.tag == "Fuel")
        {
            currentFuel += fuelAdd;
            Destroy(trigger.gameObject);
        }

        else if (trigger.gameObject.tag == "Upgrade")
        {
             maxSpeed = maxSpeed * 4;
             acceleration = acceleration * 4;
             touchedUD++;
             Destroy(trigger.gameObject);
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(bwheel.transform.position, wheelSize);
    }
}
