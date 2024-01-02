using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using System.Linq;


[System.Serializable]
public class Character
{
    [Header("Character Info")]
    public string characterName;
    //public Sprite characterImage;
    public Color characterColor;

    [Header("Character Type")]
    public bool isIntro;
    public bool isRevive;
    public bool isReviveWithBuff;
    public bool isDie;

    [Header("List Choice For Character")]
    public List<Choice> choices;
}

[System.Serializable]
public class Choice
{
    [Header("Description For Card")]
    public string description;
    public string decision1;
    public string decision2;

    [Header("Stat For Agree")]
    // Stat effects for decision 1 (agree)
    public int militaryEffect1;
    public int publicEsteem1;
    public int economy1;
    public int spiritualityEffect1;

    [Header("Stat For Disagree")]
    // Stat effects for decision 2 (disagree)
    public int militaryEffect2;
    public int publicEsteem2;
    public int economy2;
    public int spiritualityEffect2;

    [Header("Ruling Day For Each Decision")]
    //Ruling days
    public int rulingDays1;
    public int rulingDays2;

    [Header("Buff")]
    public BuffType buffType;
    public bool isDescriptionBuff;
}

public enum BuffType//enum to set buff type
{
    None,
    military,
    economy,
    publicEsteem,
    spirituality
}

public class Data : MonoBehaviour
{
    public static Data instance { get; private set; }

    [Header("Characters")]
    public Character[] characters;

    [Header("Story")]
    public Character intro;
    public Character ReviveCard;
    public Character DieCard;
    
    [Header("Revive With Buff List")]      
    public Character ReviveWithBuffCard;

    [Header("Description For Buff List")]
    public Character BuffDescription;

    [Header("Choices List To Unlock")]
    public Character[] TestLockedChoice;


    [Header("Card Elements")]
    public TextMeshProUGUI Info1;
    public TextMeshProUGUI TopAnswer;
    public TextMeshProUGUI BottomAnswer;
    public TextMeshProUGUI Character_Name;
    public Image Character_Image;

    [Header("UIColor")]
    public Image MiddleUI;
    public Image TopAndBottomUI;
    public Color DefaultColor;

    [Header("Buff")]
    public Transform buffParent;
    public GameObject buffObject;
    public BuffType[] BuffTypes;

    [Header("Buff Sprites")]
    public Sprite militaryBuffSprite;
    public Sprite economyBuffSprite;
    public Sprite publicEsteemBuffSprite;
    public Sprite spiritualityBuffSprite;

    [Header("BuffKey")]
    public string hasMilitaryBuffKey = "HasMilitaryBuff";
    public string hasEconomyBuffKey = "HasEconomyBuff";
    public string hasPublicEsteemBuffKey = "HasPublicEsteemBuff";
    public string hasSpiritualityBuffKey = "HasSpiritualityBuff";

    [Header("AmountBuff")]
    public int amountOfBuff;
    public void AddBuff() => amountOfBuff++;
    public void RemoveBuff() => amountOfBuff--;


    [Header("Keys To Unlock Choice List")]
    public string UnlockedTestChoiceKey = "UnlockedChoice";

    //Key and bool for first time playing
    private const string FirstTimeKey = "FirstTime";
    private bool isFirstTime;

    //Reference for current character and choice
    private Character currentCharacter;
    private Choice currentChoice;

    //Random index for character and choice
    private int randomCharacterIndex;
    private int randomChoiceIndex;

    //bool for buff
    public bool hasMilitaryBuff;
    public bool hasEconomyBuff;
    public bool hasPublicEsteemBuff;
    public bool hasSpiritualityBuff;

    private int buffIndex;//number in buff list
    private BuffType buffType;//type of buff
    private Choice BuffChoice;//choice to create buff
    private bool canReviveWithBuff;//bool for revive with buff

    //bool for Gameover
    [HideInInspector]    
    public bool Gameover = false;

    //bool for Unlock Test choices
    private bool UnlockedTestChoice;

