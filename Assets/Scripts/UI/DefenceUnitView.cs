using Configs;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace UI
{
    public interface IUnitView
    {
        DefenceUnitConfig Config { get; }
        void UseUnit();
        bool IsCurrentlyUsable { get; }
    }

    public class DefenceUnitView : MonoBehaviour, IDraggable, IUnitView
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private GameObject _draggedUnitView;
        [SerializeField] private TMP_Text _unitCountText;

        public Data CurrentData { get; private set; }
        public DefenceUnitConfig Config { get; private set; }

        public class Data
        {
            public DefenceUnitEntry Entry { get; private set; }

            public Data(DefenceUnitEntry entry)
            {
                Entry = entry;
            }
        }

        public UniTask InitAsync(Data data)
        {
            CurrentData = data;
            Config = data.Entry.Config;
            UpdateText();
            return UniTask.CompletedTask;
        }

        public void UseUnit()
        {
            CurrentData.Entry.Count--;
            _unitCountText.text = CurrentData.Entry.Count.ToString();
            if (CurrentData.Entry.Count <= 0)
            {
                Destroy(gameObject);
            }
        }

        public bool IsCurrentlyUsable => CurrentData.Entry.Count > 0;

        private void UpdateText()
        {
            _unitCountText.text = CurrentData.Entry.Count.ToString();
        }

        public void StartDrag(out GameObject draggedUnitView)
        {
            _draggedUnitView.gameObject.SetActive(true);
            draggedUnitView = _draggedUnitView;
            _canvasGroup.alpha = 0.5f;
            _canvasGroup.blocksRaycasts = false;
        }

        public void EndDrag()
        {
            _draggedUnitView.gameObject.SetActive(false);
            _draggedUnitView.transform.localPosition = Vector3.zero;
        }
    }

    public interface IDraggable
    {
        public Transform transform { get; }
        public void StartDrag(out GameObject draggedUnitView);
        public void EndDrag();
    }
}