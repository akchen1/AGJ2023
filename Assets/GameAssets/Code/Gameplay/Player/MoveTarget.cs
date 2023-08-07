using UnityEngine;
using UnityEngine.EventSystems;

public class MoveTarget : MonoBehaviour
{
    public bool AllowMove = true;
    public GameObject hitGameObject = null;
    public Vector3 OffSetVector = new Vector3(0, 2f, 0);

    public Transform target;

    public LayerMask layerMask;

    Camera cam;

    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    private enum ClickResult { None, UI, Interactable }
    private ClickResult clickResult;

    void Start()
    {
        eventBrokerComponent.Publish(this, new PlayerEvents.GetPlayerPosition((position) => 
        {
            transform.position = position;
        }));

        //Cache the Main Camera
        cam = Camera.main;

        useGUILayout = false;
    }

    public void OnGUI()
    {
        if (!AllowMove || cam == null) return;
        if (Event.current.type != EventType.MouseDown || Event.current.clickCount != 1) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity, layerMask);

        clickResult = ClickResult.None;
        if (EventSystem.current.IsPointerOverGameObject() && hit.collider == null)
        {
            clickResult = ClickResult.UI;
        } else if (hit.collider != null)
        {
            clickResult = ClickResult.Interactable;
        }

        UpdateTargetPosition();

        hitGameObject = hit.collider == null ? null : hit.collider.gameObject;
        if (clickResult != ClickResult.Interactable)
        {
            eventBrokerComponent.Publish(this, new InteractionEvents.CancelPendingInteraction());
        }
    }

    public void UpdateTargetPosition()
    {
        Vector3 targetPosition = cam.ScreenToWorldPoint(Input.mousePosition);
        targetPosition.z = 0;
        if (clickResult == ClickResult.Interactable)
        {
            targetPosition -= OffSetVector;
        } else if (clickResult == ClickResult.UI)
        {
            eventBrokerComponent.Publish(this, new PlayerEvents.GetPlayerPosition(position =>
            {
                targetPosition = position;
            }));
        }
        
        target.position = targetPosition;
    }

    private void SetInputStateHandler(BrokerEvent<InputEvents.SetInputState> inEvent)
    {
        AllowMove = inEvent.Payload.Active;
    }

    private void OnEnable() {
        eventBrokerComponent.Subscribe<InputEvents.SetInputState>(SetInputStateHandler);
        eventBrokerComponent.Subscribe<PlayerEvents.SetPlayerPosition>(SetPlayerPositionHandler);
    }

    private void OnDisable() {
        eventBrokerComponent.Unsubscribe<InputEvents.SetInputState>(SetInputStateHandler);
        eventBrokerComponent.Unsubscribe<PlayerEvents.SetPlayerPosition>(SetPlayerPositionHandler);
    }
    private void SetPlayerPositionHandler(BrokerEvent<PlayerEvents.SetPlayerPosition> obj)
    {
        transform.position = obj.Payload.Position;
    }

}
