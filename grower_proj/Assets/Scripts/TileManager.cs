using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public List<Vector3> tileList;
    public List<GameObject> tileObjectList;
    public Transform ground;
    public Transform tileGroup;
    public GameObject ghostTile;
    public GameObject tilePrefab;
    public float space;

    private int isBreak = 0;

    private void Update() {
        ghostTile.transform.position = ClickPos();
        Tile();
    }   

    private void Tile() {
        if (Input.GetMouseButton(0)) {
            if(!CheckTile() && isBreak != 2) {
                if(ClickPos() == new Vector3(0, 50, 0)) return;
                
                GameObject tile = Instantiate(tilePrefab, ClickPos(), Quaternion.identity);
                tile.transform.parent = tileGroup;
                tileList.Add(ClickPos());
                tileObjectList.Add(tile);
                isBreak = 1;
            } else if(CheckTile() && isBreak != 1) {
                if(ClickPos() == new Vector3(0, 50, 0)) return;
                
                for(int i = 0; i < tileList.Count; i++) {
                    if(tileList[i] == ClickPos()) {
                        Destroy(tileObjectList[i]);
                        tileObjectList.Remove(tileObjectList[i]);
                    }
                }
                tileList.Remove(ClickPos());
                isBreak = 2;
            }
        } else if(Input.GetMouseButtonUp(0)) {
            isBreak = 0;
        }
    }

    private bool CheckTile() {
        bool tile = false;
        if(tileList != null) {
            for(int i = 0; i < tileList.Count; i++) {
                if(tileList[i] == ClickPos()) {
                    tile = true;
                }
            }
        }

        return tile;
    }

    private Vector3 ClickPos() {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity)) {
            Vector3 clickPosition = hit.point;
            float size = tilePrefab.transform.localScale.x;

            Vector3 roundedPosition = new Vector3(
                Mathf.Round(clickPosition.x / size) * size + 1,
                0,
                Mathf.Round(clickPosition.z / size) * size + 1
            );

            Vector3 xFive = new Vector3(4, 0, 0);
            Vector3 zFive = new Vector3(0, 0, 4);
            roundedPosition += roundedPosition.x > ground.localScale.x / 2 ? -xFive : roundedPosition.x < -ground.localScale.x / 2 ? xFive : Vector3.zero;
            roundedPosition += roundedPosition.z > ground.localScale.z / 2 ? -zFive : roundedPosition.z < -ground.localScale.z / 2 ? zFive : Vector3.zero;

            return roundedPosition;
        }

        return new Vector3(0, 50, 0);
    }
}