    private void Start()
    {
        
        //singleton pattern
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        //Check if this is the first time the player has played the game
        isFirstTime = PlayerPrefs.GetInt(FirstTimeKey, 1) == 1;

        //Check if the player has unlocked the Test choices
        UnlockedTestChoice = PlayerPrefs.GetInt(UnlockedTestChoiceKey, 0) == 1;

        //Load Buffs state
        hasMilitaryBuff = PlayerPrefs.GetInt(hasMilitaryBuffKey, 0) == 1;
        hasEconomyBuff = PlayerPrefs.GetInt(hasEconomyBuffKey, 0) == 1;
        hasPublicEsteemBuff = PlayerPrefs.GetInt(hasPublicEsteemBuffKey, 0) == 1;
        hasSpiritualityBuff = PlayerPrefs.GetInt(hasSpiritualityBuffKey, 0) == 1;

        //Create Buffs if they exist
        if (hasMilitaryBuff)
        {
            CreateBuff(new Choice { buffType = BuffType.military });
        }
        if (hasEconomyBuff)
        {
            CreateBuff(new Choice { buffType = BuffType.economy });
        }
        if (hasPublicEsteemBuff)
        {
            CreateBuff(new Choice { buffType = BuffType.publicEsteem });
        }
        if (hasSpiritualityBuff)
        {
            CreateBuff(new Choice { buffType = BuffType.spirituality });
        }

        //Check to start Intro or not
        if (isFirstTime)
        {
            currentCharacter = intro;
        }
        else
        {
            currentCharacter = characters[0];
        }

        //Set the first choice for start game, prevent null reference
        currentChoice = currentCharacter.choices[0];

        //Unlock Test choices if the player has unlocked them
        if (UnlockedTestChoice)
        {
            UnlockChoiceList(TestLockedChoice);
        }

        //Set the current color for MiddleUI for logic purpose
        DefaultColor = MiddleUI.color;
    }

    public void MakeDecision()
    {
        if (currentChoice.isDescriptionBuff)
        {
            ShowBuffDescription();
            return;
        }
        if (currentCharacter.isReviveWithBuff)
        {
            ReviveWithBuff();
            return;
        }
        if (currentCharacter.isRevive)
        {
            Revive();
            return;
        }
        if (currentCharacter.isDie)
        {
            Kill();
            return;
        }
        if (currentCharacter.isIntro)
        {
            Intro();
            return;
        }

        MakeRandomDecision();
    }

    private void SetCardElements()
    {
        // Set the character's name
        Character_Name.text = currentCharacter.characterName;

        // Set the character's image
        //Character_Image.sprite = randomCharacter.characterImage;

        // Set the character's color
        Character_Image.color = currentCharacter.characterColor;

        // Set the choice's description
        Info1.text = currentChoice.description;

        // Set the choice's decisions
        TopAnswer.text = currentChoice.decision1;
        BottomAnswer.text = currentChoice.decision2;
    }

    private void DeleteUsingChoice(int ChoiceIndex)
    {
        //Delete using decision
        currentCharacter.choices.RemoveAt(ChoiceIndex);
        //If there are no more decisions for the character, remove the character from the list
        if (currentCharacter.choices.Count == 0 && !currentCharacter.isIntro )
        {
            List<Character> tempCharacters = new List<Character>(characters);
            tempCharacters.RemoveAt(randomCharacterIndex);
            characters = tempCharacters.ToArray();
        }
    }

    #region Random Decision Logic
    public void MakeRandomDecision()
    {
        //Check if the is any character left
        if (characters.Length == 0)
        {
            Debug.Log("No more characters left");
            return;
        }

        RandomDecision();

        if (currentChoice.buffType != BuffType.None)
        {
            if (!CheckBuffType(currentChoice.buffType))
            {
                BuffChoice = currentChoice;
            }
            else
            {
                DeleteUsingChoice(randomChoiceIndex);
                MakeRandomDecision();
                Debug.Log("Buff already have");
            }
        }

        SetCardElements();

        DeleteUsingChoice(randomChoiceIndex);

    }

    private void RandomDecision()
    {
        // Get a random character from the topics list
        randomCharacterIndex = Random.Range(0, characters.Length);
        currentCharacter = characters[randomCharacterIndex];

        // Get a random choice from the character's choices list
        randomChoiceIndex = Random.Range(0, currentCharacter.choices.Count);
        currentChoice = currentCharacter.choices[randomChoiceIndex];

        TabData.instance.EncounterCharacter(currentCharacter.characterName);
    }
    #endregion

    #region Unlock Choices Logic
    private void UnlockChoiceList(Character[] choicesToUnlock)
    {
        // Tạo một dictionary để tra cứu nhanh nhân vật theo tên
        Dictionary<string, Character> characterDictionary = characters.ToDictionary(c => c.characterName);

        foreach (Character characterToUnlock in choicesToUnlock)
        {
            // Kiểm tra xem nhân vật cần mở khóa có tồn tại trong danh sách chính hay không
            if (characterDictionary.TryGetValue(characterToUnlock.characterName, out Character matchingCharacter))
            {
                // Duyệt qua các lựa chọn của nhân vật cần mở khóa và thêm vào nhân vật trong danh sách chính
                foreach (Choice choiceToUnlock in characterToUnlock.choices)
                {
                    matchingCharacter.choices.Add(choiceToUnlock);
                }
            }
        }
    }
    #endregion

