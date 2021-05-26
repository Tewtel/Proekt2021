// Decompiled with JetBrains decompiler
// Type: AvitoAvtoRinger.VKWorker
// Assembly: AvtoRinger, Version=4.5.2.9, Culture=neutral, PublicKeyToken=null
// MVID: AF014D39-BDE2-4E75-A1F7-337ACC25C77C
// Assembly location: D:\labs\2semestr\ProektObnovaCode\AvitoRinger\AvtoRinger.exe

using System;
using System.Diagnostics;
using System.Net;
using System.Text;

namespace AvitoAvtoRinger
{
  public static class VKWorker
  {
    public static WebClient Client = new WebClient();

    public static void TokenGet() => Process.Start("https://oauth.vk.com/authorize?client_id=3960012&scope=69637&redirect_uri=https://oauth.vk.com/blank.html&display=page&v=5.65&response_type=token");

    public static string SendVkMessage(string chatID, string message, string token)
    {
      try
      {
        return VKWorker.Client.DownloadString("https://api.vk.com/method/messages.send?chat_id=" + chatID + "&message=" + message + "&access_token=" + token);
      }
      catch (Exception ex)
      {
        return ex.ToString();
      }
    }

    public static string SendVkMessage(
      string chatID,
      string message,
      string attachment,
      string token)
    {
      try
      {
        return VKWorker.Client.DownloadString("https://api.vk.com/method/messages.send?chat_id=" + chatID + "&message=" + message + "&attachment=photo" + attachment + "&access_token=" + token);
      }
      catch (Exception ex)
      {
        return ex.ToString();
      }
    }

    public static string GetPhotoServer(string token)
    {
      try
      {
        return VKWorker.Client.DownloadString("https://api.vk.com/method/photos.getMessagesUploadServer?access_token=" + token + "&v=5.65");
      }
      catch (Exception ex)
      {
        return ex.ToString();
      }
    }

    public static string SavePhoto(string server, string photo, string hash, string token)
    {
      try
      {
        return VKWorker.Client.DownloadString("https://api.vk.com/method/photos.saveMessagesPhoto?server=" + server + "&photo=" + photo + "&hash=" + hash + "&access_token=" + token + "&v=5.65");
      }
      catch (Exception ex)
      {
        return ex.ToString();
      }
    }

    public static string GetUserAvatar(string token)
    {
      try
      {
        return Encoding.UTF8.GetString(Encoding.Default.GetBytes(VKWorker.Client.DownloadString("https://api.vk.com/method/users.get?fields=photo_50&access_token=" + token + "&v=5.67")));
      }
      catch (Exception ex)
      {
        return ex.ToString();
      }
    }
  }
}
