using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.XR.Interaction.Toolkit;
public class NodeBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [HideInInspector] public string id;
    [HideInInspector] public int group;
    [HideInInspector] public List<string> entities;
    [SerializeField] public DetailsPanel detailsPanel; // Ahora será visible en el Inspector

    private Color originalColor;
    private Renderer rend;

    private XRGrabInteractable grabInteractable;


    void Awake()
    {
        // 1. Renderer con protección null
        rend = GetComponent<Renderer>();
        if (rend != null)
        {
            originalColor = rend.material.color;
        }
        else
        {
            Debug.LogError($"Falta componente Renderer en {name}. Añade MeshRenderer o SpriteRenderer.", this);
            // Crea un renderer básico como fallback (opcional)
            gameObject.AddComponent<MeshRenderer>();
            rend = GetComponent<Renderer>();
            rend.material = new Material(Shader.Find("Standard"));
            originalColor = Color.white;
        }
        // Configuración para VR
        // Obtener el XRGrabInteractable existente en lugar de añadir uno nuevo
        grabInteractable = GetComponent<XRGrabInteractable>();

        if (grabInteractable == null)
        {
            // Solo añadir si no existe
            grabInteractable = gameObject.AddComponent<XRGrabInteractable>();
        }
        grabInteractable.selectEntered.AddListener(OnSelectEntered);
        grabInteractable.hoverEntered.AddListener(OnHoverEnter);
        grabInteractable.hoverExited.AddListener(OnHoverExit);
    }
    private void OnSelectEntered(SelectEnterEventArgs args)
    {
        Debug.Log("Selección VR en nodo: " + id);
        ShowDetails();
    }

    private void OnHoverEnter(HoverEnterEventArgs args)
    {
        transform.localScale *= 1.2f;
    }

    private void OnHoverExit(HoverExitEventArgs args)
    {
        transform.localScale /= 1.2f;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale *= 1.2f;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale /= 1.2f;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("CLIC RECIBIDO EN " + gameObject.name); // Añade esto
        ShowDetails();
    }
    void Start()
    {
        // Buscar el panel si no se asigna manualmente
        if (detailsPanel == null)
        {
            detailsPanel = FindObjectOfType<DetailsPanel>();
        }
    }

    private void ShowDetails()
    {
        
        if (detailsPanel != null)
        {
            // Posiciona el panel 0.5m a la derecha del nodo
            Vector3 panelPosition = transform.position + transform.right * 0.5f;
            detailsPanel.transform.position = panelPosition;

            // Orientación hacia la cámara
            detailsPanel.transform.LookAt(Camera.main.transform);
            detailsPanel.transform.rotation = Quaternion.LookRotation(
                detailsPanel.transform.position - Camera.main.transform.position);

            // Mostrar panel
            detailsPanel.ShowDetails(id, group, entities, transform.position);
        }
        else
        {
            Debug.LogError("DetailsPanel no asignado en el Inspector", this);
        }
    }

}
