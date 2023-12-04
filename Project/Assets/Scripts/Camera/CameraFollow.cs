using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] Transform Target;
    [SerializeField] private Vector2 BoundsX;
    [SerializeField] private Vector2 BoundsY;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = ClampCameraPosition(Target.position);
    }

    Vector3 ClampCameraPosition(Vector2 position)
    {
        Vector3 Output = position;

        if (Output.x < BoundsX.x)
        {
            Output.x = BoundsX.x;
        }
        else if (Output.x > BoundsX.y)
        {
            Output.x = BoundsX.y;
        }

        if (Output.y < BoundsY.x)
        {
            Output.y = BoundsY.x;
        }
        else if (Output.y > BoundsY.y)
        {
            Output.y = BoundsY.y;
        }

        Output.z = -10;

        return Output;
    }

    private void OnDrawGizmos()
    {
        //BoundsX
        //BoundsY
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector2(BoundsX.x, BoundsY.x), new Vector2(BoundsX.x, BoundsY.y));
        Gizmos.DrawLine(new Vector2(BoundsX.y, BoundsY.x), new Vector2(BoundsX.y, BoundsY.y));

        Gizmos.DrawLine(new Vector2(BoundsX.x, BoundsY.x), new Vector2(BoundsX.y, BoundsY.x));
        Gizmos.DrawLine(new Vector2(BoundsX.x, BoundsY.y), new Vector2(BoundsX.y, BoundsY.y));
    }
}
