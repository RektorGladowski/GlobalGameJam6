using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PolygonCollider2D))]
public class Obstacle : MonoBehaviour, IAttachable
{
    public new PolygonCollider2D collider { get { if (_collider == null) _collider = GetComponent<PolygonCollider2D>(); return _collider; } }
    private PolygonCollider2D _collider;

    private PolygonCollider2D clonedCollider;

    public Rigidbody2D rigidbody2 { get { if (_rigidbody == null) _rigidbody = GetComponent<Rigidbody2D>(); return _rigidbody; } }
    private Rigidbody2D _rigidbody;

    CompositeCollider2D tmpCompositeCollider;
    Collider2D[] results = new Collider2D[100];

    private bool triedToAttach;

    List<RelativeJoint2D> relativeJoint2DList = new List<RelativeJoint2D>();

    public bool TryAttaching()
    {
        ContactFilter2D contactFilter2D = new ContactFilter2D();

        int resultsCount = collider.OverlapCollider(contactFilter2D, results);

        for (int i = 0; i < resultsCount; i++)
        {
            Rigidbody2D currentRigidBody = results[i].GetComponent<Rigidbody2D>();
            if (currentRigidBody != null) AttachJointToRigidbody(currentRigidBody);

            tmpCompositeCollider = results[i] as CompositeCollider2D;
            if (tmpCompositeCollider != null)
            {
                CloneColliderAndAttach(tmpCompositeCollider);
                break;
            }
        }
        return false;
    }

    private void AttachJointToRigidbody(Rigidbody2D rigidbody)
    {
        RelativeJoint2D relativeJoint2D = gameObject.AddComponent<RelativeJoint2D>();
        relativeJoint2D.connectedBody = rigidbody;
        relativeJoint2DList.Add(relativeJoint2D);
    }

    private void CloneColliderAndAttach(CompositeCollider2D composite)
    {
        GameObject go = new GameObject("Obstacle", typeof(PolygonCollider2D));
        go.transform.position = transform.position;
        go.transform.SetParent(composite.transform);
        clonedCollider = go.GetComponent<PolygonCollider2D>();

        clonedCollider.pathCount = collider.pathCount;
        clonedCollider.points = collider.points;
        clonedCollider.usedByComposite = true;

    }

    public void Drop()
    {
       
    }

    private void Update()
    {
    }
}

