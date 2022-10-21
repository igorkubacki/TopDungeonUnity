using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterMenu : MonoBehaviour
{
    // Text fields
    public TMP_Text levelText, hitpointText, pesosText, upgradeCostText, xpText;

    // Logic fields

    private int currentCharacterSelection = 0;
    public Image characterSelectionSprite, weaponSprite;
    public RectTransform xpBar;

    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("menu_hidden"))
            {
                anim.SetTrigger("Show");
                UpdateMenu();
            }
            else
            {
                anim.SetTrigger("Hide");
            }
        }
    }

    // character selection

    public void OnArrowClick(bool right)
    {
        if (right)
        {
            currentCharacterSelection++;

            // If we went too far away
            if (currentCharacterSelection == GameManager.instance.playerSprites.Count)
                currentCharacterSelection = 0;

            OnSelectionChanged();

        }
        else
        {
            currentCharacterSelection--;

            // If we went too far away
            if (currentCharacterSelection < 0)
                currentCharacterSelection = GameManager.instance.playerSprites.Count - 1;

            OnSelectionChanged();
        }
    }
    private void OnSelectionChanged()
    {
        characterSelectionSprite.sprite = GameManager.instance.playerSprites[currentCharacterSelection];
        GameManager.instance.player.SwapSprite(currentCharacterSelection);
    }

    // weapon upgrade
    public void OnUpgradeClick()
    {
        if (GameManager.instance.TryUpgradeWeapon())
            UpdateMenu();
    }

    // Upgrade character info
    public void UpdateMenu()
    {
        // Weapon
        weaponSprite.sprite = GameManager.instance.weaponSprites[GameManager.instance.weapon.weaponLevel];
        if (GameManager.instance.weapon.weaponLevel == GameManager.instance.weaponPrices.Count)
            upgradeCostText.text = "MAX";
        else
            upgradeCostText.text = GameManager.instance.weaponPrices[GameManager.instance.weapon.weaponLevel].ToString();
        
        // meta
        levelText.text = GameManager.instance.GetCurrentLevel().ToString();
        hitpointText.text = GameManager.instance.player.hitpoint.ToString();
        pesosText.text = GameManager.instance.pesos.ToString();

        // xp bar
        int currentLevel = GameManager.instance.GetCurrentLevel();
        if (GameManager.instance.GetCurrentLevel() == GameManager.instance.xpTable.Count)
        {
            xpText.text = GameManager.instance.experience.ToString() + " total experience points";
            xpBar.localScale = Vector3.one;
        }
        else
        {
            int previousLevelXp = GameManager.instance.GetXpToLevel(currentLevel - 1);
            int currentLevelXp = GameManager.instance.GetXpToLevel(currentLevel);

            int diff = currentLevelXp - previousLevelXp;
            int currentXpIntoLevel = GameManager.instance.experience - previousLevelXp;

            float completionRatio = (float)currentXpIntoLevel / (float)diff;
            xpBar.localScale = new Vector3(completionRatio, 1, 1);
            xpText.text = currentXpIntoLevel.ToString() + " / " + diff;
        }
    }
}
