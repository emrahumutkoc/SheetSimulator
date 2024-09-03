using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CartManager : MonoBehaviour {

    [SerializeField] private GameObject cartItemTemplate;
    [SerializeField] private GameObject cartItemBody;
    [SerializeField] private MarketManager marketManager;
    [SerializeField] private BalanceManager balanceManager;
    [SerializeField] private Text subTotalText;
    [SerializeField] private Text totalText;
    [SerializeField] private Text remainingText;

    private List<CartItem> currentCart = new List<CartItem>();

    private void Start() {
        SetTotalSection();
    }

    public void AddItemToCartModal(CartItem cartItem) {
        // Debug.Log("asda");

        bool isExist = false;
        for (int i = 0; i < currentCart.Count; i++) {
            if (currentCart[i].product.name == cartItem.product.name) {
                // int newQuantity = cartItem.quantity;

                currentCart[i] = cartItem;
                foreach (Transform child in cartItemBody.transform) {
                    CartItemManager cartItemManager = child.GetComponentInChildren<CartItemManager>();

                    Debug.Log("ssss" + child.name);
                    CartItem item = cartItemManager.GetCartItem();
                    if (item != null && item.product.name == cartItem.product.name) {
                        Debug.Log("ddddd" + cartItemManager.GetCartItem().product.name);
                        cartItemManager.SetCartItem(currentCart[i]);
                    }


                    /*if (cartItemManager != null && cartItemManager.GetCartItem().product.name == cartItem.product.name) {
                        cartItemManager.SetCartItem(currentCart[i]);
                    }*/
                }
                isExist = true;
            }
        }

        if (!isExist) {
            currentCart.Add(cartItem);
            GameObject item = Instantiate(cartItemTemplate, cartItemBody.transform);
            CartItemManager cartItemManager = item.GetComponentInChildren<CartItemManager>();
            cartItemManager.SetCartItem(cartItem);

            item.SetActive(true);
        }

        UpdateCartUI();
        SetTotalSection();

        /*for (int i = 0; i < cartItems.Count; i++) {
            GameObject item = Instantiate(cartItemTemplate, cartItemBody.transform);
            // CartItemManager cartItemManager = item.GetComponentInChildren<CartItemManager>();
            // cartItemManager.SetCartItem(cartItems[i]);
            item.SetActive(true);
            Debug.Log("name" + cartItems[i].product.name);
        }*/

    }

    public void UpdateCartList(List<CartItem> cart) {
        /*foreach (Transform child in cartItemBody.transform) {
            Destroy(child.gameObject);


           *//* CartItemManager cartItemManager = child.GetComponentInChildren<CartItemManager>();

            Debug.Log("ssss" + child.name);
            CartItem item = cartItemManager.GetCartItem();
            if (item != null && item.product.name == cartItem.product.name) {
                Debug.Log("ddddd" + cartItemManager.GetCartItem().product.name);
                cartItemManager.SetCartItem(currentCart[i]);
            }


            *//*if (cartItemManager != null && cartItemManager.GetCartItem().product.name == cartItem.product.name) {
                cartItemManager.SetCartItem(currentCart[i]);
            }*//*
        }*/
        currentCart = cart;
        for (int i = 0; i < cart.Count; i++) {

            CartItem cartItem = cart[i];
            bool isExist = false;
            foreach (Transform child in cartItemBody.transform) {
                CartItemManager manager = child.GetComponentInChildren<CartItemManager>();
                CartItem item2 = manager.GetCartItem();

                if (item2 != null && item2.product.name == cartItem.product.name) {
                    manager.SetCartItem(item2);
                    isExist = true;
                }
            }

            if (!isExist) {
                GameObject item = Instantiate(cartItemTemplate, cartItemBody.transform);
                CartItemManager cartItemManager = item.GetComponentInChildren<CartItemManager>();
                cartItemManager.SetCartItem(cart[i]);
                item.SetActive(true);
            }
        }

        SetTotalSection();

    }

    private void UpdateCartUI() {

    }

    public void SetUpdatedCart(List<CartItem> cartItems) {
        currentCart = cartItems;
    }

    public void DecreaseItemQuantity(CartItem item) {
        for (int i = 0; i < currentCart.Count; i++) {
            if (currentCart[i].product.name == item.product.name) {
                currentCart[i].quantity -= 1;
                AddItemToCartModal(currentCart[i]);
            }
        }

        marketManager.SetNewCart(currentCart);
        Debug.Log("decrease" + item.product.name);
    }

    public void IncreaseItemQuanity(CartItem item) {
        int totalItem = 0;
        for (int i = 0; i < currentCart.Count; i++) {
            totalItem += currentCart[i].quantity;
        }  

        if ((totalItem + item.quantity) <= 10) {
            for (int i = 0; i < currentCart.Count; i++) {
                if (currentCart[i].product.name == item.product.name) {
                    currentCart[i].quantity += 1;
                    AddItemToCartModal(currentCart[i]);
                }
            }

            marketManager.SetNewCart(currentCart);
        }
        
        Debug.Log("increase " + item.product.name);
    }

    public void DeleteItem(CartItem item) {
        for (int i = 0; i < currentCart.Count; i++) {
            if (currentCart[i].product.name == item.product.name) {
                currentCart.RemoveAt(i);
            }
        }

        for (int i = 0; i < currentCart.Count; i++) {
            Debug.Log(currentCart[i].product.name);
        }
        marketManager.SetNewCart(currentCart);
        SetTotalSection();
        // Debug.Log("delete " + item.product.name);
    }

    public void ClearCart() {
        for (int i = 0; i < currentCart.Count; i++) {
            currentCart.RemoveAt(i);
        }
        marketManager.SetNewCart(currentCart);
        SetTotalSection();

        foreach (Transform child in cartItemBody.transform) {
            CartItemManager manager = child.GetComponentInChildren<CartItemManager>();
            manager.DeleteItem();
        }
    }

    public void SetCurrentCart(List<CartItem> cart) {
        currentCart = cart;
    }

    public List<CartItem> GetCurrentCart() {
        return currentCart;
    }

    public void SetTotalSection() {
        double subtotal = 0;

        if (currentCart.Count > 0) {
            for (int i = 0; i < currentCart.Count; i++) {
                CartItem cartItem = currentCart[i];
                subtotal += cartItem.quantity * cartItem.product.price;
            }
        }

        subTotalText.text = subtotal.ToString();
        totalText.text = subtotal == 0 ? 0.ToString() : (subtotal + 8f).ToString();


        balanceManager.SetTotal(subtotal, currentCart);
    }
}
