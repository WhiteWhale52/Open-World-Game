using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using CustomExtensions;
using System.Linq;

public class GameBehavior : MonoBehaviour,IManager
{
    public int MaxItems = 3;

    public TMP_Text HealthText;
    public TMP_Text ItemText;
    public TMP_Text ProgressText;
    [SerializeField] int _playerHP = 10;
    public Button WinButton;
    public Button LossButton;
    [SerializeField] Slider PlayerHealth;
    [SerializeField] Gradient PlayerProgress;
    [SerializeField] Image Fill;
    private string _State;
    public string State
    {
        get { return _State; }
        set { _State = value; }
    }
    
    private int _itemsCollected = 0;

    void Start()
    {
        ItemText.text += _itemsCollected;
        Fill.color = new Color(0.2149121f, 0.8018868f, 0.02017319f);
        Initialize();
        //HealthText.text += _playerHP;
    }
    public Stack<Loot> LootStack = new Stack<Loot>();

    public void Initialize()
    {
        _State = "Game Manager Initialized..";
       // Debug.Log(_State);
        _State.FancyDebug();
        LootStack.Push(new Loot("Sword of Doom", 5));
        LootStack.Push(new Loot("HP Boost", 1));
        LootStack.Push(new Loot("Golden Key", 3));
        LootStack.Push(new Loot("Pair of Winged Boots", 2));
        LootStack.Push(new Loot("Mythril Bracer", 4));

    }
    public void PrintLootReport()
    {   FilterLoot();
        var currentItem=LootStack.Pop();
        var nextItem = LootStack.Peek();
        Debug.LogFormat("You got a {0}! You've got a good chance of finding a {1} next!",currentItem.name,nextItem.name);
        Debug.LogFormat("There are {0} random loot items waiting for you!", LootStack.Count);
       
    }
    public void FilterLoot()
    {
        var rareLoot = from item in LootStack
                       where item.rarity >= 3
                       orderby item.rarity
                       select item;
        foreach(var item in rareLoot)
        {
            Debug.LogFormat("Rare item: {0}!", item.name);
        }
    }
    //public bool LootPredicate(Loot loot)
    //{
    //    return loot.rarity >= 3;
    //}

    public int Items
    {
        get { return _itemsCollected; }
        set
        {
            _itemsCollected = value;
            ItemText.text = "Items: " + Items;

            if (_itemsCollected >= MaxItems)
            {
                WinButton.gameObject.SetActive(true);
                UpdateScene("You've found all the items!");
            }
            else
            {
                ProgressText.text = "Item found, only " + (MaxItems - _itemsCollected) + " more!";
            }
        }
    }

    public int PlayerHP
    {

        get { return _playerHP; }
        set
        {
            _playerHP = value;
            PlayerHealth.value = PlayerHP;
            Fill.color= PlayerProgress.Evaluate(PlayerHealth.normalizedValue);
            //HealthText.text = "Health: " + PlayerHP;

            if (_playerHP <= 0)
            {
                LossButton.gameObject.SetActive(true);
                UpdateScene("You want another life with that?");
            }
            else
            {
                ProgressText.text = "Ouch... that's got hurt.";
                Debug.LogFormat("Lives: {0}", _playerHP);

            }

        }
    }

    public void RestartScene()
    {
            Utilities.RestartLevel(0);
    }

    public void UpdateScene(string updatedText)
    {
        ProgressText.text = updatedText;
        Time.timeScale = 0f;
    }
}