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

    [Header("EMERGENCY FIX")]
    public Transform panelAnchor; // Crea un hijo vacío en tu nodo y arrástralo aquí

    private void ShowPanelNow()
    {
        if (detailsPanel == null) return;

        // 1. Posicionamiento infalible
        detailsPanel.transform.SetPositionAndRotation(
            panelAnchor.position,
            Quaternion.LookRotation(panelAnchor.position - Camera.main.transform.position)
        );

        // 2. Activación forzada
        detailsPanel.gameObject.SetActive(true);
        Canvas canvas = detailsPanel.GetComponent<Canvas>();
        if (canvas != null)
        {
            canvas.worldCamera = Camera.main;
            canvas.enabled = false;
            canvas.enabled = true;
        }

        // 3. Debug visual
        GameObject debugSphere = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        debugSphere.transform.position = panelAnchor.position;
        debugSphere.transform.localScale = Vector3.one * 0.2f;
        Destroy(debugSphere, 3f);

        Debug.Log($"PANEL ACTIVADO EN: {panelAnchor.position}");
    }


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
        ShowPanelNow();
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
        ShowPanelNow();
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
            // Posicionamiento relativo al HMD
            Transform cameraTransform = Camera.main.transform;
            detailsPanel.transform.position = cameraTransform.position +
                                           cameraTransform.forward * 0.8f +
                                           cameraTransform.right * 0.2f;

            // Orientación siempre frontal al usuario
            detailsPanel.transform.LookAt(2 * detailsPanel.transform.position - cameraTransform.position);

            // Forzar renderizado
            Canvas canvas = detailsPanel.GetComponent<Canvas>();
            canvas.enabled = false;
            canvas.enabled = true;

            // Debug visual inmediato
            Debug.Log($"Panel visible en: {detailsPanel.transform.position}");
            Debug.DrawLine(cameraTransform.position, detailsPanel.transform.position, Color.green, 5f);
        }
        else
        {
            Debug.LogError("DetailsPanel no asignado", this);
        }



    }

    void Update()
    {
        if (detailsPanel != null && detailsPanel.gameObject.activeSelf)
        {
            // Posiciona el panel 0.5m frente al usuario
            detailsPanel.transform.position = Camera.main.transform.position +
                                           Camera.main.transform.forward * 0.5f;

            // Orienta el panel hacia el usuario
            detailsPanel.transform.LookAt(Camera.main.transform);
            detailsPanel.transform.rotation *= Quaternion.Euler(0, 180f, 0); // Voltea el texto
        }
    }
}
