using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/*public interface ICartItem {
    Product product { get; set; }
    int quantity { get; set; }
}*/

public class CartItem {
    public Product product { get; set; }
    public int quantity { get; set; }

    public CartItem(Product product, int quantity) {
        this.product = product;
        this.quantity = quantity;
    }
}

public class MarketManager : MonoBehaviour {
    [SerializeField] private Button marketCloseButton;
    [SerializeField] private GameObject marketUI;
    [SerializeField] private GameObject cartUI;
    [SerializeField] private CartManager cartManager;
    [SerializeField] private GameObject productTemplate;
    [SerializeField] private GameObject productListBody;
    [SerializeField] private Text cartTotalPriceText;
    [SerializeField] private Text cartItemCountText;
    [SerializeField] private List<Product> productsObjects = new List<Product>();


    private List<CartItem> cart = new List<CartItem>();
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
        UpdateTotalAndItemCount();
    }

    public void CloseMarketUI() {
        marketUI.SetActive(false);
    }

    public void ToggleCartUI() {
        cartUI.SetActive(!cartUI.activeSelf);
    }

    public void UpdateTotalAndItemCount() {
        cartTotalPriceText.text = "$" + CalculateCartTotal().ToString();
        cartItemCountText.text = SumTotalItemsInCart().ToString();
    }

    public void AddToCart(CartItem cartItem) {
        int totalItem = 0;
        for (int i = 0; i < cart.Count; i++) {
            totalItem += cart[i].quantity;
        }

        if ((totalItem + cartItem.quantity) > 10) {
            return;
        }

        bool isExist = false;
        CartItem newItem = cartItem;
        for (int i = 0; i < cart.Count; i++) {
            if (cart[i].product.name == cartItem.product.name) {
                cart[i].quantity += cartItem.quantity;
                newItem = cart[i];
                isExist = true;
            }
        }

        if (!isExist) {
            cart.Add(cartItem);
        }

        UpdateTotalAndItemCount();

        // cartManager.AddItemToCartModal(newItem);
        cartManager.UpdateCartList(cart);

        /*for (int i = 0; i < cart.Count; i++) {
            Debug.Log("name: " + cart[i].product.name + "quantity : " + cart[i].quantity);
        }*/
    }

    private double CalculateCartTotal() {
        double total = 0.00;

        if (cart.Count > 0) {
            for (int i = 0; i < cart.Count; i++) {
                total += cart[i].quantity * cart[i].product.price;
            }
        }

        return total;
    }

    public int SumTotalItemsInCart() {
        int total = 0;
        if (cart.Count > 0) {
            for (int i = 0; i < cart.Count; i++) {
                total += cart[i].quantity;
            }
        }
        return total;
    }

    public void DeleteItemCartForMarket(CartItem item) {
        for (int i = 0; i < cart.Count; i++) {
            if (cart[i].product.name == item.product.name) {
                cart.RemoveAt(i);
            }
        }

        UpdateTotalAndItemCount();
    }

    public void SetNewCart(List<CartItem> newCart) {
        cart = newCart;
        UpdateTotalAndItemCount();
    }
}
