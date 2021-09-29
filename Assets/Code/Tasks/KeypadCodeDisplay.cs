using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeypadCodeDisplay : MonoBehaviour
{
    public List<KeypadTask> tasksToDisplay = new List<KeypadTask>();

    public Image content;
    public Image itemPrefab;

    public List<Image> imagesSpawned = new List<Image>();

    bool NeedToUpdate = false;

    private void Start()
    {
        KeypadTask.taskCompleted += RemoveTask;
        TaskManager.taskHasError += AddTask;
    }

    private void OnDestroy()
    {
        KeypadTask.taskCompleted -= RemoveTask;
        TaskManager.taskHasError -= AddTask;
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

            foreach(KeypadTask task in tasksToDisplay)
            {
                Image spawnedItem = Instantiate(itemPrefab, content.transform);
                imagesSpawned.Add(spawnedItem);
            }
            NeedToUpdate = false;
        }
    }

    void RemoveTask(Task task)
    {
        KeypadTask keypadTask = task as KeypadTask;
        tasksToDisplay.Remove(keypadTask);
        Debug.Log("removing task");
        NeedToUpdate = true;
    }

    void AddTask(Task task)
    {
        if(task is KeypadTask)
        {
            KeypadTask keypadTask = task as KeypadTask;
            tasksToDisplay.Add(keypadTask);
            Debug.Log("adding task");
            NeedToUpdate = true;
        }
    }
}
