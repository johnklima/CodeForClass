using UnityEngine;

public class CannonBall : MonoBehaviour
{
    public float G = 9.8f;
    public Vector3 direction;

    public Transform start;
    public Transform end;

    public bool inAir;
    public float launchAngle = 45;

    public BallGravity grav;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    private void Start()
    {
        grav = GetComponent<BallGravity>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && !inAir)
        {
            //lift up and forward
            transform.position = start.position;
            transform.position += Vector3.up + transform.forward * 2;

            transform.LookAt(end);
            grav.enabled = true;
            grav.impulse = fire(transform.position, end.position, launchAngle);

            inAir = true; //set to false when it hits something (on collision enter)
        }
    }

    public Vector3 fire(Vector3 startPos, Vector3 targPos, float angle)
    {
        direction = targPos - startPos;
        return calculateIterativeTrajectory(startPos, targPos, angle, false);
    }

    private Vector3 calculateIterativeTrajectory(Vector3 startPoint, Vector3 endPoint, float desiredAngle,
        bool checkAngle)
    {
        Vector3 t;
        Vector3 c;

        t = endPoint;
        c = startPoint;

        //get a flat distance
        t.y = c.y = 0;
        var flatdistance = Vector3.Distance(t, c);

        Vector3 P1, P2, p2;
        p2 = P2 = endPoint - startPoint;

        //get just the flat direction without y component
        p2.y = 0;
        p2.Normalize();
        P1 = p2;

        //unitize the vector
        P2.Normalize();


        float angle = 0;

        //if we are asked to use a specific angle, this is our angle, for better or worse        
        angle = Mathf.Deg2Rad * desiredAngle;

        //add a bit of inclination just in case
        angle += Mathf.Acos(Vector3.Dot(P1, Vector3.up)) * 0.15f;

        //get direct angle in rads, this is a minimum launch angle
        var directAngle = Mathf.Acos(Vector3.Dot(P1, P2));

        if (angle < directAngle)
            angle = directAngle;

        //any angle less than 45 is 45 (optimal angle)
        //any angle greater is the angle, plus HALF the angle of the vector to y up.
        //with balistics the angle ALWAYS has to be greater than the direct angle from
        //point to point. so we add half of this angle to 90, splitting the difference
        if (checkAngle)
        {
            if (angle < 0.785398163f)
                angle = 0.785398163f;


            //if it is too close to pure vertical make it less than pure vertical
            if (angle > Mathf.PI / 2 - 0.05f)
                angle = Mathf.PI / 2 - 0.05f;

            //if we are going from up to down, use direct angle
            if (startPoint.y > endPoint.y)
                angle = directAngle;
        }

        var Y = endPoint.y - startPoint.y;


        // perform the trajectory calculation to arrive at the gun powder charge aka 
        // target velocity for the distance we desire, based on launch angle
        float rng = 0;
        float Vo = 0;
        var trydistance = flatdistance;
        var iters = 0;

        //now iterate until we find the correct proposed distance to land on spot.
        //this method is based on the idea of calculating the time to peak, and then
        //peak to landing at the height differential. we can then extract an XY distance 
        //achieved regardless of height differential. by iterating with a binary heuristic
        //we increase or decrease the initial velocity until we acheive our XY distance

        angle = Mathf.Abs(angle); // no negative numbers please!

        var f = Mathf.Sin(angle * 2.0f);


        while (Mathf.Abs(rng - flatdistance) > 0.001f && iters < 64)
        {
            //make sure we dont squirt on a negative number. we can IGNORE that result
            if (trydistance > 0)
            {
                //---------------create an initial force-------------------------
                //-----------( / f seems to do nothing??? )----------------------
                //-it behaves as a constant and Vo is adjusted in relation to f?-
                //---------------------------------------------------------------

                var sqrtcheck = trydistance * G / f;

                if (sqrtcheck > 0)
                    Vo = Mathf.Sqrt(sqrtcheck);
                else
                    Debug.Log("sqrtcheck < 0 initial force");

                //find the vector of it in our trajectory planar space
                var Vy = Vo * Mathf.Sin(angle);
                var Vx = Vo * Mathf.Cos(angle);

                //get a height and thus time to peak
                var H = -Y + Vy * Vy / (2 * G);
                var upt = Vy / G; // time to max height

                //if again we squirt on a neg, but it is because our angle and force
                //are too accute. note: we handle up and down trajectory differently
                if (2 * H / G < 0)
                {
                    if (endPoint.y < startPoint.y)
                        rng = flatdistance; //if going down we are done
                    else
                        rng = 0; //if up we are not
                }
                else
                {
                    sqrtcheck = 2 * H / G;
                    if (sqrtcheck > 0)
                    {
                        var dnt = Mathf.Sqrt(sqrtcheck); // time from max height to impact
                        rng = Vx * (upt + dnt);
                    }
                    else
                    {
                        Debug.Log("sqrtcheck < 0 H / Gravity");
                    }
                }

                if (rng > flatdistance)
                    trydistance -=
                        (rng - flatdistance) /
                        2; //using a binary zero-in, it takes about 8 iterations to arrive at target
                else if (rng < flatdistance)
                    trydistance += (flatdistance - rng) / 2;


                //Debug.Log("ITERS = " + iters);
            }
            else
            {
                iters = 64;
            }

            iters++;
        }

        var angV = new Vector3(direction.x, 0, direction.z);

        angV.Normalize();
        var side = Vector3.Cross(angV, Vector3.up);
        side.Normalize();


        //we need to rotate that by our actual launch angle
        angV = Quaternion.AngleAxis(Mathf.Rad2Deg * angle, side) * angV;

        //multiply by calculated "powder charge"
        angV *= Vo;

        Debug.Log(Vo.ToString());

        return angV;
    }
}