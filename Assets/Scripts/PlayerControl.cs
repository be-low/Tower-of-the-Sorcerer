using System;
using System.Collections.Generic;
using Billow;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

public class PlayerControl : MonoBehaviour
{
    public float Speed = 20;
    public GameObject Ui;
    public GameObject[] FloorMaps;
    private Rigidbody2D _rigidbody2D;
    private const float Tolerance = 0.001f;
    private GameObject _currMap;
    private static Vector3Int[] Directions = {Vector3Int.up, Vector3Int.down, Vector3Int.left, Vector3Int.right};
    [SerializeField] private int _floor;
    [SerializeField] private Fighter _fighter;
    [SerializeField] private Dictionary<string, int> _prop;
    [SerializeField] private Dictionary<int, GameObject> _towerDictionary;
    [SerializeField] private bool _stairLock;

    private static readonly int[] Medicines = {0, 100, 200, 400, 800},
        Gems = {0, 1, 2, 4, 8};

    // Use this for initialization
    void Start()
    {
        _floor = 1;
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _prop = new Dictionary<string, int>();
        _fighter = gameObject.AddComponent<Fighter>();

        _towerDictionary = new Dictionary<int, GameObject>();
        _currMap = FloorMaps[_floor - 1];
    }

    private void LoadMap()
    {
        _currMap.SetActive(false);
        _currMap = FloorMaps[_floor - 1];
        _currMap.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateVelocity();
    }

    void UpdateVelocity()
    {
        float x = Input.GetAxis("Horizontal"),
            y = Input.GetAxis("Vertical");
        Vector2 v = new Vector2(x, y).normalized;
        float vx = Math.Abs(v.x), vy = Math.Abs(v.y);
        if (Math.Abs(vx - vy) < Tolerance)
        {
            _rigidbody2D.velocity = Vector2.zero;
        }
        else
        {
            if (vy < 0.707)
            {
                float dx = x * Speed * Time.deltaTime;
                _rigidbody2D.velocity = new Vector2(dx, 0);
            }

            if (vx < 0.707)
            {
                float dy = y * Speed * Time.deltaTime;
                _rigidbody2D.velocity = new Vector2(0, dy);
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        string otherName = other.gameObject.name;
        Debug.Log(otherName);
        if (otherName.StartsWith("Prop"))
        {
            UpdateProp(otherName.Substring(4), 1);
            Destroy(other.gameObject);
        }
        else if (otherName.StartsWith("Door"))
        {
            if (TryOpenDoor(otherName.Substring(4)))
            {
                Destroy(other.gameObject);
            }
        }
        else if (otherName.StartsWith("Enemy"))
        {
            if (TryAttack(other.gameObject.GetComponent<Fighter>()))
            {
//                GetComponent<Animation>().Play("");
                UpdateFight();
                Destroy(other.gameObject);
            }
        }
        else if (otherName.StartsWith("Medicine"))
        {
            EatMedicine(otherName);
            Destroy(other.gameObject);
        }
        else if (otherName.StartsWith("Gem"))
        {
            PickUpGem(otherName);
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        string otherName = other.gameObject.name;
        if (otherName.StartsWith("Stair") && !_stairLock)
        {
            _stairLock = true;
            var ForeWall = GameObject.FindGameObjectWithTag("WallMap").GetComponent<Tilemap>();
//            var pos = ForeWall.WorldToCell(other.transform.position);
            OnStair(otherName, other.transform.position);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        string otherName = other.gameObject.name;
        if (otherName.StartsWith("Stair") && _stairLock)
        {
            _stairLock = false;
        }
    }

    void OnStair(string stair, Vector3 pos)
    {
        if (!_towerDictionary.ContainsKey(_floor))
        {
            _towerDictionary.Add(_floor, _currMap);
        }
        else
        {
            _towerDictionary[_floor] = _currMap;
        }

        string d = stair.Substring(5);
        switch (d)
        {
            case "Up":
                UpStair();
                break;
            case "Down":
                DownStair();
                break;
        }

        LoadMap();

//        var tilemap = GameObject.FindGameObjectWithTag("WallMap").GetComponent<Tilemap>();
//        Vector3Int dist = pos;
//        foreach (var direction in Directions)
//        {
//            dist = pos + direction;
//            var x = tilemap.GetTile(pos);
//            if (x == null)
//                break;
//        }

        transform.position = pos;
    }

    void UpStair()
    {
        // TODO do up stair
        _floor++;
        Debug.Log(_floor);
    }

    void DownStair()
    {
        // TODO do down stair
        _floor--;
        Debug.Log(_floor);
    }

    void PickUpGem(string gem)
    {
        string type = gem.Substring(3, gem.Length - 5);
        int level = int.Parse(gem.Substring(gem.Length - 1));
        switch (type)
        {
            case "Attach":
                _fighter.AttachPower += Gems[level];
                break;
            case "Defensive":
                _fighter.DefensivePower += Gems[level];
                break;
            default:
                Debug.Log(gem);
                break;
        }

        UpdateFight();
    }

    void EatMedicine(string medicine)
    {
        int level;
        if (int.TryParse(medicine.Substring(9), out level))
        {
            _fighter.Hp += Medicines[level];
            UpdateFight();
        }
        else
        {
            Debug.Log(medicine);
        }
    }

    bool TryOpenDoor(string clr)
    {
        string k = "Key" + clr;
        if (!_prop.ContainsKey(k) || _prop[k] <= 0) return false;
        UpdateProp(k, -1);
        return true;
    }

    bool TryAttack(Fighter other)
    {
        return _fighter.BattleWith(other);
    }

    void UpdateFight()
    {
        Ui.SendMessage("UpdateFighter", _fighter);
    }

    void UpdateProp(string key, int inc)
    {
        int count;
        if (_prop.TryGetValue(key, out count))
        {
            count += inc;
            _prop[key] = count;
        }
        else
        {
            Assert.IsTrue(inc > 0);
            count = inc;
            _prop.Add(key, count);
        }

        if (key.StartsWith("Key"))
        {
            string clr = key.Substring(3);
            Ui.SendMessage("UpdateKey", new KeyValuePair<string, int>(clr, count));
        }
    }
}