using UnityEditor;
using UnityEditor.Overlays;
using UnityEngine;
using UnityEngine.UIElements;

namespace CoinDash.GameLogic.Editor
{
    [Overlay(typeof(SceneView), "Lv")]
    public class LevelProgressSliderOverlay: Overlay
    {
        private Slider _slider;
        private FloatField _maxYField;
        private Label currentYText;
        private float _maxY = 10f;
        private float _lastScenePivotY;
        
        private const float wheelScrollSpeed = 0.4f;
        private const string MaxYPrefKey = "CoinDash.LevelProgressSlider.MaxY";
        
        private void LoadMaxYFromPrefs()
        {
            _maxY = EditorPrefs.GetFloat(MaxYPrefKey, 10f);
        }

        private void SaveMaxYToPrefs()
        {
            EditorPrefs.SetFloat(MaxYPrefKey, _maxY);
        }

        public override VisualElement CreatePanelContent()
        {
            LoadMaxYFromPrefs();
            
            var root = new VisualElement
            {
                style =
                {
                    flexDirection = FlexDirection.Column,
                    paddingTop = 4,
                    paddingBottom = 4,
                    paddingLeft = 8,
                    paddingRight = 8
                }
            };

            // root.Add(new Label("Camera Y Control"));

            // Max Y input field
            _maxYField = new FloatField()
            {
                value = _maxY,
                style = { width = 30, marginBottom = 4 }
            };
            _maxYField.RegisterValueChangedCallback(OnMaxYChanged);
            root.Add(_maxYField);
            
            currentYText = new Label()
            {
                text = Mathf.Clamp(SceneView.lastActiveSceneView?.pivot.y ?? 0f, 0f, _maxY).ToString("F1"),
                style =
                {
                    width = 30,
                    marginBottom = 4
                }
            };
            root.Add(currentYText);

            // Vertical slider
            _slider = new Slider("Y", 0f, _maxY)
            {
                value = Mathf.Clamp(SceneView.lastActiveSceneView?.pivot.y ?? 0f, 0f, _maxY),
                direction = SliderDirection.Vertical,
                style =
                {
                    height = 300,
                    width = 20
                }
            };

            _lastScenePivotY = _slider.value;
            _slider.RegisterValueChangedCallback(OnSliderChanged);
            
            // Add mouse wheel support for the slider
            _slider.RegisterCallback<WheelEvent>(OnSliderWheelEvent);
            
            root.Add(_slider);

            return root;
        }
        
        private void OnSliderWheelEvent(WheelEvent evt)
        {
            // Prevent event from propagating to parent elements
            evt.StopPropagation();
    
            // Calculate the value change based on wheel delta
            // Negative delta means scrolling up, positive means scrolling down
            // We invert the delta so scrolling up increases the value
            float deltaValue = -evt.delta.y * wheelScrollSpeed;
    
            // Calculate new value
            float newValue = Mathf.Clamp(_slider.value + deltaValue, _slider.lowValue, _slider.highValue);
    
            // Only update if the value actually changed
            if (Mathf.Abs(newValue - _slider.value) > 0.001f)
            {
                _slider.value = newValue;
        
                // Update scene view pivot
                if (SceneView.lastActiveSceneView != null)
                {
                    Vector3 pivot = SceneView.lastActiveSceneView.pivot;
                    pivot.y = newValue;
                    SceneView.lastActiveSceneView.pivot = pivot;
                    currentYText.text = newValue.ToString("F1");
                    _lastScenePivotY = newValue;
                }
            }
        }
        
        public override void OnCreated()
        {
            base.OnCreated();
            SceneView.beforeSceneGui += OnBeforeSceneGUI;
        }

        public override void OnWillBeDestroyed()
        {
            SceneView.beforeSceneGui -= OnBeforeSceneGUI;
            base.OnWillBeDestroyed();
        }
        
        private void OnBeforeSceneGUI(SceneView sceneView)
        {
            if (_slider == null || sceneView == null) return;

            float currentPivotY = sceneView.pivot.y;
            
            // Only update if there is a meaningful change in the Y position and it wasn't caused by the slider
            if (Mathf.Abs(currentPivotY - _lastScenePivotY) > 0.001f)
            {
                _slider.SetValueWithoutNotify(Mathf.Clamp(currentPivotY, 0f, _maxY));
                currentYText.text = currentPivotY.ToString("F1");
                _lastScenePivotY = currentPivotY;
            }
        }

        private void OnSliderChanged(ChangeEvent<float> evt)
        {
            if (SceneView.lastActiveSceneView == null) return;

            var sceneView = SceneView.lastActiveSceneView;
            // var pos = sceneView.camera.transform.position;
            // pos.y = evt.newValue;
            // sceneView.camera.transform.position = pos;

            // sceneView.pivot = new Vector3(sceneView.pivot.x, evt.newValue, sceneView.pivot.z);
            // sceneView.pivot = pos;
            // sceneView.LookAtDirect(pos, sceneView.rotation);
            // sceneView.Repaint();
            Vector3 pivot = sceneView.pivot;
            pivot.y = evt.newValue;
            sceneView.pivot = pivot;
        }

        private void OnMaxYChanged(ChangeEvent<float> evt)
        {
            _maxY = Mathf.Max(0.01f, evt.newValue); // Prevent zero/negative
            _slider.highValue = _maxY;

            // Clamp current slider value to new max
            _slider.value = Mathf.Clamp(_slider.value, 0f, _maxY);
            
            // Save the new value to EditorPrefs
            SaveMaxYToPrefs();
        }
    }
}