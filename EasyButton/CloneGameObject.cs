using UnityEngine;
using System.Collections.Generic;
using UnityEditor;

public class CloneGameObject : ScriptableWizard
{
    public GameObject firstObjectToClone;
    public GameObject secondObjectToClone;
    public GameObject parentObjectForClones;
    public int xCount = 1;
    public int yCount = 1;
    public int zCount = 1;
    public float XAxisSpacing = 1;
    public float YAxisSpacing = 1;
    public float ZAxisSpacing = 1;
    public bool randomizeYRotation;

    private List<GameObject> allClones;

    System.Random random = new System.Random();

    [MenuItem("Tools/Clone Game Object")]
    static void CloneGameObjectWizard()
    {
        ScriptableWizard.DisplayWizard<CloneGameObject>("Clone Game Object", "Clone Now");
    }

    void OnWizardCreate()
    {   
        cloneAll();
    }


    // Helper Functions
    void cloneAll()
    {
        for(int layerCounter = 0; layerCounter < zCount; layerCounter++)
        {
            cloneRowsOfColumns(layerCounter);
        }
    }

    void cloneRowsOfColumns(int layerCounter)
    {
        for (int rowCounter = 0; rowCounter < yCount; rowCounter++)
        {
            cloneColumns(rowCounter, layerCounter);
        }
    }

    void cloneColumns(int rowCounter, int layerCounter)
    {
        for (int columnCounter = 0; columnCounter < xCount; columnCounter++)
        {
            GameObject objectToClone = setObjectToClone();
            Vector3 clonePosition = setTransform(columnCounter, rowCounter, layerCounter);
            Transform cloneParent = setParent(columnCounter, rowCounter, layerCounter);
            Quaternion cloneRotation = setRotation(objectToClone);

            GameObject newObject = Instantiate(objectToClone,
                                               clonePosition,
                                               cloneRotation,
                                               cloneParent);
            newObject.name = objectToClone.name + "-" + columnCounter + "-" + rowCounter + "-" + layerCounter;

            // Move to parent container if object uses randomYRotation parent
            if (randomizeYRotation)
                cloneParent.transform.parent = parentObjectForClones.transform;
        }
    }

    GameObject setObjectToClone()
    {
        int randomize = random.Next(2);
        if (secondObjectToClone == null || randomize == 1)
            return firstObjectToClone;
        return secondObjectToClone;
    }

    Transform setParent(int columnCounter, int rowCounter, int layerCounter)
    {
        if (randomizeYRotation)
        {
            // Create a new parent to enable rotation on the center of cloned object
            GameObject rotationObject = new GameObject("clone-" + columnCounter + "-" + rowCounter + "-" + layerCounter);
            return rotationObject.transform;
        }
        return parentObjectForClones.transform;
    }

    Quaternion setRotation(GameObject objectToClone)
    {
        // Rotation value should match original object's rotation
        Quaternion rotation = new Quaternion();
        rotation.Set(objectToClone.transform.rotation.x,
            objectToClone.transform.rotation.y,
            objectToClone.transform.rotation.z,
            objectToClone.transform.rotation.w);

        if (randomizeYRotation)
            rotation.y = Random.rotation.y;
        return rotation;
    }

    Vector3 setTransform(int columnCounter, int rowCounter, int layerCounter)
    {
        Vector3 clonePosition;
        clonePosition.x = parentObjectForClones.transform.position.x + (XAxisSpacing * columnCounter);
        clonePosition.y = parentObjectForClones.transform.position.y + (YAxisSpacing * rowCounter);
        clonePosition.z = parentObjectForClones.transform.position.z + (ZAxisSpacing * layerCounter);
        return clonePosition;
    }
}