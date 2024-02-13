using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class TrackedImageState : MonoBehaviour
{
    #region Attributes
    [SerializeField]
    [Tooltip("Image manager on the AR Session Origin")]
    private ARTrackedImageManager m_ImageManager;

    [SerializeField]
    [Tooltip("Reference Image Library")]
    private XRReferenceImageLibrary m_ImageLibrary;

    [SerializeField]
    [Tooltip("List of Gameobjects, needs to have same count as XRReference Image Libarary and watch out for the order")]
    private List<GameObject> markerBasedObjects;

    [SerializeField]
    [Tooltip("List of Gameobjects, which do not spawn with marker")]
    private List<GameObject> notMarkerBasedObjects;

    [SerializeField]
    [Tooltip("Path prefab to create pipes")]
    private GameObject path;

    [SerializeField]
    [Tooltip("Mask to see Pipes")]
    private GameObject wall;

    private List<GameObject> spawnedObjects = new List<GameObject>();
    private List<MarkerToPrefab> markerToPrefabList = new List<MarkerToPrefab>();
    private List<PipeConnection> pipeConnection = new List<PipeConnection>();

    //Simple structure to annotate where the pipe starts and where it ends.
    //"fromObject(output);toObject(input)"
    private List<string> connectionDesciption = new List<string>() {
        "Kuehlschrank;Energieflussbox",
        "Waschmaschine;Energieflussbox",
    };

    private GameObject wallMask;

    #endregion

    #region Properties
    public ARTrackedImageManager ImageManager
    {
        get => m_ImageManager;
        set => m_ImageManager = value;
    }

    public XRReferenceImageLibrary ImageLibrary
    {
        get => m_ImageLibrary;
        set => m_ImageLibrary = value;
    }

    public List<GameObject> SpawnedObjects
    {
        get => spawnedObjects;
        set => spawnedObjects = value;
    }

    public List<GameObject> MarkerBasedObjects
    {
        get => markerBasedObjects;
        set => markerBasedObjects = value;
    }
    #endregion

    private void OnEnable()
    {
        for (int i = 0; i < this.ImageLibrary.count; i++)
        {
            markerToPrefabList.Add(new MarkerToPrefab(this.ImageLibrary[i].name, this.MarkerBasedObjects[i]));
        }

        var allModelObjects = notMarkerBasedObjects.Concat(MarkerBasedObjects).ToList();
        this.connectionDesciption.ForEach(description =>
        {
            var keyValue = description.Split(';');
            var from = allModelObjects.FirstOrDefault(x => x.name.Equals(keyValue[0]));
            var to = allModelObjects.FirstOrDefault(x => x.name.Equals(keyValue[1]));
            pipeConnection.Add(new PipeConnection(from, to, path));
        });
        m_ImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDestroy()
    {
        m_ImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }
    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {

        foreach (ARTrackedImage trackedImage in eventArgs.added)
        {
            var foundIndex = this.markerToPrefabList.FindIndex(markerToPrefab => markerToPrefab.MarkerName == trackedImage.referenceImage.name);
            if (foundIndex > -1)
            {
                markerToPrefabList[foundIndex].Instance = Instantiate(markerToPrefabList[foundIndex].Prefab, trackedImage.transform.position, markerToPrefabList[foundIndex].Prefab.transform.rotation);
                this.spawnedObjects.Add(markerToPrefabList[foundIndex].Instance);
            }
        }

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
        {
            // image is tracking or tracking with limited state, show visuals and update it's position and rotation
            if (trackedImage.trackingState == TrackingState.Tracking)
            {
                var foundIndex = this.markerToPrefabList.FindIndex(markerToPrefab => markerToPrefab.MarkerName == trackedImage.referenceImage.name);
                if (foundIndex > -1)
                {
                    markerToPrefabList[foundIndex].Instance.transform.SetPositionAndRotation(trackedImage.transform.position, markerToPrefabList[foundIndex].Prefab.transform.rotation);
                    markerToPrefabList[foundIndex].Updated = true;
                }
            }
        }

        if (markerToPrefabList.All(markerToPrefab => markerToPrefab.Instance != null && markerToPrefab.Updated)
            && this.pipeConnection.All(pipe => pipe.pipe == null)
            && this.wallMask == null)
        {
            initWall();
            //instantiateNotMarkerBasedObjects();
            initPipeConnection();
            pipeConnection.ForEach(pipe => {
                var outputname = "to" + pipe.to.name.Substring(0, pipe.to.name.Length - 7);
                var inputname = "from" + pipe.from.name.Substring(0, pipe.from.name.Length - 7);
                var output = pipe.from.transform.Find("Output").Find(outputname);
                var input = pipe.to.transform.Find("Input").Find(inputname);
                pipe.pipe = Instantiate(pipe.pipePrefab);
                pipe.pipe.tag = "Pipe";
                pipe.pipe.name = inputname + outputname;
                if (pipe.pipe.name.Contains("Kuehlschrank"))
                {
                    pipe.pipe.GetComponent<BezierParticle>().setGameMaterialColor(Color.blue);
                }
                else
                {
                    pipe.pipe.GetComponent<BezierParticle>().setGameMaterialColor(Color.red);
                }
                pipe.pipe.GetComponent<PathGenerator>().Connect(output.transform.position, input.transform.position);
            });
        }
    }

    // Method to display not marker based Objects. Currently not used in the project. Method can be deleted if
    // project will only use Marker based instanciating
    private void instantiateNotMarkerBasedObjects()
    {
        var prefab = notMarkerBasedObjects.FirstOrDefault(x => x.name.Contains("Kaeltespeicher"));
        var position = markerToPrefabList.FirstOrDefault(x => x.Instance.name.Contains("Kuehlschrank")).Instance.transform.position;
        var kaeltespeicher = Instantiate(prefab, position + new Vector3(0f, 0f, 5f), Quaternion.Euler(0f, 0f, 0f));
        spawnedObjects.Add(kaeltespeicher);

        prefab = notMarkerBasedObjects.FirstOrDefault(x => x.name.Contains("Waermepumpe"));
        position = kaeltespeicher.transform.position;
        var waermepumpe = Instantiate(prefab, position + new Vector3(5f, 0f, 2f), Quaternion.Euler(0f, 0f, 0f));
        spawnedObjects.Add(waermepumpe);

        prefab = notMarkerBasedObjects.FirstOrDefault(x => x.name.Contains("Waermespeicher"));
        position = kaeltespeicher.transform.position;
        var waermespeicher = Instantiate(prefab, position + new Vector3(2f, 0f, -2f), Quaternion.Euler(0f, 0f, 0f));
        spawnedObjects.Add(waermespeicher);

        spawnedObjects.ForEach(spawnedObject =>
            pipeConnection.ForEach(instance =>
            {
                if (spawnedObject.name.Contains(instance.fromPrefab.name))
                {
                    instance.from = spawnedObject;
                }
                else if (spawnedObject.name.Contains(instance.toPrefab.name))
                {
                    instance.to = spawnedObject;
                }
            })
          );
    }

    private void initPipeConnection()
    {
        spawnedObjects.ForEach(spawnedObject =>
            pipeConnection.ForEach(instance =>
            {
                if (spawnedObject.name.Contains(instance.fromPrefab.name))
                {
                    instance.from = spawnedObject;
                }
                else if (spawnedObject.name.Contains(instance.toPrefab.name))
                {
                    instance.to = spawnedObject;
                }
            })
        );
    }

    private void initWall()
    {
        if(this.wallMask != null)
        {
            return;
        }
        var leftMostMarkerToPrefab = markerToPrefabList.Aggregate((current, next) =>
        current.Instance.transform.position.x < next.Instance.transform.position.x ? current : next);
        var wallPosition = leftMostMarkerToPrefab.Instance.transform.position + new Vector3(-1, 0, 0.5f);
        this.wallMask = Instantiate(this.wall, wallPosition, Quaternion.Euler(0f, 0f, 0f));
        this.wallMask.GetComponentInChildren<WallMaskController>().Init();
    }
}

// Used for Marker based Object
public class MarkerToPrefab
{
    public string MarkerName { get; set; }
    public GameObject Prefab { get; set; }
    public GameObject Instance { get; set; }
    public bool Updated { get; set; } = false;
    public MarkerToPrefab(string name, GameObject prefab)
    {
        this.MarkerName = name;
        this.Prefab = prefab;
    }
}

// Used for Pipe Creation, Contains all Models (Marker based and Not Marker based) and all Pipe connections
public class PipeConnection
{
    public GameObject fromPrefab {get; set;}
    public GameObject toPrefab { get; set; }
    public GameObject pipePrefab { get; set; }
    public GameObject from { get; set; }
    public GameObject to { get; set; }
    public GameObject pipe { get; set; }
    public PipeConnection(GameObject fromPrefab, GameObject toPrefab, GameObject pipePrefab)
    {
        this.fromPrefab = fromPrefab;
        this.toPrefab = toPrefab;
        this.pipePrefab = pipePrefab;
    }
}
