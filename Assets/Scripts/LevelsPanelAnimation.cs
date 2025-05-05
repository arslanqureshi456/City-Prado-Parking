using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelsPanelAnimation : MonoBehaviour
{
    public Scrollbar levelsScrollBar;

    void OnEnable()
    {
        int unlockLevel = PlayerPrefs.GetInt("unlocklevel", 0); // Get the current unlocked level

        // Update scrollbar based on unlock level
        UpdateScrollbar(unlockLevel);
    }

    void UpdateScrollbar(int unlockLevel)
    {
 

        if (unlockLevel < 0)
        {
          //  levelsScrollBar.value = 0.0f; // Handle unexpected values
        }
        else if (unlockLevel >= 0 && unlockLevel < 4)
        {
            levelsScrollBar.value = 0.0f; // Show first four levels, value remains at 0
        }
        else if (unlockLevel >= 4 && unlockLevel < 8)
        {
            levelsScrollBar.value = 0.166f; // Levels 4 to 7
        }
        else if (unlockLevel >= 8 && unlockLevel < 12)
        {
            levelsScrollBar.value = 0.332f; // Levels 8 to 11
        }
        else if (unlockLevel >= 12 && unlockLevel < 16)
        {
            levelsScrollBar.value = 0.498f; // Levels 12 to 15
        }
        else if (unlockLevel >= 16 && unlockLevel < 20)
        {
            levelsScrollBar.value = 0.664f; // Levels 16 to 19
        }
        else if (unlockLevel >= 20 && unlockLevel < 24)
        {
            levelsScrollBar.value = 0.83f; // Maximum for levels 20 to 25
        }
        else if (unlockLevel >= 24)
        {
            levelsScrollBar.value = 0.996f; // Maximum for levels 20 to 25
        }
    }

    // Method to mark level as complete
    public void MarkLevelComplete(int level)
    {
        if (level >= 0 && level <= 25)
        {
            PlayerPrefs.SetInt("level_" + level, 1); // Mark as completed
            PlayerPrefs.Save(); // Save changes
        }
    }
}
