﻿using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour {

    public int width = 6;
    public int height = 6;
    public Text cellLabelPrefab;

    public Color defaultColor = Color.white;

    Canvas gridCanvas;

    public HexCell cellPrefab;
    HexMesh hexMesh;

    HexCell[] cells;

    void Awake() {
        gridCanvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>();

        cells = new HexCell[height * width];
        for (int z = 0, i = 0; z < height; z++) {
            for (int x = 0; x < width; x++) {
                CreateCell(x, z, i++);
            }
        }
    }

    void Start() {
        Refresh();
        //hexMesh.Triangulate(cells);
    }

    public void Refresh() {
        hexMesh.Triangulate(cells);
    }

    void CreateCell(int x, int z, int i) {
        Vector3 position;
        position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
        position.y = 0;
        position.z = z * (HexMetrics.outerRadius * 1.5f);

        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        cell.color = defaultColor;

        if (x > 0) {
            cell.SetNeighbour(HexDirection.W, cells[i - 1]);
        }
        if (z > 0) {
            if ((z & 1) == 0) {
                cell.SetNeighbour(HexDirection.SE, cells[i - width]);
                if (x > 0) {
                    cell.SetNeighbour(HexDirection.SW, cells[i - width - 1]);
                }
            } else {
                cell.SetNeighbour(HexDirection.SW, cells[i - width]);
                if (x < width - 1) {
                    cell.SetNeighbour(HexDirection.SE, cells[i - width + 1]);
                }
            }
        }

        Text label = Instantiate<Text>(cellLabelPrefab);
        label.rectTransform.SetParent(gridCanvas.transform, false);
        label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        label.text = cell.coordinates.ToStringSeparateLines();

        cell.uiRect = label.rectTransform;
    }

    public HexCell GetCell(Vector3 position) {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
        return cells[index];
    }

    void TouchCell(Vector3 pos) {
        pos = transform.InverseTransformPoint(pos);
        HexCoordinates coordinates = HexCoordinates.FromPosition(pos);
        int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
    }

    public void ColorCell(Vector3 pos, Color col) {
        pos = transform.InverseTransformPoint(pos);
        HexCoordinates coordinates = HexCoordinates.FromPosition(pos);
        int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
        HexCell cell = cells[index];
        cell.color = col;
        Refresh();
    }
}
