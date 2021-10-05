using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThreeWayLeverPositionDisplay : MonoBehaviour
{
    List<ThreeWayLeverTask> tasksToDisplay = new List<ThreeWayLeverTask>();

    public List<Image> content;
    public Image itemPrefab;

    List<Image> imagesSpawned = new List<Image>();

    bool NeedToUpdate = false;

    private void Awake()
    {
        ThreeWayLeverTask.taskCompleted += RemoveTask;
        TaskManager.taskHasError += AddTask;
        GameState.makeThreeWayLeverTask += AddTask;
    }


    private void OnDestroy()
    {
        ThreeWayLeverTask.taskCompleted -= RemoveTask;
        TaskManager.taskHasError -= AddTask;
        GameState.makeThreeWayLeverTask -= AddTask;
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

            foreach (ThreeWayLeverTask task in tasksToDisplay)
            {
                //8 is amount of lines per display
                int display = (int)((float)imagesSpawned.Count / (float)8);
                Image spawnedItem = Instantiate(itemPrefab, content[display].transform);

                ThreeWayLeverPrefabManager manager = spawnedItem.GetComponent<ThreeWayLeverPrefabManager>();
                manager.position.text = task.targetPosition.ToString();
                manager.taskName.text = task.taskName;

                imagesSpawned.Add(spawnedItem);
            }
            NeedToUpdate = false;
        }
    }

    void RemoveTask(Task task)
    {
        ThreeWayLeverTask threeWayLeverTask = task as ThreeWayLeverTask;
        tasksToDisplay.Remove(threeWayLeverTask);
        Debug.Log("removing task");
        NeedToUpdate = true;
    }

    void AddTask(Task task)
    {
        if (task is ThreeWayLeverTask)
        {
            ThreeWayLeverTask threeWayLeverTask = task as ThreeWayLeverTask;
            tasksToDisplay.Add(threeWayLeverTask);
            //  Debug.Log("adding 3WL task");
            NeedToUpdate = true;
        }
    }
}