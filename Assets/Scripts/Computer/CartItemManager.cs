using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CartItemManager : MonoBehaviour {
    [SerializeField] private CartManager cartManager;
    [SerializeField] private Text productNameText;
    [SerializeField] private Text unitText;
    [SerializeField] private Text priceText;
    [SerializeField] private Text totalText;

    private CartItem cartItem;


    public void SetCartItem(CartItem item) {
        cartItem = item;
        UpdateUI();
    }

    public CartItem GetCartItem() {
        return cartItem;
    }
    public void IncreaseQuantity() {
        CartItem newCartItem = new CartItem(cartItem.product, 1);
        cartManager.IncreaseItemQuanity(newCartItem);
    }

    public void DecreaseQuantity() {
        if (cartItem.quantity > 1) {
            cartManager.DecreaseItemQuantity(cartItem);
        }
    }    
    
    public void DeleteItem() {
        cartManager.DeleteItem(cartItem);
        Destroy(gameObject);
    }

    public void UpdateUI() {
        productNameText.text = cartItem.product.name;
        unitText.text = cartItem.quantity.ToString();
        priceText.text = '$' + cartItem.product.price.ToString();
        totalText.text = '$' + (cartItem.quantity * cartItem.product.price).ToString();
    }
}
