/**
 * GoogleIABListener.cs
 * 
 * GoogleIABListener listens to the Google In-App Billing events.
 * File location: Assets/Scripts/NeatPlug/IAP/GoogleIAB/GoogleIABListener.cs
 * 
 * Please read the code comments carefully, or visit 
 * http://www.neatplug.com/integration-guide-unity3d-google-iab-plugin to find information 
 * about how to integrate and use this program.
 * 
 * End User License Agreement: http://www.neatplug.com/eula
 * 
 * (c) Copyright 2012 NeatPlug.com All Rights Reserved.
 * 
 * @version 1.4.8
 * @iab_api v3 
 *
 */

using UnityEngine;
using System.Collections;

public class GoogleIABListener : MonoBehaviour {
	
	// Debug information printing switch, turn it off in production environment.
	private bool _debug = true;
	
	private static bool _instanceFound = false;

	void Awake()
	{
		// Do not modify the codes below.
		if (_instanceFound)
		{
			Destroy(gameObject);
			return;
		}
		_instanceFound = true;		
		DontDestroyOnLoad(this);
		GoogleIAB.Instance();		
	}
	
	void OnEnable()
	{
		// Register the IAP events.
		// Do not modify the codes below.		
		GoogleIABAgent.OnBillingSupported += OnBillingSupported;
		GoogleIABAgent.OnSubscriptionSupported += OnSubscriptionSupported;		
		GoogleIABAgent.OnItemDataReady += OnItemDataReady;		
		GoogleIABAgent.OnItemDataFailed += OnItemDataFailed;
		GoogleIABAgent.OnOwnedItemReported += OnOwnedItemReported;		
		GoogleIABAgent.OnPurchaseCompleted += OnPurchaseCompleted;
		GoogleIABAgent.OnPurchaseFailed += OnPurchaseFailed;
		GoogleIABAgent.OnPurchaseCancelled += OnPurchaseCancelled;		
		GoogleIABAgent.OnItemAlreadyOwned += OnItemAlreadyOwned;
		GoogleIABAgent.OnPurchaseCancelledByGoogle += OnPurchaseCancelledByGoogle;
		GoogleIABAgent.OnItemConsumed += OnItemConsumed;
		GoogleIABAgent.OnItemFailedToConsume += OnItemFailedToConsume;
		GoogleIABAgent.OnReceiptPosted += OnReceiptPosted;
		GoogleIABAgent.OnFailedToPostReceipt += OnFailedToPostReceipt;
	}

	void OnDisable()
	{
		// Unregister the IAP events.
		// Do not modify the codes below.		
		GoogleIABAgent.OnBillingSupported -= OnBillingSupported;
		GoogleIABAgent.OnSubscriptionSupported -= OnSubscriptionSupported;		
		GoogleIABAgent.OnItemDataReady -= OnItemDataReady;		
		GoogleIABAgent.OnItemDataFailed -= OnItemDataFailed;
		GoogleIABAgent.OnOwnedItemReported -= OnOwnedItemReported;		
		GoogleIABAgent.OnPurchaseCompleted -= OnPurchaseCompleted;
		GoogleIABAgent.OnPurchaseFailed -= OnPurchaseFailed;
		GoogleIABAgent.OnPurchaseCancelled -= OnPurchaseCancelled;		
		GoogleIABAgent.OnItemAlreadyOwned -= OnItemAlreadyOwned;
		GoogleIABAgent.OnPurchaseCancelledByGoogle -= OnPurchaseCancelledByGoogle;
		GoogleIABAgent.OnItemConsumed -= OnItemConsumed;
		GoogleIABAgent.OnItemFailedToConsume -= OnItemFailedToConsume;
		GoogleIABAgent.OnReceiptPosted -= OnReceiptPosted;
		GoogleIABAgent.OnFailedToPostReceipt -= OnFailedToPostReceipt;
	}
	
