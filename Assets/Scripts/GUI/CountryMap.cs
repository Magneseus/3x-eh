using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountryMap : MonoBehaviour
{

    public GameObject CityNodePrefab;
    public GameObject CityEdgePrefab;

    private List<CityNode> cityNodes = new List<CityNode>();
    private Dictionary<CityNode, List<string>> cityEdges = new Dictionary<CityNode, List<string>>();
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SpawnCityNode(string cityName, Vector3 position, List<string> connectedCityNames)
    {
        GameObject go = Instantiate(CityNodePrefab, this.transform);

        Vector3 curPos = go.transform.localPosition;
        curPos += position;
        go.transform.localPosition = curPos;

        go.GetComponent<CityNode>().CityName = cityName;
        cityNodes.Add(go.GetComponent<CityNode>());
        cityEdges.Add(go.GetComponent<CityNode>(), connectedCityNames);

    }
    public void SpawnEdges()
    {
        foreach (CityNode cityNode in cityNodes)
        {
            List<CityNode> connectedCityNodes = RetrieveConnectedCityNodes(cityEdges[cityNode]);
            //TODO: Spawn edges between connected cities
            GameObject edge = Instantiate(CityEdgePrefab, this.transform);
            LineRenderer line = edge.GetComponent<LineRenderer>();
            line.positionCount = connectedCityNodes.Count + 1;
            line.SetPosition(0, cityNode.transform.position);
            int count = 1;
            foreach (CityNode connectedCity in connectedCityNodes)
            {
                line.SetPosition(count, connectedCity.transform.position);
                count++;
            }
        }
    }

    public List<CityNode> RetrieveConnectedCityNodes(List<string> connectedCityNames)
    {
        List<CityNode> connectedCityNodes = new List<CityNode>();

        foreach (string cityName in connectedCityNames)
        {
            foreach (CityNode cityNode in cityNodes)
            {
                if (cityNode.CityName.Equals(cityName))
                {
                    connectedCityNodes.Add(cityNode);
                }
            }
        }
        return connectedCityNodes;
    }
}
