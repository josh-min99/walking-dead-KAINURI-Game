using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq; 
public class TeamInfo : MonoBehaviour
{
    public board board;
    Vector3 position;
    public GameObject zm1Prefab;
    public GameObject zm2Prefab;
    public GameObject zm3Prefab;
    public GameObject zm4Prefab;
    public GameObject zm5Prefab;
    public GameObject am1Prefab;
    public GameObject am2Prefab;
    public GameObject am3Prefab;
    public GameObject am4Prefab;
    public GameObject am5Prefab;
    public GameObject armorPrefab;
    public GameObject reversePrefab;
    public GameObject aidKitPrefab;
    public GameObject carPrefab;
    public GameObject vaccinePrefab;
    public GameObject gasBombPrefab;
    public GameObject raiboPrefab;
    public Piece[] temp;

    // 생성된 오브젝트를 저장할 리스트
    private List<GameObject> spawnedObjects = new List<GameObject>();


    private void OnEnable()
    {

        int n = 0;
        temp = board.Pieces;
        var sortedPieces = temp
        .Where(piece => piece != null && !piece.isIllusion)
        .OrderBy(piece => piece.id)
        .ToList();

        foreach(var piece in sortedPieces)
        {
            if (board.team)
            {
                position = new Vector3(-32f, 183.4f, -2f);
            }
            else
            {
                position = new Vector3(-35f, 168f, -2f);
            }

            position.x += (n % 3) * 3.5f;
            position.y -= (n / 3) * 4f;
            if (piece != null && (!piece.isIllusion))
            {

                GameObject obj = null;
                switch (piece.type)
                {
                    case PieceType.zombi1:
                        obj = Instantiate(zm1Prefab, position, Quaternion.identity);
                        break;
                    case PieceType.zombi2:
                        obj = Instantiate(zm2Prefab, position, Quaternion.identity);
                        break;
                    case PieceType.zombi3:
                        obj = Instantiate(zm3Prefab, position, Quaternion.identity);
                        break;
                    case PieceType.zombi4:
                        obj = Instantiate(zm4Prefab, position, Quaternion.identity);
                        break;
                    case PieceType.zombi5:
                        obj = Instantiate(zm5Prefab, position, Quaternion.identity);
                        break;
                    case PieceType.army1:
                        obj = Instantiate(am1Prefab, position, Quaternion.identity);
                        break;
                    case PieceType.army2:
                        obj = Instantiate(am2Prefab, position, Quaternion.identity);
                        break;
                    case PieceType.army3:
                        obj = Instantiate(am3Prefab, position, Quaternion.identity);
                        break;
                    case PieceType.army4:
                        obj = Instantiate(am4Prefab, position, Quaternion.identity);
                        break;
                    case PieceType.army5:
                        obj = Instantiate(am5Prefab, position, Quaternion.identity);
                        break;
                }


                if (obj != null)
                {

                    GameObject levelTextObj = new GameObject("LevelText");
                    levelTextObj.transform.SetParent(obj.transform);
                    levelTextObj.transform.localPosition = new Vector3(0.7f, -1.15f, 0); // 발 밑에 오도록 위치 조정
                    if (board.team)
                    {
                        levelTextObj.transform.localPosition = new Vector3(1.4f, -2.1f, 0);
                    }

                    TextMeshPro tmpro = levelTextObj.AddComponent<TextMeshPro>();
                    tmpro.text = piece.level.ToString();
                    tmpro.fontSize = 4f; // 크기 조정
                    tmpro.fontStyle = FontStyles.Bold;
                    tmpro.alignment = TextAlignmentOptions.Center;
                    tmpro.color = Color.white; // 원하는 색상


                    GameObject idTextObj = new GameObject("IdText");
                    idTextObj.transform.SetParent(obj.transform);
                    idTextObj.transform.localPosition = new Vector3(-1.6f, -2.15f, 0); // 발 밑에 오도록 위치 조정
                    if (board.team)
                    {
                        idTextObj.transform.localPosition = new Vector3(-2.9f, -4.3f, 0);
                    }
                    TextMeshPro tmpro2 = idTextObj.AddComponent<TextMeshPro>();
                    tmpro2.text = piece.id.ToString();
                    tmpro2.fontSize = 4f; // 크기 조정
                    tmpro2.fontStyle = FontStyles.Bold;
                    tmpro2.alignment = TextAlignmentOptions.Center;
                    tmpro2.color = Color.white; // 원하는 색상



                    // GameObject imageObj = Instantiate(levelImagePrefab); // 미리 만들어둔 이미지 프리팹
                    // imageObj.transform.SetParent(obj.transform);
                    // imageObj.transform.localPosition = new Vector3(1f, -1f, 0);
                    int c = 0;
                    foreach (itemType item in piece.haveItem)
                    {
                        switch (item)
                        {
                            case itemType.armor:
                                GameObject armorObj = Instantiate(armorPrefab); // 미리 만들어둔 이미지 프리팹
                                Sprite imsprite1 = armorObj.GetComponent<SpriteRenderer>().sprite;
                                CreateOutline2(armorObj, imsprite1);
                                armorObj.transform.SetParent(obj.transform);
                                armorObj.transform.localPosition = new Vector3(-0.8f + (c * 0.9f), -2f, -2);
                                if (board.team)
                                {
                                    armorObj.transform.localPosition = new Vector3(-1.5f + (c * 1.8f), -4f, -2);
                                }
                                c++;
                                break;
                            case itemType.reverse:
                                GameObject reverseObj = Instantiate(reversePrefab); // 미리 만들어둔 이미지 프리팹
                                reverseObj.transform.SetParent(obj.transform);
                                reverseObj.transform.localPosition = new Vector3(-1.1f, 1f, -2);
                                if (board.team)
                                {
                                    reverseObj.transform.localPosition = new Vector3(-2f, 1.9f, -2);
                                }
                                c++;
                                break;
                            case itemType.aidKit:
                                GameObject aidKitObj = Instantiate(aidKitPrefab); // 미리 만들어둔 이미지 프리팹
                                Sprite imsprite2 = aidKitObj.GetComponent<SpriteRenderer>().sprite;
                                CreateOutline2(aidKitObj, imsprite2);
                                aidKitObj.transform.SetParent(obj.transform);
                                aidKitObj.transform.localPosition = new Vector3(-0.8f + (c * 0.9f), -2f, -2);
                                if (board.team)
                                {
                                    aidKitObj.transform.localPosition = new Vector3(-1.5f + (c * 1.8f), -4f, -2);
                                }
                                c++;
                                break;
                            case itemType.car:
                                GameObject carObj = Instantiate(carPrefab); // 미리 만들어둔 이미지 프리팹
                                Sprite imsprite3 = carObj.GetComponent<SpriteRenderer>().sprite;
                                CreateOutline2(carObj, imsprite3);
                                carObj.transform.SetParent(obj.transform);
                                carObj.transform.localPosition = new Vector3(-0.8f + (c * 0.9f), -2f, -2);
                                c++;
                                break;
                            case itemType.raibo:
                                // GameObject raiboTextObj = new GameObject("raiboText");
                                // raiboTextObj.transform.SetParent(obj.transform);
                                // raiboTextObj.transform.localPosition = new Vector3(1f, 1f, 0);
                                // if (board.team)
                                // {
                                //     raiboTextObj.transform.localPosition = new Vector3(2f, 2f, 0);
                                // }

                                // TextMeshPro tmpro3 = raiboTextObj.AddComponent<TextMeshPro>();
                                // tmpro3.text = "raibo";
                                // tmpro3.fontSize = 2f; // 크기 조정
                                // tmpro3.fontStyle = FontStyles.Bold;
                                // tmpro3.alignment = TextAlignmentOptions.Center;
                                // tmpro3.color = Color.black; // 원하는 색상
                                // break;
                                GameObject raiboObj = Instantiate(raiboPrefab); // 미리 만들어둔 이미지 프리팹
                                Sprite imsprite4 = raiboObj.GetComponent<SpriteRenderer>().sprite;
                                CreateOutline2(raiboObj, imsprite4);
                                raiboObj.transform.SetParent(obj.transform);
                                raiboObj.transform.localPosition = new Vector3(1f, 1f, 0);
                                if (board.team)
                                {
                                    raiboObj.transform.localPosition = new Vector3(2f, 2f, 0);
                                }
                                break;
                            case itemType.advantage:

                                if (board.team)
                                {
                                    GameObject gasBombObj = Instantiate(gasBombPrefab);
                                    Sprite imsprite5 = gasBombObj.GetComponent<SpriteRenderer>().sprite;
                                    CreateOutline2(gasBombObj, imsprite5);
                                    gasBombObj.transform.SetParent(obj.transform);
                                    gasBombObj.transform.localPosition = new Vector3(2.5f, 0f, 0);
                                }
                                else
                                {
                                    GameObject vaccineObj = Instantiate(vaccinePrefab);
                                    Sprite imsprite6 = vaccineObj.GetComponent<SpriteRenderer>().sprite;
                                    CreateOutline2(vaccineObj, imsprite6);
                                    vaccineObj.transform.SetParent(obj.transform);
                                    vaccineObj.transform.localPosition = new Vector3(1f, 0f, 0);
                                }
                                 // 미리 만들어둔 이미지 프리팹

                                break;
                                // GameObject adTextObj = new GameObject("adText");
                                // adTextObj.transform.SetParent(obj.transform);
                                // adTextObj.transform.localPosition = new Vector3(1f, 0.5f, 0);

                                // TextMeshPro tmpro4 = adTextObj.AddComponent<TextMeshPro>();
                                // if (board.team)
                                // {
                                //     tmpro4.text = "gasBomb";
                                //     adTextObj.transform.localPosition = new Vector3(2f, 1f, 0);
                                // }
                                // else
                                // {
                                //     tmpro4.text = "vaccine";
                                // }

                                // tmpro4.fontSize = 2f; // 크기 조정
                                // tmpro4.fontStyle = FontStyles.Bold;
                                // tmpro4.alignment = TextAlignmentOptions.Center;
                                // tmpro4.color = Color.black; // 원하는 색상
                                // break;


                        }
                    }

                    spawnedObjects.Add(obj);
                    n++;
                }

            }
        }
    }

    private void OnDisable()
    {
        // 생성된 모든 오브젝트 삭제
        foreach (GameObject obj in spawnedObjects)
        {
            if (obj != null)
            {
                Destroy(obj);
            }
        }
        // 리스트 초기화
        spawnedObjects.Clear();
    }
    
    public void CreateOutline2(GameObject obj, Sprite pieceSprite, float scale = 1.2f)
    {
        // 테두리용 자식 오브젝트 생성
        GameObject outlineObj = new GameObject("Outline");

        outlineObj.transform.SetParent(obj.transform);
        outlineObj.transform.localPosition = Vector3.zero;
        outlineObj.transform.localScale = Vector3.one * scale; // 본체보다 약간 크게

        SpriteRenderer outlineSr = outlineObj.AddComponent<SpriteRenderer>();
        outlineSr.sprite = pieceSprite;
        outlineSr.color = Color.red; // 알파 0.7
        obj.GetComponent<SpriteRenderer>().sortingOrder = 1;
        outlineSr.sortingOrder = 0; // 본체 아래에 그려지게

        // outlineObj.SetActive(false); // 기본은 비활성화
    }
}

