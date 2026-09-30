using UnityEngine;

public class IKSystem : MonoBehaviour
{

    public IKSegment[] segments;

    public int childcount;
    public Transform target;

    public bool isReaching;
    public bool isDragging;
    
    private IKSegment firstSegment;
    private IKSegment lastSegment;


    private void Awake()
    {
        //lets buffer our segements in an array
        childcount = transform.childCount;
        segments = new IKSegment[childcount];
        int i = 0;
        foreach (Transform child in transform)
        {
            segments[i] = child.GetComponent<IKSegment>();
            i++;
        }


        firstSegment = segments[0];
        lastSegment = segments[childcount - 1];
    }



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (isDragging)
        {
            lastSegment.drag(target.position);
        }
        else if (isReaching)
        {
            //call reach on the last
            lastSegment.reach(target.position);

            firstSegment.transform.position = transform.position;
            firstSegment.updateSegmentAndChildren();
        }

    }
}