	/**
	 * Fired when the check for the In-App Billing support is done.
	 * 
	 * By default, the plugin will check if In-App Billing is supported on current
	 * device as soon as App launches. There are a few cases the check may return false:
	 * The version of Google Play Software installed on the device is too old, or the
	 * user is using the device in the country where In-App Billing is not supported.
	 * For other possible cases, please refer to Google android developer site.
	 * 
	 * @param supported
	 *             bool - true for supported, false for unsupported.
	 * 
	 * @param response
	 *             string - The response code returned from Google IAB API.
	 * 
	 * The response codes are listed here:
	 * ##################################################################################################	
	 * BILLING_RESPONSE_RESULT_OK                   0   Success
	 * 
	 * BILLING_RESPONSE_RESULT_USER_CANCELED        1   User pressed back or canceled a dialog
	 * 
	 * BILLING_RESPONSE_RESULT_BILLING_UNAVAILABLE  3   Billing API version is not supported for 
	 *                                                  the type requested
	 * 
	 * BILLING_RESPONSE_RESULT_ITEM_UNAVAILABLE     4   Requested product is not available for purchase
	 * 
	 * BILLING_RESPONSE_RESULT_DEVELOPER_ERROR      5   Invalid arguments provided to the API. This error 
	 *                                                  can also indicate that the application was not 
	 *                                                  correctly signed or properly set up for In-app 
	 *                                                  Billing in Google Play, or does not have the 
	 *                                                  necessary permissions in its manifest
	 * 
	 * BILLING_RESPONSE_RESULT_ERROR                6   Fatal error during the API action
	 * 
	 * BILLING_RESPONSE_RESULT_ITEM_ALREADY_OWNED   7   Failure to purchase since item is already owned
	 * 
	 * BILLING_RESPONSE_RESULT_ITEM_NOT_OWNED       8   Failure to consume since item is not owned
	 * ###################################################################################################	
	 */
	void OnBillingSupported(bool supported, string response)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnBillingSupported: supported -> " + supported.ToString() + ", response -> " + response);
		
