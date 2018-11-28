using UnityEngine;

public class HexCell : MonoBehaviour {
    public HexCoordinates coordinates;
    public Color color;
    public int elevation;
    public RectTransform uiRect;

    [SerializeField]
    HexCell[] neighbours;

    public HexCell GetNeighbour(HexDirection direction) {
        return neighbours[(int)direction];
    }

    public void SetNeighbour(HexDirection direction, HexCell cell) {
        neighbours[(int)direction] = cell;
        cell.neighbours[(int)direction.Opposite()] = this;
    }

    public int Elevation {
        get {
            return elevation;
        }
        set {
            elevation = value;
            Vector3 position = transform.localPosition;
            position.y = value * HexMetrics.elevationStep;
            transform.localPosition = position;

            Vector3 uiPosition = uiRect.localPosition;
            uiPosition.z = elevation * -HexMetrics.elevationStep;
            uiRect.localPosition = uiPosition;
        }
    }

    public HexEdgeType GetEdgeType(HexDirection direction) {
        return HexMetrics.GetEdgeType(
            elevation, neighbours[(int)direction].elevation
        );
    }

    public HexEdgeType GetEdgeType(HexCell otherCell) {
        return HexMetrics.GetEdgeType(
            elevation, otherCell.elevation
        );
    }
}
