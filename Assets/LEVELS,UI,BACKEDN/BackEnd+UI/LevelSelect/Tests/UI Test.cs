using Firebase.Database;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class LevelSelectorTests
{
    private GameObject _levelSelectorGameObject;
    private LevelSelector _levelSelector;

    [SetUp]
    public void SetUp()
    {
        // Create a new GameObject and add the LevelSelector script to it
        _levelSelectorGameObject = new GameObject();
        _levelSelector = _levelSelectorGameObject.AddComponent<LevelSelector>();

        // Set up mock buttons
        _levelSelector.buttons = new Button[3];
        for (int i = 0; i < _levelSelector.buttons.Length; i++)
        {
            _levelSelector.buttons[i] = new GameObject().AddComponent<Button>();
            _levelSelector.buttons[i].interactable = false;
        }
    }

    [TearDown]
    public void TearDown()
    {
        // Clean up
        Object.Destroy(_levelSelectorGameObject);
    }

    [UnityTest]
    public IEnumerator GetUserData_UpdatesUnlockedLevelAndButtons()
    {
        // Mock data
        string userId = "mockUserId";
        int expectedUnlockedLevel = 2;

        // Mock Firebase data snapshot
        var mockSnapshot = new DataSnapshotStub();
        mockSnapshot.SetValue("level", expectedUnlockedLevel);

        // Mock Firebase task
        var mockTask = new TaskStub(mockSnapshot);

        // Replace the actual DBreference call with our mock task
        _levelSelector.DBreference = new DatabaseReferenceStub(mockTask);

        // Start the coroutine and wait for it to complete
        yield return _levelSelector.StartCoroutine(_levelSelector.GetUserData(userId));

        // Check if the unlocked level was correctly updated
        Assert.AreEqual(expectedUnlockedLevel, _levelSelector.unlockedLevel);

        // Check if the correct buttons were enabled
        for (int i = 0; i < _levelSelector.buttons.Length; i++)
        {
            bool shouldBeInteractable = i < expectedUnlockedLevel;
            Assert.AreEqual(shouldBeInteractable, _levelSelector.buttons[i].interactable);
        }
    }
}

// Mock classes

public class DataSnapshotStub : DataSnapshot
{
    private Dictionary<string, object> _values = new Dictionary<string, object>();

    public override object Value => _values;

    public void SetValue(string key, object value)
    {
        _values[key] = value;
    }

    public override bool Exists => true;
}

public class TaskStub : System.Threading.Tasks.Task<DataSnapshot>
{
    private DataSnapshot _result;

    public TaskStub(DataSnapshot result)
    {
        _result = result;
        RunSynchronously();
    }

    public override DataSnapshot Result => _result;

    public override bool IsCompleted => true;
}

public class DatabaseReferenceStub : DatabaseReference
{
    private TaskStub _mockTask;

    public DatabaseReferenceStub(TaskStub mockTask)
    {
        _mockTask = mockTask;
    }

    public override System.Threading.Tasks.Task<DataSnapshot> GetValueAsync()
    {
        return _mockTask;
    }
}
