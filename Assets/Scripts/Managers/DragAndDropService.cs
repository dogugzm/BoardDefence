using System;
using System.Collections.Generic;
using Lean.Touch;
using UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace Managers
{
    public interface IDragAndDropService
    {
        void StartDrag(IDraggable unitView);
    }

    public class DragAndDropService : IDragAndDropService, ITickable, IInitializable, IDisposable
    {
        private readonly IDropManager _dropManager;
        private bool _isDragging;
        private GameObject _ghostedObject;
        private IDraggable _draggable;
        private GraphicRaycaster _graphicRaycaster;

        public DragAndDropService(IDropManager dropManager, GraphicRaycaster graphicRaycaster)
        {
            _dropManager = dropManager;
            _graphicRaycaster = graphicRaycaster;
        }

        public void Initialize()
        {
            LeanTouch.OnFingerUp += HandleFingerUp;
            LeanTouch.OnFingerDown += HandleFingerDown;
        }

        void HandleFingerDown(LeanFinger finger)
        {
            if (_isDragging) return;

            var eventData = new PointerEventData(EventSystem.current)
            {
                position = finger.ScreenPosition
            };
            var results = new List<RaycastResult>();
            _graphicRaycaster.Raycast(eventData, results);

            foreach (var result in results)
            {
                if (result.gameObject.TryGetComponent<IDraggable>(out var draggable))
                {
                    StartDrag(draggable);
                    return;
                }
            }
        }

        public void StartDrag(IDraggable draggable)
        {
            if (_isDragging) return;

            _draggable = draggable;
            _isDragging = true;
            draggable.StartDrag(out var ghosted);
            _ghostedObject = ghosted;
        }

        public void Tick()
        {
            if (!_isDragging) return;

            if (LeanTouch.Fingers.Count > 0)
            {
                var finger = LeanTouch.Fingers[0];
                if (_ghostedObject != null)
                {
                    _ghostedObject.transform.position = finger.ScreenPosition;
                }
            }
        }

        private void HandleFingerUp(LeanFinger finger)
        {
            if (!_isDragging) return;
            if (_draggable is null) return;

            if (_draggable.transform.TryGetComponent(out IUnitView unitView))
            {
                if (unitView.IsCurrentlyUsable)
                {
                    var status = _dropManager.TryHandleDrop(finger.ScreenPosition, unitView.Config);
                    if (status)
                    {
                        unitView.UseUnit();
                    }
                }
            }

            _isDragging = false;
            _draggable.EndDrag();
            _draggable = null;
            _ghostedObject = null;
        }

        public void Dispose()
        {
            LeanTouch.OnFingerUp -= HandleFingerUp;
            LeanTouch.OnFingerDown -= HandleFingerDown;
        }
    }
}