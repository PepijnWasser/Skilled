using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ThreeWayLeverPositionDisplay : MonoBehaviour
{
    Dictionary<int, ThreeWayLeverTask> tasksToDisplay = new Dictionary<int, ThreeWayLeverTask>();

    public List<Image> content;
    public Image itemPrefab;

    List<Image> imagesSpawned = new List<Image>();

    bool NeedToUpdate = false;

    private void Awake()
    {
        //ThreeWayLeverTask.taskCompleted += RemoveTask;
       // TaskManager.taskHasError += AddTask;
        GameState.makeThreeWayLeverTask += AddTask;
        GameState.threeWayLeverCompleted += RemoveTask;
    }


    private void OnDestroy()
    {
     ///   ThreeWayLeverTask.taskCompleted -= RemoveTask;
      // // TaskManager.taskHasError -= AddTask;
        GameState.makeThreeWayLeverTask -= AddTask;
        GameState.threeWayLeverCompleted -= RemoveTask;
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

            foreach (ThreeWayLeverTask task in tasksToDisplay.Values)
            {
                //8 is amount of lines per display
                int display = (int)((float)imagesSpawned.Count / (float)8);
                Image spawnedItem = Instantiate(itemPrefab, content[display].transform);

                ThreeWayLeverDisplayItemPrefabManager manager = spawnedItem.GetComponent<ThreeWayLeverDisplayItemPrefabManager>();
                manager.position.text = task.targetPosition.ToString();
                manager.taskName.text = task.taskName;

                imagesSpawned.Add(spawnedItem);
            }
            NeedToUpdate = false;
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
        if (task is ThreeWayLeverTask)
        {
            ThreeWayLeverTask threeWayLeverTask = task as ThreeWayLeverTask;
            tasksToDisplay.Add(leverID, threeWayLeverTask);
            NeedToUpdate = true;
        }
    }
}