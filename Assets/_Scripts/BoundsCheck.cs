using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoundsCheck : MonoBehaviour
{
    [System.Flags]
    public enum eScreenLocs {                                        // a
        onScreen = 0,  // 0000 in binary (zero)
        offRight = 1,  // 0001 in binary
        offLeft  = 2,  // 0010 in binary
        offUp    = 4,  // 0100 in binary
        offDown  = 8   // 1000 in binary
    }
    public enum eType { center, inset, outset };

    [Header("Inscribed")]
    public eType boundsType = eType.center;                                   // a
    public float radius = 1f;
    public bool keepOnScreen = true;

    [Header("Dynamic")]
    public eScreenLocs screenLocs = eScreenLocs.onScreen;
    public float camWidth;
    public float camHeight;

    void Awake() {
        camHeight = Camera.main.orthographicSize;                             // b
        camWidth = camHeight * Camera.main.aspect;                            // c
    }

    void LateUpdate () {                                                      // d
        float checkRadius = 0;
        if (boundsType == eType.inset)  checkRadius = -radius;
        if (boundsType == eType.outset) checkRadius = radius;
        Vector3 pos = transform.position;
 
        screenLocs = eScreenLocs.onScreen;                                    // b

        if ( pos.x > camWidth + checkRadius ) {
            pos.x  = camWidth + checkRadius;
            screenLocs |= eScreenLocs.offRight;                               // c
        }
        if ( pos.x < -camWidth - checkRadius ) {
            pos.x  = -camWidth - checkRadius;
            screenLocs |= eScreenLocs.offLeft;                                // c
        }

        if ( pos.y > camHeight + checkRadius ) {
            pos.y  = camHeight + checkRadius;
            screenLocs |= eScreenLocs.offUp;                                  // c
        }
        if ( pos.y < -camHeight - checkRadius ) {
            pos.y  = -camHeight - checkRadius;
            screenLocs |= eScreenLocs.offDown;                                // c
        }

        if ( keepOnScreen && !isOnScreen ) {                                  // d
            transform.position = pos;
            screenLocs = eScreenLocs.onScreen; 
        }
    }

    public bool isOnScreen {                                                  // e
        get { return ( screenLocs == eScreenLocs.onScreen ); }
    }

    public bool LocIs( eScreenLocs checkLoc ) {
        if ( checkLoc == eScreenLocs.onScreen ) return isOnScreen;          // a
        return ( (screenLocs & checkLoc) == checkLoc );                     // b
    }
}
