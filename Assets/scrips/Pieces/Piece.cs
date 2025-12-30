using UnityEngine;
using TMPro;
using System.Collections.Generic;
using System.Linq;
public enum PieceType
{
    None = 0,
    army1 = 1,
    army2 = 2,
    army3 = 3,
    army4 = 4,
    army5 = 5,
    zombi1 = 6,
    zombi2 = 7,
    zombi3 = 8,
    zombi4 = 9,
    zombi5 = 10

}

public enum itemType
{
    None = 0,
    armor = 1,
    reverse = 2,
    car = 3,
    aidKit = 4,
    raibo = 5,
    advantage = 6
}

public class Piece : MonoBehaviour
{
    public PieceType type;
    public bool team;
    public int currentN;
    public int level;
    public int id;
    public bool turnMove;
    // public TextMeshPro tmpro;

    //item 장착
    public List<itemType> haveItem;
    public bool isIllusion = false;
    public bool hiding = false;

    public bool visitLibrary = false;
    public bool inDomitory = false;

    public GameObject boardObj;





    protected static List<int>[] availableMovesPerIndex;


    public TextMeshPro levelText;
    public int untileLayer;

    void Awake()
    {
        haveItem = new List<itemType>();
        turnMove = true;
        untileLayer = LayerMask.NameToLayer("unTile");
        levelText = GetComponentInChildren<TextMeshPro>();
        UpdateLevelText();

        availableMovesPerIndex = new List<int>[147];
        for (int i = 0; i < availableMovesPerIndex.Length; i++)
            availableMovesPerIndex[i] = new List<int>();
        availableMovesPerIndex[0].AddRange(new List<int> { 1, 2, 3 });
        availableMovesPerIndex[1].AddRange(new List<int> { 0, 5 });
        availableMovesPerIndex[2].AddRange(new List<int> { 0, 6, 7 });
        availableMovesPerIndex[3].AddRange(new List<int> { 0, 4 });
        availableMovesPerIndex[4].AddRange(new List<int> { 3, 8, 9 });
        availableMovesPerIndex[5].AddRange(new List<int> { 1, 11 });
        availableMovesPerIndex[6].AddRange(new List<int> { 2, 7, 11 });
        availableMovesPerIndex[7].AddRange(new List<int> { 2, 6, 15 });
        availableMovesPerIndex[8].AddRange(new List<int> { 4, 15 });
        availableMovesPerIndex[9].AddRange(new List<int> { 4, 16 });
        availableMovesPerIndex[10].AddRange(new List<int> { 11, 17 });
        availableMovesPerIndex[11].AddRange(new List<int> { 5, 6, 10, 12 });
        availableMovesPerIndex[12].AddRange(new List<int> { 11, 13 });
        availableMovesPerIndex[13].AddRange(new List<int> { 12, 14, 117 });
        availableMovesPerIndex[14].AddRange(new List<int> { 13, 15, 21, 118 });
        availableMovesPerIndex[15].AddRange(new List<int> { 7, 8, 14, 118 });
        availableMovesPerIndex[16].AddRange(new List<int> { 9, 23, 118 });
        availableMovesPerIndex[17].AddRange(new List<int> { 10, 18, 117 });
        availableMovesPerIndex[18].AddRange(new List<int> { 17, 19, 124 });
        availableMovesPerIndex[19].AddRange(new List<int> { 18, 117, 20, 29 });
        availableMovesPerIndex[20].AddRange(new List<int> { 117, 19, 21, 32 });
        availableMovesPerIndex[21].AddRange(new List<int> { 14, 20, 22 });
        availableMovesPerIndex[22].AddRange(new List<int> { 21, 23, 35, 33 });
        availableMovesPerIndex[23].AddRange(new List<int> { 16, 24, 22 });
        availableMovesPerIndex[24].AddRange(new List<int> { 23, 25 });
        availableMovesPerIndex[25].AddRange(new List<int> { 24, 26 });
        availableMovesPerIndex[26].AddRange(new List<int> { 25, 37 });
        availableMovesPerIndex[27].AddRange(new List<int> { 124, 47, 125, 28 });
        availableMovesPerIndex[28].AddRange(new List<int> { 27, 29 });
        availableMovesPerIndex[29].AddRange(new List<int> { 19, 28, 126 });
        availableMovesPerIndex[30].AddRange(new List<int> { 31, 126, 55 });
        availableMovesPerIndex[31].AddRange(new List<int> { 32, 30 });
        availableMovesPerIndex[32].AddRange(new List<int> { 20, 120, 31 });
        availableMovesPerIndex[33].AddRange(new List<int> { 22, 34, 120 });
        availableMovesPerIndex[34].AddRange(new List<int> { 33, 35, 40, 59 });
        availableMovesPerIndex[35].AddRange(new List<int> { 22, 34, 36 });
        availableMovesPerIndex[36].AddRange(new List<int> { 35, 37 });
        availableMovesPerIndex[37].AddRange(new List<int> { 26, 36, 41, 38 });
        availableMovesPerIndex[38].AddRange(new List<int> { 37, 119 });
        availableMovesPerIndex[39].AddRange(new List<int> { 119, 122 });
        availableMovesPerIndex[40].AddRange(new List<int> { 34, 121 });
        availableMovesPerIndex[41].AddRange(new List<int> { 37, 121, 42 });
        availableMovesPerIndex[42].AddRange(new List<int> { 41, 43, 62, 119 });
        availableMovesPerIndex[43].AddRange(new List<int> { 42 });
        availableMovesPerIndex[44].AddRange(new List<int> { 119, 45 });
        availableMovesPerIndex[45].AddRange(new List<int> { 44, 122 });
        availableMovesPerIndex[46].AddRange(new List<int> { 47, 49 });
        availableMovesPerIndex[47].AddRange(new List<int> { 46, 27, 48 });
        availableMovesPerIndex[48].AddRange(new List<int> { 47, 49 });
        availableMovesPerIndex[49].AddRange(new List<int> { 46, 48, 50 });
        availableMovesPerIndex[50].AddRange(new List<int> { 49, 51 });
        availableMovesPerIndex[51].AddRange(new List<int> { 50, 134 });
        availableMovesPerIndex[52].AddRange(new List<int> { 125, 53, 82 });
        availableMovesPerIndex[53].AddRange(new List<int> { 52, 54, 136 });
        availableMovesPerIndex[54].AddRange(new List<int> { 53, 136, 55 });
        availableMovesPerIndex[55].AddRange(new List<int> { 54, 30, 56 });
        availableMovesPerIndex[56].AddRange(new List<int> { 55, 127 });
        availableMovesPerIndex[57].AddRange(new List<int> { 145, 69 });
        availableMovesPerIndex[58].AddRange(new List<int> { 59, 145 });
        availableMovesPerIndex[59].AddRange(new List<int> { 34, 58, 123 });
        availableMovesPerIndex[60].AddRange(new List<int> { 121, 123, 61 });
        availableMovesPerIndex[61].AddRange(new List<int> { 60, 62, 146 });
        availableMovesPerIndex[62].AddRange(new List<int> { 42, 61, 63 });
        availableMovesPerIndex[63].AddRange(new List<int> { 62, 64 });
        availableMovesPerIndex[64].AddRange(new List<int> { 63, 129 });
        availableMovesPerIndex[65].AddRange(new List<int> { 66, 129, 67 });
        availableMovesPerIndex[66].AddRange(new List<int> { 122, 65 });
        availableMovesPerIndex[67].AddRange(new List<int> { 65, 130 });
        availableMovesPerIndex[68].AddRange(new List<int> { 136, 127, 75 });
        availableMovesPerIndex[69].AddRange(new List<int> { 57, 70, 127 });
        availableMovesPerIndex[70].AddRange(new List<int> { 69, 71 });
        availableMovesPerIndex[71].AddRange(new List<int> { 70, 128, 146 });
        availableMovesPerIndex[72].AddRange(new List<int> { 128, 73 });
        availableMovesPerIndex[73].AddRange(new List<int> { 72, 129, 130 });
        availableMovesPerIndex[74].AddRange(new List<int> { 129, 130 });
        availableMovesPerIndex[75].AddRange(new List<int> { 68, 76 });
        availableMovesPerIndex[76].AddRange(new List<int> { 128, 75, 87 });
        availableMovesPerIndex[77].AddRange(new List<int> { 131, 132 });
        availableMovesPerIndex[78].AddRange(new List<int> { 131, 132, 130 });
        availableMovesPerIndex[79].AddRange(new List<int> { 133, 134, 80 });
        availableMovesPerIndex[80].AddRange(new List<int> { 79, 94, 142, 81 });
        availableMovesPerIndex[81].AddRange(new List<int> { 80, 83, 135 });
        availableMovesPerIndex[82].AddRange(new List<int> { 52, 135 });
        availableMovesPerIndex[83].AddRange(new List<int> { 81, 137, 84 });
        availableMovesPerIndex[84].AddRange(new List<int> { 83, 85, 138, 143 });
        availableMovesPerIndex[85].AddRange(new List<int> { 84, 103, 138, 139, 143 });
        availableMovesPerIndex[86].AddRange(new List<int> { 87, 138, 139 });
        availableMovesPerIndex[87].AddRange(new List<int> { 76, 86, 140 });
        availableMovesPerIndex[88].AddRange(new List<int> { 90, 139, 140 });
        availableMovesPerIndex[89].AddRange(new List<int> { 90, 132 });
        availableMovesPerIndex[90].AddRange(new List<int> { 88, 89, 91 });
        availableMovesPerIndex[91].AddRange(new List<int> { 90, 92, 107 });
        availableMovesPerIndex[92].AddRange(new List<int> { 91, 93, 107, 112 });
        availableMovesPerIndex[93].AddRange(new List<int> { 92, 113 });
        availableMovesPerIndex[94].AddRange(new List<int> { 80, 141 });
        availableMovesPerIndex[95].AddRange(new List<int> { 96, 141, 142 });
        availableMovesPerIndex[96].AddRange(new List<int> { 95, 97, 98 });
        availableMovesPerIndex[97].AddRange(new List<int> { 96 });
        availableMovesPerIndex[98].AddRange(new List<int> { 96, 99, 143 });
        availableMovesPerIndex[99].AddRange(new List<int> { 98, 100 });
        availableMovesPerIndex[100].AddRange(new List<int> { 99, 101, 102 });
        availableMovesPerIndex[101].AddRange(new List<int> { 100, 143 });
        availableMovesPerIndex[102].AddRange(new List<int> { 100, 104, 144 });
        availableMovesPerIndex[103].AddRange(new List<int> { 85, 144 });
        availableMovesPerIndex[104].AddRange(new List<int> { 102, 144, 105 });
        availableMovesPerIndex[105].AddRange(new List<int> { 104, 116 });
        availableMovesPerIndex[106].AddRange(new List<int> { 107, 108, 144 });
        availableMovesPerIndex[107].AddRange(new List<int> { 91, 92, 106 });
        availableMovesPerIndex[108].AddRange(new List<int> { 106, 109 });
        availableMovesPerIndex[109].AddRange(new List<int> { 108, 110, 114, 115 });
        availableMovesPerIndex[110].AddRange(new List<int> { 109, 112 });
        availableMovesPerIndex[111].AddRange(new List<int> { 113, 114, 115 });
        availableMovesPerIndex[112].AddRange(new List<int> { 92, 110, 113, 114 });
        availableMovesPerIndex[113].AddRange(new List<int> { 93, 111, 112 });
        availableMovesPerIndex[114].AddRange(new List<int> { 109, 111, 112 });
        availableMovesPerIndex[115].AddRange(new List<int> { 109, 111, 116 });
        availableMovesPerIndex[116].AddRange(new List<int> { 105, 115 });
        availableMovesPerIndex[117].AddRange(new List<int> { 13, 17, 19, 20 });
        availableMovesPerIndex[118].AddRange(new List<int> { 14, 15, 16 });
        availableMovesPerIndex[119].AddRange(new List<int> { 38, 39, 42, 44 });
        availableMovesPerIndex[120].AddRange(new List<int> { 32, 33 });
        availableMovesPerIndex[121].AddRange(new List<int> { 40, 41, 60 });
        availableMovesPerIndex[122].AddRange(new List<int> { 45, 66, 39 });
        availableMovesPerIndex[123].AddRange(new List<int> { 59, 60 });
        availableMovesPerIndex[124].AddRange(new List<int> { 18, 27 });
        availableMovesPerIndex[125].AddRange(new List<int> { 27, 52 });
        availableMovesPerIndex[126].AddRange(new List<int> { 29, 30 });
        availableMovesPerIndex[127].AddRange(new List<int> { 56, 68, 69 });
        availableMovesPerIndex[128].AddRange(new List<int> { 71, 72, 76 });
        availableMovesPerIndex[129].AddRange(new List<int> { 64, 65, 73,74 });
        availableMovesPerIndex[130].AddRange(new List<int> { 67, 73, 74, 78 });
        availableMovesPerIndex[131].AddRange(new List<int> { 77, 78 });
        availableMovesPerIndex[132].AddRange(new List<int> { 77, 78, 89 });
        availableMovesPerIndex[133].AddRange(new List<int> { 79 });
        availableMovesPerIndex[134].AddRange(new List<int> { 51, 79 });
        availableMovesPerIndex[135].AddRange(new List<int> { 81, 82 });
        availableMovesPerIndex[136].AddRange(new List<int> { 53, 54, 68 });
        availableMovesPerIndex[137].AddRange(new List<int> { 83 });
        availableMovesPerIndex[138].AddRange(new List<int> { 84, 85, 86 });
        availableMovesPerIndex[139].AddRange(new List<int> { 85, 86, 88 });
        availableMovesPerIndex[140].AddRange(new List<int> { 87, 88 });
        availableMovesPerIndex[141].AddRange(new List<int> { 94, 95 });
        availableMovesPerIndex[142].AddRange(new List<int> { 80, 95 });
        availableMovesPerIndex[143].AddRange(new List<int> { 84, 85, 98, 101 });
        availableMovesPerIndex[144].AddRange(new List<int> { 102, 103, 104, 106 });
        availableMovesPerIndex[145].AddRange(new List<int> { 57, 58 });
        availableMovesPerIndex[146].AddRange(new List<int> { 61, 71 });

    }

