using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Product", menuName ="ScriptableObjects/Product")]
public class Product : ScriptableObject {
    public string productName;
    public string description;
    public float price;
    public string image = "test";
    public GameObject productPrefab;
    public int maxOrderableQuantity = 10;
}
