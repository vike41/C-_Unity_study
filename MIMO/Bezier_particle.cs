using PathCreation;
using System;
using System.Collections.Generic;
using UnityEngine;

public class BezierParticle : MonoBehaviour
{
    public PathCreator pathCreator;
    public Shader shader;
    public GameObject particleBlueprint;
    public float maxSpeed = 5.0f;
    public float minSpeed = 3.0f;
    public float particleScale = 0.25f;
    public float spacing = 0.125f;
    public float radius = 0.125f;

    public bool CanRunParticleInit { get; set; } = false;

    private List<float> distanceTravelledList = new List<float>();
    private List<GameObject> sphereList = new List<GameObject>();
    private int numObj = 0;
    private Material gameobjectMaterial;
    private bool canRunAnimation = false;

    // Start is called before the first frame update
    void Start()
    {
        //this.Init();
    }

    // Update is called once per frame
    void Update()
    {
        if (CanRunParticleInit)
        {
            this.Init();
        }

        if (this.canRunAnimation)
        {
            this.runAnimation();
        }
    }

    public void setSpeed(bool isSwitched)
    {
        this.maxSpeed = isSwitched ? maxSpeed : minSpeed;
    }

    public void setGameMaterialColor(Color color)
    {
        if(this.gameobjectMaterial == null)
        {
            this.gameobjectMaterial = new Material(shader);
        }
        this.gameobjectMaterial.SetColor(this.gameobjectMaterial.shader.GetPropertyName(0), color);
    }

    private void runAnimation()
    {
        for (int i = 0; i < this.sphereList.Count; i++)
        {
            this.distanceTravelledList[i] += this.maxSpeed * Time.deltaTime;
            this.sphereList[i].transform.position = pathCreator.path.GetPointAtDistance(this.distanceTravelledList[i]);
            this.sphereList[i].transform.rotation = pathCreator.path.GetRotationAtDistance(this.distanceTravelledList[i]);
        }
    }

    private void Init()
    {
        if (this.gameobjectMaterial == null)
        {
            this.gameobjectMaterial = new Material(shader);
        }
        var particle = Instantiate(this.particleBlueprint, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
        particle.transform.localScale = new Vector3(this.particleScale, this.particleScale, this.particleScale);
        particle.GetComponent<SphereCollider>().radius = radius;
        particle.GetComponent<Renderer>().material = gameobjectMaterial;

        var offset = spacing + 2 * radius;
        this.numObj = Convert.ToInt32(pathCreator.path.length / offset);

        for (int i = 0; i < numObj; i++)
        {
            var particleCopy = Instantiate(particle, new Vector3(0.0f, 0.0f, 0.0f), Quaternion.identity);
            particleCopy.layer = LayerMask.NameToLayer("Visible");
            this.sphereList.Add(particleCopy);
            this.distanceTravelledList.Add(i * offset);
        }
        Destroy(particle);

        this.CanRunParticleInit = false;
        this.canRunAnimation = true;
    }
}
