using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Inscribed")]
    public float speed    = 10f;   // The movement speed is 10m/s
    public float fireRate = 0.3f;  // Seconds/shot (Unused)
    public float health   = 10;    // Damage needed to destroy this enemy
    public int   score    = 100;   // Points earned for destroying this
    public float powerUpDropChance = 1f;

    protected bool calledShipDestroyed = false;
    protected BoundsCheck bndCheck;                                             // b

    void Awake() {                                                            // c
        bndCheck = GetComponent<BoundsCheck>();
    }

    // This is a Property: A method that acts like a field
    public Vector3 pos {                                                       // a
        get {
            return this.transform.position;
        }
        set {
            this.transform.position = value;
        }
    }

    void Update() {
        Move();                                                                // b

        if ( bndCheck.LocIs( BoundsCheck.eScreenLocs.offDown ) ) {             // a
            Destroy( gameObject );
        }
        
    }

    public virtual void Move() { // c
        Vector3 tempPos = pos;
        tempPos.y -= speed * Time.deltaTime;
        pos = tempPos;
    }

    void OnCollisionEnter( Collision coll ) {
        GameObject otherGO = coll.gameObject;
        
        // Check for collisions with ProjectileHero
        ProjectileHero p = otherGO.GetComponent<ProjectileHero>();
        if ( p != null ) {                                                    // b
            // Only damage this Enemy if it’s on screen
            if ( bndCheck.isOnScreen ) {                                      // c
                // Get the damage amount from the Main WEAP_DICT.
                health -= Main.GET_WEAPON_DEFINITION( p.type ).damageOnHit;
                if ( health <= 0 ) {    
                    if (!calledShipDestroyed){
                        calledShipDestroyed = true;
                        Main.SHIP_DESTROYED( this );
                    }                                      // d
                    // Destroy this Enemy
                    Destroy( this.gameObject );
                }
            }
            // Destroy the ProjectileHero regardless
            Destroy( otherGO );
        } else {
            print( "Enemy hit by non-ProjectileHero: " + otherGO.name );      // f
        }
    }
}
