using UnityEngine;
using System.Collections;
using System.Xml.Linq;
using UnityEngine.EventSystems;

public class UIJoystick : AObject, IBeginDragHandler, IEndDragHandler, IDragHandler {

    public enum Mode {

        XY,
        XZ,

    }

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
    public bool simulateInput = true;
#endif

    public static UIJoystick instance { get; private set; }

    [SerializeField] private Mode mode;

    [SerializeField] private float radius = 5f;

    [SerializeField] private float resetSpeed = 0.1f;

    [SerializeField] private Transform root;

    private bool isDragging;

    void Start() {

        instance = this;
    }

    private void LateUpdate() {

        if ( !isDragging ) {

            localPosition = Vector3.Lerp( localPosition, Vector3.zero, resetSpeed );
        }
        else {

            var currentVector = Vector3.ClampMagnitude( localPosition, radius );
            localPosition = currentVector;
        }
    }

    public Vector3 GetValue() {

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        if ( simulateInput ) {

            return FormVector( Input.GetAxis( "Horizontal" ), Input.GetAxis( "Vertical" ) );
        }
#endif
        var result = Vector3.ClampMagnitude( ( position - root.position ) / radius, 1f );

        return FormVector( result.x, result.y );
    }

    public void OnBeginDrag( PointerEventData eventData ) {

        isDragging = true;
    }

    public void OnEndDrag( PointerEventData eventData ) {

        isDragging = false;
    }

    public void OnDrag( PointerEventData eventData ) {

        position = eventData.position;
    }

    private Vector3 FormVector( float horizontal, float vertical ) {

        switch ( mode ) {

            case Mode.XZ:
                return new Vector3( horizontal, 0, vertical );

            case Mode.XY:
            default:
                return new Vector3( horizontal, vertical );
        }
    }

}