		/// Your code here...
	}
		
	/**
	 * Fired when the check for subcription support is done.	
	 *
	 * @param supported
	 *             bool - true for supported, false for unsupported.
	 * 
	 * @param response
	 *             string - The response code returned from Google IAB API.
	 */
	void OnSubscriptionSupported(bool supported, string response)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnSubscriptionSupported: supported -> " + supported.ToString() + ", response -> " + response);	
		
		/// Your code here...
	}
	
	/**
	 * Fired when the item data is ready to query.
	 * Do your item query then if you need.
	 */
	void OnItemDataReady()
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnItemDataReady() Fired.");
		
		/// Your code here... (Sample codes provided below)
		
		/*************************************************************
		  BEGIN CODE SAMPLE : Sample of getting item information.
		 *************************************************************
		 
		// Get item info:
		GoogleIABItem iabItem = GoogleIAB.Instance().GetItemInfo("your_item_sku");		
		Debug.Log ("iabItem.description: " + iabItem.description);
		Debug.Log ("iabItem.price: " + iabItem.price);
		Debug.Log ("iabItem.currency: " + iabItem.currency);
		Debug.Log ("iabItem.title: " + iabItem.title);
		Debug.Log ("iabItem.type: " + iabItem.type);			
		
		// Get item price:
		float itemPrice = GoogleIAB.Instance().GetItemPrice("your_item_sku");
		Debug.Log ("itemPrice: " + itemPrice);		
		
		*************************************************************
		 END CODE SAMPLE : Sample of getting item information.
		*************************************************************/
	}	
	
	/**
	 * Fired when failed to query item data.
	 * 
	 * @param err
	 *         string - The error code returned.
	 */
	void OnItemDataFailed(string err)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnItemDataFailed Fired. Err -> " + err);	
		
		/// Your code here...
	}
	
	/**
	 * Fired when receiving an owned item report event.
	 * 
	 * This indicates that the item type is "NonConsumable" and the user has already 
	 * owned the item. By default the plugin gets notified with the event every time your 
	 * app launches, it is suggested that you should redeliver the item to the user here 
	 * if the locally saved data record cannot be found. (Probably the user cleared the 
	 * PlayerPrefs data or a new device is being used) 
	 * 
	 * @param sku
	 *           string - IAP item identifier, the sku you defined at Google Play's publisher site.	
	 */	
	void OnOwnedItemReported(string sku)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnOwnedItemReported(" + sku + ") Fired.");
		
		/// This is the best place to re-deliver the NonConsumable item if you cannot 
		/// find the locally saved data records saying the item has been delivered.	
		
		/// Your code here...
	}		
	
	/**
	 * Fired when the purchase successfully completed.	
	 * 
	 * This is where you should deliver the item to the user.
	 * 
	 * @param receipt
	 *           GoogleIABReceipt - An object which contains the purchase information. 
	 *                              { sku, purchaseTime, orderId, purchaseToken,
	 *                                purchaseState, packageName, developerPayload }
	 */
	void OnPurchaseCompleted(GoogleIABReceipt receipt)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnPurchaseCompleted Fired. { \n" 
				+ "  sku -> " + receipt.sku + ", \n" 
				+ "  purchaseTime -> " + receipt.purchaseTime.ToString() + ", \n"		
				+ "  orderId -> " + receipt.orderId + ", \n"
			    + "  purchaseToken -> " + receipt.purchaseToken + ", \n"
			    + "  purchaseState -> " + receipt.purchaseState + ", \n"
			    + "  packageName -> " + receipt.packageName + ", \n"
			    + "  originalJson -> " + receipt.originalJson + ", \n"       
				+ "  developerPayload -> " + receipt.developerPayload + ", \n"        
				+ "  data -> " + receipt.data + "\n"
			    + "}\n"
			);
		
		/// Your code here...
	}	
	
	/**
	 * Fired when the purchase failed.
	 * 		 
	 * @param sku
	 *           string - IAP item identifier, the Product ID you defined at Google Play's publisher site.	
	 * 
	 * @param err
	 *           string - The reason for failure.
	 */	
	void OnPurchaseFailed(string sku, string err)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnPurchaseFailed Fired. sku -> " + sku + ", err -> " + err);	
		
		/// Your code here...		
	}	
	
	/**
	 * Fired when the purchase cancelled by the user.
	 * 		 
	 * @param sku
	 *           string - IAP item identifier, the Product ID you defined at Google Play's publisher site.
	 */	
	void OnPurchaseCancelled(string sku)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnPurchaseCancelled Fired. sku -> " + sku);	
		
		/// Your code here...			
	}
	
	/**
	 * Fired if the user has already owned this NonConsumable item when a corresponding
	 * purchase is attempted.
	 *
	 * This indicates that the item type is "NonConsumable" and the user has already owned
	 * the item. This event is only triggered in case you ignored the default automatic
	 * owned item reporting happened in OnOwnedItemReported() at app launches, but 
	 * you are not suggested to do so since requiring the user who has already purchased 
	 * the NonConsumable item to perform the purchase again, and tell the user "You have already
	 * owned the item", is obviously causing confusion.
	 *
	 * In most cases you should only play with the "OnOwnedItemReported()" event.
	 * But you can use this where you really need it to be that way.
	 *
	 * @param sku
	 *           string - IAP item identifier, the sku you defined at Google Play's publisher site.	
	 */	
	void OnItemAlreadyOwned(string sku)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnItemAlreadyOwned(" + sku + ") Fired.");		
		
		/// Your code here...
	}	
	
	/**
	 * Fired when the purchase cancelled by Google.
	 * 	
	 * The cancellation is primarily caused by user's credit card validation failure.
	 * 
	 * @param sku
	 *           string - IAP item identifier, the sku you defined at Google Play's publisher site.
	 */		
	void OnPurchaseCancelledByGoogle(string sku)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnPurchaseCancelledByGoogle: sku -> " + sku);	
		
		/// Your code here...			
	}		
	
	/**
	 * Fired when the purchased item successfully consumed.
	 * 
	 * @param sku
	 *           string - IAP item identifier, the sku you defined at Google Play's publisher site.	
	 * 	
	 */	
	void OnItemConsumed(string sku)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnItemConsumed(" + sku + ") Fired.");		
		
		/// Your code here...
	}
	
	/**
	 * Fired when the purchased item failed to be consumed.
	 * 
	 * @param sku
	 *           string - IAP item identifier, the sku you defined at Google Play's publisher site.	
	 * 	 
	 */
	void OnItemFailedToConsume(string sku)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnItemFailedToConsume(" + sku + ") Fired.");		
		
		/// Your code here...
	}
	
	/**
	 * Fired when the receipt is successfully posted to server.
	 * 
	 * @param response
	 *           string - The response from your server.
	 * 	
	 */	
	void OnReceiptPosted(string response)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnReceiptPosted Fired. Server Response: " + response);		
		
		/// Your code here...
	}
	
	/**
	 * Fired when the receipt data failed to be posted.
	 * 
	 * @param err
	 *           string - The error string.	
	 * 	 
	 */
	void OnFailedToPostReceipt(string err)
	{
		if (_debug)
			Debug.Log (this.GetType().ToString() + " - OnFailedToPostReceipt Fired. Err: " + err);		
		
		/// Your code here...
	}	
	
}
