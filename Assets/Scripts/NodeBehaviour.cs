using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class NodeBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [HideInInspector] public string id;
    [HideInInspector] public int group;
    [HideInInspector] public List<string> entities;

    public TextMeshPro label;       // optional drag; si no, lo buscamos
    private Color originalColor;
    private Renderer rend;

    void Awake()
    {
        // Captura el Renderer
        rend = GetComponent<Renderer>();
        if (rend != null)
            originalColor = rend.material.color;

        // Si no arrastraste el label en el inspector, lo buscamos en los hijos
        if (label == null)
        {
            label = GetComponentInChildren<TextMeshPro>();
            if (label == null)
            {
                Debug.LogWarning($"[NodeBehaviour] No se encontró TextMeshPro en '{name}'. Asigna el campo 'label'.");
            }
        }
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
        ShowDetails();
    }

    private void ShowDetails()
    {
        Debug.Log($"Node {id} (Group {group}) Entities:\n- {string.Join("\n- ", entities)}");
    }
}
