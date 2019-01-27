using System.Collections.Generic;
using UnityEngine;
using Hellmade.Sound;

[RequireComponent(typeof(PolygonCollider2D), typeof(Rigidbody2D), typeof(Interactable))]

public class Obstacle : MonoBehaviour, IAttachable
{
    public new PolygonCollider2D collider { get { if (_collider == null) _collider = GetComponent<PolygonCollider2D>(); return _collider; } }
    private PolygonCollider2D _collider;

    public Rigidbody2D rigidbody2 { get { if (_rigidbody == null) _rigidbody = GetComponent<Rigidbody2D>(); return _rigidbody; } }
    private Rigidbody2D _rigidbody;

    private PolygonCollider2D clonedCollider;
    public AudioManager am;

    List<RelativeJoint2D> relativeJoint2DList = new List<RelativeJoint2D>();

    public float health = 3;

    void Start()
    {
       am = GameObject.Find("AudioManager").GetComponent<AudioManager>();
    }

    public bool isAttached
    {
        get
        {
            for (int i = 0; i < relativeJoint2DList.Count; i++)
            {
                if (relativeJoint2DList[i].connectedBody != null) return true;
            }
            return false;
        }
    }

    CompositeCollider2D tmpCompositeCollider;
    Collider2D[] results = new Collider2D[100];

    private bool triedToAttach;

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
                tmpCompositeCollider.GetComponent<HouseManager>().Rebuild();
                TutorialManager.instance.OnWallPlaced();
                return true;
            }
        }
        return false;
    }

    private void AttachJointToRigidbody(Rigidbody2D rigidbody)
    {
        am.playAudio("BuildOneWall", 0.1f);

        rigidbody2.bodyType = RigidbodyType2D.Dynamic;

        RelativeJoint2D relativeJoint2D = gameObject.AddComponent<RelativeJoint2D>();
        relativeJoint2D.connectedBody = rigidbody;
        relativeJoint2DList.Add(relativeJoint2D);
       
    }

    private void CloneColliderAndAttach(CompositeCollider2D composite)
    {
  
        GameObject go = new GameObject("Obstacle", typeof(PolygonCollider2D));
        go.transform.position = transform.position;
        go.transform.rotation = transform.rotation;
        go.transform.SetParent(composite.transform);

        clonedCollider = go.GetComponent<PolygonCollider2D>();
        clonedCollider.pathCount = collider.pathCount;
        clonedCollider.points = collider.points;
        clonedCollider.usedByComposite = true;
    }

    public virtual void Drop()
    {
        rigidbody2.bodyType = RigidbodyType2D.Dynamic;
        collider.isTrigger = true;

        for (int i = 0; i < relativeJoint2DList.Count; i++)
        {
            Destroy(relativeJoint2DList[i]);
        }

        if(clonedCollider != null) {
            HouseManager houseManager = clonedCollider.GetComponentInParent<HouseManager>();
            Destroy(clonedCollider.gameObject);
            houseManager?.Rebuild();
        }
    }

    public void Damage(float damage)
    {
        am.playAudio("BuildingDamage", 0.1f);
        health -= damage;
        if (health <= 0)
        {
            Drop();
        }
    }
}