    public void CloneFrom(Piece original)
    {
        this.currentN = original.currentN;
        this.turnMove = original.turnMove;

        // 리스트를 새로 만들지 않고 원본 리스트를 그대로 사용
        this.haveItem = original.haveItem;

        this.isIllusion = original.isIllusion;
        this.hiding = original.hiding;
        this.visitLibrary = original.visitLibrary;
        this.inDomitory = original.inDomitory;

        this.transform.position = original.transform.position;
        this.transform.rotation = original.transform.rotation;
    }

    public List<int> getNextMove(List<int> prev)
    {
        boardObj = GameObject.Find("board");

        board board = boardObj.GetComponent<board>();
        List<int> temp = new List<int>();
        for (int i = 0; i < prev.Count; i++)
        {
            if (!(board.tiles[prev[i]].layer == untileLayer))
            {
                temp.AddRange(availableMovesPerIndex[prev[i]]);
            }

        }
        temp.AddRange(prev);

        temp = temp.Distinct().ToList();
        return temp;
    }

    public void SetLevel(int newLevel)
    {
        level = newLevel;
        UpdateLevelText();
    }

    private void UpdateLevelText()
    {
        if (levelText != null)
            levelText.text = "Lv " + level.ToString();
    }


    [HideInInspector]
    public GameObject outlineObj; // Outline SpriteRenderer 오브젝트


