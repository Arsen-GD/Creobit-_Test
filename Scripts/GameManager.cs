using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    //Touch
    private Camera mainCamera;
    private GameObject currentTouch;
    private GameObject previusTouch;
    //Items
    [SerializeField] private GameObject cubePrefab;
    [SerializeField] private GameObject circlePrefab;
    [SerializeField] private GameObject trianglePrefab;
    [SerializeField] private GameObject[] itemsToPickFrom;
    [SerializeField] private Transform itemContainer;
    private List<Vector3> positionsItem = new List<Vector3>();
    private List<GameObject> activeItem = new List<GameObject>();
    //Grid    
    [SerializeField] private int gridX = 6;
    [SerializeField] private int gridY = 12;
    [SerializeField] private float gridSpacingOffset = 2f;
    [SerializeField] private Vector3 gridStart; 
    private Vector3 positionRandomization;
    //other
    [SerializeField] private float moveSpeed;
    [SerializeField] private Scene scene;
    private Player playerScript;
    

    void Start()
    {
        mainCamera = GameObject.Find("MainCamera").GetComponent<Camera>();
        playerScript = GetComponent<Player>();
        gridStart = itemContainer.position;
        scene= SceneManager.GetActiveScene();
        playerScript.AddStartEnergy(8);
        SpawnGrid();

        if (scene.buildIndex != 1)
        {
            StartCoroutine(CheckField());            
            playerScript.playerEnergy.gameObject.SetActive(true);
        }
        
       
    }

   
    void Update()
    {
        SelectObject();
        Debug.Log(scene.buildIndex);
    }

    public void SpawnCube()
    {
        if (activeItem.Count < positionsItem.Count)
        {
            GameObject clone = Instantiate(cubePrefab, FindPos(), cubePrefab.transform.rotation);
            clone.GetComponent<SizeObj>().RandomaizeSize(1, 9);
            activeItem.Add(clone);
        }
        else
        {
            StartCoroutine(playerScript.HinttCoroutine("No space"));
        }

    }
    public void SpawnTriangle()
    {
        if (activeItem.Count < positionsItem.Count)
        {
            GameObject clone = Instantiate(trianglePrefab, FindPos(), trianglePrefab.transform.rotation);
            
            playerScript.playerEnergy.gameObject.SetActive(true);
            StartCoroutine(playerScript.HinttCoroutine("Choose cube"));
            activeItem.Add(clone);
        }
        else
        {
            StartCoroutine(playerScript.HinttCoroutine("No space"));
        }

    }
    public void SpawnCircle()
    {
        if (activeItem.Count < positionsItem.Count)
        {
            GameObject clone = Instantiate(circlePrefab, FindPos(), circlePrefab.transform.rotation);
            clone.GetComponent<SizeObj>().RandomaizeSize(1,9);
            activeItem.Add(clone);
        }
        else
        {
            StartCoroutine(playerScript.HinttCoroutine("No space"));
        }
    }
    public void DeleteItem()
    {
        if (currentTouch)
        {
            Destroy(currentTouch.gameObject);
            activeItem.Remove(currentTouch);

        }
        else
        {
           StartCoroutine(playerScript.HinttCoroutine("Object is not selected"));
        }

    }
    public void RestartGrid()
    {
        foreach (var item in activeItem)
        {
            Destroy(item);
        }

        activeItem.Clear();
        positionsItem.Clear();
        SpawnGrid();

    }
    public void ItemChangeSize(float size)
    {
        if (currentTouch.tag == ("Cube") || currentTouch.tag == ("Circle"))
        {
            currentTouch.GetComponent<SizeObj>().ChangeSize(size);
        }
        
    }
    public void EscapeMainMenu()
    {
        SceneManager.LoadScene(0);
    }


    private void SelectObject()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out hit))
            {
                currentTouch = hit.collider.gameObject;

               
                StartCoroutine(currentTouch.gameObject.GetComponent<Selectable>().SelectCoroutine());

                if (currentTouch.CompareTag("Circle"))
                {
                    if (previusTouch != null && previusTouch.gameObject.tag == ("Cube"))
                    {
                        
                        StartCoroutine(MoveCube(previusTouch, currentTouch));

                    }
                    if (previusTouch != null && previusTouch.gameObject.tag == ("Triangle"))
                    {
                        StartCoroutine(playerScript.HinttCoroutine("Choose cube"));
                        StartCoroutine(currentTouch.gameObject.GetComponent<Selectable>().WrongSelectCoroutine());
                    }
                    
                    previusTouch = currentTouch;
                }

                if (currentTouch.gameObject.CompareTag("Cube"))
                {
                    if (previusTouch != null && previusTouch.gameObject.tag == ("Triangle") && playerScript.Energy != 0)
                    {
                        currentTouch.GetComponent<SizeObj>().DownSize();
                        playerScript.RemoveEnergy();
                        Destroy(previusTouch.gameObject, 1f);
                        activeItem.Remove(previusTouch);


                    }

                    previusTouch = currentTouch;
                   
                }

                if (currentTouch.gameObject.CompareTag("Triangle")) previusTouch = currentTouch;
                

                


            }
        }
    }
    private void SpawnGrid()
    {
        for (int x = 0; x < gridX; x++)
        {
            for (int y = 0; y < gridY; y++)
            {
                Vector3 spawnPosition = new Vector3(x * gridSpacingOffset, y * gridSpacingOffset, 2) + gridStart;
                positionsItem.Add(spawnPosition);
                
                PickAndSpawn(RandomizedPosition(spawnPosition));
            }
        }
    }
    private void PickAndSpawn(Vector3 positionToSpawn)
    {
        GameObject item;
        int randomIndex = Random.Range(0, itemsToPickFrom.Length);

        switch (scene.buildIndex)
        {
            case 1:
                item = Instantiate(itemsToPickFrom[randomIndex], positionToSpawn, transform.rotation, itemContainer.transform);
                if (item.tag != ("Triangle") && item != null) item.GetComponent<SizeObj>().RandomaizeSize(1, 10);
                break;
            case 2:
                item = Instantiate(RandomWithWeigth(5, 4, 1), positionToSpawn, transform.rotation, itemContainer.transform);
                if (item.tag != ("Triangle") && item != null) item.GetComponent<SizeObj>().RandomaizeSize(1, 10);
                break;
            case 3:
                item = Instantiate(RandomWithWeigth(4, 3, 1), positionToSpawn, transform.rotation, itemContainer.transform);
                if (item.tag == ("Circle")) item.GetComponent<SizeObj>().RandomaizeSize(1, 3);
                else if (item.tag == ("Cube")) item.GetComponent<SizeObj>().RandomaizeSize(1, 10);

                break;
            default:                
                item = Instantiate(itemsToPickFrom[randomIndex], positionToSpawn, transform.rotation, itemContainer.transform);
                break;
        }                       


        activeItem.Add(item);
    }
    private IEnumerator MoveCube(GameObject cube, GameObject currentObj)
    {
        if (cube.gameObject.GetComponent<SizeObj>().Size <= currentObj.gameObject.GetComponent<SizeObj>().Size)
        {

            playerScript.AddScore();
            while (cube.transform.position != currentObj.transform.position)
            {
                cube.transform.position = Vector3.MoveTowards(cube.transform.position,
                    currentObj.transform.position, Time.deltaTime * moveSpeed);
                activeItem.Remove(cube);
                yield return null;
            }
            
            Destroy(cube, 1.5f);
        }
        else
        {

            StartCoroutine(cube.gameObject.GetComponent<Selectable>().WrongSelectCoroutine());
        }
    }
    private Vector3 FindPos()
    {
        foreach (var item in positionsItem)
        {
            Collider[] hitColliders = Physics.OverlapBox(item, transform.localScale / 2, Quaternion.identity);
            if (hitColliders.Length > 0)
            {
                continue;

            }
            else if (hitColliders.Length == 0)
            {
                return item;
            }

          
           
        }
        return Vector3.zero; 
    }
    private Vector3 RandomizedPosition(Vector3 position)
    {
        Vector3 randomizedPosition = new Vector3(Random.Range(-positionRandomization.x, positionRandomization.x), Random.Range(-positionRandomization.y, positionRandomization.y), Random.Range(-positionRandomization.z, positionRandomization.z)) + position;

        return randomizedPosition;

    }

    private GameObject RandomWithWeigth(int cube, int circle, int triangle)
    {
        int weightrandom = Random.Range(1, 10);

        if (weightrandom <= triangle) return trianglePrefab;
        else if (weightrandom > triangle && weightrandom <= circle) return circlePrefab;
        else if (weightrandom >= cube) return cubePrefab;

        return null;

        
    }
    private IEnumerator CheckField()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            if (activeItem.Count != positionsItem.Count)
            {
                yield return new WaitForSeconds(1f);
                if (scene.buildIndex != 3)
                {
                    PickAndSpawn(FindPos());
                }
                else
                {
                    SpawnCube();
                }
               
            }
            
            
        }
        
    }


}
