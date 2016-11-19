using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class rotator : MonoBehaviour {

    Rigidbody rb;
    Quaternion targetRotation;
    Quaternion defaultRotation;

    private Vector3 eulerAngleVelocity;

    bool rotateFlag = true;

    string currentAxis = "none";

    public Button yawbt, rollbt, bothbt, resetbt;

    private float limitedAngle = 0.32f;
    private float fixedRotateSpeed = 65f;

	void Start () {
        rb = GetComponent<Rigidbody>();

        defaultRotation = transform.rotation;

//        StartCoroutine(printEulerAngle());

        // listen to buttons
        yawbt.onClick.AddListener(delegate {
            resetRotation();
            StopCoroutine("resetRotationForBoth");
            currentAxis = "yaw";
        });
        rollbt.onClick.AddListener(delegate {
            resetRotation();
            StopCoroutine("resetRotationForBoth");
            currentAxis = "roll";
        });
        bothbt.onClick.AddListener(delegate {
            resetRotation();
            StartCoroutine("resetRotationForBoth");
            currentAxis = "both";
        });
        resetbt.onClick.AddListener(delegate {
            resetRotation();
            StopCoroutine("resetRotationForBoth");
            currentAxis = "none";
        });
	}

    void resetRotation()
    {
        transform.rotation = defaultRotation;
    }

    // swith to check limit on current selected axis 
    void onAxisSet(string axis)
    {
        switch (axis)
        {
            case "yaw":
                if (rb.rotation.y >= limitedAngle)
                {
                    rotateFlag = !rotateFlag;
                    setRotationVelocity("yaw_false");
                }
                else if (rb.rotation.y <= -limitedAngle)
                {
                    rotateFlag = !rotateFlag;
                    setRotationVelocity("yaw_true");
                }
                else if (rb.rotation.y == 0)
                {
                    rotateFlag = !rotateFlag;
                    setRotationVelocity("yaw_true");
                }

                break;

            case "roll":
                if (rb.rotation.z >= limitedAngle)
                {
                    rotateFlag = !rotateFlag;
                    setRotationVelocity("roll_false");
                }
                else if (rb.rotation.z <= -limitedAngle)
                {
                    rotateFlag = !rotateFlag;
                    setRotationVelocity("roll_true");
                }
                else if (rb.rotation.z == 0)
                {
                    rotateFlag = !rotateFlag;
                    setRotationVelocity("roll_true");
                }

                break;

            case "both":
                if (rb.rotation.y >= limitedAngle || rb.rotation.z <= -limitedAngle)
                {
                    rotateFlag = !rotateFlag;
                    setRotationVelocity("both_false");
                }
                else if (rb.rotation.y <= -limitedAngle || rb.rotation.z >= limitedAngle)
                {
                    rotateFlag = !rotateFlag;
                    setRotationVelocity("both_true");
                }
                else if (rb.rotation.y == 0 || rb.rotation.z == 0)
                {
                    rotateFlag = !rotateFlag;
                    setRotationVelocity("both_true");
                }

                break;
        }
    }

    void setRotationVelocity(string axis_flag)
    {
        switch (axis_flag)
        {
            case "yaw_true":
                eulerAngleVelocity = new Vector3(0, fixedRotateSpeed, 0);
                break;

            case "yaw_false":
                eulerAngleVelocity = new Vector3(0, -fixedRotateSpeed, 0);
                break;

            case "roll_true":
                eulerAngleVelocity = new Vector3(0, 0, fixedRotateSpeed);
                break;

            case "roll_false":
                eulerAngleVelocity = new Vector3(0, 0, -fixedRotateSpeed);
                break;

            case "both_true":
                eulerAngleVelocity = new Vector3(0, fixedRotateSpeed, -fixedRotateSpeed);
                break;

            case "both_false":
                eulerAngleVelocity = new Vector3(0, -fixedRotateSpeed, fixedRotateSpeed);
                break;
        }
    }

    void moveAxis(string axis)
    {
        onAxisSet(axis);

        Quaternion deltaRotation = Quaternion.Euler(eulerAngleVelocity * Time.deltaTime);
        targetRotation = rb.rotation * deltaRotation;

        rb.MoveRotation(targetRotation); 
    }

    void FixedUpdate() 
    {
        if (currentAxis == "none")
        {
            resetRotation();
        }
        else
        {
//            Debug.Log(eulerAngleVelocity);
            moveAxis(currentAxis);
        }
    }

    IEnumerator printEulerAngle()
    {
        while (true)
        {
            Debug.Log(rb.rotation.y);

            yield return new WaitForSeconds(0.5f);
        }
    }

    // work around for asynchronous between yaw and roll.
    // since after several rounds, rotation becomes shorter and shorter.
    // thus reset every times before it happens to shift making thing looks better.
    IEnumerator resetRotationForBoth()
    {
        while (true)
        {
            resetRotation();
            yield return new WaitForSeconds(4.65f);
        }
    }
}
