using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeypadCodeDisplay : MonoBehaviour
{
    Dictionary<int, KeypadTask> tasksToDisplay = new Dictionary<int, KeypadTask>();

    public List<Image> content;
    public Image itemPrefab;

    List<Image> imagesSpawned = new List<Image>();

    bool NeedToUpdate = false;

    private void Awake()
    {
        KeypadTask.taskCompleted += RemoveTask;
        TaskManager.taskHasError += AddTask;
        GameState.makeKeypadTask += AddTask;
        GameState.keypadCompleted += RemoveTask;
    }

    private void OnDestroy()
    {
        KeypadTask.taskCompleted -= RemoveTask;
        TaskManager.taskHasError -= AddTask;
        GameState.makeKeypadTask -= AddTask;
        GameState.keypadCompleted += RemoveTask;
    }

    private void Update()
    {
        if (NeedToUpdate)
        {
            foreach(Image image in imagesSpawned)
            {
                Destroy(image.gameObject);
            }
            imagesSpawned.Clear();

            foreach(KeypadTask task in tasksToDisplay.Values)
            {
                int display =  (int)((float)imagesSpawned.Count / (float)8);
                Image spawnedItem = Instantiate(itemPrefab, content[display].transform);
                
                KeypadPrefabManager manager = spawnedItem.GetComponent<KeypadPrefabManager>();
                manager.code.text = task.code;
                manager.taskName.text = task.taskName;
                
                imagesSpawned.Add(spawnedItem);
            }
            NeedToUpdate = false;
        }
    }

    void RemoveTask(Task task)
    {
        if (task is KeypadTask)
        {
            KeypadTask keypadTask = task as KeypadTask;
            tasksToDisplay.Remove(keypadTask.keyPad.keypadID);
            Debug.Log("removing task");
            NeedToUpdate = true;
        }
    }

    void RemoveTask(int taskID)
    {
        if (tasksToDisplay.ContainsKey(taskID))
        {
            tasksToDisplay.Remove(taskID);
            Debug.Log("removing task");
            NeedToUpdate = true;
        }
    }

    void AddTask(Task task, int leverID)
    {
        if (task is KeypadTask)
        {
            KeypadTask keypadTask = task as KeypadTask;
            tasksToDisplay.Add(leverID, keypadTask);
            NeedToUpdate = true;
        }
    }
}
