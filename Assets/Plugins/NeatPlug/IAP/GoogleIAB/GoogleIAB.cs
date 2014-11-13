/**
 * GoogleIAB.cs
 * 
 * A Singleton class encapsulating public access methods for Google IAB processes. 
 * 
 * Please read the code comments carefully, or visit 
 * http://www.neatplug.com/integration-guide-unity3d-google-iab-plugin to find information how 
 * to use this program.
 * 
 * End User License Agreement: http://www.neatplug.com/eula
 * 
 * (c) Copyright 2012 NeatPlug.com All rights reserved.
 * 
 * @version 1.4.8
 * @iab_api v3
 *
 */

using UnityEngine;
using System;
using System.Collections;
using System.Globalization;

public class GoogleIAB  {
	
	#region Enums
	
	public enum PurchaseType
	{
		Consumable_AutoConsume = 0,
		Consumable_ManuallyConsume,
		NonConsumable,
		Subscription		
	};	
	
	#endregion
	
	#region Fields
	
	private static GoogleIAB _instance = null;	
	
	private class GoogleIABNativeHelper : IGoogleIABNativeHelper {
		
#if UNITY_ANDROID	
		private AndroidJavaObject _plugin = null;
#endif		
		public GoogleIABNativeHelper()
		{
			
		}
		
		public void CreateInstance(string className, string instanceMethod)
		{	
#if UNITY_ANDROID			
			AndroidJavaClass jClass = new AndroidJavaClass(className);
			_plugin = jClass.CallStatic<AndroidJavaObject>(instanceMethod);	
#endif			
		}
		
		public void Call(string methodName, params object[] args)
		{
#if UNITY_ANDROID			
			_plugin.Call(methodName, args);	
#endif
		}
		
		public void Call(string methodName, string signature, object arg)
		{
#if UNITY_ANDROID			
			var method = AndroidJNI.GetMethodID(_plugin.GetRawClass(), methodName, signature);			
			AndroidJNI.CallObjectMethod(_plugin.GetRawObject(), method, AndroidJNIHelper.CreateJNIArgArray(new object[] {arg}));
#endif			
		}
		
		public ReturnType Call<ReturnType> (string methodName, params object[] args)
		{
#if UNITY_ANDROID			
			return _plugin.Call<ReturnType>(methodName, args);
#else
			return default(ReturnType);			
#endif			
		}
	
	};		
	
	#endregion
	
	#region Functions	
	
	/**
	 * Constructor.
	 */
	private GoogleIAB()
	{	
#if UNITY_ANDROID		
		GoogleIABAndroid.Instance().SetNativeHelper(new GoogleIABNativeHelper());
#endif
	}
	
	/**
	 * Instance method.
	 */
	public static GoogleIAB Instance()
	{		
		if (_instance == null) 
		{
			_instance = new GoogleIAB();
		}
		
		return _instance;
	}	
	
	/**
	 * Initialization.
	 * 
	 * @param skusConsumable
	 *              string[] - A set of Consumable product IDs you want the plugin to automatically 
	 *                         retrieve information for you.
	 * 
	 * @param skusNonConsumable
	 *              string[] - A set of Non-consumable product IDs you want the plugin to automatically 
	 *                         retrieve information for you.
	 * 
	 * @param skusSubscription
	 *              string[] - A set of Subscription product IDs you want the plugin to automatically 
	 *                         retrieve information for you.
	 * 
	 * @param serverReceiveReceiptURL
	 *              string - Setting a valid URL on your own server enables posting the
	 *                       StoreKit Receipt to your server, for further server-to-server 
	 *                       verification and other server-side processes you need.
	 *                       e.g. creating subscirption entries, preparing downloadable 
	 *                       content, etc...
	 * 
	 */
	public void Initialize(string publicKey, string[] skusConsumable, string[] skusNonConsumable, 
	                       string[] skusSubscription, string serverReceiveReceiptURL)
	{
#if UNITY_ANDROID		
		GoogleIABAndroid.Instance().Initialize(publicKey, skusConsumable, skusNonConsumable, 
		                                       skusSubscription, serverReceiveReceiptURL);
#endif		
	}	
	
	/**
	 * Initiate an in-app purchase request to the plugin.
	 * 
	 * @param sku
	 *           string - IAP item identifier, the Product ID you defined at Google Play's publisher site.
	 * 	
	 * 	
	 */
	public void Purchase(string sku)
	{
#if UNITY_ANDROID		
		GoogleIABAndroid.Instance().Purchase(sku, null);
#endif			
	}		
	
