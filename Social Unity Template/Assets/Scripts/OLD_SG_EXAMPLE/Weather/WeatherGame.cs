using UnityEngine;
using TMPro;
using System;
using System.Collections;
using Newtonsoft.Json;

public class WeatherGame : MonoBehaviour
{
  public TMP_Text weatherField;
  public float desiredAccuracyInMeters = 500f;
  public float updateDistanceInMeters = 500f;
  public int serviceInitWaitTimeInSeconds = 15;
  public string weatherUrl = "api.openweathermap.org/data/2.5/weather?lat={0}&lon={1}&appid={2}&units={3}";
  public string weatherApiKey = "PASTE_KEY_HERE";
  public string weatherUnits = "metric";

  float latitude = 0f;
  float longitude = 0f;

  string locationName;
  string weatherMain;
  string weatherDescription;
  double tempAvg;
  double tempFeelsLike;
  double tempMin;
  double tempMax;
  double pressure;
  double humidity;
  double windSpeed;


  void Start()
  {
#if UNITY_EDITOR
    // Mock location for in-editor testing (location: Garching FZ)
    latitude = 48.2648f;
    longitude = 11.6713f;
    StartCoroutine(WeatherCoroutine());
#else
		StartCoroutine(LocationCoroutine());
#endif
  }

  void Render()
  {
    string weatherText = "{0}:\n{1}\nTemperature:\n{2}°C - {3}°C\n(feels like {4}°C)\nPressure: {5} hPa\nHumidity: {6}%\nWind Speed: {7} m/s";
    weatherText = String.Format(weatherText, locationName, weatherDescription, tempMin, tempMax, tempFeelsLike, pressure, humidity, windSpeed);
    weatherText += "\n\n" + GetAdditionalText();
    weatherField.text = weatherText;
  }
  string GetAdditionalText()
  {
    if (weatherMain.Equals("Thunderstorm"))
    {
      return "It's thundering! Your crops are in danger! But lightning spells can't miss.";
    }
    if (weatherMain.Equals("Drizzle"))
    {
      return "Your crops are enjoying a light drizzle. Fires are put out quicker";
    }
    if (weatherMain.Equals("Rain"))
    {
      return "It’s raining, so your cabbages are growing faster and your fire spells do less damage!";
    }
    if (weatherMain.Equals("Snow"))
    {
      return "The snow freezes the growth of all crops. Mana costs of ice spells are halved.";
    }
    if (weatherMain.Equals("Atmosphere"))
    {
      return "You can hardly see through this thick atmosphere. Your accuracy is halved.";
    }
    if (tempFeelsLike >= 35)
    {
      return "Some of your crops can't stand this blistering heat! Also, no ice spells can be cast.";
    }
    return "There are no special weather effects.";
  }

  void ParseJsonWeather(string jsonString)
  {
    WeatherData data = JsonConvert.DeserializeObject<WeatherData>(jsonString);
    locationName = data.Name;
    weatherMain = data.Weather[0].Main;
    weatherDescription = data.Weather[0].Description;
    windSpeed = data.Wind.Speed;

    Main main = data.Main;
    tempAvg = main.Temp;
    tempFeelsLike = main.FeelsLike;
    tempMin = main.TempMin;
    tempMax = main.TempMax;
    pressure = main.Pressure;
    humidity = main.Humidity;
  }

  IEnumerator WeatherCoroutine()
  {
    string fullUrl = String.Format(weatherUrl, latitude, longitude, weatherApiKey, weatherUnits);
    using (WWW www = new WWW(fullUrl))
    {
      yield return www;
      Debug.Log(www.text);
      ParseJsonWeather(www.text);
      Render();
    }
  }

  IEnumerator LocationCoroutine()
  {
#if UNITY_ANDROID
    if (!UnityEngine.Android.Permission.HasUserAuthorizedPermission(UnityEngine.Android.Permission.CoarseLocation))
    {
      UnityEngine.Android.Permission.RequestUserPermission(UnityEngine.Android.Permission.CoarseLocation);
    }

    // First, check if user has location service enabled
    if (!UnityEngine.Input.location.isEnabledByUser)
    {
      Debug.LogFormat("Android and Location not enabled");
      yield break;
    }

#elif UNITY_IOS
		if (!UnityEngine.Input.location.isEnabledByUser) 
		{
			Debug.LogFormat("IOS and Location not enabled");
			yield break;
		}
#endif
    // Start service before querying location
    Input.location.Start(desiredAccuracyInMeters, updateDistanceInMeters);

    // Wait until service initializes
    int waitTime = serviceInitWaitTimeInSeconds;
    while (UnityEngine.Input.location.status == LocationServiceStatus.Initializing && waitTime > 0)
    {
      yield return new WaitForSecondsRealtime(1);
      waitTime--;
    }

    // Service didn't initialize in time
    if (waitTime < 1)
    {
      Debug.LogFormat("Timed out");
      yield break;
    }

    // Connection has failed
    if (UnityEngine.Input.location.status != LocationServiceStatus.Running)
    {
      Debug.LogFormat("Unable to determine device location. Failed with status {0}", UnityEngine.Input.location.status);
      yield break;
    }
    else
    {
      Debug.LogFormat("Location service live. status {0}", UnityEngine.Input.location.status);
      // Access granted and location value could be retrieved
      Debug.LogFormat("Location: "
        + UnityEngine.Input.location.lastData.latitude + " "
        + UnityEngine.Input.location.lastData.longitude + " "
        + UnityEngine.Input.location.lastData.altitude + " "
        + UnityEngine.Input.location.lastData.horizontalAccuracy + " "
        + UnityEngine.Input.location.lastData.timestamp);

      latitude = UnityEngine.Input.location.lastData.latitude;
      longitude = UnityEngine.Input.location.lastData.longitude;
      StartCoroutine(WeatherCoroutine());
    }

    // Stop service since there is no need to query location updates continuously
    UnityEngine.Input.location.Stop();
  }
}