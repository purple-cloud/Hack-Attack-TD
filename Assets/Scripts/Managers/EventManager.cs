﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Contains all of the custom events needed among scripts.
/// </summary>
public class EventManager : Singleton<EventManager> {
	#region DELEGATES

	public delegate void Cancel();
	public delegate void CanvasClick();
	public delegate void RefreshPanel();
    public delegate void ComponentPlaced();
	public delegate void RightClickComponent();

	#endregion

	/// <summary>
	/// When cancel has been pressed.
	/// </summary>
	public static event Cancel onCancel;

	/// <summary>
	/// When empty space on canvas is pressed with either one of the mouse buttons.
	/// </summary>
	public static event CanvasClick onCanvasClick;

	/// <summary>
	/// Event triggered by other classes to let them know that changes has happened and they need to update their
	/// object contents.
	/// </summary>
	public static event RefreshPanel onRefreshPanel;

    /// <summary>
    /// When a component/structure has been placed successfully.
    /// </summary>
    public static event ComponentPlaced onComponentPlaced;

	/// <summary>
	/// When a component has been right clicked, regardless of its state. 
	/// </summary>
	public static event RightClickComponent onRightClickComponent;

	private bool refreshPanelEventIsTriggered = false;
	private bool componentPlacedEventIsTriggered = false;
	private bool rightClickComponentEventIsTriggered = false;

	void OnGUI() {
		if (Input.GetButtonDown("Cancel")) {
			// Notify all subscribed classes when user wishes to cancel either by pressing escape, or
			// clicking on a non-occupied area on canvas
			onCancel?.Invoke();
		}

		if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1)) {
			if (EventSystem.current.IsPointerOverGameObject() == false) {
				// Notify if canvas is pressed (where there are no objects) using mouse buttons
				onCanvasClick?.Invoke();
			}
		}
	}

	void Update() {
        // Trigger 'onRefreshPanel' event
        if (Instance.refreshPanelEventIsTriggered) {
            if (GameManager.Instance.modulePanel.activeSelf) {
                onRefreshPanel?.Invoke();
                Instance.refreshPanelEventIsTriggered = false;
            }
		}

        // Trigger 'onComponentPlaced' event
        if (Instance.componentPlacedEventIsTriggered) {
            onComponentPlaced?.Invoke();
            Instance.componentPlacedEventIsTriggered = false;
        }

		// Invokes 'onRightClickComponent' event
		if (Instance.rightClickComponentEventIsTriggered) {
			onRightClickComponent?.Invoke();
			Instance.rightClickComponentEventIsTriggered = false;
		}
	}

	/// <summary>
	/// Every panel subscribed to onRefreshPanel will be asked to update their contents. This method is executed 
	/// by other classes.
	/// </summary>
	public static void TriggerRefreshPanelEvent() {
		Instance.refreshPanelEventIsTriggered = true;
	}

    /// <summary>
    /// Once a component has been placed, invoke the event internally.
    /// </summary>
    public static void TriggerComponentPlacedEvent() {
        Instance.componentPlacedEventIsTriggered = true;
    }

    /// <summary>
    /// Once a component has been right clicked, invoke the event internally.
    /// <seealso cref="Component.OnPointerUp(PointerEventData)"/>
    /// </summary>
    public static void TriggerRightClickOnComponentEvent() {
		Instance.rightClickComponentEventIsTriggered = true;
	}

    /// <summary>
    /// Removes all subscribed methods to 'onCanvasClick' event.
    /// </summary>
    public static void ClearOnCanvasClickSubscribers() {
        onCanvasClick = null;
    }
}
