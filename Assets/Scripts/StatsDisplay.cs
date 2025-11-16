using UnityEngine;
using TMPro;

public class StatsDisplay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _display;
    [SerializeField] private Spawner _spawner;
    [SerializeField] private string _title;

    private void Start()
    {
        UpdateDisplay(0, 0, 0);
    }

    private void OnEnable()
    {
        _spawner.UpdatedStats += UpdateDisplay;
    }

    private void OnDisable()
    {
        _spawner.UpdatedStats -= UpdateDisplay;
    }

    private void UpdateDisplay(int objectsSpawnedCount, int objectsActiveCount, int objectsCreatedCount)
    {
        _display.text = $"{_title}\nSpawned: {objectsSpawnedCount}\nActive: {objectsActiveCount}\nCreated: {objectsCreatedCount}";
    }
}