    #region Intro Logic
    private void Intro()
    {
        TabData.instance.EncounterCharacter(currentCharacter.characterName);
        currentChoice = currentCharacter.choices[0];
        SetCardElements();
        DeleteUsingChoice(0);
        if (currentCharacter.choices.Count == 0)
        {
            TabData.instance.EncounterStory(0);
            currentCharacter.isIntro = false;
            PlayerPrefs.SetInt(FirstTimeKey, 0);
            PlayerPrefs.Save();
        }
    }
    #endregion

    #region Revive Logic
    private void Revive()//Logic for revive card
    {
        currentChoice = currentCharacter.choices[0];
        SetCardElements();
        GameManager.Instance.ResetElementStats();
        MiddleUI.DOColor(DefaultColor, 1f);
        GameManager.Instance.isChecked = false;
        currentCharacter.isRevive = false;
        TabData.instance.EncounterStory(1);
    }
    
    private void RevivePlayer()//calling from Kill Logic to set revive card
    {
        currentCharacter = ReviveCard;
        TabData.instance.EncounterCharacter(currentCharacter.characterName);
    }
    #endregion

    #region Kill Logic
    private void Kill()//Logic for die card
    {
        if (canReviveWithBuff)
        {
            SetCardElements();
            ReviveWithBuffPlayer(buffIndex);
            canReviveWithBuff = false;
            return;
        }
        if (TabData.instance.canRevive)
        {
            SetCardElements();
            RevivePlayer();
            TabData.instance.canRevive = false;
            return;
        }
            SetCardElements();
            Gameover = true;
    }

    public void KillPlayer(Choice choice)//calling from GameManager to set die card
    {
        currentCharacter = DieCard;
        currentChoice = choice;
        MiddleUI.DOColor(TopAndBottomUI.color, 1f);
        TabData.instance.EncounterCharacter(currentCharacter.characterName);
    }

    public Choice FindChoiceInDieCard(int DieCardIndex)//calling from GameManager to find correct choice in die card list
    {
        foreach (Choice choice in DieCard.choices)
        {
            if (choice == DieCard.choices[DieCardIndex])
            {
                return choice;
            }
        }
        return null;
    }
    #endregion

    #region Buff Logic
    private void CreateBuff(Choice Buff)//Create buff object in buff Parent, +1 to amount of buff
    {
        if (amountOfBuff >= 5) return;

        AddBuff();

        GameObject newBuff = Instantiate(buffObject, buffParent);
        //SetBuffSprite(Buff.buffType,newBuff);
        SetBuffColor(Buff.buffType, newBuff);
    }

    private void SetBuffSprite(BuffType buffType,GameObject Buff)//Set up sprite for buff object
    {
        switch (buffType)
        {
            case BuffType.military:
                Buff.GetComponentInChildren<Image>().sprite = militaryBuffSprite;
                hasMilitaryBuff = true;
                PlayerPrefs.SetInt(hasMilitaryBuffKey, 1);
                LoadBuffDescription(0);
                BuffTypes[0] = BuffType.military;
                break;
            case BuffType.economy:
                Buff.GetComponentInChildren<Image>().sprite = economyBuffSprite;
                hasEconomyBuff = true;
                PlayerPrefs.SetInt(hasEconomyBuffKey, 1);
                LoadBuffDescription(1);
                BuffTypes[1] = BuffType.economy;
                break;
            case BuffType.publicEsteem:
                Buff.GetComponentInChildren<Image>().sprite = publicEsteemBuffSprite;
                hasPublicEsteemBuff = true;
                PlayerPrefs.SetInt(hasPublicEsteemBuffKey, 1);
                LoadBuffDescription(2);
                BuffTypes[2] = BuffType.publicEsteem;
                break;
            case BuffType.spirituality:
                Buff.GetComponentInChildren<Image>().sprite = spiritualityBuffSprite;
                hasSpiritualityBuff = true;
                PlayerPrefs.SetInt(hasSpiritualityBuffKey, 1);
                LoadBuffDescription(3);
                BuffTypes[3] = BuffType.spirituality;
                break;
        }
    }

