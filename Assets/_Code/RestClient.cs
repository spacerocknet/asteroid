using UnityEngine;
using System.Collections;
using System.Net;
using System.IO;
using System.Threading;
using System;
using SimpleJSON;


public class RestClient : MonoBehaviour 
{
    public string result;

	public RestClient ()
	{
	}

	public string GetGameConfig() {
		HTTP.Request request = new HTTP.Request( "get", "http://m1.spacerock.net:9000/v1/quiz/config" );
		request.synchronous = true;
		request.Send();
		
		while( !request.isDone )
		{
			Thread.Sleep(300);
		}

		//var json = SimpleJSON.JSON.Parse(request.response.Text);
		//Debug.Log (json["categories"][0]);
		
		return request.response.Text;
	}

	public string GetCategories() {
		HTTP.Request request = new HTTP.Request( "get", "http://m1.spacerock.net:9000/v1/quiz/categories" );
		request.synchronous = true;
		request.Send();
		
		while( !request.isDone )
		{
			Thread.Sleep(300);
		}

		// parse some JSON, for example:
		//JSONObject thing = new JSONObject( request.response.Text );
		//Debug.Log (someRequest.response.Text);

		return request.response.Text;
	}

	public string GetQuizzes(string category, int num, Action<string> callback) {
		Hashtable data = new Hashtable();
		data.Add( "userId", "1000abc" );
		data.Add( "category", category );
		data.Add( "num", num );
		
		HTTP.Request theRequest = new HTTP.Request("post", "http://m1.spacerock.net:9000/v1/quiz/request", data );
		theRequest.synchronous = true;
		//theRequest.Send((request) => {
		//	                          Debug.Log (request.response.Text);
		//	                          callback(request.response.Text);
		//                              });

		theRequest.Send ();
		callback(theRequest.response.Text);
		return theRequest.response.Text;

		/*
		theRequest.Send ();

		while( !theRequest.isDone )
		{
			Thread.Sleep(100);
		}
		
		//return theRequest.response.Text;
		var json = SimpleJSON.JSON.Parse(theRequest.response.Text);
		Debug.Log ("categories:::::::::::::::::::" + json["categories"][0]);
		return json;
		*/
	}

	public IEnumerator SomeRoutine() {
		HTTP.Request someRequest = new HTTP.Request( "get", "http://m1.spacerock.net:9000/v1/quiz/categories" );
		someRequest.Send();
		
		while( !someRequest.isDone )
		{
			yield return null;
		}
		
		// parse some JSON, for example:
		//JSONObject thing = new JSONObject( request.response.Text );
		Debug.Log (someRequest.response.Text);
	}

	public IEnumerator DownloadCategories(Action<string> callback)
	{
			// Pull down the JSON from our web-service
			
		    WWW w = new WWW("http://m1.spacerock.net:9000/v1/quiz/categories");
		    yield return null;
		    //yield return w;
			
			//print("Waiting for sphere definitions\n");
			
			// Add a wait to make sure we have the definitions
			
			yield return new WaitForSeconds(1f);
			
			//print("Received sphere definitions\n");
			
			// Extract the spheres from our JSON results
			
			//Debug.Log (w.text);
		    callback (w.text);
		    yield break;
		    
		}

	/*
	    public IEnumerator loadAsset( string url )
	    {
		       WWW www = new WWW( url );
		       float elapsedTime = 0.0f;
		
		       while (!www.isDone) {
			      elapsedTime += Time.deltaTime;
			      if (elapsedTime >= 10.0f) break;
			         yield return null;  
		       }
		
		       if (!www.isDone || !string.IsNullOrEmpty(www.error)) {
			       Debug.LogError("Load Failed");
			       yield break;
		       }
		
		       // load successful
		       result = www.text;
		       return www.text;
	    }
	    */
	
}

