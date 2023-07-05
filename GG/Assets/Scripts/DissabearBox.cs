using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DissabearBox : MonoBehaviour
{
    [Tooltip("The objects to make disappear and reappear on trigger enter and exit.")]
    public GameObject[] objectsToDisappear;

    [Tooltip("The new roof gameobjects to activate on trigger enter and exit.")]
    public GameObject[] newRoofObjects;

    [Tooltip("The duration of the color transition.")]
    public float colorTransitionDuration = 1f;

    private Renderer[][] _renderers;
    private Color[][][] _originalColors;
    private float _colorTransitionTimer;
    private bool _isTransitioning;
    private bool _triggerExitInProgress;

    private void Start()
    {
        _renderers = new Renderer[objectsToDisappear.Length][];
        _originalColors = new Color[objectsToDisappear.Length][][];
        for (int i = 0; i < objectsToDisappear.Length; i++)
        {
            _renderers[i] = objectsToDisappear[i].GetComponentsInChildren<Renderer>();
            _originalColors[i] = new Color[_renderers[i].Length][];
            for (int j = 0; j < _renderers[i].Length; j++)
            {
                _originalColors[i][j] = new Color[_renderers[i][j].materials.Length];
                for (int k = 0; k < _renderers[i][j].materials.Length; k++)
                {
                    _originalColors[i][j][k] = _renderers[i][j].materials[k].GetColor("_DiffuseColor");
                }
            }
        }
    }

    private void Update()
    {
        if (_isTransitioning)
        {
            // Gradually change the alpha of the new roof's material
            _colorTransitionTimer += Time.deltaTime;
            float t = Mathf.Clamp01(_colorTransitionTimer / colorTransitionDuration);
            float newAlpha;
            if (_triggerExitInProgress)
                newAlpha = Mathf.Lerp(0f, 1f, t); // Fading to 1 (default state) on trigger exit
            else
                newAlpha = Mathf.Lerp(1f, 0f, t); // Fading to 0 on trigger enter

            for (int i = 0; i < newRoofObjects.Length; i++)
            {
                Renderer[] newRoofRenderers = newRoofObjects[i].GetComponentsInChildren<Renderer>();
                for (int j = 0; j < newRoofRenderers.Length; j++)
                {
                    Material[] materials = newRoofRenderers[j].materials;
                    for (int k = 0; k < materials.Length; k++)
                    {
                        Color color = materials[k].GetColor("_DiffuseColor");
                        color.a = newAlpha;
                        materials[k].SetColor("_DiffuseColor", color);
                    }
                }
            }

            if (_colorTransitionTimer >= colorTransitionDuration)
            {
                // Transition completed, reset the timer and stop transitioning
                _colorTransitionTimer = 0f;
                _isTransitioning = false;

                if (_triggerExitInProgress)
                {
                    // Deactivate the new roof and activate the old one
                    for (int i = 0; i < objectsToDisappear.Length; i++)
                    {
                        objectsToDisappear[i].SetActive(true);
                    }
                    for (int i = 0; i < newRoofObjects.Length; i++)
                    {
                        newRoofObjects[i].SetActive(false);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Deactivate the old roof and activate the new one on trigger enter
        for (int i = 0; i < objectsToDisappear.Length; i++)
        {
            objectsToDisappear[i].SetActive(false);
        }
        for (int i = 0; i < newRoofObjects.Length; i++)
        {
            newRoofObjects[i].SetActive(true);
        }

        // Start the alpha transition
        _colorTransitionTimer = 0f;
        _isTransitioning = true;
        _triggerExitInProgress = false;
    }

    private void OnTriggerExit(Collider other)
    {
        // Start the alpha transition on trigger exit
        _colorTransitionTimer = 0f;
        _isTransitioning = true;
        _triggerExitInProgress = true;
    }
}