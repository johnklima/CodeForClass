using UnityEngine;

public class IKSegment : MonoBehaviour
{

    public Vector3 Apos = new(0, 0, 0);
    public Vector3 Bpos = new(0, 0, 0);

    public float length = 1;

    public IKSegment parent;
    public IKSegment child;


    public void updateSegmentAndChildren()
    {
        updateSegment();

        //update its children
        if (child)
            child.updateSegmentAndChildren();
    }

    public void drag(Vector3 target)
    {
        pointAt(target);
        transform.position = target - transform.forward * length;

        if (parent)
            parent.drag(transform.position);
    }

    public void pointAt(Vector3 target)
    {
        transform.LookAt(target);

        //prevent twist? not sure but here is the place to try.
        var rot = new Vector3(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
        transform.rotation = Quaternion.Euler(rot);
    }

    public void reach(Vector3 target)
    {
        drag(target);
        updateSegment();
    }

    public void updateSegment()
    {
        if (parent)
        {
            Apos = parent.Bpos;         //could also use parent endpoint...
            transform.position = Apos;  //move me to Bpos (parent endpoint)
        }
        else
        {
            //Apos is always my position
            Apos = transform.position;
        }

        //Bpos is always where the endpoint will be, as calculated from length 
        calculateBpos();
    }

    private void calculateBpos()
    {
        Bpos = Apos + transform.forward * length;
    }

    
}
