using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BalanceManager : MonoBehaviour {
    [SerializeField] private Text remainingText;
    [SerializeField] private Text balanceText;
    [SerializeField] private CartManager cartManager;
    [SerializeField] private DeliveryManager deliveryManager;

    private double balanceAmount = 100000;
    private double totalCart = 0;
    private List<CartItem> cart = new List<CartItem>();

    public void PlaceOrder() {
        if (cart.Count > 0 && balanceAmount - totalCart >= 0 && totalCart != 0 && deliveryManager.GetCountAvailableSlots() >= cart.Count) {
            Debug.Log("order placed");
            for (int i = 0; i < cart.Count; i++) {
                CartItem cartItem = cart[i];
                Debug.Log("item placed: " + cartItem.product.name.ToString());
                for (int j = 0; j < cartItem.quantity; j++) {
                    
                }

            }
            double newBalance = balanceAmount - totalCart;
            UpdateBalance(newBalance);
            deliveryManager.StartDeliveryProcess(cart);

            cartManager.ClearCart();
        }
    }

    public void SetTotal(double total, List<CartItem> currentCart) {
        totalCart = total;
        if (total > 0) {
            totalCart += 8f;
        }
        cart = currentCart;

        remainingText.text = (balanceAmount - totalCart).ToString();
        balanceText.text = balanceAmount.ToString();
    }

    private void UpdateBalance(double newBalance) {
        balanceAmount = newBalance;
        remainingText.text = newBalance.ToString();
        balanceText.text = newBalance.ToString();
    }
}
