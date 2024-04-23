using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketManager : MonoBehaviour {
    [SerializeField] private Button marketCloseButton;
    [SerializeField] private GameObject marketUI;
    [SerializeField] private GameObject cartUI;
    [SerializeField] private GameObject productTemplate;
    [SerializeField] private GameObject productListBody;
    [SerializeField] private List<Product> productsObjects = new List<Product>();

    private void Start() {
        ListAllBuyableItems();
        // StartCoroutine(spawnUI());
    }


    private IEnumerator spawnUI() {
        WaitForSeconds wait = new WaitForSeconds(1);

        while (true) {
            if (productListBody != null && productTemplate != null) {
                GameObject item = Instantiate(productTemplate, productListBody.transform);
                item.SetActive(true);
            }
            yield return wait;
        }
    }

    private void ListAllBuyableItems() {
        for (int i = 0; i < productsObjects.Count; i++) {
            GameObject item = Instantiate(productTemplate, productListBody.transform);
            AddToCartManager addToCartManager = item.GetComponentInChildren<AddToCartManager>();
            addToCartManager.SetProduct(productsObjects[i]);
            item.SetActive(true);
        }
    }

    public void CloseMarketUI() {
        marketUI.SetActive(false);
    }

    public void ToggleCartUI() {
        cartUI.SetActive(!cartUI.activeSelf);
    }

    public void AddToCart(Product product) {
        Debug.Log("product added to cart : " + product.name);
    }
}
