using UnityEngine;

public class Tile : MonoBehaviour
{

    

    [HideInInspector]
    public GameObject outlineObj;

    public bool isMine;
    public bool oppoIsMine;
    public bool isVisited = false;
    

    public void SetOutlineActive(bool active, Color? outlineColor = null)
    {
        if (outlineObj != null)
        {
            outlineObj.SetActive(active);
            if (outlineColor.HasValue)
            {
                var sr = outlineObj.GetComponent<SpriteRenderer>();
                if (sr != null)
                    sr.color = new Color(outlineColor.Value.r, outlineColor.Value.g, outlineColor.Value.b, 0.5f);
            }
        }

    }
}