    private void SetBuffColor(BuffType buffType, GameObject Buff)//Set up color for buff object
    {
        switch (buffType)
        {
            case BuffType.military:
                Buff.GetComponentInChildren<Image>().color = Color.red;
                hasMilitaryBuff = true;
                PlayerPrefs.SetInt(hasMilitaryBuffKey, 1);
                LoadBuffDescription(0);
                BuffTypes[0] = BuffType.military;
                break;
            case BuffType.economy:
                Buff.GetComponentInChildren<Image>().color = Color.green;
                hasEconomyBuff = true;
                PlayerPrefs.SetInt(hasEconomyBuffKey, 1);
                LoadBuffDescription(1);
                BuffTypes[1] = BuffType.economy;
                break;
            case BuffType.publicEsteem:
                Buff.GetComponentInChildren<Image>().color = Color.blue;
                hasPublicEsteemBuff = true;
                PlayerPrefs.SetInt(hasPublicEsteemBuffKey, 1);
                LoadBuffDescription(2);
                BuffTypes[2] = BuffType.publicEsteem;
                break;
            case BuffType.spirituality:
                Buff.GetComponentInChildren<Image>().color = Color.yellow;
                hasSpiritualityBuff = true;
                PlayerPrefs.SetInt(hasSpiritualityBuffKey, 1);
                LoadBuffDescription(3);
                BuffTypes[3] = BuffType.spirituality;
                break;

        }
    }

    public void CheckAndCreateBuff()//Calling from Card Script to check and create buff if agree
    {
        if (BuffChoice != null)
        {
            CreateBuff(BuffChoice);
            BuffChoice = null;
        }
    }

    public void SetUpBuff(int Buff,BuffType buffTypeData)//Set up before revive with buff card
    {
        buffIndex = Buff;
        buffType = buffTypeData;
        canReviveWithBuff = true;
    }

    public void ReviveWithBuffPlayer(int buffIndex)//Calling from Kill Logic to set revive with buff card
    {
        currentCharacter = ReviveWithBuffCard;
        currentChoice = currentCharacter.choices[buffIndex];
        TabData.instance.EncounterCharacter(currentCharacter.characterName);
    }

    private void ReviveWithBuff()//Logic for revive with buff card
    {
        SetCardElements();
        MiddleUI.DOColor(DefaultColor, 1f);
        RemoveBuffObject(buffType);
        GameManager.Instance.isChecked = false;
        currentCharacter.isReviveWithBuff = false;
        TabData.instance.EncounterStory(1);
    }

    private bool CheckBuffType(BuffType buffType)//Check if buff type already exists in buff Parent
    {
        foreach (BuffType existingBuff in BuffTypes)
        {
            if (existingBuff == buffType)
            {
                Debug.Log("Buff already have");
                return true; // BuffType already exists
            }
        }
        return false; // BuffType does not exist
    }

    private void RemoveBuffObject(BuffType buffType)//Remove buff object from the buff Parent, -1 to amount of buff
    {
        Debug.Log("Remove buff object");
        RemoveBuff();
        // Duyệt qua tất cả các buffObject con trong buffParent
        foreach (Transform child in buffParent)
        {
            Image buffImage = child.GetComponentInChildren<Image>();

            if (buffImage != null)
            {
                // Kiểm tra loại buff và xóa nếu khớp
                switch (buffType)
                {
                    case BuffType.military:
                        if (buffImage.color == Color.red)
                        {
                            Destroy(child.gameObject);
                            return;
                        }
                        break;
                    case BuffType.economy:
                        if (buffImage.color == Color.green)
                        {
                            Destroy(child.gameObject);
                            return;
                        }
                        break;
                    case BuffType.publicEsteem:
                        if (buffImage.color == Color.blue)
                        {
                            Destroy(child.gameObject);
                            return;
                        }
                        break;
                    case BuffType.spirituality:
                        if (buffImage.color == Color.yellow)
                        {
                            Destroy(child.gameObject);
                            return;
                        }
                        break;
                }
            }
        }
    }
    #endregion

    #region Logic for Buff Description
    private void ShowBuffDescription()//Logic for buff description card
    {
        SetCardElements();
        currentChoice.isDescriptionBuff = false;
    }

    private void LoadBuffDescription(int i)//Calling from SetBuffSprite to load buff description card
    {
        currentCharacter = BuffDescription;
        currentChoice = currentCharacter.choices[i];
        TabData.instance.EncounterCharacter(currentCharacter.characterName);
    }
    #endregion

    #region Return Character And Choice
    public Character CurrentCharacter//Return current Character for Progress system to use
    {
        get { return currentCharacter; }
    }

    public Choice CurrentChoice//Return current Choice for Card Script to use
    {
        get { return currentChoice; }
    }
    #endregion
}