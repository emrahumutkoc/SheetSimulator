using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddToCartManager : MonoBehaviour {

    [SerializeField] private MarketManager marketManager;
    [SerializeField] private Text nameText;
    [SerializeField] private Text descriptionText;
    [SerializeField] private Text priceText;
    [SerializeField] private Text currentQuantityText;
    [SerializeField] private Text totalText;

    private Product product;

    private void Start() {
        if (product != null) {
            nameText.text = product.name;
            descriptionText.text = product.description;
            currentQuantityText.text = 1.ToString();
            priceText.text = product.price.ToString() + "$";
            totalText.text = ((int.Parse(currentQuantityText.text)) * product.price).ToString() + "$";

        }
    }

    public void SetProduct(Product newProduct) {
        product = newProduct;
    }

    public void IncreaseQuantity() {
        int currentQuantity = int.Parse(currentQuantityText.text);
        if (currentQuantity + 1 <= product.maxOrderableQuantity) {
            currentQuantityText.text = (int.Parse(currentQuantityText.text) + 1).ToString();
            UpdateTotal();
        }
    }

    public void DecreaseQuantity() {
        int currentQuantity = int.Parse(currentQuantityText.text);

        if (currentQuantity - 1 >= 1) {
            currentQuantityText.text = (int.Parse(currentQuantityText.text) - 1).ToString();
            UpdateTotal();
        }
    }

    public void UpdateTotal() {
        totalText.text = (product.price * int.Parse(currentQuantityText.text)).ToString() + "$";
    }

    public void OnAddToCart() {
        CartItem cartItem = new CartItem(product, int.Parse(currentQuantityText.text));
        marketManager.AddToCart(cartItem);
    }
}
