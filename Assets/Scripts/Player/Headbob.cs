using TMPro;
using UnityEngine;

public class Headbob : MonoBehaviour
{
    public bool enableVariableWalkSpeed;

    public float walkingBobbingSpeed = 5f;
    public float bobbingAmount = 0.1f;
    public float tiltAmount = 2f;
    public PlayerCharacter controller;

    public Transform footStepPosition;
    public AudioClip[] footSteps;

    float defaultPosY = 0;
    float timer = 0;

    private float standingBobbingSpeed;
    private float crouchingBobbingSpeed;
    private float standingBobbingAmount;
    private float crouchingBobbingAmount;

    private float AppliedWalkingBobspeed
    {
        get
        {
            return walkingBobbingSpeed;
        }
    }

    private void Awake()
    {
        standingBobbingSpeed = walkingBobbingSpeed;
        standingBobbingAmount = bobbingAmount;
        crouchingBobbingAmount = standingBobbingAmount / 1.3f;
        crouchingBobbingSpeed = standingBobbingSpeed / 2;
    }



    // Start is called before the first frame update
    void Start()
    {
        defaultPosY = transform.localPosition.y;
    }

    // Update is called once per frame
    void Update()
    {
        FootstepUpdate();
    }

    private void FootstepUpdate()
    {
        if (controller.NormalizedWalkVelocity > 0 && controller.IsGrounded)
        {
            float currentInputMagnitude = controller.NormalizedWalkVelocity;
            timer += Time.deltaTime * AppliedWalkingBobspeed * 2;
            transform.localPosition = new Vector3(transform.localPosition.x, defaultPosY + Mathf.Sin(timer) * bobbingAmount * currentInputMagnitude, transform.localPosition.z);
            Vector3 eulerAngles = transform.localEulerAngles;
            eulerAngles.z = Mathf.Sin(timer * 0.5f) * tiltAmount * currentInputMagnitude;
            transform.localRotation = Quaternion.Euler(eulerAngles);
        }
        else
        {
            timer = 0;
            transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(transform.localPosition.y, defaultPosY, Time.deltaTime * AppliedWalkingBobspeed), transform.localPosition.z);
            Vector3 eulerAngles = transform.localEulerAngles;
            eulerAngles.z = Mathf.LerpAngle(eulerAngles.z, 0, Time.deltaTime * AppliedWalkingBobspeed);
            transform.localRotation = Quaternion.Euler(eulerAngles);
        }
    }

    public void SetCrouching(bool enableCrouching)
    {
        bobbingAmount = enableCrouching ? crouchingBobbingAmount : standingBobbingAmount;
        walkingBobbingSpeed = enableCrouching ? crouchingBobbingSpeed : standingBobbingSpeed;
    }
}