    // 호출 시 outlineObj 생성
    public void CreateOutline(Sprite pieceSprite, Color outlineColor, float scale = 1.5f)
    {
        // 테두리용 자식 오브젝트 생성
        outlineObj = new GameObject("Outline");

        outlineObj.transform.SetParent(transform);
        outlineObj.transform.localPosition = Vector3.zero;
        outlineObj.transform.localScale = Vector3.one * scale; // 본체보다 약간 크게

        SpriteRenderer outlineSr = outlineObj.AddComponent<SpriteRenderer>();
        outlineSr.sprite = pieceSprite;
        outlineSr.color = outlineColor; // 알파 0.7
        outlineSr.sortingOrder = 0; // 본체 아래에 그려지게

        outlineObj.SetActive(false); // 기본은 비활성화
    }

    public void SetOutlineActive(bool active)
    {
        if (outlineObj != null)
            // Debug.Log("active 할게요");
            outlineObj.SetActive(active);
    }

    public virtual List<int> GetAvailableMoves(ref Piece[] board, int TILE_NUMBER)
    {
        List<int> r = new List<int>();

        r.Add(1);
        r.Add(2);
        r.Add(3);
        r.Add(4);

        return r;
    }


    public static List<int> GetNodesAtDepth(int startIndex, int depth)
    {
        var visited = new HashSet<int>();
        var currentLevel = new List<int> { startIndex };
        visited.Add(startIndex);

        for (int d = 0; d < depth; d++)
        {
            var nextLevel = new List<int>();
            foreach (var node in currentLevel)
            {
                foreach (var neighbor in availableMovesPerIndex[node])
                {
                    if (!visited.Contains(neighbor))
                    {
                        nextLevel.Add(neighbor);
                        visited.Add(neighbor);
                    }
                }
            }
            currentLevel = nextLevel;
            if (currentLevel.Count == 0) break; // 더 이상 퍼질 노드가 없으면 중단
        }

        return currentLevel;
    }



    public int GetNodeDepth(int startNode, int targetNode)
    {
        if (startNode == targetNode)
            return 0;

        Queue<(int node, int depth)> queue = new Queue<(int, int)>();
        HashSet<int> visited = new HashSet<int>();

        queue.Enqueue((startNode, 0));
        visited.Add(startNode);

        while (queue.Count > 0)
        {
            var (current, depth) = queue.Dequeue();

            foreach (int next in availableMovesPerIndex[current])
            {
                if (next == targetNode)
                    return depth + 1;

                if (!visited.Contains(next))
                {
                    visited.Add(next);
                    queue.Enqueue((next, depth + 1));
                }
            }
        }

        // targetNode에 도달할 수 없는 경우
        return -1;
    }

    


}