	/**
	 * Initiate an in-app purchase request to the plugin.
	 * 
	 * @param sku
	 *           string - IAP item identifier, the Product ID you defined at Google Play's publisher site.
	 * 
	 * @param type
	 *           PurchaseType - the purchase type.
	 * 	
	 */
	public void Purchase(string sku, PurchaseType type)
	{
#if UNITY_ANDROID		
		GoogleIABAndroid.Instance().Purchase(sku, (int)type, null);
#endif			
	}		
	
	/**
	 * Initiate an in-app purchase request to the plugin.
	 * 
	 * @param sku
	 *           string - IAP item identifier, the Product ID you defined at Google Play's publisher site.
	 * 	
	 * 
	 * @param payload
	 *          string -  a developer payload that is associated with a given purchase, 
	 *          if null, no payload is sent.Developer Payload is a developer-specified 
	 *          string that contains supplemental information about a purchase. 	 
	 */
	public void Purchase(string sku, string payload)
	{
#if UNITY_ANDROID		
		GoogleIABAndroid.Instance().Purchase(sku, payload);
#endif			
	}	
	
	/**
	 * Initiate an in-app purchase request to the plugin.
	 * 
	 * @param sku
	 *           string - IAP item identifier, the Product ID you defined at Google Play's publisher site.
	 * 
	 * @param type
	 *           PurchaseType - the purchase type.
     *                          {Consumable_AutoConsume, Consumable_ManuallyConsume, NonConsumable}
     *                          This parameter is used to force the item purchase type to be the specified value
     *                          regardless of settings in GoogleIABAgent properties: Consumable Product Skus and
     *                          Nonconsumable Product Skus. 
	 * 
	 * @param payload
	 *          string -  a developer payload that is associated with a given purchase, 
	 *          if null, no payload is sent.Developer Payload is a developer-specified 
	 *          string that contains supplemental information about a purchase. 	 
	 */
	public void Purchase(string sku, PurchaseType type, string payload)
	{
#if UNITY_ANDROID		
		GoogleIABAndroid.Instance().Purchase(sku, (int)type, payload);
#endif			
	}
	
	/**
	 * Get the specified item information.
	 * 
	 * The item information is retrieved at app launch and it is cached in plugin for better performance.
	 * You should always call this function in GoogleIABListener::OnItemDataReady() to make sure the data
	 * has been ready for you to query.
	 * 
	 * @param sku
	 * 			string - IAP item identifier, the sku you defined at Google Play's publisher site.
	 * 
	 * @return 
	 * 			GoogleIABItem - A Google IAB Item which contains { title, description, price, type }
	 */
	public GoogleIABItem GetItemInfo(string sku)
	{
		GoogleIABItem item = null;		
		
#if UNITY_ANDROID		
		item = GoogleIABAndroid.Instance().GetItemInfo(sku);
#endif
		
		return item;
	}	
	
	/**
	 * Get the price of specified item.
	 * 
	 * The item information is retrieved at app launch and it is cached in plugin for better performance.
	 * You should always call this function in GoogleIABListener::OnItemDataReady() to make sure the data
	 * has been ready for you to query.
	 * 
	 * @param sku
	 * 			string - IAP item identifier, the sku you defined at Google Play's publisher site.
	 * 
	 * @return 
	 * 			float - The price of the item
	 */	
	public float GetItemPrice(string sku)
	{		
		float price = 0.0f;
		
#if UNITY_ANDROID		
		price = GoogleIABAndroid.Instance().GetItemPrice(sku);
#endif
		
		return price;		
	}	
	
	/**
	 * Get the purchase information if there is an owned item (An item owned and not yet consumed).
	 * 
	 * Return null if there isn't an owned item with the sku specified.
	 * 
	 * @param sku
	 * 			string - IAP item identifier, the sku you defined at Google Play's publisher site.
	 * 
	 * @return 
	 * 			GoogleIABPurchaseInfo - A Google IAB purchae info obj which contains 
	 *                 {sku, orderId, purchaseToken, purchaseState, purchaseTime, developerPayload}
	 * 
	 */
	public GoogleIABPurchaseInfo GetPurchaseInfo(string sku)
	{
		GoogleIABPurchaseInfo purhcaseInfo = null;
		
		#if UNITY_ANDROID		
		purhcaseInfo = GoogleIABAndroid.Instance().GetPurchaseInfo(sku);
		#endif
		
		return purhcaseInfo;
	}
	
	/**
	 * Consume a product.
	 * 
	 * This provides flexibility for consuming a Consumable_ManuallyConsume product.
	 * 
	 * @param sku
	 *           string - IAP item identifier, the Product ID you defined at Google Play's publisher site.
	 * 			 
	 */
	public void Consume(string sku)
	{
#if UNITY_ANDROID		
		GoogleIABAndroid.Instance().Consume(sku);
#endif			
	}	
	
	#endregion
}
