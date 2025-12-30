using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq; 
public class OppoInfo : MonoBehaviour
{
    public board board;
    Vector3 position;
    public PieceDTO[] temp;
    public bool text5 = true;
    public bool text4= true;
    public bool text3= true;
    public bool text2= true;
    public bool text1= true;
    public TextUICreater textcreator;

    public int mine = 0;
    public int illusion = 0;
    public int gps = 0;
    public int bigbomb = 0;
    public int changePlace = 0;
    public int reverse = 0;
    public int teleport = 0;
    public int armor = 0;
    public int drone = 0;
    public int car = 0;

    // 생성된 오브젝트를 저장할 리스트
    private List<GameObject> spawnedObjects = new List<GameObject>();


    private void OnEnable()
    {
        text5 = true;
        text4 = true;
        text3 = true;
        text2 = true;
        text1 = true;

        int n = 0;
        temp = board.OppositePieces;
        Debug.Log(temp);


        foreach (var piece in temp)
        {
            if (board.team)
            {
                position = new Vector3(-63f, 210f, -1f);
            }
            else
            {
                position = new Vector3(-323f, 190f, -1f);
            }
            position.y -= n * 60f;
            if (piece != null && (!piece.isIllusion))
            {
                GameObject obj = null;
                if (piece.level > 10 && text5)
                {

                    // obj.transform.localPosition = position; // 발 밑에 오도록 위치 조정

                    // TextMeshPro tmpro = obj.AddComponent<TextMeshPro>();
                    if (!board.team)
                    {
                        obj = textcreator.CreateTextUI(position, "좀비킹", 30);
                    }
                    else
                    {
                        obj = textcreator.CreateTextUI(position, "슈퍼솔져", 30);
                    }
                    text5 = false;
                }
                else if (piece.level > 7 && text4)
                {
                    if (!board.team)
                    {
                        obj = textcreator.CreateTextUI(position, "특수좀비", 30);
                    }
                    else
                    {
                        obj = textcreator.CreateTextUI(position, "사이보그", 30);
                    }
                    text4 = false;
                    // obj = new GameObject("4");
                    // obj.transform.localPosition = position; // 발 밑에 오도록 위치 조정

                    // TextMeshPro tmpro = obj.AddComponent<TextMeshPro>();
                    // if (board.team)
                    // {
                    //     tmpro.text = "Special Zombie";
                    // }
                    // else
                    // {
                    //     tmpro.text = "Cyborg";
                    // }

                    // tmpro.fontSize = 15f; // 크기 조정
                    // tmpro.fontStyle = FontStyles.Bold;
                    // tmpro.alignment = TextAlignmentOptions.Center;
                    // tmpro.color = Color.black; // 원하는 색상
                    // text4 = false;
                }
                else if (piece.level > 4 && text3)
                {
                    if (!board.team)
                    {
                        obj = textcreator.CreateTextUI(position, "근육좀비", 30);
                    }
                    else
                    {
                        obj = textcreator.CreateTextUI(position, "특전사", 30);
                    }
                    text3 = false;

                }
                else if (piece.level > 2 && text2)
                {
                    if (!board.team)
                    {
                        obj = textcreator.CreateTextUI(position, "일반좀비", 30);
                    }
                    else
                    {
                        obj = textcreator.CreateTextUI(position, "보병", 30);
                    }
                    text2 = false;
                }
                else if (text1)
                {
                    if (!board.team)
                    {
                        obj = textcreator.CreateTextUI(position, "아기좀비", 30);
                    }
                    else
                    {
                        obj = textcreator.CreateTextUI(position, "신병", 30);
                    }
                    text1 = false;
                }
                spawnedObjects.Add(obj);
                n++;

            }
        }
        GameObject obj2 = null;
        if (board.team)
        {
            position = new Vector3(322f, 225f, -1f);
        }
        else
        {
            position = new Vector3(66f, 214f, -1f);
        }
        if (mine != 0)
        {
            obj2 = textcreator.CreateTextUI(position, "지뢰: " + mine.ToString(), 30);
            spawnedObjects.Add(obj2);
            position.y -= 45f;
        }
        if (illusion != 0)
        {
            obj2 = textcreator.CreateTextUI(position, "분신술: " + illusion.ToString(), 30);
            spawnedObjects.Add(obj2);
            position.y -= 45f;
        }
        if (gps != 0)
        {
            obj2 = textcreator.CreateTextUI(position, "GPS추적기: " + gps.ToString(), 30);
            spawnedObjects.Add(obj2);
            position.y -= 45f;
        }
        if (bigbomb != 0)
        {
            obj2 = textcreator.CreateTextUI(position, "메가폭탄: " + bigbomb.ToString(), 30);
            spawnedObjects.Add(obj2);
            position.y -= 45f;
        }
        if (changePlace != 0)
        {
            obj2 = textcreator.CreateTextUI(position, "말체인지: " + changePlace.ToString(), 30);
            spawnedObjects.Add(obj2);
            position.y -= 45f;
        }
        if (reverse != 0)
        {
            obj2 = textcreator.CreateTextUI(position, "인과역전: " + reverse.ToString(), 30);
            spawnedObjects.Add(obj2);
            position.y -= 45f;
        }
        if (teleport != 0)
        {
            obj2 = textcreator.CreateTextUI(position, "텔레포트: " + teleport.ToString(), 30);
            spawnedObjects.Add(obj2);
            position.y -= 45f;
        }
        if (armor != 0)
        {
            obj2 = textcreator.CreateTextUI(position, "방탄복: " + armor.ToString(), 30);
            spawnedObjects.Add(obj2);
            position.y -= 45f;
        }
        if (drone != 0)
        {
            obj2 = textcreator.CreateTextUI(position, "정찰드론: " + drone.ToString(), 30);
            spawnedObjects.Add(obj2);
            position.y -= 45f;
        }
        if (car != 0)
        {
            obj2 = textcreator.CreateTextUI(position, "군용차: " + car.ToString(), 30);
            spawnedObjects.Add(obj2);
            position.y -= 45f;
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
    
}


