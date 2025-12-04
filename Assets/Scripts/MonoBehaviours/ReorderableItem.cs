using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Prototype
{
    public class ReorderableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        [SerializeField] private GameObject _character;
        public GameObject Character => _character;
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private Canvas _canvas;
        [SerializeField] private RectTransform _rectTransform;

        private GameObject _placeholder;
        private RectTransform _placeholderRect;
        private Transform _originalParent;
        private int _originalIndex;
        
        public void OnBeginDrag(PointerEventData eventData)
        {
            _originalParent = transform.parent;
            _originalIndex = transform.GetSiblingIndex();
            
            _placeholder = new GameObject("Placeholder");
            _placeholderRect = _placeholder.AddComponent<RectTransform>();
            _placeholder.transform.SetParent(_originalParent);
            
            _placeholderRect.localScale = Vector3.one;
            _placeholderRect.sizeDelta = _rectTransform.sizeDelta;
            
            LayoutElement layout = _placeholder.AddComponent<LayoutElement>();
            layout.preferredWidth = _rectTransform.rect.width;
            layout.preferredHeight = _rectTransform.rect.height;
            layout.flexibleWidth = 0f;
            layout.flexibleHeight = 0f;
            
            _placeholder.transform.SetSiblingIndex(_originalIndex);
            
            _canvasGroup.alpha = 0.6f;
            _canvasGroup.blocksRaycasts = false;
            
            transform.SetParent(_canvas.transform, worldPositionStays: true);
        }

        public void OnDrag(PointerEventData eventData)
        {
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)_canvas.transform,
                eventData.position,
                eventData.pressEventCamera,
                out Vector2 localPoint
            );

            transform.localPosition = localPoint;
            
            UpdatePlaceholderPosition();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = true;
            
            transform.SetParent(_originalParent, false);
            transform.SetSiblingIndex(_placeholder.transform.GetSiblingIndex());

            Destroy(_placeholder);
        }
        
        private void UpdatePlaceholderPosition()
        {
            if (_originalParent == null || _placeholder == null)
                return;

            int newIndex = _originalParent.childCount;
            
            for (int i = 0; i < _originalParent.childCount; i++)
            {
                Transform child = _originalParent.GetChild(i);
                
                if (child == _placeholder.transform)
                    continue;
                
                if (transform.position.x < child.position.x)
                {
                    newIndex = child.GetSiblingIndex();

                    if (_placeholder.transform.GetSiblingIndex() < newIndex) newIndex--;

                    break;
                }
            }

            _placeholder.transform.SetSiblingIndex(newIndex);
        }
    }
}
