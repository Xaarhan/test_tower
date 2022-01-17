using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Main : MonoBehaviour
{
    void Start() {
        shared = this;
        new ResMan();
        mainCamera = worldCamera;
        _map = new Map();
    }

    void Update() {
        delta = (int)Mathf.Round(Time.deltaTime * 1000);
        _map.Update(delta);
    }


    public GameObject addGameObject(string asset_name) {
        GameObject go = Instantiate(ResMan.getPrefub( asset_name)) as GameObject;
        return go;
    }


    public GameObject addGameObject(string asset_name, GameObject container) {
        GameObject go = Instantiate(ResMan.getPrefub(asset_name), container.transform) as GameObject;
        return go;
    }


    public GameObject addGameObject(string asset_name, Vector3 pos) {
        GameObject go = Instantiate(ResMan.getPrefub(asset_name), pos, new Quaternion()) as GameObject;
        return go;
    }

    public void removeGameObject(GameObject go) {
        Destroy(go);
    }


    public void onSliderChange( float val ) {
        _map.setTimePersent(val);
    }
    

    public void Pause() {
        _map.pause = !_map.pause;
        timeline.gameObject.SetActive(_map.pause);
    }



    private Map _map;

    public static Camera mainCamera;
    public static Main shared;
    public static int delta;
    public Camera worldCamera;
    public GameObject towerAnim;
    public BoxCollider finish;
    public Slider timeline;

}
