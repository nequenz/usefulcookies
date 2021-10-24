using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public sealed class CMover : CParent
{


    public static ComponentList<CMover> CList = new ComponentList<CMover>();

    public float Angle { set; get; } = 0.0f;

    public float Speed { set; get; } = 10.0f;

    public float SpeedRotation { set; get; } = 200.0f;

    public bool IsMovementBlocked { set; get; }  = false;

    public bool IsRotationBlocked { set; get; } = false;


    private Vector2 Friction = new Vector2(0.75f, 0.75f);


    private Vector2 Acceleration;


    //set


    public void setPosition(Vector2 Pos) => transform.position = Pos;



    //get

    public float getDistanceTo(Vector3 Point) => Vector3.Distance(transform.position, Point);

    public Vector2 getFrition() => Friction;

    public Vector2 getAcceleration() => Acceleration;

    public float getAngle() => Angle;

    public float getAngleToVector(Vector3 Target)
    {

        Vector3 Delta = Target - transform.position;

        float Langle = Mathf.Atan2(Delta.y, Delta.x) * Mathf.Rad2Deg;

        if (Langle < 0)
        {
            Langle += 360;
        }

        return Langle;
    }

    public float getAngleToVector(Vector3 Target, float OffsetAngle)
    {
        Vector3 Delta = Target - transform.position;

        float Langle = (Mathf.Atan2(Delta.y, Delta.x) * Mathf.Rad2Deg) + OffsetAngle;

        if (Langle < 0)
        {
            Langle += 360;
        }
        return Langle;
    }

    public Vector3 getPosition() => gameObject.transform.position;




    //action



    public void moveToPoint(Vector3 Point) => moveByAngle(getAngleToVector(Point));


    public void moveByAngle()
    {
        if (IsMovementBlocked) return;
        moveByAngle((Angle) * Mathf.Deg2Rad);
    }


    public void addAccelerattionByAngle(float Angle)
    {
        float DeltaSpeed = Time.deltaTime * Speed;

        Acceleration.x += DeltaSpeed * Mathf.Cos(Angle);
        Acceleration.y += DeltaSpeed * Mathf.Sin(Angle);
    }


    public void accelerateByAngle(float Angle)
    {
        float DeltaSpeed = Time.deltaTime * Speed;

        Acceleration.x = DeltaSpeed * Mathf.Cos(Angle);
        Acceleration.y = DeltaSpeed * Mathf.Sin(Angle);
    }


    public void updateAcceleration()
    {
        Acceleration.x *= Friction.x;
        Acceleration.y *= Friction.y;
    }


    public void moveByAngle(float Angle)
    {
        if (IsMovementBlocked) return;

        float DeltaSpeed = Time.deltaTime * Speed;

        Vector3 MoveVector = transform.position;

        MoveVector.x -= -DeltaSpeed * Mathf.Cos(Angle);
        MoveVector.y +=  DeltaSpeed * Mathf.Sin(Angle);

        transform.position = MoveVector;

    }


    public void rotate(float OffsetAngle)
    {
        if (IsRotationBlocked) return;

        Angle += OffsetAngle*Time.deltaTime*SpeedRotation;

        updateRotation();
    }


    public void setRotation(float SetAngle)
    {
        if (IsRotationBlocked) return;

        Angle = SetAngle;

        updateRotation();
    }


    public void rotateToVector(Vector3 Target)
    {
        if (IsRotationBlocked) return; 

        float AngleToVector     = getAngleToVector(Target);

        float AngleToVector2    = getAngleToVector(Target,-1.0f);

        float prevPath0         = Mathf.Abs(AngleToVector - Angle);

        float prevPath1         = Mathf.Abs(360 - prevPath0);

        float checkPath0        = Mathf.Abs(AngleToVector2 - Angle);

        //fix
        float _Diffrence = Mathf.Abs((prevPath0 - checkPath0));

        if (_Diffrence <= 0.1) _Diffrence = 0.1f;

        float Diffrence         = Mathf.Floor( ( prevPath0 - checkPath0 )/ _Diffrence );

        //Game.GUIText = "Diffrence:" + Diffrence+"\nPrevPath0:"+ prevPath0+"\nmy angle:"+(Angle)+"\nAngle to vec:"+ AngleToVector + "\ncheckPath0:"+ checkPath0;

        float RAngle = (Diffrence * Time.deltaTime * SpeedRotation);

        if (prevPath0 > prevPath1) Angle -= RAngle;
        else Angle += RAngle;

        updateRotation();
    }


    private void updateRotation()
    {
        if (Angle > 360) Angle -= 360;
        if (Angle < 0) Angle += 360;

        transform.rotation = Quaternion.AngleAxis(Angle+180, new Vector3(0, 0, 1));
    }


    public bool isOutsideCamera()
    {
        Vector3 Vec = Camera.main.WorldToViewportPoint(transform.position);
        return (Vec.x < 0 || Vec.x >= 1 || Vec.y >= 1 || Vec.y < 0);
    }








    private void Start()
    {
        updateRotation();
    }



    private void Update()
    {
        updateAcceleration();
    }



}
