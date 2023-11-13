using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent( typeof(BoundsCheck) )]
public class ProjectileHero : MonoBehaviour {
    private BoundsCheck  bndCheck;
    private Renderer    rend;

    [Header("Dynamic")]
    public Rigidbody    rigid;
    GameObject closest; 
    [SerializeField]                                                         // a
    private eWeaponType _type;    
    
    private float x0; 
    public float waveRotY  = 45; 
    public float    waveWidth = 2;
    public float    waveFrequency = 1;
    private float   birthTime;

    
    // This public property masks the private field _type
    public eWeaponType   type {                                              // c
        get { return( _type ); }
        set { SetType( value ); }                  
    }

    void Start() {
        // Set x0 to the initial x position of Enemy_1
        x0 = transform.position.x;                                                           // c
        birthTime = Time.time;

        if(this.vel == Vector3.down * 50)
        {
            waveWidth *= -1;
            this.vel = Vector3.up * 50;
        }
    }

    void Awake () {
        bndCheck = GetComponent<BoundsCheck>();
        rend = GetComponent<Renderer>();                                     // d
        rigid = GetComponent<Rigidbody>(); 
        
    }

    void Update () 
    {
        if ( bndCheck.LocIs(BoundsCheck.eScreenLocs.offUp) ) {
            Destroy( gameObject );
        }
        if(_type == eWeaponType.missile)
        {
            if (closest)
            {
                transform.position = Vector3.MoveTowards(transform.position, closest.transform.position, .05f);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        else if(_type == eWeaponType.phaser)
        {
            Vector3 tempPos = transform.position;
            // theta adjusts based on time
            float age = Time.time - birthTime;
            float theta = Mathf.PI * 2 * age / waveFrequency;
            float sin = Mathf.Sin(theta);
            tempPos.x = x0 + waveWidth * sin;
            transform.position = tempPos;
            // rotate a bit about y
            Vector3 rot = new Vector3(0, sin *waveRotY, 0);
            this.transform.rotation = Quaternion.Euler(rot);
        }
    }
        
    /// <summary>
    /// Sets the _type private field and colors this projectile to match the 
    ///   WeaponDefinition.
    /// </summary>
    /// <param name="eType">The eWeaponType to use.</param>
    public void SetType( eWeaponType eType ) {                               // e
        _type = eType;
        WeaponDefinition def = Main.GET_WEAPON_DEFINITION( _type );
        rend.material.color = def.projectileColor;
    }

    /// <summary>
    /// Allows Weapon to easily set the velocity of this ProjectileHero
    /// </summary>
    public Vector3 vel {
        get { return rigid.velocity; }
        set { rigid.velocity = value; }
    }

    public void SetClosest(GameObject c)
    {
        closest = c;
    }
    
}
