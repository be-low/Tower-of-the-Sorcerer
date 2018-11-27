using System;
using System.Collections.Generic;
using Billow;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerControl : MonoBehaviour
{
    public float Speed = 20;
    public GameObject Ui;
    private Rigidbody2D _rigidbody2D;
    private const float Tolerance = 0.001f;
    private GameObject[] _currentMap;
    [SerializeField] private int _floor;
    [SerializeField] private Fighter _fighter;
    [SerializeField] private Dictionary<string, int> _prop;
    [SerializeField] private Dictionary<int, GameObject[]> _tower;

    private static readonly int[] Medicines = {0, 100, 200, 400, 800},
        Gems = {0, 1, 2, 4, 8};

    // Use this for initialization
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        _prop = new Dictionary<string, int>();
        {
            _fighter = gameObject.AddComponent<Fighter>();
            _fighter.Hp = 1000;
            _fighter.AttachPower = 10;
            _fighter.DefensivePower = 10;
            _fighter.CoinCount = 0;
        }
        {
            _currentMap = new GameObject[2];
            _currentMap[0] = GameObject.FindGameObjectWithTag("WallMap");
            _currentMap[1] = GameObject.FindGameObjectWithTag("ObjectMap");
        }
    }

    // Update is called once per frame
    void Update()
    {
        updateVelocity();
    }

    void updateVelocity()
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
            updateProp(otherName.Substring(4), 1);
            Destroy(other.gameObject);
        }
        else if (otherName.StartsWith("Door"))
        {
            if (tryOpenDoor(otherName.Substring(4)))
            {
                Destroy(other.gameObject);
            }
        }
        else if (otherName.StartsWith("Enemy"))
        {
            if (tryAttack(other.gameObject.GetComponent<Fighter>()))
            {
                updateFight();
                Destroy(other.gameObject);
            }
        }
        else if (otherName.StartsWith("Medicine"))
        {
            eatMedicine(otherName);
            Destroy(other.gameObject);
        }
        else if (otherName.StartsWith("Gem"))
        {
            pickUpGem(otherName);
            Destroy(other.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        string otherName = other.gameObject.name;
        if (otherName.StartsWith("Stair"))
        {
            onStair(otherName);
        }
    }

    void onStair(string stair)
    {
        if (!_tower.ContainsKey(_floor))
        {
            _tower.Add(_floor, _currentMap);
        }
        else
        {
            _tower[_floor] = _currentMap;
        }

        string d = stair.Substring(5);
        switch (d)
        {
            case "Up":
                upStair();
                break;
            case "Down":
                downStair();
                break;
        }
    }

    void upStair()
    {
        // TODO do up stair
        _floor++;
    }

    void downStair()
    {
        // TODO do down stair
        _floor--;
    }

    void pickUpGem(string gem)
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

        updateFight();
    }

    void eatMedicine(string medicine)
    {
        int level;
        if (int.TryParse(medicine.Substring(9), out level))
        {
            _fighter.Hp += Medicines[level];
            updateFight();
        }
        else
        {
            Debug.Log(medicine);
        }
    }

    bool tryOpenDoor(string clr)
    {
        string k = "Key" + clr;
        if (!_prop.ContainsKey(k) || _prop[k] <= 0) return false;
        updateProp(k, -1);
        return true;
    }

    bool tryAttack(Fighter other)
    {
        return _fighter.BattleWith(other);
    }

    void updateFight()
    {
        Ui.SendMessage("UpdateFighter", _fighter);
    }

    void updateProp(string key, int inc)
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