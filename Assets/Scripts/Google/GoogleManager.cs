
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;

public class GoogleManager : MonoBehaviour
{
    
    public Text LogText;

    private void Start()
    {
        var config = new PlayGamesClientConfiguration.Builder().EnableSavedGames().Build();
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        Login();
    }
    
    #region ���� �α���

    public void Login()
    {
        Social.localUser.Authenticate((bool success) =>
        {
            if (success) LogText.text = Social.localUser.id + " \n " + Social.localUser.userName;
            else LogText.text = "Google Login Failed !!";
        });
    }

    public void Logout()
    {
        ((PlayGamesPlatform)Social.Active).SignOut();
        LogText.text = "Google Logout";
    }
    #endregion

    #region Ŭ���� ����

    ISavedGameClient SavedGame()
    {
        return PlayGamesPlatform.Instance.SavedGame;
    }

    public void LoadCloud()
    {
        SavedGame().OpenWithAutomaticConflictResolution("mysave",
            DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLastKnownGood, LoadGame);
    }
    void LoadGame(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
            SavedGame().ReadBinaryData(game, LoadData);
    }
    void LoadData(SavedGameRequestStatus status, byte[] LoadedData)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            string data = System.Text.Encoding.UTF8.GetString(LoadedData);
            LogText.text = data;
        }
        else LogText.text = "Load Failed !";
    }



    public void SaveCloud()
    {
        SavedGame().OpenWithAutomaticConflictResolution("mysave",
            DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLastKnownGood, SaveGame);
    }

    public void SaveGame(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            var update = new SavedGameMetadataUpdate.Builder().Build();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes("ň�K�K");
            SavedGame().CommitUpdate(game, update, bytes, SaveData);
        }
    }
    void SaveData(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            LogText.text = "���� ����";
        }
        else LogText.text = "���� ����";
    }



    public void DeleteCloud()
    {
        SavedGame().OpenWithAutomaticConflictResolution("mysave",
            DataSource.ReadCacheOrNetwork, ConflictResolutionStrategy.UseLongestPlaytime, DeleteGame);
    }

    void DeleteGame(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            SavedGame().Delete(game);
            LogText.text = "���� ����";
        }
        else LogText.text = "���� ����";
    }
    #endregion
}
