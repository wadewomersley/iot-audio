# IOT audio

[![N|Solid](https://d353t7rt0g6j4y.cloudfront.net/blog/wp-content/uploads/2015/02/200px.png)](https://wade.one/)

I was going to develop this more before releasing it but it's so simple right now I've decided to release it now and build up as it works perfectly on a Pi 2 with Windows IOT and integrates fine with a Google Home device using IFTTT.

# Requirements
 - Visual Studio 2017
 - Raspberry Pi with Windows IOT installed.

# Getting Started

  - Build to your IOT Pi using Visual Studio
  - Use Windows File Explorer and place MP3's in `\\PI_IP_ADDRESS\c$\Data\Users\DefaultAccount\Music`. Your login with be the same as accessing the PI from the web UI.
  - Connect on port 16000.
  - You'll see an info bar with your API key, keep it and access the API in the future with `http://IP_OR_HOSTNAME:16000/?APIKEY`
  - Optional: Set the app as the default start up app in the IOT web UI.


You can also:
  - Publish port 16000 publicly and use the API remotely to send commands to update music.
  
# Play requests using IFTTT Web Requests and Google Home
{"FileName":"{{TextField}}.mp3","apiKey":"APIKEYHERE"}
- e.g. set up a hook with: `If You say "change ambient music to $", then make a web request` and PUT to `http://IP_OR_HOSTNAME:16000/api/play` with the above JSON.