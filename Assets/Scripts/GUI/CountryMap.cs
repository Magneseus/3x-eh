using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountryMap : MonoBehaviour {

    public GameObject CityNodePrefab;

    private List<CityNode> cityNodes;

	// Use this for initialization
	void Start () {
        cityNodes = new List<CityNode>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SpawnCityNode(string cityName, Vector3 position, List<string> connectedCityNames)
    {
        GameObject go = Instantiate(CityNodePrefab, this.transform);

        Vector3 curPos = go.transform.localPosition;
        curPos += position;
        go.transform.localPosition = curPos;

        go.GetComponent<CityNode>().CityName = cityName;

        //TODO: Spawn edges between connected cities

    }
}
