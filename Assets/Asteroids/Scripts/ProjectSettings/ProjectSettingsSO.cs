using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectSettings", menuName = "ScriptableObjects/ProjectSettings", order = 0)]
public class ProjectSettingsSO : ScriptableObject
{
    public List<ProjectObject> projectObjects;
}