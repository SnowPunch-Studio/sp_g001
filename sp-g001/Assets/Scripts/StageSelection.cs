using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelection : MonoBehaviour
{
    public static void ChangeScene(StageSelection instance, string sceneName) => SceneManager.LoadScene(sceneName);
}
