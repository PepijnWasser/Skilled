using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TwoWayLeverPositionDisplay : MonoBehaviour
{
    List<TwoWayLeverTask> tasksToDisplay = new List<TwoWayLeverTask>();

    public List<Image> content;
    public Image itemPrefab;

    List<Image> imagesSpawned = new List<Image>();

    bool NeedToUpdate = false;

    private void Awake()
    {
        TwoWayLeverTask.taskCompleted += RemoveTask;
        TaskManager.taskHasError += AddTask;
        GameState.makeTwoWayleverTask += AddTask;
    }


    private void OnDestroy()
    {
        TwoWayLeverTask.taskCompleted -= RemoveTask;
        TaskManager.taskHasError -= AddTask;
        GameState.makeTwoWayleverTask -= AddTask;
    }

    private void Update()
    {
        if (NeedToUpdate)
        {
            foreach (Image image in imagesSpawned)
            {
                Destroy(image.gameObject);
            }
            imagesSpawned.Clear();

            foreach (TwoWayLeverTask task in tasksToDisplay)
            {
                //8 is amount of lines per display
                int display = (int)((float)imagesSpawned.Count / (float)8);
                Image spawnedItem = Instantiate(itemPrefab, content[display].transform);

                TwoWayLeverPrefabManager manager = spawnedItem.GetComponent<TwoWayLeverPrefabManager>();
                manager.position.text = task.targetPosition.ToString();
                manager.taskName.text = task.taskName;

                imagesSpawned.Add(spawnedItem);
            }
            NeedToUpdate = false;
        }
    }

    void RemoveTask(Task task)
    {
        TwoWayLeverTask twoWayLeverTask = task as TwoWayLeverTask;
        tasksToDisplay.Remove(twoWayLeverTask);
        Debug.Log("removing task");
        NeedToUpdate = true;
    }

    void AddTask(Task task)
    {
        if (task is TwoWayLeverTask)
        {
            TwoWayLeverTask twoWayLeverTask = task as TwoWayLeverTask;
            tasksToDisplay.Add(twoWayLeverTask);
            //  Debug.Log("adding TWL task");
            NeedToUpdate = true;
        }
    }
}
